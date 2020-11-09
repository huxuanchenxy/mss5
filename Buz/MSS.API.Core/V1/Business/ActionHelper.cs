using MSS.API.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSS.API.Core.V1.Business
{
    static class ActionHelper
    {
        /// <summary>
        /// 根据数据库查询结果获取对应的菜单树结构
        /// </summary>
        /// <param name="laa">数据库查询权限结果</param>
        /// <returns>前端所需要的菜单数据结构列表</returns>
        public static List<MenuTree> GetMenuTree(List<ActionAll> laa)
        {
            List<MenuTree> lmt = new List<MenuTree>();
            IEnumerable<IGrouping<int, ActionAll>> groupAction = laa.GroupBy(a => a.GroupID);
            foreach (IGrouping<int, ActionAll> group in groupAction)
            {
                MenuTree at = new MenuTree();
                at.children = new List<MenuTree>();
                at.id = group.Key;
                at.parentID = 0;
                var tmp = group.FirstOrDefault();
                at.iconCls = tmp.GroupIcon;
                at.iconClsActive = tmp.GroupActiveIcon;
                at.name = tmp.GroupName;
                at.path = tmp.GroupURL;
                at.order = tmp.GroupOrder;
                foreach (ActionAll a in group.OrderBy(g => g.ActionOrder))
                {
                    MenuTree at1 = new MenuTree();
                    at1.id = a.ActionID;
                    at1.name = a.ActionName;
                    at1.parentID = a.GroupID;
                    at1.path = a.ActionURL;
                    at1.iconCls = a.ActionIcon;
                    at.children.Add(at1);
                }
                lmt.Add(at);
            }
            return lmt;
        }

        /// <summary>
        /// 根据数据库查询结果获取对应的已选中的权限树结构（前端）
        /// </summary>
        /// <param name="laa">数据库查询权限结果</param>
        /// <param name="checkedAction">已经选中的权限ID</param>
        /// <returns>前端所需要的已勾选的权限数据结构列表</returns>
        public static List<ActionTree> GetCheckedActionTree(List<ActionAll> laa, List<int> checkedAction)
        {
            List<ActionTree> lat = new List<ActionTree>();
            IEnumerable<IGrouping<int, ActionAll>> groupAction = laa.GroupBy(a => a.GroupID);
            foreach (IGrouping<int, ActionAll> group in groupAction.Where(a => a.Key != 0))
            {
                ActionTree at = new ActionTree();
                at.children = new List<ActionTree>();
                at.id = group.Key;
                at.parentID = 0;
                var tmp = group.FirstOrDefault();
                at.text = tmp.GroupName;
                foreach (ActionAll a in group.OrderBy(g => g.ActionOrder))
                {
                    ActionTree at1 = new ActionTree();
                    at1.id = a.ActionID;
                    at1.text = a.ActionName;
                    at1.parentID = a.GroupID;
                    if (checkedAction.Where(me=>me==at1.id).Count()>0)
                    {
                        at1.isChecked = true;
                    }
                    at.children.Add(at1);
                    at1.children = new List<ActionTree>();
                    foreach (ActionAll aa in laa.Where(me => me.ParentMenu == at1.id).OrderBy(me => me.ActionOrder))
                    {
                        ActionTree at2 = new ActionTree();
                        at2.id = aa.ActionID;
                        at2.text = aa.ActionName;
                        at2.parentID = aa.ParentMenu;
                        if (checkedAction.Where(me => me == at2.id).Count() > 0)
                        {
                            at2.isChecked = true;
                        }
                        at1.children.Add(at2);
                    }
                }
                lat.Add(at);
            }
            return lat;
        }

        /// <summary>
        /// 根据数据库查询结果获取对应的权限树结构
        /// </summary>
        /// <param name="laa">数据库查询权限结果</param>
        /// <returns>前端所需要的所有可勾选的权限数据结构列表</returns>
        public static List<ActionTree> GetActionTree(List<ActionAll> laa)
        {
            List<ActionTree> lat = new List<ActionTree>();
            IEnumerable<IGrouping<int, ActionAll>> groupAction = laa.GroupBy(a => a.GroupID);
            foreach (IGrouping<int, ActionAll> group in groupAction.Where(a => a.Key != 0))
            {
                ActionTree at = new ActionTree();
                at.children = new List<ActionTree>();
                at.id = group.Key;
                at.parentID = 0;
                var tmp = group.FirstOrDefault();
                at.text = tmp.GroupName;
                foreach (ActionAll a in group.OrderBy(g => g.ActionOrder))
                {
                    ActionTree at1 = new ActionTree();
                    at1.id = a.ActionID;
                    at1.text = a.ActionName;
                    at1.parentID = a.GroupID;
                    at.children.Add(at1);
                    if (laa.Where(me => me.ParentMenu == at1.id).Count() > 0)
                    {
                        at1.children = new List<ActionTree>();
                        foreach (ActionAll aa in laa.Where(me => me.ParentMenu == at1.id).OrderBy(me => me.ActionOrder))
                        {
                            ActionTree at2 = new ActionTree();
                            at2.id = aa.ActionID;
                            at2.text = aa.ActionName;
                            at2.parentID = aa.ParentMenu;
                            at1.children.Add(at2);
                        }
                    }
                }
                lat.Add(at);
            }
            return lat;
        }

    }
}
