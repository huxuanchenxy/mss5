using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.Data
{
    public class Role : BaseEntity
    {
        public string role_name { get; set; }
        public string description { get; set; }
    }
}
