using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.Data
{
    public abstract class BaseEntity
    {
        public int id { get; set; }
        public DateTime created_time { get; set; }
        public int created_by { get; set; }
        public DateTime updated_time { get; set; }
        public int updated_by { get; set; }
    }

    public abstract class BaseQueryParm
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 每页显示行数
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// asc/desc:顺序/降序
        /// </summary>
        public string order { get; set; }
    }
}
