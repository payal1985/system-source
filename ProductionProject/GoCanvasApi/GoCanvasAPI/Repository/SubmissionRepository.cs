using GoCanvasAPI.ApiUtilities;
using GoCanvasAPI.DBContext;
using GoCanvasAPI.DBModels;
using GoCanvasAPI.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace GoCanvasAPI.Repository
{
    public class SubmissionRepository : ISubmissionRepository
    {
        GoCanvasContext _dbContext;
        public SubmissionRepository(GoCanvasContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Submission> GetLatestSubmission(SubmissionRootObject submissionRootObject)
        {
            //bool result = false;
            //var subRootObj = _dbContext.SubmissionModels.Where(item => !submissionRootObject.Submissions.Submission.Select(s => s.Id).Contains(item.SubmissionId)).ToListAsync();
            List<Submission> submissions = new List<Submission>();

            var entity = _dbContext.SubmissionModels.Where(item => submissionRootObject.Submissions.Submission.Select(x => x.Id).Contains(item.SubmissionId)).ToList();

            if (entity != null && entity.Count > 0)
            {
                submissions = submissionRootObject.Submissions.Submission.Where(sub => entity.All(e => e.SubmissionId != sub.Id)).ToList();
            }
            else
            {
                submissions = submissionRootObject.Submissions.Submission.ToList();
            }

            return submissions;
        }
        public async Task InsertSubmission(List<SubmissionModel> submission)
        {
            // Retrieve entity by id            

            var entity = _dbContext.SubmissionModels.Where(item => submission.Select(x => x.SubmissionId).Contains(item.SubmissionId)).ToList();

            if (entity != null && entity.Count > 0)
            {

                //foreach (var e in entity)
                //{
                //    var data = _object.Where(o => o.SubmissionId == e.SubmissionId).FirstOrDefault();
                //    _dbContext.Update(entity);
                //}
            }
            else
            {
                await _dbContext.AddRangeAsync(submission);
                await _dbContext.SaveChangesAsync();
            }


            #region single entity save code
            //var entity = _dbContext.SubmissionModels.Where(item => item.SubmissionId == submission.SubmissionId).FirstOrDefault();

            //// Validate entity is not null
            //if (entity != null)
            //{
            //    entity.FormId = submission.FormId;
            //    entity.FormName = submission.FormName;
            //    entity.Date = submission.Date;
            //    entity.DeviceDate = submission.DeviceDate;
            //    entity.UserName = submission.UserName;
            //    entity.FirstName = submission.FirstName;
            //    entity.LastName = submission.LastName;
            //    entity.ResponseID = submission.ResponseID;
            //    entity.WebAccessToken = submission.WebAccessToken;
            //    entity.No = submission.No;
            //    entity.SubmissionNumber = submission.SubmissionNumber;
            //    entity.SubFormSatus = submission.SubFormSatus;
            //    entity.SubFormVersion = submission.SubFormVersion;

            //    _dbContext.Update(entity);
            //}
            //else
            //{
            //    await _dbContext.AddAsync(submission);
            //}

            // Save changes in database
            // await _dbContext.SaveChangesAsync();
            //Save();
            #endregion
        }

        //public async Task InsertSubmission(SubmissionModel submission)
        //{
        //    // Retrieve entity by id            

        //    //var entity = _dbContext.SubmissionModels.Where(item => item.SubmissionId == submission.SubmissionId).FirstOrDefault();
        //    var entity = _dbContext.SubmissionModels.Where(item => item.SubmissionId.Contains(submission.SubmissionId)).FirstOrDefault();

        //    // Validate entity is not null
        //    if (entity != null)
        //    {
        //        //entity.FormId = submission.FormId;
        //        //entity.FormName = submission.FormName;
        //        //entity.Date = submission.Date;
        //        //entity.DeviceDate = submission.DeviceDate;
        //        //entity.UserName = submission.UserName;
        //        //entity.FirstName = submission.FirstName;
        //        //entity.LastName = submission.LastName;
        //        //entity.ResponseID = submission.ResponseID;
        //        //entity.WebAccessToken = submission.WebAccessToken;
        //        //entity.No = submission.No;
        //        //entity.SubmissionNumber = submission.SubmissionNumber;
        //        //entity.SubFormSatus = submission.SubFormSatus;
        //        //entity.SubFormVersion = submission.SubFormVersion;

        //        _dbContext.Update(entity);
        //        _dbContext.SaveChanges();
        //    }
        //    else
        //    {
        //        await _dbContext.AddAsync(submission);
        //        await _dbContext.SaveChangesAsync();
        //    }


        //}

        public async Task InsertSubmission_Section1(List<Submission_Section1Model> _object)
        {
            try
            {
                //var entity = _dbContext.Submission_Section1Models.Where(item => item.SubmissionId == _object.SubmissionId && item.Section_Screen_Response_Guid == _object.Section_Screen_Response_Guid).;
                var entity = _dbContext.Submission_Section1Models.Where(item => _object.Select(x => x.SubmissionId).Contains(item.SubmissionId) &&
                                                                               _object.Select(x => x.Section_Screen_Response_Guid).Contains(item.Section_Screen_Response_Guid)).ToList();

                // Validate entity is not null
                if (entity != null && entity.Count == 0)
                {
                    foreach (var e in entity)
                    {
                        var data = _object.Where(o => o.SubmissionId == e.SubmissionId && o.Section_Screen_Response_Guid == e.Section_Screen_Response_Guid).FirstOrDefault();
                        //e.SectionName = data.SectionName;
                        //e.Section_Screen_Name = data.Section_Screen_Name;
                        //e.Section_Screen_Response_Guid = data.Section_Screen_Response_Guid;
                        //e.Section_Screen_Response_Label = data.Section_Screen_Response_Label;
                        e.Section_Screen_Response_Value = data.Section_Screen_Response_Value;
                        //e.Section_Screen_Response_Type = data.Section_Screen_Response_Type;

                        _dbContext.Update(e);
                        _dbContext.SaveChanges();
                    }
                }
                else
                {
                    await _dbContext.AddRangeAsync(_object);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task InsertSubmission_Section2(List<Submission_Section2Model> _object)
        {
            try
            {


                //var entity = _dbContext.Submission_Section2Models.AsNoTracking().FirstOrDefault(item => item.SubmissionId == _object.SubmissionId && item.Section_Screen_ResponseGroup_Guid == _object.Section_Screen_ResponseGroup_Guid && item.Section_Screen_ResponseGroup_Response_Guid == _object.Section_Screen_ResponseGroup_Response_Guid);
                var entity = _dbContext.Submission_Section2Models.Where(item => _object.Select(x => x.SubmissionId).Contains(item.SubmissionId)
                                                  && _object.Select(x => x.Section_Screen_ResponseGroup_Guid).Contains(item.Section_Screen_ResponseGroup_Guid)
                                                  //&& _object.Select(x => x.Section_Screen_ResponseGroup_Response_Label).Contains(item.Section_Screen_ResponseGroup_Response_Label) 
                                                  //&& _object.Select(x => x.Section_Screen_ResponseGroup_Response_Value).Contains(item.Section_Screen_ResponseGroup_Response_Value)
                                                  ).ToList();

                //// Validate entity is not null
                if (entity != null && entity.Count > 0)
                {
                    foreach (var e in entity)
                    {
                        var data = _object.Where(o => o.SubmissionId == e.SubmissionId && o.Section_Screen_ResponseGroup_Guid == e.Section_Screen_ResponseGroup_Guid).FirstOrDefault();
                        e.Section_Screen_ResponseGroup_Response_Value = data.Section_Screen_ResponseGroup_Response_Value;

                        _dbContext.Update(e);
                        _dbContext.SaveChanges();
                    }
                }
                else
                {
                    await _dbContext.AddRangeAsync(_object);

                    await _dbContext.SaveChangesAsync();
                }
                // Save changes in database

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //await Task.Delay(1000);
        }

        public async Task InsertSubmission_Section2_ResourceGroup(List<Submission_Section2_ResourceGroupModel> _object)
        {
            try
            {


                var entity = _dbContext.Submission_Section2_ResourceGroupModels.Where(item => _object.Select(x => x.SubmissionId).Contains(item.SubmissionId)
                                                  && _object.Select(x => x.ResGrp_Section_Screen_Response_Guid).Contains(item.ResGrp_Section_Screen_Response_Guid)
                                                  && _object.Select(x => x.Section_Screen_ResponseGroup_Response_Value).Contains(item.Section_Screen_ResponseGroup_Response_Value)
                                                  //&& _object.Select(x => x.ResGrp_Section_Screen_Response_Value).Contains(item.ResGrp_Section_Screen_Response_Value)
                                                  ).ToList();


                // Validate entity is not null
                if (entity != null && entity.Count > 0)
                {
                    foreach (var e in entity)
                    {
                        var data = _object.Where(o => o.SubmissionId == e.SubmissionId
                                                && o.ResGrp_Section_Screen_Response_Guid == e.ResGrp_Section_Screen_Response_Guid
                                                && o.Section_Screen_ResponseGroup_Response_Value == e.Section_Screen_ResponseGroup_Response_Value).FirstOrDefault();

                        e.ResGrp_Section_Screen_Response_Value = data.ResGrp_Section_Screen_Response_Value;
                        e.Section_Screen_ResponseGroup_Response_Value = data.Section_Screen_ResponseGroup_Response_Value;

                        _dbContext.Update(e);
                        _dbContext.SaveChanges();
                    }
                }
                else
                {
                    await _dbContext.AddRangeAsync(_object);
                    await _dbContext.SaveChangesAsync();

                }

                // Save changes in database
            }
            catch (Exception ex)
            {
                throw;
            }

            //await Task.Delay(1000);
        }

        public async Task InsertImages(List<ImageDataModel> _object)
        {
            try
            {


                // Retrieve entity by id            
                //var entity = _dbContext.ImageDataModels.FirstOrDefault(item => item.ImageId == image.ImageId && item.SubmissionNumber == image.SubmissionNumber);            
                var entity = _dbContext.ImageDataModels.Where(item => _object.Select(x => x.SubmissionId).Contains(item.SubmissionId) && _object.Select(x => x.ImageId).Contains(item.ImageId)).ToList();

                // Validate entity is not null
                if (entity != null && entity.Count > 0)
                {
                    foreach (var e in entity)
                    {
                        var data = _object.Where(o => o.SubmissionId == e.SubmissionId
                                                && o.ImageId == e.ImageId).FirstOrDefault();

                        e.ImageNumber = data.ImageNumber;

                        _dbContext.Update(e);
                        _dbContext.SaveChanges();
                    }

                }
                else
                {
                    await _dbContext.AddRangeAsync(_object);
                    await _dbContext.SaveChangesAsync();
                }

                // Save changes in database

                //await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /* public async Task<bool> InsertAllSubmissionData(SubmissionRootObject _listObject, ApiUtility _apiUtilities)
         {
             bool result = false;
             try
             {
                 //foreach (var latestobjectlist in _listObject.Submissions.Submission)
                 //{
                 using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                 {
                     var latestobject = _listObject.Submissions.Submission.LastOrDefault();

                     SubmissionModel Submission_Model = new SubmissionModel();

                       Submission_Model.SubmissionId = latestobject.Id;
                       Submission_Model.FormId = latestobject.Form[0].Id ?? "";
                       Submission_Model.FormName = latestobject.Form[0].Name.text ?? "";
                       Submission_Model.SubFormSatus = latestobject.Form[0].Status.text ?? "";
                       Submission_Model.SubFormVersion = latestobject.Form[0].Version.text ?? "";
                       Submission_Model.Date = (!string.IsNullOrEmpty(latestobject.Date.Text) ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(latestobject.Date.Text), TimeZoneInfo.Local) : Convert.ToDateTime("1900/01/01"));
                       Submission_Model.DeviceDate = (!string.IsNullOrEmpty(latestobject.DeviceDate.Text) ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(latestobject.DeviceDate.Text), TimeZoneInfo.Local) : Convert.ToDateTime("1900/01/01"));
                       Submission_Model.UserName = latestobject.UserName.Text ?? "";
                       Submission_Model.FirstName = latestobject.FirstName.Text ?? "";
                       Submission_Model.LastName = latestobject.LastName.Text ?? "";
                       Submission_Model.ResponseID = latestobject.ResponseID.Text ?? "";
                       Submission_Model.WebAccessToken = (latestobject.WebAccessToken != null ? (latestobject.WebAccessToken.Text ?? "") : "");
                       Submission_Model.No = latestobject.No.Text ?? "";
                       Submission_Model.SubmissionNumber = latestobject.SubmissionNumber.Text ?? "";

                     if (Submission_Model != null)
                         await InsertSubmission(Submission_Model);

                     //foreach (var latestobject in latestobjectlist)
                     //{
                         foreach (var section in latestobject.Sections.Section)
                         {

                             if (section.Screens.Screen.FirstOrDefault().Responses != null && section.Screens.Screen.FirstOrDefault().Responses.Response.Count >= 1)
                             {

                                 List<Submission_Section1Model> listSubmission_Section1Model = section.Screens.Screen[0].Responses.Response.Select(x => new Submission_Section1Model
                                 {
                                     //  Sub_Section1_table_id = new Guid(),
                                     SectionName = section.Name.text,
                                     Section_Screen_Name = section.Screens.Screen[0].Name.text,

                                     //SubmissionId = model.SubmissionId,
                                     SubmissionId = Submission_Model.SubmissionId,

                                     Section_Screen_Response_Guid = x.Guid,
                                     Section_Screen_Response_Label = x.Label.Text,
                                     Section_Screen_Response_Value = x.Value.Text,
                                     Section_Screen_Response_Type = x.Type.Text,
                                 }
                                 ).ToList();

                                 if (listSubmission_Section1Model.Count > 0)
                                     await InsertSubmission_Section1(listSubmission_Section1Model);
                             }
                             else if (section.Screens.Screen.FirstOrDefault().ResponseGroups != null && section.Screens.Screen.FirstOrDefault().ResponseGroups.ResponseGroup.Count >= 1)
                             {
                                 List<Submission_Section2Model> listSubmission_Section2Model = section.Screens.Screen[0].ResponseGroups.ResponseGroup.Select(x => new Submission_Section2Model
                                 {
                                     SectionName = section.Name.text,
                                     Section_Screen_Name = section.Screens.Screen[0].Name.text,
                                     //SubmissionId = model.SubmissionId,
                                     SubmissionId = Submission_Model.SubmissionId,
                                     Section_Screen_ResponseGroup_Guid = x.Guid,
                                     Section_Screen_ResponseGroup_Response_Guid = x.Response.Guid,
                                     Section_Screen_ResponseGroup_Response_Label = x.Response.Label.Text,
                                     Section_Screen_ResponseGroup_Response_Value = x.Response.Value.Text,
                                     Section_Screen_ResponseGroup_Response_Type = x.Response.Type.Text,
                                 }).ToList();

                                 if (listSubmission_Section2Model.Count > 0)
                                     await InsertSubmission_Section2(listSubmission_Section2Model);



                                 foreach (var resgrp in section.Screens.Screen[0].ResponseGroups.ResponseGroup)
                                 {

                                     foreach (var sec2resgrp in resgrp.Section.Screens.Screen)
                                     {
                                         List<Submission_Section2_ResourceGroupModel> listSubmission_Section2_ResourceGroupModel = sec2resgrp.Responses.Response.Select(
                                             x => new Submission_Section2_ResourceGroupModel
                                             {
                                                 ResGrp_Section_Name = resgrp.Section.Name.text,
                                                 ResGrp_Section_Screen_Name = sec2resgrp.Name.text,
                                                 ResGrp_Section_Screen_Response_Guid = x.Guid,
                                                 ResGrp_Section_Screen_Response_Label = x.Label.Text,
                                                 ResGrp_Section_Screen_Response_Type = x.Type.Text,
                                                 ResGrp_Section_Screen_Response_Value = x.Value.Text,
                                                 SubmissionId = Submission_Model.SubmissionId,
                                                 //SubmissionId = model.SubmissionId,
                                                 Section_Screen_ResponseGroup_Response_Value = resgrp.Response.Value.Text
                                             }
                                             ).ToList();

                                         if (listSubmission_Section2_ResourceGroupModel.Count > 0)
                                             await InsertSubmission_Section2_ResourceGroup(listSubmission_Section2_ResourceGroupModel);

                                         //section2ResGrpModel.ResGrp_Section_Screen_Name = sec2resgrp.Name.text;
                                         foreach (var sec2resgrpres in sec2resgrp.Responses.Response)
                                         {
                                             //InsertSubmission_Section2_ResourceGroup(section2ResGrpModel);
                                             if (sec2resgrpres.Label.Text == "Upload Image" && !string.IsNullOrEmpty(sec2resgrpres.Value.Text))
                                             {

                                                 if (sec2resgrpres.Numbers != null && sec2resgrpres.Numbers.Number != null)
                                                 {
                                                     List<ImageDataModel> listImageDataModel = sec2resgrpres.Numbers.Number.Select(x => new ImageDataModel
                                                     {
                                                         ImageId = sec2resgrpres.Value.Text ?? "",
                                                         ImageNumber = x.Text ?? "",
                                                         SubmissionId = Submission_Model.SubmissionId,
                                                         //SubmissionId = model.SubmissionId
                                                     }).ToList();

                                                     if (listImageDataModel.Count > 0)
                                                     {
                                                         await InsertImages(listImageDataModel);
                                                         List<ImagesModel> imageModel = listImageDataModel.Select(x => new ImagesModel { ImageId = Convert.ToInt64(x.ImageId), Number = Convert.ToInt32(x.ImageNumber) }).ToList();
                                                         var imageresult = await _apiUtilities.ImageDataUI(imageModel);
                                                     }
                                                 }
                                                 else
                                                 {
                                                     List<ImageDataModel> listImageDataModel = new List<ImageDataModel>();

                                                     listImageDataModel.Add(new ImageDataModel()
                                                     {
                                                         ImageId = sec2resgrpres.Value.Text ?? "",
                                                         ImageNumber = "0",
                                                         SubmissionId = Submission_Model.SubmissionId,
                                                     });


                                                     if (listImageDataModel.Count > 0)
                                                     {
                                                         await InsertImages(listImageDataModel);
                                                         List<ImagesModel> imageModel = listImageDataModel.Select(x => new ImagesModel { ImageId = Convert.ToInt64(x.ImageId), Number = Convert.ToInt32(x.ImageNumber) }).ToList();
                                                         var imageresult = await _apiUtilities.ImageDataUI(imageModel);
                                                     }
                                                 }


                                             }
                                         }
                                     }
                                 }
                             }
                         }

                    // }

                     scope.Complete();
                 }
                 #region old code
                 //foreach (var submissionitem in mainitem.Submissions.Submission)
                 //{
                 //    SubmissionModel model = new SubmissionModel();
                 //    //model.SubmissionId = (!string.IsNullOrEmpty(submissionitem.Id) ? int.Parse(submissionitem.Id) : 0);
                 //    model.SubmissionId = submissionitem.Id ?? "";
                 //    //model.FormId = (!string.IsNullOrEmpty(submissionitem.Form[0].Id) ? int.Parse(submissionitem.Form[0].Id) : 0);
                 //    model.FormId = submissionitem.Form[0].Id ?? "";
                 //    model.FormName = submissionitem.Form[0].Name.text ?? "" ;
                 //    model.SubFormSatus = submissionitem.Form[0].Status.text ?? "";
                 //    //model.SubFormVersion = (!string.IsNullOrEmpty(submissionitem.Form[0].Version.text) ? int.Parse(submissionitem.Form[0].Version.text) : 0);
                 //    model.SubFormVersion = submissionitem.Form[0].Version.text ?? "";
                 //    model.Date = (!string.IsNullOrEmpty(submissionitem.Date.Text) ? Convert.ToDateTime(submissionitem.Date.Text) : Convert.ToDateTime("1900/01/01"));
                 //    model.DeviceDate = (!string.IsNullOrEmpty(submissionitem.DeviceDate.Text) ? Convert.ToDateTime(submissionitem.DeviceDate.Text) : Convert.ToDateTime("1900/01/01"));
                 //    model.UserName = submissionitem.UserName.Text ?? "";
                 //    model.FirstName = submissionitem.FirstName.Text ?? "";
                 //    model.LastName = submissionitem.LastName.Text ?? "";
                 //    model.ResponseID = submissionitem.ResponseID.Text ?? "";
                 //    model.WebAccessToken = (submissionitem.WebAccessToken != null ? (submissionitem.WebAccessToken.Text ?? "") : "");
                 //    //model.No =  (!string.IsNullOrEmpty(submissionitem.No.Text) ? int.Parse(submissionitem.No.Text) : 0);
                 //    model.No =  submissionitem.No.Text ?? "";
                 //    //model.SubmissionNumber = int.Parse(submissionitem.SubmissionNumber.Text);
                 //    model.SubmissionNumber = submissionitem.SubmissionNumber.Text ?? "";

                 //    InsertSubmission(model);

                 //    //List<Submission_Section1Model> section1Model = new List<Submission_Section1Model>();

                 //    foreach (var section in submissionitem.Sections.Section)
                 //    {

                 //        if (section.Screens.Screen.FirstOrDefault().Responses != null && section.Screens.Screen.FirstOrDefault().Responses.Response.Count >= 1)
                 //        {
                 //            Submission_Section1Model section1Model = new Submission_Section1Model();

                 //            section1Model.SectionName = section.Name.text;
                 //            section1Model.Section_Screen_Name = section.Screens.Screen[0].Name.text;
                 //           // section1Model.submissionModel = model;
                 //            section1Model.SubmissionId = model.SubmissionId;

                 //            foreach (var response in section.Screens.Screen[0].Responses.Response)
                 //            {
                 //                section1Model.Section_Screen_Response_Guid = response.Guid;
                 //                section1Model.Section_Screen_Response_Label = response.Label.Text;
                 //                section1Model.Section_Screen_Response_Value = response.Value.Text;
                 //                section1Model.Section_Screen_Response_Type = response.Type.Text;

                 //                InsertSubmission_Section1(section1Model);
                 //            }

                 //            //foreach (var response in section.Screens.Screen[0].Responses.Response)
                 //            //{
                 //            //    section1Model.Add(new Submission_Section1Model()
                 //            //    {
                 //            //        SectionName = section.Name.text,
                 //            //        Section_Screen_Name = section.Screens.Screen[0].Name.text,
                 //            //        SubmissionNumber = model.SubmissionNumber,
                 //            //        Section_Screen_Response_Guid = response.Guid,
                 //            //        Section_Screen_Response_Label = response.Label.Text,
                 //            //        Section_Screen_Response_Value = response.Value.Text,
                 //            //        Section_Screen_Response_Type = response.Type.Text
                 //            //    });
                 //            //}

                 //            //InsertSubmission_Section1(section1Model);
                 //        }
                 //        else if (section.Screens.Screen.FirstOrDefault().ResponseGroups != null && section.Screens.Screen.FirstOrDefault().ResponseGroups.ResponseGroup.Count >= 1)
                 //        {
                 //            Submission_Section2Model section2Model = new Submission_Section2Model();

                 //            section2Model.SectionName = section.Name.text;
                 //            section2Model.Section_Screen_Name = section.Screens.Screen[0].Name.text;
                 //            section2Model.SubmissionId = model.SubmissionId;
                 //            //section2Model.submissionModel2 = model;

                 //            foreach (var resgrp in section.Screens.Screen[0].ResponseGroups.ResponseGroup)
                 //            {
                 //                section2Model.Section_Screen_ResponseGroup_Guid = resgrp.Guid;
                 //                section2Model.Section_Screen_ResponseGroup_Response_Label = resgrp.Response.Label.Text;
                 //                section2Model.Section_Screen_ResponseGroup_Response_Value = resgrp.Response.Value.Text;
                 //                section2Model.Section_Screen_ResponseGroup_Response_Type = resgrp.Response.Type.Text;

                 //                InsertSubmission_Section2(section2Model);

                 //                Submission_Section2_ResourceGroupModel section2ResGrpModel = new Submission_Section2_ResourceGroupModel();
                 //                section2ResGrpModel.ResGrp_Section_Name = resgrp.Section.Name.text;
                 //                section2ResGrpModel.SubmissionId = model.SubmissionId;

                 //                foreach (var sec2resgrp in resgrp.Section.Screens.Screen)
                 //                {
                 //                    section2ResGrpModel.ResGrp_Section_Screen_Name = sec2resgrp.Name.text;
                 //                    foreach (var sec2resgrpres in sec2resgrp.Responses.Response)
                 //                    {
                 //                        section2ResGrpModel.ResGrp_Section_Screen_Response_Guid = sec2resgrpres.Guid;
                 //                        section2ResGrpModel.ResGrp_Section_Screen_Response_Label = sec2resgrpres.Label.Text;
                 //                        section2ResGrpModel.ResGrp_Section_Screen_Response_Type = sec2resgrpres.Type.Text;
                 //                        section2ResGrpModel.ResGrp_Section_Screen_Response_Value = sec2resgrpres.Value.Text;

                 //                        //sec2resgrpres.Numbers.Number.Count
                 //                        InsertSubmission_Section2_ResourceGroup(section2ResGrpModel);

                 //                        if (sec2resgrpres.Numbers != null && sec2resgrpres.Numbers.Number != null)
                 //                        {
                 //                            ImageDataModel imageDataModel = new ImageDataModel();
                 //                            //imageDataModel.ImageId = Convert.ToInt64(sec2resgrpres.Value.Text);
                 //                            imageDataModel.ImageId = sec2resgrpres.Value.Text ?? "";
                 //                            //imageDataModel.SubmissionId = model.SubmissionId;

                 //                            foreach (var img in sec2resgrpres.Numbers.Number)
                 //                            {
                 //                                //imageDataModel.ImageNumber = int.Parse(img.Text);
                 //                                imageDataModel.ImageNumber = img.Text ?? "";

                 //                                InsertImages(imageDataModel);
                 //                            }

                 //                        }
                 //                    }
                 //                }
                 //            }
                 //        }
                 //    }
                 //}


                 //scope.Complete();
                 //foreach (var entity in _dbContext.ChangeTracker.Entries())
                 //{
                 //    entity.State = EntityState.Detached;
                 //}

                 //}

                 #endregion
                 //}
                 result = true;

             }
             catch (Exception ex)
             {
                 //result = false;
                 throw;
             }

             return result;
         }*/

        #region current working method as of 8/31/2021
        //public async Task<bool> InsertAllSubmissionData(SubmissionRootObject _listObject, ApiUtility _apiUtilities)
        //{
        //    bool result = false;
        //    using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
        //    {
        //        try
        //        {


        //            //using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //            //{

        //            List<SubmissionModel> listSubModel = _listObject.Submissions.Submission.Select(x => new SubmissionModel
        //            {


        //                SubmissionId = x.Id,
        //                FormId = x.Form[0].Id ?? "",
        //                FormName = x.Form[0].Name.text ?? "",
        //                SubFormSatus = x.Form[0].Status.text ?? "",
        //                SubFormVersion = x.Form[0].Version.text ?? "",
        //                Date = (!string.IsNullOrEmpty(x.Date.Text) ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.Date.Text), TimeZoneInfo.Local) : Convert.ToDateTime("1900/01/01")),
        //                DeviceDate = (!string.IsNullOrEmpty(x.DeviceDate.Text) ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.DeviceDate.Text), TimeZoneInfo.Local) : Convert.ToDateTime("1900/01/01")),
        //                UserName = x.UserName.Text ?? "",
        //                FirstName = x.FirstName.Text ?? "",
        //                LastName = x.LastName.Text ?? "",
        //                ResponseID = x.ResponseID.Text ?? "",
        //                WebAccessToken = (x.WebAccessToken != null ? (x.WebAccessToken.Text ?? "") : ""),
        //                No = x.No.Text ?? "",
        //                SubmissionNumber = x.SubmissionNumber.Text ?? ""

        //            }).ToList();


        //            if (listSubModel != null && listSubModel.Count > 0)
        //                await InsertSubmission(listSubModel);

        //            //throw new Exception();

        //            foreach (var latestobject in _listObject.Submissions.Submission)
        //            {
        //                var submissionId = latestobject.Id;

        //                foreach (var section in latestobject.Sections.Section)
        //                {

        //                    if (section.Screens.Screen.FirstOrDefault().Responses != null && section.Screens.Screen.FirstOrDefault().Responses.Response.Count >= 1)
        //                    {

        //                        List<Submission_Section1Model> listSubmission_Section1Model = section.Screens.Screen[0].Responses.Response.Select(x => new Submission_Section1Model
        //                        {
        //                            SectionName = section.Name.text,
        //                            Section_Screen_Name = section.Screens.Screen[0].Name.text,
        //                            SubmissionId = submissionId,
        //                            Section_Screen_Response_Guid = x.Guid,
        //                            Section_Screen_Response_Label = x.Label.Text,
        //                            Section_Screen_Response_Value = x.Value.Text,
        //                            Section_Screen_Response_Type = x.Type.Text,
        //                        }
        //                        ).ToList();

        //                        if (listSubmission_Section1Model.Count > 0)
        //                            await InsertSubmission_Section1(listSubmission_Section1Model);
        //                    }
        //                    else if (section.Screens.Screen.FirstOrDefault().ResponseGroups != null && section.Screens.Screen.FirstOrDefault().ResponseGroups.ResponseGroup.Count >= 1)
        //                    {
        //                        List<Submission_Section2Model> listSubmission_Section2Model = section.Screens.Screen[0].ResponseGroups.ResponseGroup.Select(x => new Submission_Section2Model
        //                        {
        //                            SectionName = section.Name.text,
        //                            Section_Screen_Name = section.Screens.Screen[0].Name.text,
        //                            SubmissionId = submissionId,
        //                            Section_Screen_ResponseGroup_Guid = x.Guid,
        //                            Section_Screen_ResponseGroup_Response_Guid = x.Response.Guid,
        //                            Section_Screen_ResponseGroup_Response_Label = x.Response.Label.Text,
        //                            Section_Screen_ResponseGroup_Response_Value = x.Response.Value.Text,
        //                            Section_Screen_ResponseGroup_Response_Type = x.Response.Type.Text,
        //                        }).ToList();

        //                        if (listSubmission_Section2Model.Count > 0)
        //                            await InsertSubmission_Section2(listSubmission_Section2Model);

        //                        foreach (var resgrp in section.Screens.Screen[0].ResponseGroups.ResponseGroup)
        //                        {

        //                            foreach (var sec2resgrp in resgrp.Section.Screens.Screen)
        //                            {
        //                                List<Submission_Section2_ResourceGroupModel> listSubmission_Section2_ResourceGroupModel = sec2resgrp.Responses.Response.Select(
        //                                    x => new Submission_Section2_ResourceGroupModel
        //                                    {
        //                                        ResGrp_Section_Name = resgrp.Section.Name.text,
        //                                        ResGrp_Section_Screen_Name = sec2resgrp.Name.text,
        //                                        ResGrp_Section_Screen_Response_Guid = x.Guid,
        //                                        ResGrp_Section_Screen_Response_Label = x.Label.Text,
        //                                        ResGrp_Section_Screen_Response_Type = x.Type.Text,
        //                                        ResGrp_Section_Screen_Response_Value = x.Value.Text,
        //                                        SubmissionId = submissionId,
        //                                        Section_Screen_ResponseGroup_Response_Value = resgrp.Response.Value.Text
        //                                    }
        //                                    ).ToList();

        //                                if (listSubmission_Section2_ResourceGroupModel.Count > 0)
        //                                    await InsertSubmission_Section2_ResourceGroup(listSubmission_Section2_ResourceGroupModel);

        //                                foreach (var sec2resgrpres in sec2resgrp.Responses.Response)
        //                                {
        //                                    if (sec2resgrpres.Label.Text == "Upload Image" && !string.IsNullOrEmpty(sec2resgrpres.Value.Text))
        //                                    {

        //                                        if (sec2resgrpres.Numbers != null && sec2resgrpres.Numbers.Number != null)
        //                                        {
        //                                            List<ImageDataModel> listImageDataModel = sec2resgrpres.Numbers.Number.Select(x => new ImageDataModel
        //                                            {
        //                                                ImageId = sec2resgrpres.Value.Text ?? "",
        //                                                ImageNumber = x.Text ?? "",
        //                                                SubmissionId = submissionId,
        //                                            }).ToList();

        //                                            if (listImageDataModel.Count > 0)
        //                                            {
        //                                                await InsertImages(listImageDataModel);
        //                                                List<ImagesModel> imageModel = listImageDataModel.Select(x => new ImagesModel { ImageId = Convert.ToInt64(x.ImageId), Number = Convert.ToInt32(x.ImageNumber) }).ToList();
        //                                                var imageresult = await _apiUtilities.ImageDataUI(imageModel);
        //                                            }
        //                                        }
        //                                        else
        //                                        {

        //                                            List<ImageDataModel> listImageDataModel = new List<ImageDataModel>();

        //                                            listImageDataModel.Add(new ImageDataModel()
        //                                            {
        //                                                ImageId = sec2resgrpres.Value.Text ?? "",
        //                                                ImageNumber = "0",
        //                                                SubmissionId = submissionId,
        //                                            });


        //                                            //List<ImageDataModel> listImageDataModel = sec2resgrpres.Value.Text.Select(x => new ImageDataModel
        //                                            //{
        //                                            //    ImageId = sec2resgrpres.Value.Text ?? "",
        //                                            //    ImageNumber = "0",
        //                                            //    SubmissionId = submissionId,
        //                                            //}).ToList();

        //                                            if (listImageDataModel.Count > 0)
        //                                            {
        //                                                await InsertImages(listImageDataModel);
        //                                                List<ImagesModel> imageModel = listImageDataModel.Select(x => new ImagesModel { ImageId = Convert.ToInt64(x.ImageId), Number = Convert.ToInt32(x.ImageNumber) }).ToList();
        //                                                var imageresult = await _apiUtilities.ImageDataUI(imageModel);
        //                                            }
        //                                        }


        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }


        //            await transaction.CommitAsync();

        //            result = true;

        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            //result = false;
        //            throw;
        //        }
        //    }


        //    return result;
        //}
        #endregion

        public async Task<bool> InsertAllSubmissionData(List<Submission> _listObject, ApiUtility _apiUtilities)
        {
            bool result = false;
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {


                    //using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    //{

                    List<SubmissionModel> listSubModel = _listObject.Select(x => new SubmissionModel
                    {


                        SubmissionId = x.Id,
                        FormId = x.Form[0].Id ?? "",
                        FormName = x.Form[0].Name.text ?? "",
                        SubFormSatus = x.Form[0].Status.text ?? "",
                        SubFormVersion = x.Form[0].Version.text ?? "",
                        Date = (!string.IsNullOrEmpty(x.Date.Text) ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.Date.Text), TimeZoneInfo.Local) : Convert.ToDateTime("1900/01/01")),
                        DeviceDate = (!string.IsNullOrEmpty(x.DeviceDate.Text) ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.DeviceDate.Text), TimeZoneInfo.Local) : Convert.ToDateTime("1900/01/01")),
                        UserName = x.UserName.Text ?? "",
                        FirstName = x.FirstName.Text ?? "",
                        LastName = x.LastName.Text ?? "",
                        ResponseID = x.ResponseID.Text ?? "",
                        WebAccessToken = (x.WebAccessToken != null ? (x.WebAccessToken.Text ?? "") : ""),
                        No = x.No.Text ?? "",
                        SubmissionNumber = x.SubmissionNumber.Text ?? ""

                    }).ToList();


                    if (listSubModel != null && listSubModel.Count > 0)
                        await InsertSubmission(listSubModel);

                    //throw new Exception();

                    foreach (var latestobject in _listObject)
                    {
                        var submissionId = latestobject.Id;

                        foreach (var section in latestobject.Sections.Section)
                        {

                            if (section.Screens.Screen.FirstOrDefault().Responses != null && section.Screens.Screen.FirstOrDefault().Responses.Response.Count >= 1)
                            {

                                List<Submission_Section1Model> listSubmission_Section1Model = section.Screens.Screen[0].Responses.Response.Select(x => new Submission_Section1Model
                                {
                                    SectionName = section.Name.text,
                                    Section_Screen_Name = section.Screens.Screen[0].Name.text,
                                    SubmissionId = submissionId,
                                    Section_Screen_Response_Guid = x.Guid,
                                    Section_Screen_Response_Label = x.Label.Text,
                                    Section_Screen_Response_Value = x.Value.Text,
                                    Section_Screen_Response_Type = x.Type.Text,
                                }
                                ).ToList();

                                if (listSubmission_Section1Model.Count > 0)
                                    await InsertSubmission_Section1(listSubmission_Section1Model);
                            }
                            else if (section.Screens.Screen.FirstOrDefault().ResponseGroups != null && section.Screens.Screen.FirstOrDefault().ResponseGroups.ResponseGroup.Count >= 1)
                            {
                                List<Submission_Section2Model> listSubmission_Section2Model = section.Screens.Screen[0].ResponseGroups.ResponseGroup.Select(x => new Submission_Section2Model
                                {
                                    SectionName = section.Name.text,
                                    Section_Screen_Name = section.Screens.Screen[0].Name.text,
                                    SubmissionId = submissionId,
                                    Section_Screen_ResponseGroup_Guid = x.Guid,
                                    Section_Screen_ResponseGroup_Response_Guid = x.Response.Guid,
                                    Section_Screen_ResponseGroup_Response_Label = x.Response.Label.Text,
                                    Section_Screen_ResponseGroup_Response_Value = x.Response.Value.Text,
                                    Section_Screen_ResponseGroup_Response_Type = x.Response.Type.Text,
                                }).ToList();

                                if (listSubmission_Section2Model.Count > 0)
                                    await InsertSubmission_Section2(listSubmission_Section2Model);

                                foreach (var resgrp in section.Screens.Screen[0].ResponseGroups.ResponseGroup)
                                {

                                    foreach (var sec2resgrp in resgrp.Section.Screens.Screen)
                                    {
                                        List<Submission_Section2_ResourceGroupModel> listSubmission_Section2_ResourceGroupModel = sec2resgrp.Responses.Response.Select(
                                            x => new Submission_Section2_ResourceGroupModel
                                            {
                                                ResGrp_Section_Name = resgrp.Section.Name.text,
                                                ResGrp_Section_Screen_Name = sec2resgrp.Name.text,
                                                ResGrp_Section_Screen_Response_Guid = x.Guid,
                                                ResGrp_Section_Screen_Response_Label = x.Label.Text,
                                                ResGrp_Section_Screen_Response_Type = x.Type.Text,
                                                ResGrp_Section_Screen_Response_Value = x.Value.Text,
                                                SubmissionId = submissionId,
                                                Section_Screen_ResponseGroup_Response_Value = resgrp.Response.Value.Text
                                            }
                                            ).ToList();

                                        if (listSubmission_Section2_ResourceGroupModel.Count > 0)
                                            await InsertSubmission_Section2_ResourceGroup(listSubmission_Section2_ResourceGroupModel);

                                        foreach (var sec2resgrpres in sec2resgrp.Responses.Response)
                                        {
                                            if (sec2resgrpres.Label.Text == "Upload Image" && !string.IsNullOrEmpty(sec2resgrpres.Value.Text))
                                            {

                                                if (sec2resgrpres.Numbers != null && sec2resgrpres.Numbers.Number != null)
                                                {
                                                    List<ImageDataModel> listImageDataModel = sec2resgrpres.Numbers.Number.Select(x => new ImageDataModel
                                                    {
                                                        ImageId = sec2resgrpres.Value.Text ?? "",
                                                        ImageNumber = x.Text ?? "",
                                                        SubmissionId = submissionId,
                                                    }).ToList();

                                                    if (listImageDataModel.Count > 0)
                                                    {
                                                        await InsertImages(listImageDataModel);
                                                        List<ImagesModel> imageModel = listImageDataModel.Select(x => new ImagesModel { ImageId = Convert.ToInt64(x.ImageId), Number = Convert.ToInt32(x.ImageNumber) }).ToList();
                                                        var imageresult = await _apiUtilities.ImageDataUI(imageModel);
                                                    }
                                                }
                                                else
                                                {

                                                    List<ImageDataModel> listImageDataModel = new List<ImageDataModel>();

                                                    listImageDataModel.Add(new ImageDataModel()
                                                    {
                                                        ImageId = sec2resgrpres.Value.Text ?? "",
                                                        ImageNumber = "0",
                                                        SubmissionId = submissionId,
                                                    });


                                                    //List<ImageDataModel> listImageDataModel = sec2resgrpres.Value.Text.Select(x => new ImageDataModel
                                                    //{
                                                    //    ImageId = sec2resgrpres.Value.Text ?? "",
                                                    //    ImageNumber = "0",
                                                    //    SubmissionId = submissionId,
                                                    //}).ToList();

                                                    if (listImageDataModel.Count > 0)
                                                    {
                                                        await InsertImages(listImageDataModel);
                                                        List<ImagesModel> imageModel = listImageDataModel.Select(x => new ImagesModel { ImageId = Convert.ToInt64(x.ImageId), Number = Convert.ToInt32(x.ImageNumber) }).ToList();
                                                        var imageresult = await _apiUtilities.ImageDataUI(imageModel);
                                                    }
                                                }


                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }


                    await transaction.CommitAsync();

                    result = true;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    //result = false;
                    throw;
                }
            }


            return result;
        }

        #region Old Code
        //public async Task<bool> InsertAllSubmissionData(SubmissionRootObject _listObject, ApiUtility _apiUtilities)
        //{
        //    bool result = false;
        //    try
        //    {
        //        //foreach (var mainitem in _listObject)
        //        //{
        //        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //        //using (var scope = new TransactionScope())
        //        {
        //            //var latestobject = _listObject.Submissions.Submission.LastOrDefault();

        //            //SubmissionModel model = new SubmissionModel();
        //            //model.SubmissionId = latestobject.Id ?? "";
        //            //model.FormId = latestobject.Form[0].Id ?? "";
        //            //model.FormName = latestobject.Form[0].Name.text ?? "";
        //            //model.SubFormSatus = latestobject.Form[0].Status.text ?? "";
        //            //model.SubFormVersion = latestobject.Form[0].Version.text ?? "";
        //            ////model.Date = (!string.IsNullOrEmpty(latestobject.Date.Text) ? Convert.ToDateTime(latestobject.Date.Text) : Convert.ToDateTime("1900/01/01"));
        //            ////model.DeviceDate = (!string.IsNullOrEmpty(latestobject.DeviceDate.Text) ? Convert.ToDateTime(latestobject.DeviceDate.Text) : Convert.ToDateTime("1900/01/01"));

        //            //model.Date = (!string.IsNullOrEmpty(latestobject.Date.Text) ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(latestobject.Date.Text),TimeZoneInfo.Local) : Convert.ToDateTime("1900/01/01"));
        //            //model.DeviceDate = (!string.IsNullOrEmpty(latestobject.DeviceDate.Text) ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(latestobject.DeviceDate.Text),TimeZoneInfo.Local) : Convert.ToDateTime("1900/01/01"));

        //            //model.UserName = latestobject.UserName.Text ?? "";
        //            //model.FirstName = latestobject.FirstName.Text ?? "";
        //            //model.LastName = latestobject.LastName.Text ?? "";
        //            //model.ResponseID = latestobject.ResponseID.Text ?? "";
        //            //model.WebAccessToken = (latestobject.WebAccessToken != null ? (latestobject.WebAccessToken.Text ?? "") : "");
        //            //model.No = latestobject.No.Text ?? "";
        //            //model.SubmissionNumber = latestobject.SubmissionNumber.Text ?? "";

        //            //await InsertSubmission(model);

        //            var latestobjectlist = _listObject.Submissions.Submission;

        //            List<SubmissionModel> listSubmission_Model = latestobjectlist.Select(x => new SubmissionModel
        //            {
        //                SubmissionId = x.Id,
        //                FormId = x.Form[0].Id ?? "",
        //                FormName = x.Form[0].Name.text ?? "",
        //                SubFormSatus = x.Form[0].Status.text ?? "",
        //                SubFormVersion = x.Form[0].Version.text ?? "",

        //                Date = (!string.IsNullOrEmpty(x.Date.Text) ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.Date.Text), TimeZoneInfo.Local) : Convert.ToDateTime("1900/01/01")),
        //                DeviceDate = (!string.IsNullOrEmpty(x.DeviceDate.Text) ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.DeviceDate.Text), TimeZoneInfo.Local) : Convert.ToDateTime("1900/01/01")),

        //                UserName = x.UserName.Text ?? "",
        //                FirstName = x.FirstName.Text ?? "",
        //                LastName = x.LastName.Text ?? "",
        //                ResponseID = x.ResponseID.Text ?? "",
        //                WebAccessToken = (x.WebAccessToken != null ? (x.WebAccessToken.Text ?? "") : ""),
        //                No = x.No.Text ?? "",
        //                SubmissionNumber = x.SubmissionNumber.Text ?? ""
        //            }).ToList();

        //            if (listSubmission_Model.Count > 0)
        //                await InsertSubmission(listSubmission_Model);

        //            foreach (var latestobject in latestobjectlist)
        //            {
        //                foreach (var section in latestobject.Sections.Section)
        //                {

        //                    if (section.Screens.Screen.FirstOrDefault().Responses != null && section.Screens.Screen.FirstOrDefault().Responses.Response.Count >= 1)
        //                    {

        //                        //foreach (var response in section.Screens.Screen[0].Responses.Response)
        //                        //{
        //                        //    Submission_Section1Model section1Model = new Submission_Section1Model();
        //                        //    section1Model.SectionName = section.Name.text;
        //                        //    section1Model.Section_Screen_Name = section.Screens.Screen[0].Name.text;
        //                        //    // section1Model.submissionModel = model;
        //                        //    section1Model.SubmissionId = model.SubmissionId;

        //                        //    section1Model.Section_Screen_Response_Guid = response.Guid;
        //                        //    section1Model.Section_Screen_Response_Label = response.Label.Text;
        //                        //    section1Model.Section_Screen_Response_Value = response.Value.Text;
        //                        //    section1Model.Section_Screen_Response_Type = response.Type.Text;

        //                        //    InsertSubmission_Section1(section1Model);
        //                        //}

        //                        List<Submission_Section1Model> listSubmission_Section1Model = section.Screens.Screen[0].Responses.Response.Select(x => new Submission_Section1Model
        //                        {
        //                            //  Sub_Section1_table_id = new Guid(),
        //                            SectionName = section.Name.text,
        //                            Section_Screen_Name = section.Screens.Screen[0].Name.text,

        //                            //SubmissionId = model.SubmissionId,
        //                            SubmissionId = latestobject.Id,

        //                            Section_Screen_Response_Guid = x.Guid,
        //                            Section_Screen_Response_Label = x.Label.Text,
        //                            Section_Screen_Response_Value = x.Value.Text,
        //                            Section_Screen_Response_Type = x.Type.Text,
        //                        }
        //                        ).ToList();

        //                        if (listSubmission_Section1Model.Count > 0)
        //                            await InsertSubmission_Section1(listSubmission_Section1Model);
        //                    }
        //                    else if (section.Screens.Screen.FirstOrDefault().ResponseGroups != null && section.Screens.Screen.FirstOrDefault().ResponseGroups.ResponseGroup.Count >= 1)
        //                    {
        //                        List<Submission_Section2Model> listSubmission_Section2Model = section.Screens.Screen[0].ResponseGroups.ResponseGroup.Select(x => new Submission_Section2Model
        //                        {
        //                            SectionName = section.Name.text,
        //                            Section_Screen_Name = section.Screens.Screen[0].Name.text,
        //                            //SubmissionId = model.SubmissionId,
        //                            SubmissionId = latestobject.Id,
        //                            Section_Screen_ResponseGroup_Guid = x.Guid,
        //                            Section_Screen_ResponseGroup_Response_Guid = x.Response.Guid,
        //                            Section_Screen_ResponseGroup_Response_Label = x.Response.Label.Text,
        //                            Section_Screen_ResponseGroup_Response_Value = x.Response.Value.Text,
        //                            Section_Screen_ResponseGroup_Response_Type = x.Response.Type.Text,
        //                        }).ToList();

        //                        if (listSubmission_Section2Model.Count > 0)
        //                            await InsertSubmission_Section2(listSubmission_Section2Model);


        //                        //Submission_Section2Model section2Model = new Submission_Section2Model();

        //                        //section2Model.SectionName = section.Name.text;
        //                        //section2Model.Section_Screen_Name = section.Screens.Screen[0].Name.text;
        //                        //section2Model.SubmissionId = model.SubmissionId;
        //                        ////section2Model.submissionModel2 = model;

        //                        foreach (var resgrp in section.Screens.Screen[0].ResponseGroups.ResponseGroup)
        //                        {
        //                            //section2Model.Section_Screen_ResponseGroup_Guid = resgrp.Guid;
        //                            //section2Model.Section_Screen_ResponseGroup_Response_Guid = resgrp.Response.Guid;
        //                            //section2Model.Section_Screen_ResponseGroup_Response_Label = resgrp.Response.Label.Text;
        //                            //section2Model.Section_Screen_ResponseGroup_Response_Value = resgrp.Response.Value.Text;
        //                            //section2Model.Section_Screen_ResponseGroup_Response_Type = resgrp.Response.Type.Text;

        //                            //InsertSubmission_Section2(section2Model);

        //                            //Submission_Section2_ResourceGroupModel section2ResGrpModel = new Submission_Section2_ResourceGroupModel();
        //                            //section2ResGrpModel.ResGrp_Section_Name = resgrp.Section.Name.text;
        //                            //section2ResGrpModel.SubmissionId = model.SubmissionId;

        //                            foreach (var sec2resgrp in resgrp.Section.Screens.Screen)
        //                            {
        //                                List<Submission_Section2_ResourceGroupModel> listSubmission_Section2_ResourceGroupModel = sec2resgrp.Responses.Response.Select(
        //                                    x => new Submission_Section2_ResourceGroupModel
        //                                    {
        //                                        ResGrp_Section_Name = resgrp.Section.Name.text,
        //                                        ResGrp_Section_Screen_Name = sec2resgrp.Name.text,
        //                                        ResGrp_Section_Screen_Response_Guid = x.Guid,
        //                                        ResGrp_Section_Screen_Response_Label = x.Label.Text,
        //                                        ResGrp_Section_Screen_Response_Type = x.Type.Text,
        //                                        ResGrp_Section_Screen_Response_Value = x.Value.Text,
        //                                        SubmissionId = latestobject.Id,
        //                                        //SubmissionId = model.SubmissionId,
        //                                        Section_Screen_ResponseGroup_Response_Value = resgrp.Response.Value.Text
        //                                    }
        //                                    ).ToList();

        //                                if (listSubmission_Section2_ResourceGroupModel.Count > 0)
        //                                    await InsertSubmission_Section2_ResourceGroup(listSubmission_Section2_ResourceGroupModel);

        //                                //section2ResGrpModel.ResGrp_Section_Screen_Name = sec2resgrp.Name.text;
        //                                foreach (var sec2resgrpres in sec2resgrp.Responses.Response)
        //                                {
        //                                    //section2ResGrpModel.ResGrp_Section_Screen_Response_Guid = sec2resgrpres.Guid;
        //                                    //section2ResGrpModel.ResGrp_Section_Screen_Response_Label = sec2resgrpres.Label.Text;
        //                                    //section2ResGrpModel.ResGrp_Section_Screen_Response_Type = sec2resgrpres.Type.Text;
        //                                    //section2ResGrpModel.ResGrp_Section_Screen_Response_Value = sec2resgrpres.Value.Text;


        //                                    //InsertSubmission_Section2_ResourceGroup(section2ResGrpModel);
        //                                    if (sec2resgrpres.Label.Text == "Upload Image" && !string.IsNullOrEmpty(sec2resgrpres.Value.Text))
        //                                    {

        //                                        if (sec2resgrpres.Numbers != null && sec2resgrpres.Numbers.Number != null)
        //                                        {
        //                                            List<ImageDataModel> listImageDataModel = sec2resgrpres.Numbers.Number.Select(x => new ImageDataModel
        //                                            {
        //                                                ImageId = sec2resgrpres.Value.Text ?? "",
        //                                                ImageNumber = x.Text ?? "",
        //                                                SubmissionId = latestobject.Id,
        //                                                //SubmissionId = model.SubmissionId
        //                                            }).ToList();

        //                                            if (listImageDataModel.Count > 0)
        //                                            {
        //                                                await InsertImages(listImageDataModel);
        //                                                List<ImagesModel> imageModel = listImageDataModel.Select(x => new ImagesModel { ImageId = Convert.ToInt64(x.ImageId), Number = Convert.ToInt32(x.ImageNumber) }).ToList();
        //                                                var imageresult = await _apiUtilities.ImageDataUI(imageModel);
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            List<ImageDataModel> listImageDataModel = new List<ImageDataModel>();

        //                                            listImageDataModel.Add(new ImageDataModel()
        //                                            {
        //                                                ImageId = sec2resgrpres.Value.Text ?? "",
        //                                                ImageNumber = "0",
        //                                                SubmissionId = latestobject.Id,
        //                                                //SubmissionId = model.SubmissionId
        //                                            });

        //                                            //List<ImageDataModel> listImageDataModel = sec2resgrpres.Label.Text.Select(x => new ImageDataModel
        //                                            //{
        //                                            //    ImageId = sec2resgrpres.Value.Text ?? "",
        //                                            //    ImageNumber = "0",
        //                                            //    SubmissionId = model.SubmissionId
        //                                            //}).ToList();

        //                                            if (listImageDataModel.Count > 0)
        //                                            {
        //                                                await InsertImages(listImageDataModel);
        //                                                List<ImagesModel> imageModel = listImageDataModel.Select(x => new ImagesModel { ImageId = Convert.ToInt64(x.ImageId), Number = Convert.ToInt32(x.ImageNumber) }).ToList();
        //                                                var imageresult = await _apiUtilities.ImageDataUI(imageModel);
        //                                            }
        //                                        }


        //                                    }
        //                                    //if (sec2resgrpres.Numbers != null && sec2resgrpres.Numbers.Number != null)
        //                                    //{
        //                                    //    //ImageDataModel imageDataModel = new ImageDataModel();
        //                                    //    ////imageDataModel.ImageId = Convert.ToInt64(sec2resgrpres.Value.Text);
        //                                    //    //imageDataModel.ImageId = sec2resgrpres.Value.Text ?? "";
        //                                    //    ////imageDataModel.SubmissionId = model.SubmissionId;

        //                                    //    List<ImageDataModel> listImageDataModel = sec2resgrpres.Numbers.Number.Select(x => new ImageDataModel
        //                                    //    {
        //                                    //        ImageId = sec2resgrpres.Value.Text ?? "",
        //                                    //        ImageNumber = x.Text ?? "",
        //                                    //        SubmissionId = model.SubmissionId
        //                                    //    }).ToList();

        //                                    //    //List<ImagesModel> imageModel = sec2resgrpres.Numbers.Number.Select(x => new ImagesModel
        //                                    //    //{
        //                                    //    //    ImageId = Convert.ToInt64(sec2resgrpres.Value.Text),
        //                                    //    //    Number = Convert.ToInt32(x.Text)
        //                                    //    //}).ToList();

        //                                    //    if (listImageDataModel.Count > 0)
        //                                    //    {
        //                                    //        await InsertImages(listImageDataModel);
        //                                    //        List<ImagesModel> imageModel = listImageDataModel.Select(x => new ImagesModel { ImageId = Convert.ToInt64(x.ImageId), Number = Convert.ToInt32(x.ImageNumber) }).ToList();
        //                                    //        var imageresult = await _apiUtilities.ImageDataUI(imageModel);
        //                                    //    }
        //                                    //    //foreach (var img in sec2resgrpres.Numbers.Number)
        //                                    //    //{
        //                                    //    //    //imageDataModel.ImageNumber = int.Parse(img.Text);
        //                                    //    //    imageDataModel.ImageNumber = img.Text ?? "";

        //                                    //    //   // InsertImages(imageDataModel);
        //                                    //    //}

        //                                    //}
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            scope.Complete();
        //        }
        //        #region old code
        //        //foreach (var submissionitem in mainitem.Submissions.Submission)
        //        //{
        //        //    SubmissionModel model = new SubmissionModel();
        //        //    //model.SubmissionId = (!string.IsNullOrEmpty(submissionitem.Id) ? int.Parse(submissionitem.Id) : 0);
        //        //    model.SubmissionId = submissionitem.Id ?? "";
        //        //    //model.FormId = (!string.IsNullOrEmpty(submissionitem.Form[0].Id) ? int.Parse(submissionitem.Form[0].Id) : 0);
        //        //    model.FormId = submissionitem.Form[0].Id ?? "";
        //        //    model.FormName = submissionitem.Form[0].Name.text ?? "" ;
        //        //    model.SubFormSatus = submissionitem.Form[0].Status.text ?? "";
        //        //    //model.SubFormVersion = (!string.IsNullOrEmpty(submissionitem.Form[0].Version.text) ? int.Parse(submissionitem.Form[0].Version.text) : 0);
        //        //    model.SubFormVersion = submissionitem.Form[0].Version.text ?? "";
        //        //    model.Date = (!string.IsNullOrEmpty(submissionitem.Date.Text) ? Convert.ToDateTime(submissionitem.Date.Text) : Convert.ToDateTime("1900/01/01"));
        //        //    model.DeviceDate = (!string.IsNullOrEmpty(submissionitem.DeviceDate.Text) ? Convert.ToDateTime(submissionitem.DeviceDate.Text) : Convert.ToDateTime("1900/01/01"));
        //        //    model.UserName = submissionitem.UserName.Text ?? "";
        //        //    model.FirstName = submissionitem.FirstName.Text ?? "";
        //        //    model.LastName = submissionitem.LastName.Text ?? "";
        //        //    model.ResponseID = submissionitem.ResponseID.Text ?? "";
        //        //    model.WebAccessToken = (submissionitem.WebAccessToken != null ? (submissionitem.WebAccessToken.Text ?? "") : "");
        //        //    //model.No =  (!string.IsNullOrEmpty(submissionitem.No.Text) ? int.Parse(submissionitem.No.Text) : 0);
        //        //    model.No =  submissionitem.No.Text ?? "";
        //        //    //model.SubmissionNumber = int.Parse(submissionitem.SubmissionNumber.Text);
        //        //    model.SubmissionNumber = submissionitem.SubmissionNumber.Text ?? "";

        //        //    InsertSubmission(model);

        //        //    //List<Submission_Section1Model> section1Model = new List<Submission_Section1Model>();

        //        //    foreach (var section in submissionitem.Sections.Section)
        //        //    {

        //        //        if (section.Screens.Screen.FirstOrDefault().Responses != null && section.Screens.Screen.FirstOrDefault().Responses.Response.Count >= 1)
        //        //        {
        //        //            Submission_Section1Model section1Model = new Submission_Section1Model();

        //        //            section1Model.SectionName = section.Name.text;
        //        //            section1Model.Section_Screen_Name = section.Screens.Screen[0].Name.text;
        //        //           // section1Model.submissionModel = model;
        //        //            section1Model.SubmissionId = model.SubmissionId;

        //        //            foreach (var response in section.Screens.Screen[0].Responses.Response)
        //        //            {
        //        //                section1Model.Section_Screen_Response_Guid = response.Guid;
        //        //                section1Model.Section_Screen_Response_Label = response.Label.Text;
        //        //                section1Model.Section_Screen_Response_Value = response.Value.Text;
        //        //                section1Model.Section_Screen_Response_Type = response.Type.Text;

        //        //                InsertSubmission_Section1(section1Model);
        //        //            }

        //        //            //foreach (var response in section.Screens.Screen[0].Responses.Response)
        //        //            //{
        //        //            //    section1Model.Add(new Submission_Section1Model()
        //        //            //    {
        //        //            //        SectionName = section.Name.text,
        //        //            //        Section_Screen_Name = section.Screens.Screen[0].Name.text,
        //        //            //        SubmissionNumber = model.SubmissionNumber,
        //        //            //        Section_Screen_Response_Guid = response.Guid,
        //        //            //        Section_Screen_Response_Label = response.Label.Text,
        //        //            //        Section_Screen_Response_Value = response.Value.Text,
        //        //            //        Section_Screen_Response_Type = response.Type.Text
        //        //            //    });
        //        //            //}

        //        //            //InsertSubmission_Section1(section1Model);
        //        //        }
        //        //        else if (section.Screens.Screen.FirstOrDefault().ResponseGroups != null && section.Screens.Screen.FirstOrDefault().ResponseGroups.ResponseGroup.Count >= 1)
        //        //        {
        //        //            Submission_Section2Model section2Model = new Submission_Section2Model();

        //        //            section2Model.SectionName = section.Name.text;
        //        //            section2Model.Section_Screen_Name = section.Screens.Screen[0].Name.text;
        //        //            section2Model.SubmissionId = model.SubmissionId;
        //        //            //section2Model.submissionModel2 = model;

        //        //            foreach (var resgrp in section.Screens.Screen[0].ResponseGroups.ResponseGroup)
        //        //            {
        //        //                section2Model.Section_Screen_ResponseGroup_Guid = resgrp.Guid;
        //        //                section2Model.Section_Screen_ResponseGroup_Response_Label = resgrp.Response.Label.Text;
        //        //                section2Model.Section_Screen_ResponseGroup_Response_Value = resgrp.Response.Value.Text;
        //        //                section2Model.Section_Screen_ResponseGroup_Response_Type = resgrp.Response.Type.Text;

        //        //                InsertSubmission_Section2(section2Model);

        //        //                Submission_Section2_ResourceGroupModel section2ResGrpModel = new Submission_Section2_ResourceGroupModel();
        //        //                section2ResGrpModel.ResGrp_Section_Name = resgrp.Section.Name.text;
        //        //                section2ResGrpModel.SubmissionId = model.SubmissionId;

        //        //                foreach (var sec2resgrp in resgrp.Section.Screens.Screen)
        //        //                {
        //        //                    section2ResGrpModel.ResGrp_Section_Screen_Name = sec2resgrp.Name.text;
        //        //                    foreach (var sec2resgrpres in sec2resgrp.Responses.Response)
        //        //                    {
        //        //                        section2ResGrpModel.ResGrp_Section_Screen_Response_Guid = sec2resgrpres.Guid;
        //        //                        section2ResGrpModel.ResGrp_Section_Screen_Response_Label = sec2resgrpres.Label.Text;
        //        //                        section2ResGrpModel.ResGrp_Section_Screen_Response_Type = sec2resgrpres.Type.Text;
        //        //                        section2ResGrpModel.ResGrp_Section_Screen_Response_Value = sec2resgrpres.Value.Text;

        //        //                        //sec2resgrpres.Numbers.Number.Count
        //        //                        InsertSubmission_Section2_ResourceGroup(section2ResGrpModel);

        //        //                        if (sec2resgrpres.Numbers != null && sec2resgrpres.Numbers.Number != null)
        //        //                        {
        //        //                            ImageDataModel imageDataModel = new ImageDataModel();
        //        //                            //imageDataModel.ImageId = Convert.ToInt64(sec2resgrpres.Value.Text);
        //        //                            imageDataModel.ImageId = sec2resgrpres.Value.Text ?? "";
        //        //                            //imageDataModel.SubmissionId = model.SubmissionId;

        //        //                            foreach (var img in sec2resgrpres.Numbers.Number)
        //        //                            {
        //        //                                //imageDataModel.ImageNumber = int.Parse(img.Text);
        //        //                                imageDataModel.ImageNumber = img.Text ?? "";

        //        //                                InsertImages(imageDataModel);
        //        //                            }

        //        //                        }
        //        //                    }
        //        //                }
        //        //            }
        //        //        }
        //        //    }
        //        //}


        //        //scope.Complete();
        //        //foreach (var entity in _dbContext.ChangeTracker.Entries())
        //        //{
        //        //    entity.State = EntityState.Detached;
        //        //}

        //        //}

        //        #endregion
        //        //}
        //        result = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        //result = false;
        //        throw;
        //    }

        //    return result;
        //}
        #endregion

    }
}
