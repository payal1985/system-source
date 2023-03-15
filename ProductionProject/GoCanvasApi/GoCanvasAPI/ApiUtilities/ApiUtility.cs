using GoCanvasAPI.Models;
using GoCanvasAPI.ViewModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace GoCanvasAPI.ApiUtilities
{
    public class ApiUtility
    {
        //string BaseAddressUrl;
        private readonly IConfiguration _config;
        private string username { get; set; }
        private string password { get; set; }

        public ApiUtility(IConfiguration config)
        {

            _config = config;
            username = _config.GetValue<string>("GoCanvasAccessCredential:UserName");
            password = _config.GetValue<string>("GoCanvasAccessCredential:Pwd");
            //BaseAddressUrl = _config.GetValue<string>("BaseAddressUrl");
        }

        public async Task<HttpResponseMessage> ApiGetCallMethod(string webApi)
        {
            //var clientBaseAddress = ConfigurationManager.AppSettings["ClientBaseAddress"].ToString();
            //var clientBaseAddress = _appConfigs.BaseAddressUrl;
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                using (HttpClient client = new HttpClient())
                {                    
                    client.BaseAddress = new Uri(_config.GetValue<string>("BaseAddressUrl"));
                    //string bearerToken = string.Format("gU6qd8fyXetl0fEQSowxi5YJwUg/HgmgbLtgbcG4pzY=");
                    string bearerToken = string.Format(_config.GetValue<string>("BearerToken"));
                    var header = new AuthenticationHeaderValue("Bearer", bearerToken);
                    client.DefaultRequestHeaders.Authorization = header;
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    //response = await client.GetAsync(webApi).Result;                    
                    response = await client.GetAsync(webApi);                    
                }
            }
            catch (Exception ex)
            {
                var exMsg = $"Error Message due ApiCallMethod to connect service now url =>{ex.Message}";
                //Logger.Info(exMsg);
                
            }

            return response;
        }

        public async Task<CanvasResult> GetFormsData()
        {
            CanvasResult canvasResult = new CanvasResult();

            //string webApi = "apiv2/forms.xml?username=" + userName;
            string webApi = _config.GetValue<string>("WebApiUrl:GetForms") + username;
            var response = await ApiGetCallMethod(webApi);

            //var result = response.Content.ReadAsStringAsync().Result;
            var result = await response.Content.ReadAsStringAsync();

            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(response.Content.ReadAsStringAsync().Result);
            //string result1 = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CanvasResult));
            using (StringReader textReader = new StringReader(result))
            {
                canvasResult = (CanvasResult)xmlSerializer.Deserialize(textReader);
            }

            return canvasResult;
            //yield return canvasResult;
        }

        public async Task<CanvasResult> GetSubmissionData(string formName)
        {
            CanvasResult canvasResult = new CanvasResult();

            try
            {
                string webApi = _config.GetValue<string>("WebApiUrl:GetSubmission") + username + "&password=" + password + "&form_name=" + formName;
                var response = await ApiGetCallMethod(webApi);

                //var result = response.Content.ReadAsStringAsync().Result;
                var result = await response.Content.ReadAsStringAsync();

                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(response.Content.ReadAsStringAsync().Result);
                //string result1 = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(CanvasResult));
                using (StringReader textReader = new StringReader(result))
                {
                    canvasResult = (CanvasResult)xmlSerializer.Deserialize(textReader);
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            
            //yield return canvasResult;
            return canvasResult;
        }

        //public byte[] GetImageByImageId(long image_id, string username, string password, int? number)
        public async Task<byte[]> GetImageByImageId(long image_id, int number)
        {
            string webApi = _config.GetValue<string>("WebApiUrl:GetImages");

            if(number > 0)
                webApi = webApi + image_id + "&number=" + number + "&username=" + username + "&password=" + password;
            else
                webApi = webApi + image_id + "&username="+ username + "&password=" + password;

            var response = await ApiGetCallMethod(webApi);

            //if (response.Content != null && response.IsSuccessStatusCode)
            //{
            //    // var json = response.Content.ReadAsStreamAsync().Result;                       
            //    response.Content.ReadAsByteArrayAsync().Result;
            //}
            return  await response.Content.ReadAsByteArrayAsync();          

        }


        #region UI Render Methods

        public async Task<HttpResponseMessage> ApiCallMethodForUI(string webApi)
        {
            //var clientBaseAddress = ConfigurationManager.AppSettings["ClientBaseAddress"].ToString();
            //var clientBaseAddress = _appConfigs.BaseAddressUrl;
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:25197/api/");
                    //HTTP GET
                    
                    response =await client.GetAsync(webApi);

                    var json = response.Content.ReadAsStringAsync().Result;

                }
            }
            catch (Exception ex)
            {
                var exMsg = $"Error Message due ApiCallMethod to connect service now url =>{ex.Message}";
                //Logger.Info(exMsg);

            }

            return response;
        }

        //public List<RootObject> FormDataUI()
        public async Task<RootObject> FormDataUI()
        {
            // string webapi = "gocanvas";
            // HttpResponseMessage response = ApiCallMethodForUI(webapi);
            // var json = response.Content.ReadAsStringAsync().Result;

            //// var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ViewModel.Forms>>(json);
            // var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RootObject>>(json);

            // return data;

            var canvasResult = await GetFormsData();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(canvasResult);
            //var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RootObject>>(json);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);

            //return await Task.FromResult(data.ToList()); ;
            return data;

        }

        //public async Task<List<SubmissionRootObject>> SubmissionDataUI(string formname)
        public async Task<SubmissionRootObject> SubmissionDataUI(string formname)
        {
            // string webapi = "submissions" + "?formName="+ formname;
            //HttpResponseMessage response =await ApiCallMethodForUI(webapi);
            //var json = response.Content.ReadAsStringAsync().Result;

            //// var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ViewModel.Forms>>(json);
            //var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubmissionRootObject>>(json);

            var canvasResult = await GetSubmissionData(formname);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(canvasResult);
            //var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubmissionRootObject>>(json);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<SubmissionRootObject>(json);

           // return await Task.FromResult(data);
            return data;
        }

        public async Task<bool> ImageDataUI(List<ImagesModel> listModelData)        
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool result = false;
            try
            {
                foreach (var item in listModelData)
                {
                    var imageBinary = await GetImageByImageId(item.ImageId, item.Number);

                    //apiUtility.GetImageByImageId(image_id, number);

                    /*write to localfolder code*/
                    
                    string localpath = string.Format(_config.GetValue<string>("AttachmentLocation"));

                    if (!Directory.Exists(localpath))
                    {
                        Directory.CreateDirectory(localpath);
                    }

                    string fullPath = (item.Number > 0) ? localpath + item.ImageId + "_" + item.Number + ".jpg" : localpath + item.ImageId + ".jpg";

                    System.IO.File.WriteAllBytes(fullPath, imageBinary);
                }

                result = true;
                //foreach (var item in listModelData)
                //{
                //    foreach (var imgNum in item.Number)
                //    {
                //        string webapi = "files" + "?image_id=" + item.ImageId + "&number=" + imgNum;
                //        response = await ApiCallMethodForUI(webapi);

                //        var json = response.Content.ReadAsStringAsync().Result;
                //        var data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                //    }
                //}

                //result = true;
                //// var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ViewModel.Forms>>(json);
            }
            catch(Exception ex)
            {
                throw;
            }


            return result;
        }

       // public async Task<HttpResponseMessage> ReferenceDataUI(string xml)
        public async Task<bool> ReferenceDataUI(string xml)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool result = false;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_config.GetValue<string>("BaseAddressUrl"));
                    string webapiurl = _config.GetValue<string>("WebApiUrl:PostReferenceData") + username + "&password=" + password;
                    string bearerToken = string.Format(_config.GetValue<string>("BearerToken"));
                    var header = new AuthenticationHeaderValue("Bearer", bearerToken);
                    client.DefaultRequestHeaders.Authorization = header;
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    var Content = new StringContent(xml, Encoding.UTF8, "application/xml");
                    //response = client.PostAsync(webapiurl, Content).Result;
                    response = await client.PostAsync(webapiurl, Content);

                    result = (response.IsSuccessStatusCode) ? true : false;

                }
            }
            catch (Exception ex)
            {
                var exMsg = $"Error Message due to post refernce data =>{ex.Message}";
                //Logger.Info(exMsg);

            }

            return result;
        }

        public bool MoveImageData(string img_id,string img_name)
        {
            bool result = false;
           
            try
            {
                ////test code.
                //string sourcelocalpath = string.Format(_config.GetValue<string>("AttachmentLocation"));
                //string destlocalpath = string.Format(_config.GetValue<string>("ReAttachmentLocation"));
                //if (!Directory.Exists(destlocalpath))
                //{
                //    Directory.CreateDirectory(destlocalpath);                  

                //}
                //else
                //{
                //    string sourcefullPath = sourcelocalpath + "11512182336" + ".jpg";
                //    string destfullPath = destlocalpath + "11512182336" + ".jpg";

                //    if (File.Exists(sourcefullPath))
                //    {
                //        File.Move(sourcefullPath, destfullPath);
                //        result = true;
                //    }
                //}
         

                string sourcelocalpath = string.Format(_config.GetValue<string>("AttachmentLocation"));
                string destlocalpath = string.Format(_config.GetValue<string>("ReAttachmentLocation"));

                if (Directory.Exists(sourcelocalpath))
                {
                    string sourcefullPath = sourcelocalpath + img_id + ".jpg";
                    string destfullPath = destlocalpath + img_name;

                    if (File.Exists(sourcefullPath))
                    {
                        //File.Copy(sourcefullPath, destfullPath,true);
                        //File.Delete(sourcefullPath);
                        File.Move(sourcefullPath, destfullPath,true);

                        if (File.Exists(destfullPath))
                            result = true;
                       
                    }                   
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return result;

            //string fullPath = (item.Number > 0) ? localpath + item.ImageId + "_" + item.Number + ".jpg" : localpath + item.ImageId + ".jpg";

           // System.IO.File.WriteAllBytes(fullPath, imageBinary);
        }
        #endregion
    }
}
