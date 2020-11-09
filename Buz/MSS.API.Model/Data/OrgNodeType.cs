using System;
using System.Collections.Generic;
using System.Text;
using Dapper.FluentMap.Mapping;
namespace MSS.API.Model.Data
{
    public class OrgNodeType : BaseEntity
    {
        public string TypeName { get; set; }
        public bool HasChildren { get; set;}
        public bool HasUsers { get; set; }
        public bool HasUsersLeafOnly { get; set; }
    }

    public class OrgNodeTypeMap : EntityMap<OrgNodeType>
    {
        public OrgNodeTypeMap()
        {
            Map(o => o.TypeName).ToColumn("type_name");
            Map(o => o.HasChildren).ToColumn("has_children");
            Map(o => o.HasUsers).ToColumn("has_users");
            Map(o => o.HasUsersLeafOnly).ToColumn("has_users_leafonly");


            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.IsDel).ToColumn("is_del");
        }
    }
}
