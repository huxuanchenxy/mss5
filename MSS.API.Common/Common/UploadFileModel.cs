using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
namespace MSS.API.Common
{
    public class UploadFileModel
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

}
