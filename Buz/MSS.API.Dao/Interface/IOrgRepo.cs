using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSS.API.Model.DTO;

namespace MSS.API.Dao.Interface
{
    public interface IOrgRepo<T> where T:BaseEntity
    {
        Task<List<OrgTree>> ListAllOrgNode();
        Task<List<OrgTree>> ListNodeByNodeType(int nodeType);
        Task<OrgTree> SaveOrgNode(OrgTree node);
        Task<OrgTree> UpdateOrgNode(OrgTree node);
        Task<bool> DeleteOrgNode(OrgTree node);
        Task<bool> CheckNodeExist(OrgTree node);
        Task<OrgTree> GetNode(int id);
        Task<OrgTree> GetNodeView(int id);
        Task<int> SaveOrgNodeProperty(List<OrgNodeProperty> props);
        Task<int> DeleteOrgNodeProperty(OrgTree node);
        Task<List<OrgNodeTypeProperty>> ListNodeTypeProperty(int id);
        Task<List<OrgUser>> ListOrgNodeUsers(int id);
        Task<List<OrgUser>> ListUnOrgNodeUsers(int id);
        Task<List<User>> ListUsersNotThisNode(int id);
        Task<int> BindOrgNodeUsers(List<OrgUser> users);
        Task<int> UnbindOrgNodeUsers(OrgTree node);
        Task<List<OrgNodeType>> ListNodeType();
        Task<OrgUser> GetOrgUserByUserID(int userId);

        Task<bool> hasChildren(int id);

        // 取出所有已关联组织的用户
        Task<List<OrgUser>> ListAllOrgUser();
        Task<int> DeleteOrgNodeUsers(OrgUserView users);
        Task<List<OrgUser>> ListUserByNode(int node);
    }
}
