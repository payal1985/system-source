using SSInventory.Share.Models.Dto.Submission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface ISubmissionService
    {
        Task<List<SubmissionModel>> ReadAsync(List<int> ids = null, int? userId = null, string status = null, string inventoryAppId = null, int? clientId = null);
        Task<int> InsertAsync(SubmissionModel model);
        Task<bool> UpdateStatusAsync(List<int> ids, string status);
        Task DeleteAsync(List<int> ids);
        Task<List<ExportSubmissionModel>> GetSubmissionForExport(List<ExportSubmissionModel> input);
    }
}