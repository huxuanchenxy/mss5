using Dapper;
using MSS.API.Dao.Interface;
using MSS.API.Model.Data;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Coded By admin 2020/11/20 13:34:59
namespace MSS.API.Dao.Implement
{
    public class DocumentRepo : BaseRepo, IDocumentRepo<Document>
    {
        public DocumentRepo(DapperOptions options) : base(options) { }

        public async Task<DocumentPageView> GetPageList(DocumentParm parm)
        {
            return await WithConnection(async c =>
            {

                StringBuilder sql = new StringBuilder();
                sql.Append($@"  SELECT 
                id,
                doc_name,
                doc_version,
                doc_num,
                doc_type,
                doc_type2,
                eqp_type_id,
                active_time,
                dead_time,
                created_time,
                created_by,
                updated_time,
                updated_by,is_del FROM document
                 ");
                StringBuilder whereSql = new StringBuilder();
                //whereSql.Append(" WHERE ai.ProcessInstanceID = '" + parm.ProcessInstanceID + "'");

                //if (parm.AppName != null)
                //{
                //    whereSql.Append(" and ai.AppName like '%" + parm.AppName.Trim() + "%'");
                //}

                sql.Append(whereSql);
                //验证是否有参与到流程中
                //string sqlcheck = sql.ToString();
                //sqlcheck += ("AND ai.CreatedByUserID = '" + parm.UserID + "'");
                //var checkdata = await c.QueryFirstOrDefaultAsync<TaskViewModel>(sqlcheck);
                //if (checkdata == null)
                //{
                //    return null;
                //}

                var data = await c.QueryAsync<Document>(sql.ToString());
                var total = data.ToList().Count;
                sql.Append(" order by " + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                var ets = await c.QueryAsync<Document>(sql.ToString());

                DocumentPageView ret = new DocumentPageView();
                ret.rows = ets.ToList();
                ret.total = total;
                return ret;
            });
        }

        public async Task<Document> Save(Document obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO `document`(
                    
                    doc_name,
                    doc_version,
                    doc_num,
                    doc_type,
                    doc_type2,
                    eqp_type_id,
                    active_time,
                    dead_time,
                    created_time,
                    created_by,
                    updated_time,
                    updated_by,
                    is_del
                ) VALUES 
                (
                    @DocName,
                    @DocVersion,
                    @DocNum,
                    @DocType,
                    @DocType2,
                    @EqpTypeId,
                    @ActiveTime,
                    @DeadTime,
                    @CreatedTime,
                    @CreatedBy,
                    @UpdatedTime,
                    @UpdatedBy,
                    @IsDel
                    );
                    ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                obj.Id = newid;
                return obj;
            });
        }

        public async Task<Document> GetByID(long id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<Document>(
                    "SELECT * FROM document WHERE id = @id", new { id = id });
                return result;
            });
        }

        public async Task<int> Update(Document obj)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync($@" UPDATE document set 
                    
                    doc_name=@DocName,
                    doc_version=@DocVersion,
                    doc_num=@DocNum,
                    doc_type=@DocType,
                    doc_type2=@DocType2,
                    eqp_type_id=@EqpTypeId,
                    active_time=@ActiveTime,
                    dead_time=@DeadTime,
                    created_time=@CreatedTime,
                    created_by=@CreatedBy,
                    updated_time=@UpdatedTime,
                    updated_by=@UpdatedBy,
                    is_del=@IsDel
                 where id=@Id", obj);
                return result;
            });
        }

        public async Task<int> Delete(string[] ids, int userID)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" Update document set is_del=1" +
                ",updated_time=@updated_time,updated_by=@updated_by" +
                " WHERE id in @ids ", new { ids = ids, updated_time = DateTime.Now, updated_by = userID });
                return result;
            });
        }
    }
}



