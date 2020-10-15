using System;
using System.Collections.Generic;

namespace MSS.API.Common.Utility
{
    public static class Const
    {
        # region 常量
        public enum Sex
        {
            man,
            woman
        }

        public enum IsDeleted
        {
            no,
            yes
        }

        public enum IsSuper
        {
            no,
            yes
        }
        #endregion

        # region 初始默认值
        public static int PAGESIZE;

        public static string INIT_PASSWORD;

        public static int PWD_RANDOM_MAX;
        #endregion

        #region 返回值相关
        public enum ErrType
        {
            /// <summary>
            /// 正常
            /// </summary>
            OK,
            /// <summary>
            /// 系统错误，数据库操作或者catch
            /// </summary>
            SystemErr,
            /// <summary>
            /// 不允许重复的字段重复了
            /// </summary>
            Repeat,
            /// <summary>
            /// 有关联，一般用于删除操作
            /// </summary>
            Associated,
            /// <summary>
            /// 一般用于查询数据进行操作时，没有找到对应的记录
            /// </summary>
            NoRecord,
            /// <summary>
            /// 参数不正确
            /// </summary>
            ErrParm,
            /// <summary>
            /// 密码不正确
            /// </summary>
            ErrPwd

        }

        /// <summary>
        /// service返回controller常用统一结构
        /// 如果有特殊需求，还需要特殊处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class MSSResult<T>
        {
            /// <summary>
            /// 查询结果集
            /// </summary>
            public List<T> data { get; set; }
            /// <summary>
            /// 可能的相关数据
            /// </summary>
            public object relatedData { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// ErrType，0表示成功
            /// </summary>
            public int code { get; set; }
        }
        /// <summary>
        /// service返回controller常用统一结构
        /// </summary>
        public class MSSResult
        {
            /// <summary>
            /// 查询结果对象
            /// </summary>
            public object data { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// ErrType，0表示成功
            /// </summary>
            public int code { get; set; }
        }
        #endregion

        #region 字典
        #region 访问action权限
        public const string STR_ACTION_LEVEL = "action_level";
        public enum ACTION_LEVEL
        {
            /// <summary>
            /// 允许用户勾选
            /// </summary>
            AllowSelection,
            /// <summary>
            /// 所有用户都具有访问权限
            /// </summary>
            AllowAll,
            /// <summary>
            /// 所有用户都不具有访问权限
            /// </summary>
            NotAllowAll
        }
        #endregion

        #region 权限组类别
        public const string STR_GROUP_TYPE = "group_type";
        public enum GROUP_TYPE
        {
            /// <summary>
            /// PC菜单
            /// </summary>
            PCMenu,
            /// <summary>
            /// 手机菜单
            /// </summary>
            MobileMenu
        }
        #endregion
        #endregion

    }


}
