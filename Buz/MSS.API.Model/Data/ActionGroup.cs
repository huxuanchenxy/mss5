using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.Data
{
    public class ActionGroup : BaseEntity
    {
        public string group_name { get; set; }
        public string request_url { get; set; }
        public string icon { get; set; }
        public string active_icon { get; set; }
        public int group_type { get; set; }
        public int group_order { get; set; }

    }

    public class ActionGroupMap : BaseEntityMap
    {
        public ActionGroupMap() : base() { }
    }
}
