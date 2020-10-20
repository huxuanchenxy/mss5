using System;
using System.Collections.Generic;
using System.Text;
using MSS.API.Dao.Interface;
using MSS.API.Dao;
using MSS.API.Model.Data;
using System.Threading.Tasks;
using Dapper;
using MSS.API.Model.DTO;
using static MSS.API.Common.Utility.Const;
using System.Linq;
using System.Data;

namespace MSS.API.Dao.Implement
{
    public class RoleRepo : BaseRepo, IRoleRepo<Role>
    {
        public RoleRepo(DapperOptions options) : base(options) { }

        public async Task<MSSResult<RoleView>> GetPageByParm(RoleQueryParm parm)
        {
            return await WithConnection(async c =>
            {
                MSSResult<RoleView> mRet = new MSSResult<RoleView>();
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT DISTINCT a.*,u1.user_name as created_name,u2.user_name as updated_name ")
                .Append(" FROM Role a ")
                .Append(" left join user u1 on a.created_by=u1.id left join user u2 on a.updated_by=u2.id ")
                .Append(" left join role_action ra on a.id=ra.role_id ")
                .Append(" left join action_info act on act.id=ra.action_id ")
                .Append(" WHERE 1=1 ");
                StringBuilder whereSql = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(parm.searchName))
                {
                    whereSql.Append(" and a.role_name like '%"+ parm.searchName + "%' ");
                }
                if (parm.searchAction != null)
                {
                    whereSql.Append(" and act.id=" + parm.searchAction);
                }
                if (parm.searchActionGroup != null)
                {
                    whereSql.Append(" and act.group_id=" + parm.searchActionGroup);
                }
                sql.Append(whereSql)
                .Append(" order by a." + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var tmp = await c.QueryAsync<RoleView>(sql.ToString());
                if (tmp!=null)
                {
                    mRet.data = tmp.ToList();
                }
                mRet.relatedData = await c.QueryFirstOrDefaultAsync<int>(
                    "select count(DISTINCT a.id) from role a " +
                    " left join role_action ra on a.id = ra.role_id " +
                    " left join action_info act on act.id=ra.action_id where 1=1 " + whereSql.ToString());
                return mRet;
            });
        }
        public async Task<Role> GetByID(int id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<Role>(
                    "SELECT * FROM Role WHERE id = @id", new { id = id });
                return result;
            });
        }

        public async Task<bool> IsNameRepeat(string name)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<int>(
                    "SELECT count(*) FROM Role WHERE role_name = @name", new { name = name });
                return result>0?true:false;
            });
        }

        public async Task<int> Add(RoleStrActions roleStrActions)
        {
            return await WithConnection(async c =>
            {
                StringBuilder sql = new StringBuilder();
                IDbTransaction trans = c.BeginTransaction();
                try
                {
                    sql.Append(" insert into Role ")
                    .Append(" values (0,@role_name,@description, ")
                    .Append(" @created_time,@created_by,@updated_time,@updated_by); ")
                    .Append("select last_insert_id()");
                    int result = await c.QueryFirstOrDefaultAsync<int>(sql.ToString(), roleStrActions, trans);
                    foreach (string item in roleStrActions.actions.Split(','))
                    {
                        sql.Clear();
                        sql.Append(" insert into Role_Action ")
                        .Append(" values (0," + result + "," + item + ") ");
                        await c.ExecuteAsync(sql.ToString(), trans);
                    }
                    trans.Commit();
                    return result;
                }
                catch(Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.ToString());
                }
            });
        }

        public async Task<int> Update(RoleStrActions roleStrActions)
        {
            return await WithConnection(async c =>
            {
                StringBuilder sql = new StringBuilder();
                IDbTransaction trans = c.BeginTransaction();
                try
                {
                    sql.Append(" update Role ")
                    .Append(" set role_name=@role_name,description=@description, ")
                    .Append(" updated_time=@updated_time,updated_by=@updated_by where id=@id ");
                    int result = await c.ExecuteAsync(sql.ToString(), roleStrActions, trans);
                    sql.Clear().Append("delete from Role_Action where role_id=@id");
                    await c.ExecuteAsync(sql.ToString(), roleStrActions, trans);
                    foreach (string item in roleStrActions.actions.Split(','))
                    {
                        sql.Clear()
                        .Append(" insert into Role_Action ")
                        .Append(" values (0," + roleStrActions.Id + "," + item + ") ");
                        await c.ExecuteAsync(sql.ToString(), trans);
                    }
                    trans.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.ToString());
                }
            });
        }

        public async Task<int> Delete(string[] ids)
        {
            return await WithConnection(async c =>
            {
                StringBuilder sql = new StringBuilder();
                IDbTransaction trans = c.BeginTransaction();
                try
                {
                    int result = await c.ExecuteAsync(" Delete from Role WHERE id in @ids ", new { ids = ids },trans);
                    await c.ExecuteAsync(" Delete from Role_Action WHERE role_id in @ids ", new { ids = ids },trans);
                    trans.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.ToString());
                }
            });
        }

        public async Task<List<Role>> GetAll()
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<Role>("select * from Role")).ToList();
                return result;
            });
        }

        public async Task<List<RoleAction>> GetRoleActionAll()
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<RoleAction>("select * from role_action")).ToList();
                return result;
            });
        }

    }
}
