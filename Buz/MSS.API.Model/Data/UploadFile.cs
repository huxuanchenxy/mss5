using Dapper.FluentMap.Mapping;

namespace MSS.API.Model.Data
{
    public class UploadFile
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public int SystemResource { get; set; }
        public string SystemResourceName { get; set; }
        public int Entity { get; set; }

    }

    public class UploadFileMap : EntityMap<UploadFile>
    {
        public UploadFileMap()
        {
            Map(o => o.ID).ToColumn("id");
            Map(o => o.FileName).ToColumn("file_name");
            Map(o => o.FilePath).ToColumn("file_path");
            Map(o => o.SystemResource).ToColumn("system_resource");
            Map(o => o.SystemResourceName).ToColumn("resourceName");
            Map(o => o.Type).ToColumn("type");
            Map(o => o.TypeName).ToColumn("name");
            Map(o => o.Entity).ToColumn("entity_id");
        }
    }

    public class UploadFileRelation
    {
        public int ID { get; set; }
        public int Entity { get; set; }
        public int File { get; set; }
        public int Type { get; set; }
        public int SystemResource { get; set; }
    }

    public class UploadFileRelationMap : EntityMap<UploadFileRelation>
    {
        public UploadFileRelationMap()
        {
            Map(o => o.File).ToColumn("file_id");
            Map(o => o.Entity).ToColumn("entity_id");
            Map(o => o.Type).ToColumn("type");
            Map(o => o.SystemResource).ToColumn("system_resource");
        }
    }
}
