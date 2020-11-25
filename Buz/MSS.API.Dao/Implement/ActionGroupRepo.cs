using Dapper;
using MSS.API.Dao.Interface;
using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Dao.Implement
{
    public class ActionGroupRepo : BaseRepo, IActionGroupRepo<ActionGroup>
    {
        public ActionGroupRepo(DapperOptions options) : base(options) { }

        public async Task<MSSResult<ActionGroupView>> GetPageByParm(ActionGroupQueryParm parm)
        {
            return await WithConnection(async c =>
            {
                MSSResult<ActionGroupView> mRet = new MSSResult<ActionGroupView>();
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT a.*,u1.user_name as created_name,u2.user_name as updated_name,d.name as group_type_name ")
                .Append(" FROM Action_Group a ")
                .Append(" left join user u1 on a.created_by=u1.id left join user u2 on a.updated_by=u2.id ")
                .Append(" left join dictionary_tree d on a.group_type=d.id ")
                .Append(" WHERE 1=1 ");
                StringBuilder whereSql = new StringBuilder();
                if (parm.searchType!=null)
                {
                    whereSql.Append(" and a.group_type="+ parm.searchType);
                }
                if (!string.IsNullOrWhiteSpace(parm.searchName))
                {
                    whereSql.Append(" and a.group_name like '%" + parm.searchName + "%' ");
                }
                sql.Append(whereSql)
                .Append(" order by a." + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var tmp = await c.QueryAsync<ActionGroupView>(sql.ToString());
                if (tmp != null)
                {
                    mRet.data = tmp.ToList();
                }
                mRet.relatedData = await c.QueryFirstOrDefaultAsync<int>(
                    "SELECT count(*) FROM Action_Group a where 1=1 "+whereSql.ToString());
                return mRet;
            });
        }


        public async Task<int> Add(ActionGroup actionGroup)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" insert into Action_Group " +
                    " values (0,@group_name,@request_url,@group_type,@group_order,@icon, " +
                    " @active_icon,@CreatedTime,@CreatedBy,@UpdatedTime,@UpdatedBy) ", actionGroup);
                return result;
            });
        }

        public async Task<int> Update(ActionGroup actionGroup)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" UPDATE Action_Group " +
                    " set group_name=@group_name,request_url=@request_url,group_type=@group_type,group_order=@group_order,icon=@icon, " +
                    " active_icon=@active_icon,updated_time=@UpdatedTime,updated_by=@UpdatedBy where id=@id", actionGroup);
                return result;
            });
        }

        public async Task<List<ActionGroup>> GetAll()
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<ActionGroup>(" SELECT * FROM action_group ")).ToList();
                return result;
            });
        }

        public async Task<ActionGroup> GetByID(int id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<ActionGroup>(
                    " SELECT * FROM Action_Group WHERE id = @id ", new { id = id });
                return result;
            });
        }

        public async Task<int> Delete(string[] ids)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" Delete FROM Action_Group WHERE id in @ids ", new { ids = ids });
                return result;
            });
        }
    }
}
