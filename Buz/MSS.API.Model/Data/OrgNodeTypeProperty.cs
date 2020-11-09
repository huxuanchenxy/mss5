using System;
using System.Collections.Generic;
using System.Text;
using Dapper.FluentMap.Mapping;
namespace MSS.API.Model.Data
{
    public class OrgNodeTypeProperty : BaseEntity
    {
        public int NodeTypeID { get; set; }
        public string NodeAttr { get; set; }
        public string AttrDesc { get; set; }
    }

    public class OrgNodeTypePropertyMap : EntityMap<OrgNodeTypeProperty>
    {
        public OrgNodeTypePropertyMap()
        {
            Map(o => o.NodeTypeID).ToColumn("node_type_id");
            Map(o => o.NodeAttr).ToColumn("node_attr");
            Map(o => o.AttrDesc).ToColumn("attr_desc");

            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.IsDel).ToColumn("is_del");
        }
    }
}
