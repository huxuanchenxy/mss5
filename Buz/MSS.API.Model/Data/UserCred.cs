
using Dapper.FluentMap.Mapping;
using System.Collections.Generic;

// Coded by admin 2020/11/5 13:28:06
namespace MSS.API.Model.Data
{
    public class UserCredParm : BaseQueryParm
    {

    }
    public class UserCredPageView
    {
        public List<UserCred> rows { get; set; }
        public int total { get; set; }
    }

    public class UserCred : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Endowment { get; set; }
        public int EndowmentType { get; set; }
        public int EndowmentLevel { get; set; }
        public System.DateTime ActiveTime { get; set; }
        public System.DateTime DeadTime { get; set; }
        public sbyte IsSuper { get; set; }
    }

    public class UserCredMap : EntityMap<UserCred>
    {
        public UserCredMap()
        {
            Map(o => o.Id).ToColumn("id");
            Map(o => o.UserId).ToColumn("user_id");
            Map(o => o.Endowment).ToColumn("endowment");
            Map(o => o.EndowmentType).ToColumn("endowment_type");
            Map(o => o.EndowmentLevel).ToColumn("endowment_level");
            Map(o => o.ActiveTime).ToColumn("active_time");
            Map(o => o.DeadTime).ToColumn("dead_time");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.IsDel).ToColumn("is_del");
            Map(o => o.IsSuper).ToColumn("is_super");
        }
    }

}