﻿using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.DTO
{
    public class ActionGroupQueryParm : BaseQueryParm
    {
        /// <summary>
        /// group_name模糊查询
        /// </summary>
        public string searchName { get; set; }
        /// <summary>
        /// group_type下拉查询
        /// </summary>
        public int? searchType { get; set; }
    }
}
