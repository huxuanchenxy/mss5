
using Dapper.FluentMap.Mapping;
using MSS.API.Model.Data;
using System.Collections.Generic;

// Coded by admin 2020/10/15 8:54:19
namespace MSS.API.Model.Data
{
    public class UserParm : BaseQueryParm
    {
        public string UserName { get; set; }
        public string JobNumber { get; set; }
    }
    public class UserPageView
    {
        public List<User> rows { get; set; }
        public int total { get; set; }
    }

    public class User : BaseEntity
    {
        public string AccName { get; set; }
        public string Password { get; set; }
        public int RandomNum { get; set; }
        public string UserName { get; set; }
        public string JobNumber { get; set; }
        public int RoleId { get; set; }
        public int Age { get; set; }
        public string Nation { get; set; }
        public string Nativeplace { get; set; }
        public string Edu { get; set; }
        public string JobTitle { get; set; }
        public string Position { get; set; }
        public string IdCard { get; set; }
        public System.DateTime Birth { get; set; }
        public int Sex { get; set; }
        public string Mobile { get; set; }
        public string MobileShort { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string IdPhoto { get; set; }
        public int OutMan { get; set; }
        public bool IsSuper { get; set; }
        public string CreatedName { get; set; }
        public string UpdatedName { get; set; }

        public List<UserWorkType> WorkType { get; set; }
    }

    public class UserWorkType
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
    }

    public class UserMap : EntityMap<User>
    {
        public UserMap()
        {
            Map(o => o.Id).ToColumn("id");
            Map(o => o.AccName).ToColumn("acc_name");
            Map(o => o.Password).ToColumn("password");
            Map(o => o.RandomNum).ToColumn("random_num");
            Map(o => o.UserName).ToColumn("user_name");
            Map(o => o.JobNumber).ToColumn("job_number");
            Map(o => o.RoleId).ToColumn("role_id");
            Map(o => o.Age).ToColumn("age");
            Map(o => o.Nation).ToColumn("nation");
            Map(o => o.Nativeplace).ToColumn("nativeplace");
            Map(o => o.Edu).ToColumn("edu");
            Map(o => o.JobTitle).ToColumn("job_title");
            Map(o => o.WorkType).ToColumn("work_type");
            Map(o => o.Position).ToColumn("position");
            Map(o => o.IdCard).ToColumn("id_card");
            Map(o => o.Birth).ToColumn("birth");
            Map(o => o.Sex).ToColumn("sex");
            Map(o => o.Mobile).ToColumn("mobile");
            Map(o => o.MobileShort).ToColumn("mobile_short");
            Map(o => o.Email).ToColumn("email");
            Map(o => o.Address).ToColumn("address");
            Map(o => o.IdPhoto).ToColumn("id_photo");
            Map(o => o.OutMan).ToColumn("out_man");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.IsDel).ToColumn("is_del");
            Map(o => o.IsSuper).ToColumn("is_super");
            Map(o => o.CreatedName).ToColumn("created_name");
            Map(o => o.UpdatedName).ToColumn("updated_name");
        }
    }

}