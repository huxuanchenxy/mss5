using MSS.API.Dao.Interface;
using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSS.API.Model.DTO;
using static MSS.API.Common.Utility.Const;
using static MSS.API.Common.Const;
using MSS.API.Dao.Implement;
using MSS.API.Common.Utility;
using Microsoft.Extensions.Configuration;
using CSRedis;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using MSS.API.Common;

namespace MSS.API.Core.V1.Business
{
    public class RoleService: IRoleService
    {
        //private readonly ILogger<RoleService> _logger;
        private readonly IRoleRepo<Role> _RoleRepo;
        private readonly IUserRepo<User> _UserRepo;
        private readonly IActionRepo<ActionInfo> _ActionRepo;

        private readonly int userID;

        //private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;


        public RoleService(IRoleRepo<Role> roleRepo, IUserRepo<User> userRepo, 
            IActionRepo<ActionInfo> actionRepo, IAuthHelper auth, IDistributedCache cache)
        {
            //_logger = logger;
            _RoleRepo = roleRepo;
            _UserRepo = userRepo;
            _ActionRepo = actionRepo;

            userID = auth.GetUserId();
            _cache = cache;
        }
        public async Task<MSSResult<RoleView>> GetPageByParm(RoleQueryParm parm)
        {
            MSSResult<RoleView> mRet = new MSSResult<RoleView>();
            try
            {
                parm.page = parm.page == 0 ? 1 : parm.page;
                parm.rows= parm.rows == 0 ? Common.Const.PAGESIZE : parm.rows;
                parm.sort = string.IsNullOrWhiteSpace(parm.sort) ? "id" : parm.sort;
                parm.order = parm.order.ToLower() == "desc" ? "desc" : "asc";
                mRet = await _RoleRepo.GetPageByParm(parm);
                int[] arrRoleID = mRet.data.Select(a => a.Id).ToArray();
                List<RoleActions> lra =await _ActionRepo.GetActionByRoles(arrRoleID);
                foreach (var item in mRet.data)
                {
                    List<ActionAll> actionAll = lra.Where(a => a.roleID == item.Id).ToList<ActionAll>();
                    //item.action_trees = ActionHelper.GetActionTree(actionAll);
                    List<ActionTree> actiontree = ActionHelper.ConvertToTree(actionAll);
                    item.action_trees = ActionHelper.BuildTreeRecursive(actiontree,0);
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
        public async Task<MSSResult<int>> GetByID(int id)
        {
            MSSResult<int> mRet = new MSSResult<int>();
            try
            {
                if (id==0)
                {
                    mRet.code = (int)ErrType.ErrParm;
                    mRet.msg = "参数不正确，id不可为0";
                    return mRet;
                }
                mRet.relatedData = await _RoleRepo.GetByID(id);
                //List<ActionAll> laa = await _ActionRepo.GetActionAll();
                //List<int> actions = await _ActionRepo.GetActionIDByRoleID(id);
                //mRet.data = ActionHelper.GetCheckedActionTree(laa, actions);
                mRet.data =(await _ActionRepo.GetActionIDByRoleID(id)).ToList();
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

        public async Task<MSSResult> Add(RoleStrActions roleStrActions)
        {
            MSSResult mRet = new MSSResult();
            try
            { 
                DateTime dt = DateTime.Now;
                roleStrActions.UpdatedTime = dt;
                roleStrActions.CreatedTime = dt;
                roleStrActions.CreatedBy = userID;
                roleStrActions.UpdatedBy = userID;
                bool isRepeat= await _RoleRepo.IsNameRepeat(roleStrActions.role_name);
                if (isRepeat)
                {
                    mRet.code = (int)ErrType.Repeat;
                    mRet.msg = "角色名称重复";
                }
                else
                {
                    mRet.data = await _RoleRepo.Add(roleStrActions);
                    await SaveRedis();
                    mRet.code = (int)ErrType.OK;
                }
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = (int)ErrType.SystemErr;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<MSSResult> Update(RoleStrActions roleStrActions)
        {
            MSSResult mRet = new MSSResult();
            try
            {
                roleStrActions.UpdatedTime = DateTime.Now;
                roleStrActions.UpdatedBy = userID;
                var role=await _RoleRepo.GetByID(roleStrActions.Id);
                if (role==null)
                {
                    mRet.code = (int)ErrType.NoRecord;
                    mRet.msg = "此角色已不存在";
                }
                else
                {
                    if (role.role_name!= roleStrActions.role_name)
                    {
                        bool isRepeat = await _RoleRepo.IsNameRepeat(roleStrActions.role_name);
                        if (isRepeat)
                        {
                            mRet.code = (int)ErrType.Repeat;
                            mRet.msg = "角色名称重复";
                            return mRet;
                        }
                    }
                    mRet.data = await _RoleRepo.Update(roleStrActions);
                    await SaveRedis();
                    mRet.code = (int)ErrType.OK;
                }
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = (int)ErrType.SystemErr;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<MSSResult> Delete(string ids)
        {
            MSSResult mRet = new MSSResult();
            try
            {
                string[] arrIds = ids.Split(',');
                int hasUser = await _UserRepo.GetUserCountByRole(arrIds);
                if (hasUser>0)
                {
                    mRet.code = (int)ErrType.Associated;
                    mRet.msg = "此角色下有用户，不可删除";
                }
                else
                {
                    mRet.data = await _RoleRepo.Delete(arrIds);
                    await SaveRedis();
                    mRet.code = (int)ErrType.OK;
                }
                return mRet;
            }
            catch (Exception ex)
            {
                mRet.code = (int)ErrType.SystemErr;
                mRet.msg = ex.Message;
                return mRet;
            }
        }

        public async Task<MSSResult> GetAll()
        {
            MSSResult mRet = new MSSResult();
            try
            {
                mRet.data = await _RoleRepo.GetAll();
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

        public async Task<ApiResult> GetActionTree()
        {
            ApiResult ret = new ApiResult();
            try
            {
                var list = await _ActionRepo.GetActionAll();
                var tmp =  ActionHelper.ConvertToTree(list);
                var result = ActionHelper.BuildTreeRecursive(tmp, 0);
                ret.data = result;
                return ret;
            }
            catch (Exception ex)
            {
                ret.code = Code.Failure;
                
                return ret;
            }
        }

        private async Task SaveRedis()
        {
            List<RoleAction> ras = await _RoleRepo.GetRoleActionAll();
            _cache.SetString(REDIS_AUTH_KEY_ROLEACTION, JsonConvert.SerializeObject(ras));

        }
    }
}
