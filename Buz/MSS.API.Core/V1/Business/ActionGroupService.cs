using MSS.API.Dao.Interface;
using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSS.API.Model.DTO;
using static MSS.API.Common.Utility.Const;
using MSS.API.Common.Utility;

namespace MSS.API.Core.V1.Business
{
    public class ActionGroupService: IActionGroupService
    {
        //private readonly ILogger<ActionGroupService> _logger;
        private readonly IActionGroupRepo<ActionGroup> _ActionGroupRepo;
        private readonly IActionRepo<ActionInfo> _ActionRepo;

        private readonly int userID;
        public ActionGroupService(IActionGroupRepo<ActionGroup> actionGroupRepo, 
            IActionRepo<ActionInfo> _actionRepo, IAuthHelper auth)
        {
            //_logger = logger;
            _ActionGroupRepo = actionGroupRepo;
            _ActionRepo = _actionRepo;

            userID = auth.GetUserId();
        }
        public async Task<MSSResult<ActionGroupView>> GetPageByParm(ActionGroupQueryParm parm)
        {
            MSSResult<ActionGroupView> mRet = new MSSResult<ActionGroupView>();
            try
            {
                parm.page = parm.page == 0 ? 1 : parm.page;
                parm.rows= parm.rows == 0 ? PAGESIZE : parm.rows;
                parm.sort = string.IsNullOrWhiteSpace(parm.sort) ? "id" : parm.sort;
                parm.order = parm.order.ToLower() == "desc" ? "desc" : "asc";
                mRet = await _ActionGroupRepo.GetPageByParm(parm);
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

        public async Task<MSSResult> Add(ActionGroup actionGroup)
        {
            MSSResult mRet = new MSSResult();
            try
            {
                DateTime dt = DateTime.Now;
                actionGroup.UpdatedTime = dt;
                actionGroup.CreatedTime = dt;
                actionGroup.CreatedBy = userID;
                actionGroup.UpdatedBy = userID;
                mRet.data = await _ActionGroupRepo.Add(actionGroup);
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

        public async Task<MSSResult> Update(ActionGroup actionGroup)
        {
            MSSResult mRet = new MSSResult();
            try
            {
                actionGroup.UpdatedTime = DateTime.Now;
                actionGroup.UpdatedBy = userID;
                mRet.data = await _ActionGroupRepo.Update(actionGroup);
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

        

        public async Task<MSSResult> Delete(string ids)
        {
            MSSResult mRet = new MSSResult();
            try
            {
                var action = await _ActionRepo.GetByActionGroup(ids.Split(','));
                if (action.Count() > 0)
                {
                    mRet.code = (int)ErrType.Associated;
                    mRet.msg = "权限组下挂有权限，不允许删除";
                    return mRet;
                }
                mRet.data = await _ActionGroupRepo.Delete(ids.Split(','));
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

        public async Task<MSSResult> GetByID(int id)
        {
            MSSResult mRet = new MSSResult();
            try
            {
                if (id == 0)
                {
                    mRet.code = (int)ErrType.ErrParm;
                    mRet.msg = "参数不正确，id不可为0";
                    return mRet;
                }
                mRet.data = await _ActionGroupRepo.GetByID(id);
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

        public async Task<MSSResult> GetAll()
        {
            MSSResult mRet = new MSSResult();
            try
            {
                mRet.data = await _ActionGroupRepo.GetAll();
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
    }
}
