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
using static MSS.API.Common.MyDictionary;
using System.Data;

namespace MSS.API.Dao.Implement
{
    public class UploadFileRepo : BaseRepo, IUploadFileRepo<UploadFile>
    {
        public UploadFileRepo(DapperOptions options) : base(options) { }

        public async Task<UploadFile> Save(UploadFile uploadFile)
        {
            return await WithConnection(async c =>
            {
                string sql = " insert into upload_file " +
                    " values (0,@FileName,@FilePath); ";
                sql += "SELECT LAST_INSERT_ID()";
                int newid = await c.QueryFirstOrDefaultAsync<int>(sql, uploadFile);
                uploadFile.ID = newid;
                return uploadFile;
            });
        }

        public async Task<int> Save(List<UploadFileRelation> uploadFileRelations)
        {
            return await WithConnection(async c =>
            {
                string sql;
                IDbTransaction trans = c.BeginTransaction();
                int ret;
                try
                {
                    sql = " delete from upload_file_relation " +
                        " where system_resource=@SystemResource and entity_id=@Entity";
                    ret = await c.ExecuteAsync(sql, uploadFileRelations, trans);
                    if (uploadFileRelations.Count > 1 || uploadFileRelations[0].File != 0)
                    {
                        sql = " insert into upload_file_relation " +
                            " values (0,@Entity,@File,@Type,@SystemResource) ";
                        ret = await c.ExecuteAsync(sql, uploadFileRelations, trans);
                    }
                    trans.Commit();
                    return ret;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.ToString());
                }
            });
        }


        public async Task<UploadFile> GetByID(int id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryFirstOrDefaultAsync<UploadFile>(
                    //"SELECT uf.*,ufr.system_resource,ufr.type FROM upload_file uf " +
                    //"left join upload_file_relation ufr on ufr.file_id=uf.id " +
                    "SELECT a.id Id,a.file_name FileName,a.file_path FilePath FROM upload_file a " +
                    "WHERE a.id = @id", new { id = id });
                return result;
            });
        }

        public async Task<List<UploadFile>> ListByIDs(string ids)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryAsync<UploadFile>(
                    "SELECT uf.id ID,uf.file_name FileName,uf.file_path FilePath,ufr.type,dt.name as TypeName,dt1.name as SystemResourceName,ufr.system_resource FROM upload_file uf " +
                    "left join upload_file_relation ufr on ufr.file_id=uf.id " +
                    "left join dictionary_tree dt on ufr.type=dt.id " +
                    "left join dictionary_tree dt1 on ufr.system_resource=dt1.id " +
                    "WHERE uf.id in @ids", new { ids = ids.Split(',') });
                if (result!=null && result.Count()>0)
                {
                    return result.ToList();
                }
                else
                {
                    return null;
                }
            });
        }
        public async Task<List<UploadFile>> ListByEntity(int[] ids, SystemResource systemResource)
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryAsync<UploadFile>(
                    "SELECT uf.id ID,uf.file_name FileName,uf.file_path FilePath,ufr.type,dt.name as TypeName,dt1.name as SystemResourceName," +
                    "ufr.system_resource,ufr.entity_id FROM upload_file uf " +
                    "left join upload_file_relation ufr on ufr.file_id=uf.id " +
                    "left join dictionary_tree dt on ufr.type=dt.id " +
                    "left join dictionary_tree dt1 on ufr.system_resource=dt1.id " +
                    "WHERE ufr.system_resource=@systemResource and ufr.entity_id in @ids", 
                    new { systemResource=(int)systemResource,ids });
                if (result != null && result.Count() > 0)
                {
                    return result.ToList();
                }
                else
                {
                    return null;
                }
            });
        }


        public async Task<int> Delete(int id)
        {
            return await WithConnection(async c =>
            {
                var result = await c.ExecuteAsync(" Delete from upload_file WHERE id=@id ", new { id = id });
                return result;
            });
        }
        public async Task<List<UploadFile>> ListAll()
        {
            return await WithConnection(async c =>
            {
                var result = await c.QueryAsync<UploadFile>(
                    "SELECT * FROM upload_file");
                if (result != null && result.Count() > 0)
                {
                    return result.ToList();
                }
                else
                {
                    return null;
                }
            });
        }
    }
}
