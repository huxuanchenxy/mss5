using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.DTO
{
    public class DictionaryView:Dictionary
    {
        public string created_name { get; set; }
        public string updated_name { get; set; }
    }
}
