using MSS.API.Model.Data;
using MSS.API.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MSS.API.Common.Utility.Const;

namespace MSS.API.Dao.Interface
{
    public interface IDocumentRepo<T> where T : BaseEntity
    {
        Task<DocumentPageView> GetPageList(DocumentParm param);
        Task<Document> Save(Document obj);
        Task<Document> GetByID(long id);
        Task<int> Update(Document obj);
        Task<int> Delete(string[] ids, int userID);
    }
}
