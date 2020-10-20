using MSS.API.Model.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSS.API.Dao.Interface
{
    public interface IEquipmentRepo<T>
    {
        Task<Equipment> Save(Equipment eqp);
        Task<int> Update(Equipment eqp);
        Task<int> Delete(string[] ids, int userID);
        Task<EqpView> GetPageByParm(EqpQueryParm parm);
        Task<Equipment> GetByID(int id);
        Task<Equipment> GetDetailByID(int id);
        Task<List<Equipment>> ListByTopOrg(IEnumerable<int> topOrgs, int line, int location = 0, int locationBy = 0);
        Task<List<Equipment>> ListByEqpType(string ids);
        Task<List<Equipment>> ListByTeam(int id);
        Task<List<Equipment>> ListByPosition(int location, int locationBy, int eqpType,int? topOrg);
        Task<List<Equipment>> GetAll();
        Task<List<AllArea>> GetAllArea();

        // 所有设备数
        Task<int> CountAllEqp();

        // 根据id获取设备
        Task<EqpView> ListEqpByIDs(EqpQueryByIDParm parm);

        Task<bool> CodeIsRepeat(string code);
    }
}
