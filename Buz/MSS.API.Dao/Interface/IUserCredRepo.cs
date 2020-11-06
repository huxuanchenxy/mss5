using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Dao.Interface
{
    public interface IUserCredRepo<T> where T : BaseEntity
    {
        Task<UserCredPageView> GetPageList(UserCredParm param);
        Task<UserCred> Save(UserCred obj);
        Task<UserCred> GetByID(long id);
        Task<int> Update(UserCred obj);
        Task<int> Delete(string[] ids, int userID);
    }
}
