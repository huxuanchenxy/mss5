using MSS.API.Model.Data;
using System.Collections.Generic;

namespace MSS.API.Model.DTO
{
    public class ActionView:ActionInfo
    {
        public string created_name { get; set; }
        public string updated_name { get; set; }
        public string group_name { get; set; }
        public string parent_name { get; set; }
    }

    public class MenuTree
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 权限url
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 前端相对应的图标
        /// </summary>
        public string iconCls { get; set; }
        /// <summary>
        /// 前端相对应的选中图标
        /// </summary>
        public string iconClsActive { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        public int parentID { get; set; }

        public int order { get; set; }
        /// <summary>
        /// 菜单树结构
        /// </summary>
        public List<MenuTree> children { get; set; }
    }

    public class ActionAll
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public int ActionID { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 权限URL
        /// </summary>
        public string ActionURL { get; set; }
        /// <summary>
        /// 权限顺序
        /// </summary>
        public int ActionOrder { get; set; }
        /// <summary>
        /// 权限图标
        /// </summary>
        public string ActionIcon { get; set; }
        /// <summary>
        /// 权限所属界面
        /// </summary>
        public int ParentMenu { get; set; }
        /// <summary>
        /// 一级菜单ID
        /// </summary>
        public int GroupID { get; set; }
        public int Level { get; set; }
        /// <summary>
        /// 一级菜单名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 一级菜单URL
        /// </summary>
        public string GroupURL { get; set; }
        /// <summary>
        /// 一级菜单图标
        /// </summary>
        public string GroupIcon { get; set; }
        /// <summary>
        /// 一级菜单激活时图标
        /// </summary>
        public string GroupActiveIcon { get; set; }

        public int GroupOrder { get; set; }
    }

    /// <summary>
    /// 根据ActionAll组装成此数据结构
    /// 权限树结构（主要字段有id和name）
    /// 角色选择权限时用到
    /// </summary>
    public class ActionTree
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int parentID { get; set; }
        /// <summary>
        /// 是否被选中，只有最底层才会被选中，不级联父级
        /// </summary>
        public bool isChecked { get; set; }
        /// <summary>
        /// 权限树结构嵌套
        /// </summary>
        public List<ActionTree> children { get; set; }
        ///// <summary>
        ///// 前端专用，是否全选,状态
        ///// </summary>
        //public bool isIndeterminate { get; set; }

        //private List<int> _checkedChildren = new List<int>();
        ///// <summary>
        ///// 前端专用，已选中的二级菜单
        ///// </summary>
        //public List<int> checkedChildren
        //{
        //    get { return _checkedChildren; }
        //    set { _checkedChildren = value; }
        //}
        
    }


}
