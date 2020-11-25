
using Dapper.FluentMap.Mapping;
using System.Collections.Generic;

// Coded by admin 2020/11/20 10:10:00
namespace MSS.API.Model.Data
{
    public class DocumentParm : BaseQueryParm
    {

    }
    public class DocumentPageView
    {
        public List<Document> rows { get; set; }
        public int total { get; set; }
    }

    public class Document : BaseEntity
    {
        public string DocName { get; set; }
        public string DocVersion { get; set; }
        public string DocNum { get; set; }
        public int DocType { get; set; }
        public int DocType2 { get; set; }
        public int EqpTypeId { get; set; }
        public System.DateTime ActiveTime { get; set; }
        public System.DateTime DeadTime { get; set; }

    }

    public class DocumentMap : EntityMap<Document>
    {
        public DocumentMap()
        {
            Map(o => o.Id).ToColumn("id");
            Map(o => o.DocName).ToColumn("doc_name");
            Map(o => o.DocVersion).ToColumn("doc_version");
            Map(o => o.DocNum).ToColumn("doc_num");
            Map(o => o.DocType).ToColumn("doc_type");
            Map(o => o.DocType2).ToColumn("doc_type2");
            Map(o => o.EqpTypeId).ToColumn("eqp_type_id");
            Map(o => o.ActiveTime).ToColumn("active_time");
            Map(o => o.DeadTime).ToColumn("dead_time");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.IsDel).ToColumn("is_del");
        }
    }

}