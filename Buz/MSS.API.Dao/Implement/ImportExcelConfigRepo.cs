using System;
using System.Collections.Generic;
using System.Text;
using MSS.API.Dao.Interface;
using MSS.API.Dao;
using MSS.API.Model.Data;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using MSS.API.Common;
using System.Data;
using System.IO;
using MSS.API.Common.Utility;
using MySql.Data.MySqlClient;

namespace MSS.API.Dao.Implement
{
    public interface IImportExcelConfigRepo<T> where T : BaseEntity
    {
        int BulkLoad(DataTable table);

        Task<ImportExcelConfig> Save(ImportExcelConfig obj);
        Task<int> Update(ImportExcelConfig obj);
        Task<int> Delete(string[] ids);

        Task<ImportExcelConfig> GetByFileName(string fileName);
        Task<ImportExcelConfig> GetByID(int id);
        Task<List<ImportExcelConfig>> GetAll();
        Task<ImportExcelConfigView> GetPageByParm(ImportExcelConfigParm parm);

        Task<List<ImportExcelClass>> ListClass();
        Task<ImportExcelClass> GetClassByID(int id);

        Task<ImportExcelLog> SaveLog(ImportExcelLog obj);
        Task<Object> ListLogByParm(ImportExcelLogParm parm);

    }

    public class ImportExcelConfigRepo : BaseRepo, IImportExcelConfigRepo<ImportExcelConfig>
    {
        private readonly MySqlConnection con;
        public ImportExcelConfigRepo(DapperOptions options) : base(options)
        {
            con = new MySqlConnection(options.ConnectionString);
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="_mySqlConnection"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int BulkLoad(DataTable table)
        {
            ImportExcelHelper h = new ImportExcelHelper();
            h.ToCsv(table);
            var columns = table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
            MySqlBulkLoader bulk = new MySqlBulkLoader(con)
            {
                FieldTerminator = ",",
                FieldQuotationCharacter = '"',
                EscapeCharacter = '"',
                LineTerminator = "\r\n",
                FileName = @"../uploads/" + table.TableName + ".csv",
                NumberOfLinesToSkip = 0,
                TableName = table.TableName,
            };
            bulk.Columns.AddRange(columns);
            int ret = bulk.Load();
            using (ShareFolderHelper helper = new ShareFolderHelper("test", "yfzx.2019", FilePath.CSVPATH + table.TableName + ".csv"))
            {
                //删除临时的csv文件
                File.Delete(FilePath.CSVPATH + table.TableName + ".csv");
            }
            return ret;
        }

        #region ImportExcelConfig
        public async Task<ImportExcelConfig> Save(ImportExcelConfig obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO import_excel_config VALUES 
                ( 0,@FileName,@ClassID, @Field, @Config, @Required,@CreatedTime, @CreatedBy, @UpdatedTime, @UpdatedBy);
                    ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                obj.Id = newid;
                return obj;
            });
        }

        public async Task<int> Update(ImportExcelConfig obj)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" UPDATE import_excel_config " +
                    " set file_name=@FileName,class=@ClassID,field=@Field,required=@Required,config=@Config, " +
                    " updated_time=@UpdatedTime,updated_by=@UpdatedBy where id=@id", obj);
                return result;
            });
        }

        public async Task<int> Delete(string[] ids)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" Delete from import_excel_config where id in @ids",new {ids });
                return result;
            });
        }

        public async Task<ImportExcelConfig> GetByFileName(string fileName)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<ImportExcelConfig>(
                    "SELECT * FROM import_excel_config WHERE file_name = @fileName", new { fileName });
                return result;
            });
        }


        public async Task<ImportExcelConfig> GetByID(int id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<ImportExcelConfig>(
                    "SELECT * FROM import_excel_config WHERE id = @id", new { id = id });
                return result;
            });
        }

        public async Task<List<ImportExcelConfig>> GetAll()
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<ImportExcelConfig>(
                    "SELECT * FROM import_excel_config")).ToList();
                return result;
            });
        }

        public async Task<ImportExcelConfigView> GetPageByParm(ImportExcelConfigParm parm)
        {
            return await WithConnection(async c =>
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT distinct a.*,u2.user_name as UpdatedName,iec.show_name FROM import_excel_config a ")
                .Append(" left join user u2 on a.updated_by=u2.id ")
                .Append(" left join import_excel_class iec on a.class=iec.id");
                StringBuilder whereSql = new StringBuilder();
                whereSql.Append(" WHERE 1=1 ");
                if (!string.IsNullOrWhiteSpace(parm.SearchName))
                {
                    whereSql.Append(" and a.file_name like '%" + parm.SearchName+"%'");
                }
                if (parm.SearchClass != null)
                {
                    whereSql.Append(" and a.class="+parm.SearchClass);
                }
                sql.Append(whereSql)
                .Append(" order by a." + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                List<ImportExcelConfig> ets = (await c.QueryAsync<ImportExcelConfig>(sql.ToString())).ToList();
                sql.Clear();
                sql.Append("select count(*) FROM import_excel_config a ");
                int total = await c.QueryFirstOrDefaultAsync<int>(
                    sql.ToString() + whereSql.ToString());
                ImportExcelConfigView ret = new ImportExcelConfigView();
                ret.rows = ets;
                ret.total = total;
                return ret;
            });
        }
        #endregion

        #region ImportExcelClass
        public async Task<List<ImportExcelClass>> ListClass()
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<ImportExcelClass>(
                    "SELECT * FROM import_excel_class")).ToList();
                return result;
            });
        }
        public async Task<ImportExcelClass> GetClassByID(int id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<ImportExcelClass>(
                    "SELECT * FROM import_excel_class where id=@id",new { id});
                return result;
            });
        }
        #endregion

        #region ImportExcelLog
        public async Task<ImportExcelLog> SaveLog(ImportExcelLog obj)
        {
            return await WithConnection(async c =>
            {
                string sql = $@" INSERT INTO import_excel_log VALUES 
                ( 0,@FileName,@Field, @RecordNum,@CreatedTime, @CreatedBy);
                    ";
                sql += "SELECT LAST_INSERT_ID() ";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, obj);
                obj.ID = newid;
                return obj;
            });
        }

        public async Task<Object> ListLogByParm(ImportExcelLogParm parm)
        {
            return await WithConnection(async c =>
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT distinct a.*,u2.user_name as CreatedName FROM import_excel_log a ")
                .Append(" left join user u2 on a.created_by=u2.id ");
                StringBuilder whereSql = new StringBuilder();
                whereSql.Append(" WHERE 1=1 ");
                if (!string.IsNullOrWhiteSpace(parm.FileName))
                {
                    whereSql.Append(" and a.file_name like '%" + parm.FileName + "%'");
                }
                sql.Append(whereSql)
                .Append(" order by a." + parm.sort + " " + parm.order)
                .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
                List<ImportExcelLog> ets = (await c.QueryAsync<ImportExcelLog>(sql.ToString())).ToList();
                sql.Clear();
                sql.Append("select count(*) FROM import_excel_log a ");
                int total = await c.QueryFirstOrDefaultAsync<int>(
                    sql.ToString() + whereSql.ToString());
                return new {rows=ets,total=total };
            });
        }
        #endregion

    }
}
