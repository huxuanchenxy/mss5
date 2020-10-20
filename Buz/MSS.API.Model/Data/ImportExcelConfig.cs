using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Dapper.FluentMap.Mapping;
namespace MSS.API.Model.Data
{
    public class ImportExcelConfig : BaseEntity
    {
        public string FileName { get; set; }
        public int ClassID { get; set; }
        public string ClassShow { get; set; }
        public string Field { get; set; }
        public string Config { get; set; }
        public string Required { get; set; }
        public string CreatedName { get; set; }
        public string UpdatedName { get; set; }

    }

    public class ImportExcelConfigMap : EntityMap<ImportExcelConfig>
    {
        public ImportExcelConfigMap()
        {
            Map(o => o.FileName).ToColumn("file_name");
            Map(o => o.ClassID).ToColumn("class");
            Map(o => o.ClassShow).ToColumn("show_name");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedName).ToColumn("created_name");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.UpdatedName).ToColumn("updated_name");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
        }
    }


    public class ImportExcelConfigParm : BaseQueryParm
    {
        public string SearchName { get; set; }
        public int? SearchClass { get; set; }
    }

    public class ImportExcelConfigView
    {
        public List<ImportExcelConfig> rows { get; set; }
        public int total { get; set; }
    }

    public class ImportExcelClass
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string AssemblyName { get; set; }
        public string ShowName { get; set; }
    }
    public class ImportExcelClassMap : EntityMap<ImportExcelClass>
    {
        public ImportExcelClassMap()
        {
            Map(o => o.FullName).ToColumn("full_name");
            Map(o => o.AssemblyName).ToColumn("assembly_name");
            Map(o => o.ShowName).ToColumn("show_name");
        }
    }

    public class ImportExcelLogParm : BaseQueryParm
    {
        public string FileName { get; set; }
    }
}
