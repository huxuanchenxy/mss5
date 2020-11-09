using MSS.API.Dao.Interface;
using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSS.API.Model.DTO;
using static MSS.API.Common.Utility.Const;
using static MSS.API.Common.Const;
using MSS.API.Common.Utility;
using MSS.API.Common;
using Newtonsoft.Json;
using CSRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;

namespace MSS.API.Core.V1.Business
{
    public interface IUserService
    {
        Task<ApiResult> GetByID(int id);
        Task<ApiResult> Update(User User);
        Task<ApiResult> Delete(string ids);
        Task<ApiResult> GetAll();
        Task<ApiResult> ChangePwd(string oldPwd, string newPwd);
        Task<ApiResult> ResetPwd(string ids);

        Task<ApiResult> CheckUserLogin(string acc, string password);

        Task<MSSResult<MenuTree>> GetMenu();//int? userID = null
        Task<ApiResult> GetActionByUser();

        Task<ApiResult> GetPageList(UserParm parm);
        Task<ApiResult> Save(User obj);
    }
    public class UserService : IUserService
    {
        //private readonly ILogger<UserService> _logger;
        private readonly IUserRepo<User> _repo;
        private readonly IActionRepo<ActionInfo> _ActionRepo;

        private readonly int userID;

        private readonly IDistributedCache _cache;
        public UserService(IUserRepo<User> userRepo, IActionRepo<ActionInfo> actionRepo, 
            IAuthHelper auth, IDistributedCache cache)
        {
            //_logger = logger;
            _repo = userRepo;
            _ActionRepo = actionRepo;
            userID = auth.GetUserId();
            userID = 1;//TODO 上线删掉
            _cache = cache;
        }
        public async Task<ApiResult> GetPageList(UserParm parm)
        {
            ApiResult ret = new ApiResult();
            try
            {
                //parm.UserID = _userID;
                //parm.UserID = 40;
                var data = await _repo.GetPageList(parm);
                ret.code = Code.Success;
                ret.data = data;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                ret.msg = ex.Message;
            }

            return ret;
        }

        public async Task<ApiResult> GetByID(int id)
        {
            ApiResult mRet = new ApiResult();
            try
            {
                if (id == 0)//从redis的token映射取
                {
                    mRet.data = await _repo.GetByID(userID);
                }
                else
                {
                    mRet.data = await _repo.GetByID(id);
                }
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }
        public async Task<ApiResult> Save(User user)
        {
            ApiResult mRet = new ApiResult();
            try
            {
                User u =await _repo.GetByAcc(user.AccName);
                if (u==null)
                {
                    Encrypt en = new Encrypt();
                    int r = new Random().Next(1, PWD_RANDOM_MAX);
                    user.Password = en.DoEncrypt(INIT_PASSWORD,r);
                    user.RandomNum = r;
                    DateTime dt = DateTime.Now;
                    user.UpdatedTime = dt;
                    user.CreatedTime = dt;
                    user.CreatedBy = userID;
                    user.UpdatedBy = userID;
                    //如果是外部人员系统生成accname
                    //if (user.OutMan == 1)
                    //{
                    //    user.AccName = "line5" + DateTime.Now.ToString("yyyyMMddHHmmssSSS");
                    //}
                    mRet.data = await _repo.Save(user);
                    await SaveRedis();
                    mRet.code = (int)ErrType.OK;
                }
                else
                {
                    mRet.code = Code.ImportError;
                    mRet.msg = "登录账号重复";
                }
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.ImportError;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<ApiResult> Update(User user)
        {
            ApiResult mRet = new ApiResult();
            try
            {
                user.UpdatedTime = DateTime.Now;
                user.UpdatedBy = userID;
                mRet.data = await _repo.Update(user);
                await SaveRedis();
                mRet.code = (int)ErrType.OK;
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.ImportError;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<ApiResult> Delete(string ids)
        {
            ApiResult mRet = new ApiResult();
            try
            {
                var isInOrg = await _repo.IsInOrg(ids.Split());
                if (isInOrg)
                {
                    mRet.code = Code.CheckDataRulesFail;
                    mRet.msg = "已分配到组织树中的人员不可删除";
                }
                else
                {
                    mRet.data = await _repo.Delete(ids.Split(','), userID);
                    await SaveRedis();
                    mRet.code = (int)ErrType.OK;
                }
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<ApiResult> GetAll()
        {
            ApiResult mRet = new ApiResult();
            try
            {
                mRet.data = await _repo.GetAll();
                mRet.code = (int)ErrType.OK;
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<ApiResult> ChangePwd(string oldPwd, string newPwd)
        {
            ApiResult mRet = new ApiResult();
            try
            {
                User u = await _repo.GetByID(userID);
                if (u != null)
                {
                    Encrypt en = new Encrypt();
                    if (en.DoEncrypt(oldPwd,u.RandomNum)!=u.Password)
                    {
                        mRet.code = Code.ImportError;
                        mRet.msg = "密码错误";
                    }
                    int r = new Random().Next(1, PWD_RANDOM_MAX);
                    u.Password = en.DoEncrypt(newPwd,r);
                    u.RandomNum = r;
                    DateTime dt = DateTime.Now;
                    u.UpdatedTime = dt;
                    u.UpdatedBy = userID;
                    mRet.data = await _repo.ChangePwd(u);
                    mRet.code = (int)ErrType.OK;
                }
                else
                {
                    mRet.code = Code.ImportError;
                    mRet.msg = "登录账号重复";
                }
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<ApiResult> ResetPwd(string ids)
        {
            ApiResult mRet = new ApiResult();
            try
            {
                mRet.data = await _repo.ResetPwd(ids.Split(','),userID);
                mRet.code = (int)ErrType.OK;
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<ApiResult> CheckUserLogin(string acc, string pwd)
        {
            ApiResult mRet = new ApiResult();
            try
            {
                User ui = await _repo.GetByAcc(acc);
                if (ui != null)
                {
                    Encrypt encrypt = new Encrypt();
                    string strPwd = encrypt.DoEncrypt(pwd, ui.RandomNum);
                    if (ui.Password != strPwd)
                    {
                        mRet.code = Code.ImportError;
                        mRet.msg = "密码错误";
                        //return mRet;
                    }
                    //else
                    //{
                    //    //获取此人对应的权限
                    //    mRet = await GetMenu();
                    //    mRet.relatedData = ui;
                    //    return mRet;
                    //}
                    mRet.data = ui.Id;
                }
                else
                {
                    mRet.code = Code.ImportError;
                    mRet.msg = "账号错误";
                }
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<MSSResult<MenuTree>> GetMenu()
        {
            User u = await _repo.GetByID(userID);
            MSSResult<MenuTree> mRet = new MSSResult<MenuTree>();
            try
            {
                List<ActionAll> laa = new List<ActionAll>();
                //允许勾选和不对外开放的且为菜单的url
                if (u.IsSuper)
                {
                    laa = await _ActionRepo.GetActionAll();
                    mRet.data = ActionHelper.GetMenuTree(laa.Where(
                        a => (a.Level == (int)ACTION_LEVEL.AllowSelection ||
                        a.Level == (int)ACTION_LEVEL.NotAllowAll) && a.GroupID > 0).ToList());
                }
                //根据用户ID获取对应菜单权限
                else
                {
                    laa = await _ActionRepo.GetActionByUser(userID);
                    mRet.data = ActionHelper.GetMenuTree(laa.Where(a => a.GroupID > 0).ToList());
                }
                mRet.code = (int)ErrType.OK;
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = (int)ErrType.SystemErr;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<ApiResult> GetActionByUser()
        {
            User u = await _repo.GetByID(userID);
            ApiResult mRet = new ApiResult();
            try
            {
                List<ActionAll> laa = new List<ActionAll>();
                if (u.IsSuper)
                {
                    mRet.data = await _ActionRepo.GetActionAll();
                }
                //根据用户ID获取对应所有url权限
                else
                {
                    mRet.data = await _ActionRepo.GetActionByUser(userID);
                }
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = Code.Failure;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        private async Task SaveRedis()
        {
            List<User> users = await _repo.GetAllContainSuper();
            _cache.SetString(REDIS_AUTH_KEY_USER, JsonConvert.SerializeObject(users));
        }
    }
}
