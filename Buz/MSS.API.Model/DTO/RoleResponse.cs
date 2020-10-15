using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.DTO
{
    public class RoleView:Role
    {
        public string created_name { get; set; }
        public string updated_name { get; set; }

        public List<ActionTree> action_trees { get; set; }
    }

    public class RoleActions : ActionAll
    {
        public int roleID { get; set; }
    }
}
