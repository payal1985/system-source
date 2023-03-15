using GoCanvasAPI.ApiUtilities;
using GoCanvasAPI.DBModels;
using GoCanvasAPI.Repository;
using GoCanvasAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Hosting;

namespace GoCanvasAPI.Controllers.Mvc
{
    public class HomeController : Controller
    {
        ApiUtility apiUtility;
        IConfiguration config;
        private readonly IFormRepository _formRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private IHostingEnvironment Environment;
        public HomeController(IConfiguration _config, IFormRepository formRepository, ISubmissionRepository submissionRepository, IHostingEnvironment _environment)
        {
            config = _config;
            _formRepository = formRepository;
            _submissionRepository = submissionRepository;
            Environment = _environment;
            apiUtility = new ApiUtility(config);
        }

        // GET: HomeController
        //public IActionResult Index(List<RootObject> model,string successMsg = null)
        public async Task<IActionResult> Index(string successMsg = null)
        //public IActionResult Index()
        {

            //List<RootObject> formModelList = new List<RootObject>();
            RootObject formModelList = new RootObject();
            try
            {
                var formModel = await apiUtility.FormDataUI();

                var data = JsonConvert.SerializeObject(formModel);
                //formModelList = JsonConvert.DeserializeObject<List<RootObject>>(data);
                formModelList = JsonConvert.DeserializeObject<RootObject>(data);

               //List<FormModel> formModelListDBContext = new List<FormModel>();

                //List<Form> formList = new List<Form>();
                //formList = formModel.FirstOrDefault().Forms.Form;

                var formModelListDBContext = formModel.Forms.Form.Select(x => new FormModel()
                {

                    FormId = x.Id ?? "",
                    GUID = x.GUID,
                    Name = x.Name.text,
                    OriginatingLibraryTemplateId = x.OriginatingLibraryTemplateId ?? "",
                    Status = x.Status.text,
                    Version = x.Version.text ?? ""
                }).ToList();

                //foreach (var item in formModel.Forms.Form)
                //{
                //    formModelListDBContext.Add(new FormModel()
                //    {
                //        FormId = item.Id ?? "",
                //        GUID = item.GUID,
                //        Name = item.Name.text,
                //        OriginatingLibraryTemplateId = item.OriginatingLibraryTemplateId ?? "",
                //        Status = item.Status.text,
                //        Version = item.Version.text ?? ""
                //    });
                //}

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                       await _formRepository.InsertForm(formModelListDBContext);

                    //foreach (var list in formModelListDBContext)
                    //{
                    //    await _formRepository.InsertForm(list);
                    //}
                    scope.Complete();
                }
                #region oldcode
                //if (TempData["list"] != null)
                //{
                //    formModelList = JsonConvert.DeserializeObject<List<RootObject>>(TempData["list"].ToString());


                //    List<Form> formList = new List<Form>();
                //    formList = formModelList.FirstOrDefault().Forms.Form;

                //    List<FormModel> formModelListDBContext = new List<FormModel>();

                //    foreach (var item in formModelList.FirstOrDefault().Forms.Form)
                //    {
                //        formModelListDBContext.Add(new FormModel()
                //        {
                //            FormId = item.Id ?? "",
                //            GUID = item.GUID,
                //            Name = item.Name.text,
                //            OriginatingLibraryTemplateId = item.OriginatingLibraryTemplateId ?? "",
                //            Status = item.Status.text,
                //            Version = item.Version.text ?? ""
                //        });
                //    }

                //    using (var scope = new TransactionScope())
                //    {
                //        foreach (var list in formModelListDBContext)
                //        {
                //            _formRepository.InsertForm(list);
                //        }
                //        scope.Complete();
                //    }
                //}
                #endregion

               // ViewBag.SuccessMsg = (string.IsNullOrEmpty(successMsg) ? null : successMsg);
            }
            catch(Exception ex)
            {
                throw;
            }

            if(successMsg != null)
             ViewBag.SuccessMsg =  bool.Parse(successMsg);

            return View(formModelList);
        }

        //[HttpPost]
        //public IActionResult Index(string formName,IFormCollection collection)
        //{
        //    return View();
        //}
        
        public async Task<IActionResult> Details(string formname)
        {

           // List<SubmissionRootObject> submissionModel = new List<SubmissionRootObject>();
            SubmissionRootObject submissionModel = new SubmissionRootObject();
            bool successMsg;
            try
            {
                submissionModel = await apiUtility.SubmissionDataUI(formname);
                var subModel = _submissionRepository.GetLatestSubmission(submissionModel);

                // var result = submissionModel[0].Submissions.Submission.GroupBy(s => s.SubmissionNumber, s => s.Sections, (key, g) => new { SubmissionNumber = key, Sections = g.ToList() });
                // var finalresult = result.LastOrDefault().Sections;
                //bool insertSubmissionData = false;

                //if (submissionModel.Submissions.Submission != null && submissionModel.Submissions.Submission.Count > 0)
                //    insertSubmissionData = await _submissionRepository.InsertAllSubmissionData(submissionModel, apiUtility);

                //bool insertSubmissionData = (submissionModel.Submissions.Submission != null && submissionModel.Submissions.Submission.Count > 0) ?
                //                            await _submissionRepository.InsertAllSubmissionData(submissionModel, apiUtility) :
                //                            false;

                bool insertSubmissionData = (subModel != null && subModel.Count > 0) ?
                            await _submissionRepository.InsertAllSubmissionData(subModel, apiUtility) :
                            false;

                #region oldcode
                // var result = (submissionModel[0].Submissions.Submission != null && submissionModel[0].Submissions.Submission.Count > 0) ? submissionModel[0].Submissions.Submission.LastOrDefault() : null;

                //List<ImagesModel> imageModel = new List<ImagesModel>();
                //if (result != null && result.Sections.Section.Count > 0)
                //{
                //    foreach (var item in result.Sections.Section)
                //    {

                //        if (item.Screens.Screen[0].ResponseGroups != null)
                //        {
                //            //var inneritem = item.Section[1].Screens.Screen.FirstOrDefault().ResponseGroups.ResponseGroup;
                //            var inneritem = item.Screens.Screen.Where(s => s.ResponseGroups.ResponseGroup.Count >= 1).Select(s => { return s.ResponseGroups; }).ToList();

                //            foreach (var data in inneritem)
                //            {
                //                foreach (var finaldata in data.ResponseGroup)
                //                {
                //                    if (finaldata.Section.Screens.Screen[1].Responses.Response != null)
                //                    {
                //                        var imagedata = finaldata.Section.Screens.Screen[1].Responses.Response
                //                        .Where(x => x.Label.Text == "Upload Image").ToList();
                //                        //.LastOrDefault().Numbers.Number.ToList();
                //                        // .Where(r => r.Numbers.Number != null).Select(r => { return r.Numbers.Number; }).ToList();

                //                        imageModel.Add(new ImagesModel()
                //                        {
                //                            ImageId = Convert.ToInt64(imagedata.FirstOrDefault().Value.Text),

                //                            Number = imagedata[0].Numbers.Number.Select(s => { return Convert.ToInt32(s.Text); }).ToList()
                //                        });
                //                    }
                //                }
                //            }

                //        }
                //    }
                //}

                // bool imageresult = (insertSubmissionData && imageModel.Count > 0) ? await apiUtility.ImageDataUI(imageModel) : false;

                //successMsg = (insertSubmissionData && imageresult) ? true : false;
                #endregion

                successMsg = (insertSubmissionData) ? true : false;

            }
            catch (Exception ex)
            {              
                successMsg = false;
                throw;
            }

            //return RedirectToAction("Index",new { successMsg = successMsg.ToString() });
            return RedirectToAction(nameof(HomeController.Index), new { successMsg = successMsg.ToString() });

        }



        // GET: HomeController/Details/5
        //public async Task<IActionResult> Details(string formname)
        //{
        //    List<SubmissionRootObject> submissionModel = new List<SubmissionRootObject>();
        //    bool successMsg;
        //    try
        //    {
        //        submissionModel = await apiUtility.SubmissionDataUI(formname);

        //       // var result = submissionModel[0].Submissions.Submission.GroupBy(s => s.SubmissionNumber, s => s.Sections, (key, g) => new { SubmissionNumber = key, Sections = g.ToList() });
        //       // var finalresult = result.LastOrDefault().Sections;

        //        bool insertSubmissionData = (submissionModel[0].Submissions.Submission != null && submissionModel[0].Submissions.Submission.Count > 0) ? await _submissionRepository.InsertAllSubmissionData(submissionModel) : false;

        //        var result = (submissionModel[0].Submissions.Submission != null && submissionModel[0].Submissions.Submission.Count > 0) ?  submissionModel[0].Submissions.Submission.LastOrDefault() : null;

        //        //List<ImagesModel> imageModel = new List<ImagesModel>();
        //        //if (result != null && result.Sections.Section.Count > 0)
        //        //{
        //        //    foreach (var item in result.Sections.Section)
        //        //    {

        //        //        if (item.Screens.Screen[0].ResponseGroups != null)
        //        //        {
        //        //            //var inneritem = item.Section[1].Screens.Screen.FirstOrDefault().ResponseGroups.ResponseGroup;
        //        //            var inneritem = item.Screens.Screen.Where(s => s.ResponseGroups.ResponseGroup.Count >= 1).Select(s => { return s.ResponseGroups; }).ToList();

        //        //            foreach (var data in inneritem)
        //        //            {
        //        //                foreach (var finaldata in data.ResponseGroup)
        //        //                {
        //        //                    if (finaldata.Section.Screens.Screen[1].Responses.Response != null)
        //        //                    {
        //        //                        var imagedata = finaldata.Section.Screens.Screen[1].Responses.Response
        //        //                        .Where(x => x.Label.Text == "Upload Image").ToList();
        //        //                        //.LastOrDefault().Numbers.Number.ToList();
        //        //                        // .Where(r => r.Numbers.Number != null).Select(r => { return r.Numbers.Number; }).ToList();

        //        //                        imageModel.Add(new ImagesModel()
        //        //                        {
        //        //                            ImageId = Convert.ToInt64(imagedata.FirstOrDefault().Value.Text),

        //        //                            Number = imagedata[0].Numbers.Number.Select(s => { return Convert.ToInt32(s.Text); }).ToList()
        //        //                        });
        //        //                    }
        //        //                }
        //        //            }

        //        //        }
        //        //    }
        //        //}

        //       // bool imageresult = (insertSubmissionData && imageModel.Count > 0) ? await apiUtility.ImageDataUI(imageModel) : false;

        //        //successMsg = (insertSubmissionData && imageresult) ? true : false;
        //        successMsg = (insertSubmissionData) ? true : false;
        //    }
        //    catch(Exception ex)
        //    {
        //        ViewBag.Error = $"Error Occured due to Form Data Receiving {ex.Message} \n Inner Exception is=> {ex.InnerException}";

        //        //return RedirectToAction("Index", new { successMsg = false });

        //        // return View("~/Home/Error.cshtml");

        //        successMsg = false;
        //    }
        //    //var model = TempData["list"];
        //    //return View(submissionModel);
        //    return RedirectToAction("Index", new {successMsg = successMsg });
        //}

        [HttpGet]
        public ActionResult ReferenceData()
        {
            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ReferenceData(IFormCollection collection)
        public IActionResult ReferenceData(IFormFile file, IFormCollection collection)
        {
            bool result = false;

            try
            {
                var referenceDatasetName = collection["refdataset"].ToString();
                //string wwwPath = this.Environment.WebRootPath;
                string contentPath = this.Environment.ContentRootPath;

                //string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                string path = Path.Combine(this.Environment.ContentRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                if (Request.Form.Files.Count > 0)
                {

                    string fileName = Path.GetFileName(file.FileName);

                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var dt = GetDataTable(Path.Combine(path, fileName));
                    var xml = LoadCsv(dt, referenceDatasetName);
                    //bool result = apiUtility.ReferenceDataUI(xml);
                    result = true;

                    if (result)
                        System.IO.File.Delete(Path.Combine(path, fileName));
                }

            }
            catch (Exception ex)
            {
                throw;
            }


            ViewBag.Message = result;
            
            return View();
        }

        private System.Data.DataTable GetDataTable(string fileName)
        {
            StreamReader SR = null;
            var dt = new System.Data.DataTable();
            try
            {
                SR = new StreamReader(fileName);
                string line = SR.ReadLine();
                var strArray = line.Split(',');
                
                System.Data.DataRow row;
                foreach (string s in strArray)
                    dt.Columns.Add(new System.Data.DataColumn());
                do
                {
                    line = SR.ReadLine();
                    if (!((line ?? "") == (string.Empty ?? "")))
                    {
                        row = dt.NewRow();
                        row.ItemArray = line.Split(',');
                        dt.Rows.Add(row);
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                if (SR != null)
                    SR.Close();
            }
            
            return dt;
        }

        private string LoadCsv(System.Data.DataTable dt, string refDataSetName)
        {
            string strXML = "<List Name=\"" + refDataSetName + "\">";
            strXML += "<Columns>";
            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                strXML += "<c>";
                strXML += dc.ColumnName;
                strXML += "</c>";
            }

            strXML += "</Columns>";
            strXML += "<Rows>";
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                strXML += "<r>";
                foreach (string value in dr.ItemArray)
                {
                    strXML += "<v>";
                    strXML += value;
                    strXML += "</v>";
                }

                strXML += "</r>";
            }

            strXML += "</Rows>";
            strXML += "</List>";
            return strXML;
        }
        //GET: HomeController/Create
        public ActionResult Create(string items)
        {         

            string successMsg = "";
            //var Model = Request.Form["modelArray"].ToString();
            return Json(successMsg, System.Web.Mvc.JsonRequestBehavior.AllowGet);

            //return View();
            //return RedirectToAction("Details");
        }

        //[HttpPost]
        //public JsonResult Create(List<TestModel> data)
        //{
        //    //var Model = Request.Form["modelArray"].ToString();
        //    //var newModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TestModel>(things);
        //    string successMsg = "Success";
        //    return Json(successMsg, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //}

        //    // POST: HomeController/Create
        //    [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
