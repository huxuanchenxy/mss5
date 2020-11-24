using System;
using System.Collections.Generic;
using System.Text;
using MSS.API.Dao.Interface;
using MSS.API.Dao;
using MSS.API.Model.Data;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using MSS.API.Model.DTO;

namespace MSS.API.Dao.Implement
{
    public class OrgRepo : BaseRepo, IOrgRepo<OrgTree>
    {
        public OrgRepo(DapperOptions options) : base(options) { }
        public async Task<List<OrgTree>> ListAllOrgNode()
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM org_tree";
                var list = await c.QueryAsync<OrgTree>(sql);
                return list.ToList();
            });
        }

        public async Task<List<OrgTree>> ListNodeByNodeType(int nodeType)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM org_tree where node_type=@nodeType and is_del=0";
                var list = await c.QueryAsync<OrgTree>(sql, new { nodeType });
                return list.ToList();
            });
        }


        public async Task<OrgTree> SaveOrgNode(OrgTree node)
        {
            return await WithConnection(async c =>
            {
                string sql = "INSERT INTO org_tree (parent_id, name, node_type, created_time, created_by, is_del)"
                            +" Values (@ParentID, @Name, @NodeType, @CreatedTime, @CreatedBy, @IsDel);";
                sql += "SELECT LAST_INSERT_ID()";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql,
                    new
                    { 
                        ParentID = node.ParentID,
                        CreatedBy = node.CreatedBy,
                        Name = node.Name,
                        NodeType = node.NodeType,
                        CreatedTime = node.CreatedTime,
                        IsDel = node.IsDel
                    });
                node.Id = newid;
                return node;
            });
        }



        public async Task<bool> DeleteOrgNode(OrgTree node) {
            return await WithConnection(async c =>
            {
                string sql = "UPDATE org_tree SET is_del = 1,"
                                + " updated_by = @UpdatedBy, updated_time = @UpdatedTime WHERE ID = @ID;";
                await c.ExecuteAsync(sql,
                new
                {
                    ID = node.Id,
                    UpdatedBy = node.UpdatedBy,
                    UpdatedTime = node.UpdatedTime
                });
                return true;
            });
        }

        public async Task<bool> CheckNodeExist(OrgTree node) {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM org_tree WHERE name=@Name and is_del!=1 and ID != @ID";
                if (node.ParentID == null) {
                    sql += " and ISNULL(parent_id)";
                } else {
                    sql += " and parent_id = @ParentID";
                }
                OrgTree exist = await c.QueryFirstOrDefaultAsync<OrgTree>(sql,
                new
                {
                    ID = node.Id,
                    Name = node.Name,
                    ParentID = node.ParentID
                });
                return exist != null ? true : false;
            });
        }

        public async Task<bool> hasChildren(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM org_tree WHERE parent_id = @ID and is_del!=1";
                List<OrgTree> list = (await c.QueryAsync<OrgTree>(sql,
                new
                {
                    ID = id
                })).ToList();
                return list.Count > 0 ? true : false;
            });
        }

        public async Task<OrgTree> GetNode(int id) {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM org_tree WHERE ID = @ID";
                OrgTree node = await c.QueryFirstOrDefaultAsync<OrgTree>(sql,
                new
                {
                    ID = id
                });
                return node;
            });
        }


        public async Task<OrgTree> UpdateOrgNode(OrgTree node)
        {
            return await WithConnection(async c =>
            {
                string sql = "UPDATE org_tree SET parent_id = @ParentID, name = @Name, node_type = @NodeType,"
                            + " updated_by = @UpdatedBy, updated_time = @UpdatedTime WHERE ID = @ID;";
                await c.ExecuteAsync(sql,
                new
                {
                    ID = node.Id,
                    ParentID = node.ParentID,
                    Name = node.Name,
                    NodeType = node.NodeType,
                    UpdatedBy = node.UpdatedBy,
                    UpdatedTime = node.UpdatedTime
                });
                return node;
            });
        }
        public async Task<OrgTree> GetNodeView(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM org_tree WHERE ID = @ID; SELECT * FROM org_node_property_value WHERE node_id=@ID";
                using (var multi = await c.QueryMultipleAsync(sql, new { ID = id }))
                {
                    var node = multi.Read<OrgTree>().First();
                    List<OrgNodeProperty> nodeProps = multi.Read<OrgNodeProperty>().ToList();
                    node.PropEx = nodeProps;
                    return node;
                }
            });
        }

        public async Task<int> SaveOrgNodeProperty(List<OrgNodeProperty> props)
        {
            return await WithConnection(async c =>
            {
                string sql = "INSERT INTO org_node_property_value (node_id, node_attr, attr_value, created_time, created_by, is_del)"
                            + " Values (@NodeID, @NodeAttr, @AttrValue, @CreatedTime, @CreatedBy, @IsDel);";
                List<object> param = new List<object>();
                foreach (OrgNodeProperty prop in props)
                {
                    var p = new
                    {
                        NodeID = prop.NodeID,
                        NodeAttr = prop.NodeAttr,
                        AttrValue = prop.AttrValue,
                        CreatedTime = prop.CreatedTime,
                        CreatedBy = prop.CreatedBy,
                        IsDel = 0
                    };
                    param.Add(p);
                }
                int affectedRows = await c.ExecuteAsync(sql, param);
                return affectedRows;
            });
        }

        public async Task<int> DeleteOrgNodeProperty(OrgTree node)
        {
            return await WithConnection(async c =>
            {
                string sql = "UPDATE org_node_property_value SET is_del = 1,"
                                + " updated_by = @UpdatedBy, updated_time = @UpdatedTime WHERE node_id = @nodeID and is_del != 1;";
                int affectedRows = await c.ExecuteAsync(sql,
                new
                {
                    nodeID = node.Id,
                    UpdatedBy = node.UpdatedBy,
                    UpdatedTime = node.UpdatedTime
                });
                return affectedRows;
            });
        }

        public async Task<List<OrgNodeTypeProperty>> ListNodeTypeProperty(int id) {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM org_node_type_property WHERE node_type_id = @ID";
                var prop = await c.QueryAsync<OrgNodeTypeProperty>(sql,
                new
                {
                    ID = id
                });
                return prop.ToList();
            });
        }

        public async Task<List<OrgUser>> ListOrgNodeUsers(int id)
        {
            return await WithConnection(async c =>
            {
                // string sql = "SELECT * FROM org_user WHERE org_node_id = @ID and is_del != 1";
                string sql = "SELECT a.*, b.user_name, b.email, b.mobile FROM org_user a"
                    + " INNER JOIN user AS b on a.user_id=b.id "
                    + " WHERE a.org_node_id = @ID and a.is_del != 1";
                var prop = await c.QueryAsync<OrgUser>(sql,
                new
                {
                    ID = id
                });
                return prop.ToList();
            });
        }
        public async Task<List<OrgUser>> ListUnOrgNodeUsers(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM org_user WHERE org_node_id != @ID and is_del != 1";
                var prop = await c.QueryAsync<OrgUser>(sql,
                new
                {
                    ID = id
                });
                return prop.ToList();
            });
        }
        public async Task<List<User>> ListUsersNotThisNode(int id)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * from user WHERE is_del !=1 AND"
                    +" id not in (SELECT user_id from org_user where org_node_id != @ID AND is_del != 1)";
                var prop = await c.QueryAsync<User>(sql,
                new
                {
                    ID = id
                });
                return prop.ToList();
            });
        }

        public async Task<int> BindOrgNodeUsers(List<OrgUser> users) {
            return await WithConnection(async c =>
            {
                string sql = "INSERT INTO org_user (org_node_id, user_id, created_time, created_by, is_del)"
                            + " Values (@NodeID, @UserID, @CreatedTime, @CreatedBy, @IsDel);";
                List<object> param = new List<object>();
                foreach (OrgUser user in users)
                {
                    var p = new
                    {
                        NodeID = user.NodeID,
                        UserID = user.UserID,
                        CreatedTime = user.CreatedTime,
                        CreatedBy = user.CreatedBy,
                        IsDel = 0
                    };
                    param.Add(p);
                }
                int affectedRows = await c.ExecuteAsync(sql, param);
                return affectedRows;
            });
        }

        public async Task<int> UnbindOrgNodeUsers(OrgTree node)
        {
            return await WithConnection(async c =>
            {
                string sql = "UPDATE org_user SET is_del = 1,"
                                + " updated_by = @UpdatedBy, updated_time = @UpdatedTime WHERE org_node_id = @nodeID and is_del != 1;";
                int affectedRows = await c.ExecuteAsync(sql,
                new
                {
                    nodeID = node.Id,
                    UpdatedBy = node.UpdatedBy,
                    UpdatedTime = node.UpdatedTime
                });
                return affectedRows;
            });
        }

        public async Task<List<OrgNodeType>> ListNodeType()
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM org_node_type";
                var data = await c.QueryAsync<OrgNodeType>(sql);
                return data.ToList();
            });
        }
        public async Task<OrgUser> GetOrgUserByUserID(int userId)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT * FROM org_user WHERE user_id = @UserID and is_del != 1";
                OrgUser orguser = await c.QueryFirstOrDefaultAsync<OrgUser>(sql,
                new
                {
                    UserID = userId
                });
                return orguser;
            });
        }
        public async Task<List<OrgUser>> ListAllOrgUser()
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT a.*, b.user_name FROM org_user AS a"
                    + " INNER JOIN user AS b on a.user_id=b.id WHERE a.is_del != 1";
                List<OrgUser> orguser = (await c.QueryAsync<OrgUser>(sql)).ToList();
                return orguser;
            });
        }

        public async Task<int> DeleteOrgNodeUsers(OrgUserView users)
        {
            return await WithConnection(async c =>
            {
                string sql = "UPDATE org_user SET is_del = 1,"
                                + " updated_by = @UpdatedBy, updated_time = @UpdatedTime WHERE id = @ID and is_del != 1;";
                int affectedRows = await c.ExecuteAsync(sql,
                new
                {
                    ID = users.UserIDs,
                    UpdatedBy = users.UpdatedBy,
                    UpdatedTime = users.UpdatedTime
                });
                return affectedRows;
            });
        }

        public async Task<List<OrgUser>> ListUserByNode(int node)
        {
            return await WithConnection(async c =>
            {
                string sql = "SELECT a.*, b.user_name FROM org_user AS a"
                    + " INNER JOIN user AS b on a.user_id=b.id " +
                    " WHERE a.is_del != 1 and a.org_node_id=@node and b.is_del != 1";
                var orguser = await c.QueryAsync<OrgUser>(sql, new { node });
                if (orguser.Count() > 0) return orguser.ToList();
                else return new List<OrgUser>();
            });
        }
    }
}
