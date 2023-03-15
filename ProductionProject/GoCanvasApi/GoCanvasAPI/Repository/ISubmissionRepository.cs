using GoCanvasAPI.ApiUtilities;
using GoCanvasAPI.DBModels;
using GoCanvasAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.Repository
{
    public interface ISubmissionRepository
    {
        Task InsertSubmission(List<SubmissionModel> _object);
        //Task InsertSubmission(SubmissionModel _object);
        //void InsertSubmission_Section1(Submission_Section1Model _object);
        Task InsertSubmission_Section1(List<Submission_Section1Model> _object);

        //public void InsertSubmission_Section2(Submission_Section2Model _object);
        //public void InsertSubmission_Section2_ResourceGroup(Submission_Section2_ResourceGroupModel _object);
        //public void InsertImages(ImageDataModel _object);

        Task InsertSubmission_Section2(List<Submission_Section2Model> _object);
        Task InsertSubmission_Section2_ResourceGroup(List<Submission_Section2_ResourceGroupModel> _object);
        Task InsertImages(List<ImageDataModel> _object);
        //Task InsertImagesSingle(ImageDataModel _object);

        //Task<bool> InsertAllSubmissionData(List<SubmissionRootObject> _listObject,ApiUtility apiUtilities);
        //Task<bool> InsertAllSubmissionData(SubmissionRootObject _listObject,ApiUtility apiUtilities);
        Task<bool> InsertAllSubmissionData(List<Submission> _listObject,ApiUtility apiUtilities);

        List<Submission> GetLatestSubmission(SubmissionRootObject submissionRootObject);
        //Task<List<SubmissionModel>> GetLatestSubmission(SubmissionRootObject submissionRootObject);
    }
}
