using Dapper;
using MSS.API.Model.Data;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSS.API.Dao.Interface;
using System.Collections.Generic;
using static MSS.API.Common.Utility.Const;
using MSS.API.Common.Utility;

// Coded By admin 2020/10/15 9:06:38
namespace MSS.API.Dao.Implement
{
    public class UserRepo : BaseRepo, IUserRepo<User>
    {
        public UserRepo(DapperOptions options) : base(options) { }

        public async Task<UserPageView> GetPageList(UserParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@"  SELECT 
                a.id,
                a.acc_name,
                a.password,
                a.random_num,
                a.user_name,
                a.job_number,
                a.role_id,
                a.age,
                a.nation,
                a.nativeplace,
                a.edu,
                a.job_title,
                a.position,
                a.id_card,
                a.birth,
                a.sex,
                a.mobile,
                a.mobile_short,
                a.email,
                a.address,
                a.id_photo,
                a.out_man,
                a.created_time,
                a.created_by,
                a.updated_time,
                a.updated_by,
                a.is_del,
                a.is_super,
                b.`user_name` AS created_name,
                c.`user_name` AS updated_name
                 FROM USER a 
                LEFT JOIN USER b  ON a.`created_by` = b.id
                LEFT JOIN USER c ON a.`updated_by`= c.id
                 ");

                sql.Append(" WHERE a.is_del = 0 ");
                
                if (!string.IsNullOrEmpty(parm.UserName))
                {
                    sql.Append(" and a.user_name like '%" + parm.UserName.Trim() + "%' " );
                }
                if (!string.IsNullOrEmpty(parm.JobNumber))
                {
                    sql.Append(" and a.job_number like '%" + parm.JobNumber.Trim() + "%' ");
                }
                var data = await c.QueryAsync<User>(sql.ToString());
                var total = data.ToList().Count;

                sql.Append(" order by a." + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var ets = await c.QueryAsync<User>(sql.ToString());
                await GetRefList(ets.ToList());

                UserPageView ret = new UserPageView();
                ret.rows = ets.ToList();
                ret.total = total;
                return ret;
            });
        }

        private async Task GetRefList(List<User> ets)
        {
            var allworktype = await GetWorkType();
            foreach (var e in ets)
            {
                var curWorkType = allworktype.ToList().Where(a => a.UserId == e.Id);
                e.WorkType = curWorkType.ToList();
            }
        }

        public async Task<User> Save(User obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO `user`(
                    acc_name,
                    password,
                    random_num,
                    user_name,
                    job_number,
                    role_id,
                    age,
                    nation,
                    nativeplace,
                    edu,
                    job_title,
                    position,
                    id_card,
                    birth,
                    sex,
                    mobile,
                    mobile_short,
                    email,
                    address,
                    id_photo,
                    out_man,
                    created_time,
                    created_by,
                    updated_time,
                    updated_by,
                    is_del,
                    is_super
                ) VALUES 
                (
                    @AccName,
                    @Password,
                    @RandomNum,
                    @UserName,
                    @JobNumber,
                    @RoleId,
                    @Age,
                    @Nation,
                    @Nativeplace,
                    @Edu,
                    @JobTitle,
                    @Position,
                    @IdCard,
                    @Birth,
                    @Sex,
                    @Mobile,
                    @MobileShort,
                    @Email,
                    @Address,
                    @IdPhoto,
                    @OutMan,
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

                foreach (var o in obj.WorkType)
                {
                    o.UserId = obj.Id;
                    await SaveWorkType(o);
                }
                return obj;
            });
        }

        public async Task<User> GetByID(int id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM user WHERE id = @id", new { id = id });
                var list = await GetWorkType(id);
                if (list != null && list.Count != 0)
                {
                    result.WorkType = list;
                }
                return result;
            });
        }

        public async Task<int> Update(User obj)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync($@" UPDATE user set 
                    user_name=@UserName,
                    job_number=@JobNumber,
                    role_id=@RoleId,
                    age=@Age,
                    nation=@Nation,
                    nativeplace=@Nativeplace,
                    edu=@Edu,
                    job_title=@JobTitle,
                    position=@Position,
                    id_card=@IdCard,
                    birth=@Birth,
                    sex=@Sex,
                    mobile=@Mobile,
                    mobile_short=@MobileShort,
                    email=@Email,
                    address=@Address,
                    id_photo=@IdPhoto,
                    out_man=@OutMan,
                    created_time=@CreatedTime,
                    created_by=@CreatedBy,
                    updated_time=@UpdatedTime,
                    updated_by=@UpdatedBy,
                    is_del=@IsDel,
                    is_super=@IsSuper
                 where id=@Id", obj);

                await DeleteWorkType(obj.Id);
                foreach (var o in obj.WorkType)
                {
                    o.UserId = obj.Id;
                    await SaveWorkType(o);
                }
                return result;
            });
        }

        public async Task<int> Delete(string[] ids, int userID)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" Update user set is_del=1" +
                ",updated_time=@updated_time,updated_by=@updated_by" +
                " WHERE id in @ids ", new { ids = ids, updated_time = DateTime.Now, updated_by = userID });
                return result;
            });
        }

        public async Task<User> GetByAcc(string acc)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<User>(
                    " SELECT * FROM User WHERE acc_name = @acc", new { acc = acc });
                return result;
            });
        }

        public async Task<int> GetUserCountByRole(string[] ids)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<int>(
                    "SELECT count(*) FROM User WHERE role_id in @ids", new { ids = ids });
                return result;
            });
        }

        public async Task<bool> IsInOrg(string[] ids)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<int>(" select count(*) from org_user" +
                    " WHERE user_id in @ids ", new { ids = ids });
                return result > 0;
            });
        }

        public async Task<List<User>> GetAll()
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<User>(
                    "select * from user where is_del=" + (int)IsDeleted.no + " and is_super=" + (int)IsSuper.no)).ToList();
                await GetRefList(result.ToList());
                return result;
            });
        }

        public async Task<List<User>> GetAllContainSuper()
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<User>(
                    "select * from user where is_del=" + (int)IsDeleted.no)).ToList();
                return result;
            });
        }


        public async Task<int> ChangePwd(User user)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" update user set password=@Password," +
                    " random_num=@RandomNum,updated_time=@UpdatedTime,updated_by=@UpdatedBy where id=@id", user);
                return result;
            });
        }

        public async Task<int> ResetPwd(string[] ids, int userID)
        {
            return await WithConnection(async c =>
            {
                Encrypt en = new Encrypt();
                int r = new Random().Next(1, PWD_RANDOM_MAX);
                string pwd = en.DoEncrypt(INIT_PASSWORD, r);
                var result = await c.ExecuteAsync(" update user " +
                    " set password=@Password,random_num=@RandomNum,updated_time=@UpdatedTime,updated_by=@UpdatedBy where id in @ids",
                    new { Password = pwd, RandomNum = r, UpdatedTime = DateTime.Now, UpdatedBy = userID, ids = ids });
                return result;
            });
        }



        public async Task<UserWorkType> SaveWorkType(UserWorkType obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO `user_work_type`(
                    
                    user_id,
                    work_type_id
                ) VALUES 
                (
                    @UserId,
                    @WorkTypeId
                    );
                    ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                obj.Id = newid;
                return obj;
            });
        }

        public async Task<List<UserWorkType>> GetWorkType(int userid)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryAsync<UserWorkType>(
                    $@" SELECT a.id,a.user_id UserId,
                    a.work_type_id  WorkTypeId,b.name  WorkTypeName FROM user_work_type a 
                    LEFT JOIN dictionary_tree b ON a.work_type_id = b.id 
                    WHERE a.user_id = @user_id ", new { user_id = userid });
                return result.ToList();
            });
        }

        public async Task<List<UserWorkType>> GetWorkType()
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryAsync<UserWorkType>(
                    $@" SELECT a.id,a.user_id UserId,
                    a.work_type_id  WorkTypeId,b.name  WorkTypeName FROM user_work_type a 
                    LEFT JOIN dictionary_tree b ON a.work_type_id = b.id 
                     " );
                return result.ToList();
            });
        }

        public async Task<int> DeleteWorkType(int userid)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync($@" DELETE FROM user_work_type WHERE user_id = @user_id ", new { user_id = userid });
                return result;
            });
        }
    }
}



