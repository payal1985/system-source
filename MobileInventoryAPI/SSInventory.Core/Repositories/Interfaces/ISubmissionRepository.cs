using SSInventory.Core.Models;
using SSInventory.Share.Models.Dto.Submission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface ISubmissionRepository : IRepository<Submissions>
    {
        Task<List<SubmissionModel>> ReadAsync(List<int> ids = null, int? userId = null, string status = null, string inventoryAppId = null, int? clientId = null);
        Task<int> InsertAsync(SubmissionModel model);
        Task<bool> UpdateStatusAsync(List<int> ids, string status);
        Task DeleteAsync(List<int> ids);
        Task<List<ExportSubmissionModel>> ExportSubmissions(List<ExportSubmissionModel> input);
    }
}