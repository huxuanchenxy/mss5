﻿using Dapper.FluentMap.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSS.API.Model.Data
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }

        public bool IsDel { get; set; }
    }

    public class BaseEntityMap : EntityMap<BaseEntity>
    {
        public BaseEntityMap()
        {
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.IsDel).ToColumn("is_del");
        }
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
