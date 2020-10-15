using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.DTO
{
    public class RoleQueryParm : BaseQueryParm
    {
        /// <summary>
        /// role_name模糊查询
        /// </summary>
        public string searchName { get; set; }
        /// <summary>
        /// action(级联)下拉查询(非一级)
        /// </summary>
        public int? searchAction { get; set; }
        /// <summary>
        /// action(级联)下拉查询(一级)
        /// </summary>
        public int? searchActionGroup { get; set; }
        
    }

    public class RoleStrActions : Role
    {
        /// <summary>
        /// 用逗号分隔的actionID
        /// </summary>
        public string actions { get; set; }
    }

}
