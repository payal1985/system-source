using RivianAirtableIntegrationApi.Models;

namespace RivianAirtableIntegrationApi.Repository.Interfaces
{
    public interface IRequestsRepository
    {
        Task<List<RecordsModel>> GetRequestRecords();

        Task<bool> InsertUpdateRequestRecords(List<RecordsModel> models);

        string GetConnectionString();

    }
}
