using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.Submission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;

        public SubmissionService(ISubmissionRepository submissionRepository)
        {
            _submissionRepository = submissionRepository;
        }

        public virtual async Task<List<SubmissionModel>> ReadAsync(List<int> ids = null, int? userId = null, string status = null, string inventoryAppId = null, int? clientId = null)
        {
            return await _submissionRepository.ReadAsync(ids, userId: userId, status: status, inventoryAppId: inventoryAppId, clientId: clientId);
        }

        public virtual async Task<int> InsertAsync(SubmissionModel model) => await _submissionRepository.InsertAsync(model);

        public virtual async Task<bool> UpdateStatusAsync(List<int> ids, string status)
        {
            return await _submissionRepository.UpdateStatusAsync(ids, status);
        }

        public virtual async Task DeleteAsync(List<int> ids) => await _submissionRepository.DeleteAsync(ids);

        public async Task<List<ExportSubmissionModel>> GetSubmissionForExport(List<ExportSubmissionModel> input)
        {
            return await _submissionRepository.ExportSubmissions(input);
        }
    }
}
