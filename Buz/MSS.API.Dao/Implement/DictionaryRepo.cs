using System;
using System.Collections.Generic;
using System.Text;
using MSS.API.Dao.Interface;
using MSS.API.Dao;
using MSS.API.Model.Data;
using System.Threading.Tasks;
using Dapper;
using MSS.API.Model.DTO;
using static MSS.API.Common.Utility.Const;
using System.Linq;

namespace MSS.API.Dao.Implement
{
    public class DictionaryRepo : BaseRepo, IDictionaryRepo<DictionaryTree>
    {
        public DictionaryRepo(DapperOptions options) : base(options) { }

        public async Task<List<DictionaryTree>> GetSubByCode(int pid)
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<DictionaryTree>("select * from dictionary_tree where parent_id=@pid", new { pid = pid })).ToList();
                return result;
            });
        }

        public async Task<List<DictionaryTree>> GetSubByCodeOrder(int pid)
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<DictionaryTree>("select * from dictionary_tree where parent_id=@pid order by order_no", new { pid = pid })).ToList();
                return result;
            });
        }

        public async Task<List<DictionaryTree>> GetSubByCodesOrder(IEnumerable<int> pids)
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<DictionaryTree>("select * from dictionary_tree where parent_id in @pids order by order_no", new { pids})).ToList();
                return result;
            });
        }
        public async Task<List<DictionaryRelation>> GetByParent(int pid)
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<DictionaryRelation>(
                    "select dr.*,dt.name,dt.ext from dictionary_relation dr " +
                    "left join dictionary_tree dt on dr.business_type=dt.id " +
                    "where parent_business_type=@pid", new { pid = pid })).ToList();
                return result;
            });
        }
        //public async Task<MSSResult<DictionaryView>> GetPageByParm(DictionaryQueryParm parm)
        //{
        //    return await WithConnection(async c =>
        //    {
        //        MSSResult<DictionaryView> mRet = new MSSResult<DictionaryView>();
        //        StringBuilder sql = new StringBuilder();
        //        sql.Append("SELECT a.*,u1.user_name as created_name,u2.user_name as updated_name ")
        //        .Append(" FROM Dictionary a ")
        //        .Append(" left join user u1 on a.created_by=u1.id left join user u2 on a.updated_by=u2.id ")
        //        .Append(" WHERE 1=1 ");
        //        StringBuilder whereSql = new StringBuilder();
        //        if (!string.IsNullOrWhiteSpace(parm.searchName))
        //        {
        //            whereSql.Append(" and a.code_name like '%"+ parm.searchName + "%' ");
        //        }
        //        if (parm.searchSubName!=null)
        //        {
        //            whereSql.Append(" and a.sub_code_name like '%" + parm.searchSubName + "%' ");
        //        }
        //        sql.Append(whereSql)
        //        .Append(" order by a." + parm.sort + " " + parm.order)
        //        .Append(" limit " + (parm.page - 1) * parm.rows + "," + parm.rows);
        //        var tmp = await c.QueryAsync<DictionaryView>(sql.ToString());
        //        if (tmp != null)
        //        {
        //            mRet.data = tmp.ToList();
        //        }
        //        mRet.relatedData = await c.QueryFirstOrDefaultAsync<int>(
        //            "select count(*) from Dictionary a where 1=1 "+whereSql.ToString());
        //        return mRet;
        //    });
        //}
        //public async Task<Dictionary> GetByID(int id)
        //{
        //    return await WithConnection(async c =>
        //    {
        //        var result = await c.QueryFirstOrDefaultAsync<Dictionary>(
        //            "SELECT * FROM Dictionary WHERE id = @id", new { id = id });
        //        return result;
        //    });
        //}

        //public async Task<int> Add(Dictionary Dictionary)
        //{
        //    return await WithConnection(async c =>
        //    {
        //        var result = await c.ExecuteAsync(" insert into Dictionary " +
        //            " values (0,@code,@code_name,@sub_code,@sub_code_name,@description, " +
        //            " @created_time,@created_by,@updated_time,@updated_by) ", Dictionary);
        //        return result;
        //    });
        //}

        //public async Task<int> Update(Dictionary Dictionary)
        //{
        //    return await WithConnection(async c =>
        //    {
        //        var result = await c.ExecuteAsync(" update Dictionary " +
        //            " set code=@code,code_name=@code_name,sub_code=@sub_code,sub_code_name=@sub_code_name,description=@description, " +
        //            " updated_time=@updated_time,updated_by=@updated_by where id=@id", Dictionary);
        //        return result;
        //    });
        //}

        //public async Task<int> Delete(string[] ids)
        //{
        //    return await WithConnection(async c =>
        //    {
        //        var result = await c.ExecuteAsync(" Delete from Dictionary WHERE id in @ids ", new { ids = ids });
        //        return result;
        //    });
        //}

        //public async Task<List<Dictionary>> GetSubByCode(string code)
        //{
        //    return await WithConnection(async c =>
        //    {
        //        var result = (await c.QueryAsync<Dictionary>("select * from Dictionary where code=@code",new {code=code })).ToList();
        //        return result;
        //    });
        //}
    }
}
