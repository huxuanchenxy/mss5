using System;
using System.Collections.Generic;
using System.Text;
using Dapper.FluentMap.Mapping;
namespace MSS.API.Model.Data
{
    public class OrgUser:BaseEntity
    {
        public int NodeID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }

    public class OrgUserMap : EntityMap<OrgUser>
    {
        public OrgUserMap()
        {
            Map(o => o.NodeID).ToColumn("org_node_id");
            Map(o => o.UserID).ToColumn("user_id");
            Map(o => o.UserName).ToColumn("user_name");

            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.IsDel).ToColumn("is_del");
        }
    }
}
