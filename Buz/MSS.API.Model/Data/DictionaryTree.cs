using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.Data
{
    public class DictionaryTree:BaseEntity
    {
        public int parent_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string des { get; set; }
    }
    public class DictionaryRelation
    {
        public int id { get; set; }
        public string parent_business_type { get; set; }
        public string business_type { get; set; }
        public string name { get; set; }
        public string ext { get; set; }
    }
}
