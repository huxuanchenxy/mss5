using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Dapper.FluentMap.Mapping;
namespace MSS.API.Model.Data
{
    public class OrgTree:BaseEntity
    {
        public int? ParentID { get; set; }
        public string Name { get; set; }
        public int NodeType { get; set; }
        public List<OrgNodeProperty> PropEx { get; set; }
    }

    public class OrgTreeMap : EntityMap<OrgTree>
    {
        public OrgTreeMap()
        {
            Map(o => o.ParentID).ToColumn("parent_id");
            Map(o => o.NodeType).ToColumn("node_type");
            Map(o => o.PropEx).Ignore();
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.IsDel).ToColumn("is_del");
        }
    }
}
