using System;
using System.Collections.Generic;
using System.Text;
using Dapper.FluentMap.Mapping;
namespace MSS.API.Model.Data
{
    public class OrgNodeProperty:BaseEntity
    {
        public int NodeID { get; set; }
        public string NodeAttr { get; set; }
        public string AttrValue { get; set; }
    }

    public class OrgNodePropertyMap : EntityMap<OrgNodeProperty>
    {
        public OrgNodePropertyMap()
        {
            Map(o => o.NodeID).ToColumn("node_id");
            Map(o => o.NodeAttr).ToColumn("node_attr");
            Map(o => o.AttrValue).ToColumn("attr_value");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.IsDel).ToColumn("is_del");
        }
    }
}
