using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.Data
{
    public class ActionInfo : BaseEntity
    {
        public string action_name { get; set; }
        public string request_url { get; set; }

        public string description { get; set; }

        public int action_order { get; set; }

        public string icon { get; set; }

        public int level { get; set; }

        public int? group_id { get; set; }

        public int? parent_menu { get; set; }
    }
}
