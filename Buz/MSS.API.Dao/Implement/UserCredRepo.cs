using Dapper;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSS.API.Dao.Interface;
using MSS.API.Model.Data;


// Coded By admin 2020/11/5 13:29:58
namespace MSS.API.Dao.Implement
{
    public class UserCredRepo : BaseRepo, IUserCredRepo<UserCred>
    {
        public UserCredRepo(DapperOptions options) : base(options) { }

        public async Task<UserCredPageView> GetPageList(UserCredParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@"  SELECT 
                id,
                user_id,
                endowment,
                endowment_type,
                endowment_level,
                active_time,
                dead_time,
                created_time,
                created_by,
                updated_time,
                updated_by,
                is_del,is_super FROM user_cred
                 ");
                StringBuilder whereSql = new StringBuilder();
                //whereSql.Append(" WHERE ai.ProcessInstanceID = '" + parm.ProcessInstanceID + "'");

                //if (parm.AppName != null)
                //{
                //    whereSql.Append(" and ai.AppName like '%" + parm.AppName.Trim() + "%'");
                //}

                sql.Append(whereSql);
                //验证是否有参与到流程中
                //string sqlcheck = sql.ToString();
                //sqlcheck += ("AND ai.CreatedByUserID = '" + parm.UserID + "'");
                //var checkdata = await c.QueryFirstOrDefaultAsync<TaskViewModel>(sqlcheck);
                //if (checkdata == null)
                //{
                //    return null;
                //}

                var data = await c.QueryAsync<UserCred>(sql.ToString());
                var total = data.ToList().Count;
                sql.Append(" order by " + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var ets = await c.QueryAsync<UserCred>(sql.ToString());

                UserCredPageView ret = new UserCredPageView();
                ret.rows = ets.ToList();
                ret.total = total;
                return ret;
            });
        }

        public async Task<UserCred> Save(UserCred obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO `user_cred`(
                    
                    user_id,
                    endowment,
                    endowment_type,
                    endowment_level,
                    active_time,
                    dead_time,
                    created_time,
                    created_by,
                    updated_time,
                    updated_by,
                    is_del,
                    is_super
                ) VALUES 
                (
                    @UserId,
                    @Endowment,
                    @EndowmentType,
                    @EndowmentLevel,
                    @ActiveTime,
                    @DeadTime,
                    @CreatedTime,
                    @CreatedBy,
                    @UpdatedTime,
                    @UpdatedBy,
                    @IsDel,
                    @IsSuper
                    );
                    ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                obj.Id = newid;
                return obj;
            });
        }

        public async Task<UserCred> GetByID(long id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<UserCred>(
                    "SELECT * FROM user_cred WHERE id = @id", new { id = id });
                return result;
            });
        }

        public async Task<int> Update(UserCred obj)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync($@" UPDATE user_cred set 
                    
                    user_id=@UserId,
                    endowment=@Endowment,
                    endowment_type=@EndowmentType,
                    endowment_level=@EndowmentLevel,
                    active_time=@ActiveTime,
                    dead_time=@DeadTime,
                    created_time=@CreatedTime,
                    created_by=@CreatedBy,
                    updated_time=@UpdatedTime,
                    updated_by=@UpdatedBy,
                    is_del=@IsDel,
                    is_super=@IsSuper
                 where id=@Id", obj);
                return result;
            });
        }

        public async Task<int> Delete(string[] ids, int userID)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" Update user_cred set is_del=1" +
                ",updated_time=@updated_time,updated_by=@updated_by" +
                " WHERE id in @ids ", new { ids = ids, updated_time = DateTime.Now, updated_by = userID });
                return result;
            });
        }
    }
}



