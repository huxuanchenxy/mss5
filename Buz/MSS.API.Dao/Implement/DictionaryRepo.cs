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
                var result = (await c.QueryAsync<DictionaryTree>("SELECT * FROM dictionary_tree where parent_id=@pid", new { pid = pid })).ToList();
                return result;
            });
        }

        public async Task<List<DictionaryTree>> GetSubByCodeOrder(int pid)
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<DictionaryTree>("SELECT * FROM dictionary_tree where parent_id=@pid order by order_no", new { pid = pid })).ToList();
                return result;
            });
        }

        public async Task<List<DictionaryRelation>> GetByParent(int pid)
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<DictionaryRelation>(
                    "SELECT dr.*,dt.name,dt.ext FROM dictionary_relation dr " +
                    "left join dictionary_tree dt on dr.business_type=dt.id " +
                    "where parent_business_type=@pid", new { pid = pid })).ToList();
                return result;
            });
        }


        public async Task<List<DictionaryTree>> GetSubByCodesOrder(IEnumerable<int> pids)
        {
            return await WithConnection(async c =>
            {
                var result = (await c.QueryAsync<DictionaryTree>("SELECT * FROM dictionary_tree where parent_id in @pids order by order_no", new { pids })).ToList();
                return result;
            });
        }
    }
}
