using System.Collections.Generic;
using System.Text;
using MSS.API.Dao.Interface;
using MSS.API.Model.Data;
using System.Threading.Tasks;
using Dapper;
using MSS.API.Model.DTO;
using static MSS.API.Common.Utility.Const;
using System.Linq;

namespace MSS.API.Dao.Implement
{
    public class ActionRepo : BaseRepo, IActionRepo<ActionInfo>
    {
        public ActionRepo(DapperOptions options) : base(options) { }

        public async Task<MSSResult<ActionView>> GetPageByParm(ActionQueryParm parm)
        {
            return await WithConnection(async c =>
            {
                MSSResult<ActionView> mRet = new MSSResult<ActionView>();
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.*,u1.user_name as created_name,u2.user_name as updated_name,ag.group_name, ")
                .Append(" ai.action_name as parent_name FROM Action_Info a ")
                .Append(" left join user u1 on a.created_by=u1.id left join user u2 on a.updated_by=u2.id ")
                .Append(" left join Action_Group ag on ag.id=a.group_id ")
                .Append(" left join Action_Info ai on ai.id=a.parent_menu ")
                .Append(" where 1=1 ");
                StringBuilder whereSql = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(parm.searchName))
                {
                    whereSql.Append(" and a.action_name like '%"+ parm.searchName + "%' ");
                }
                if (parm.searchGroup!=null)
                {
                    whereSql.Append(" and a.group_id="+ parm.searchGroup);
                }
                if (parm.searchParent != null)
                {
                    whereSql.Append(" and a.parent_menu=" + parm.searchParent);
                }
                sql.Append(whereSql)
                .Append(" order by a." + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var tmp = await c.QueryAsync<ActionView>(sql.ToString());
                if (tmp != null)
                {
                    mRet.data = tmp.ToList();
                }
                mRet.relatedData = await c.QueryFirstOrDefaultAsync<int>(
                    "select count(*) from Action_Info a where 1=1 "+whereSql.ToString());
                return mRet;
            });
        }
        public async Task<ActionInfo> GetByID(int id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<ActionInfo>(
                    "SELECT * FROM Action_Info WHERE id = @id", new { id = id });
                return result;
            });
        }

        public async Task<int> Add(ActionInfo action)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" insert into Action_Info " +
                    " values (0,@request_url,@action_name,@description,@action_order,@icon, " +
                    " @level,@group_id,@parent_menu, " +
                    " @created_time,@created_by,@updated_time,@updated_by) ", action);
                return result;
            });
        }

        public async Task<int> Update(ActionInfo action)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" update Action_Info " +
                    " set action_name=@action_name,request_url=@request_url,description=@description,action_order=@action_order,icon=@icon, " +
                    " group_id=@group_id,parent_menu=@parent_menu, " +//level=@level,
                    " updated_time=@updated_time,updated_by=@updated_by where id=@id", action);
                return result;
            });
        }

        public async Task<int> Delete(string[] ids)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" Delete from Action_Info WHERE id in @ids ", new { ids = ids });
                return result;
            });
        }

        public async Task<List<ActionInfo>> GetAll()
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<ActionInfo>("select * from Action_Info")).ToList();
                return result;
            });
        }

        public async Task<List<ActionInfo>> GetMenu()
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<ActionInfo>("select * from Action_Info where group_id>0")).ToList();
                return result;
            });
        }

        public async Task<List<ActionInfo>> GetByActionGroup(string[] groupIDs)
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<ActionInfo>(
                    "select * from Action_Info where group_id in @groupIDs",new { groupIDs= groupIDs })).ToList();
                return result;
            });
        }

        public async Task<List<ActionAll>> GetActionAll()
        {
            return await WithConnection(async c =>
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT a.id as ActionID,a.action_name as ActionName,a.request_url as ActionURL,a.Action_Order as ActionOrder,a.Icon as ActionIcon,")
                  .Append(" a.parent_menu as ParentMenu,a.group_id as GroupID, ag.Group_Name as GroupName, ag.Request_Url as GroupURL, ag.Icon as GroupIcon,")
                  .Append(" ag.active_icon as GroupActiveIcon,ag.Group_Order as GroupOrder, a.level as Level FROM Action_Info a")
                  .Append(" left join Action_Group ag on a.group_id = ag.id")
                  //.Append(" where a.level in ("+ACTION_LEVEL.AllowSelection+","+ACTION_LEVEL.NotAllowAll)
                  .Append(" order by ag.Group_Order");
                var result = (await c.QueryAsync<ActionAll>(sql.ToString())).ToList();
                return result;
            });
        }

        public async Task<List<ActionAll>> GetActionByUser(int userID)
        {
            return await WithConnection(async c =>
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT a.id as ActionID,a.action_name as ActionName,a.request_url as ActionURL,a.Action_Order as ActionOrder,a.Icon as ActionIcon,")
                  .Append(" a.parent_menu as ParentMenu,a.group_id as GroupID, ag.Group_Name as GroupName, ag.Request_Url as GroupURL, ag.Icon as GroupIcon,")
                  .Append(" ag.active_icon as GroupActiveIcon,a.level as Level,ag.group_order as GroupOrder")
                  .Append(" FROM Action_Info a")
                  .Append(" left join Action_Group ag on a.group_id = ag.id")
                  .Append(" right JOIN role_action ra on ra.action_id=a.id")
                  .Append(" right JOIN user u on u.role_id=ra.role_id")
                  .Append(" where u.id="+userID+"")
                  .Append(" order by ag.Group_Order");
                var result = (await c.QueryAsync<ActionAll>(sql.ToString())).ToList();
                return result;
            });
        }

        public async Task<List<RoleActions>> GetActionByRoles(int[] roleIDs)
        {
            return await WithConnection(async c =>
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT a.id as ActionID,a.action_name as ActionName,a.request_url as ActionURL,a.Action_Order as ActionOrder,a.Icon as ActionIcon,")
                  .Append(" a.parent_menu as ParentMenu,a.group_id as GroupID, ag.Group_Name as GroupName, ag.Request_Url as GroupURL, ag.Icon as GroupIcon,")
                  .Append(" a.level as Level,ra.role_id as roleID FROM Action_Info a")
                  .Append(" left join Action_Group ag on a.group_id = ag.id")
                  .Append(" right JOIN role_action ra on ra.action_id=a.id")
                  .Append(" where ra.role_id in @ids ")
                  .Append(" order by ag.Group_Order");
                var result = (await c.QueryAsync<RoleActions>(sql.ToString(),new {ids=roleIDs } )).ToList();
                return result;
            });
        }

        public async Task<List<int>> GetActionIDByRoleID(int roleID)
        {
            return await WithConnection(async c =>
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT action_id from role_action where role_id=@id");
                var result = (await c.QueryAsync<int>(sql.ToString(), new { id = roleID })).ToList();
                return result;
            });
        }

    }
}
