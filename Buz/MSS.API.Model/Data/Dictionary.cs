using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.Data
{
    public class Dictionary:BaseEntity
    {
        public string code { get; set; }
        public string code_name { get; set; }
        public int sub_code { get; set; }
        public string sub_code_name { get; set; }
        public string description { get; set; }
    }

    public class cascader
    {
        public int value { get; set; }
        public string label { get; set; }
        public List<cascader> children { get; set; }
    }
}
