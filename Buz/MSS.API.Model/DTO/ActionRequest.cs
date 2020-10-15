using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.DTO
{
    public class ActionQueryParm : BaseQueryParm
    {
        /// <summary>
        /// action_name模糊查询
        /// </summary>
        public string searchName { get; set; }
        /// <summary>
        /// group_type权限组下拉查询
        /// </summary>
        public int? searchGroup { get; set; }
        /// <summary>
        /// parent_menu下拉查询
        /// </summary>
        public int? searchParent { get; set; }
    }
}
