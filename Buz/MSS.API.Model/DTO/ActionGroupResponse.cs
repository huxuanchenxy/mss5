using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.DTO
{
    public class ActionGroupView:ActionGroup
    {
        public string created_name { get; set; }
        public string updated_name { get; set; }
        public string group_type_name { get; set; }
    }
}
