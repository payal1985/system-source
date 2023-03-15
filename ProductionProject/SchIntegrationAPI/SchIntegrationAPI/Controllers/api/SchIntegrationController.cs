using SchIntegrationAPI.ApiUtilities;
using SchIntegrationAPI.Models;
using SchIntegrationAPI.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace SchIntegrationAPI.Controllers.api
{
    public class SchIntegrationController : ApiController
    {
        private ISchIntegrationRepository accruentRepository { get; set; }
        ApiUtility apiUtility;
        AppConfigs appConfigs;
        Logger logger;
        EmailNotificationUtility emailNotificationUtility = null;

        public SchIntegrationController()
        {
            logger = LogManager.GetCurrentClassLogger();

            appConfigs = new AppConfigs();
            emailNotificationUtility = new EmailNotificationUtility(appConfigs.EmailConfigs);
            apiUtility = new ApiUtility(appConfigs, emailNotificationUtility);
            accruentRepository = new SchIntegrationRepository(appConfigs.ConnectionString, emailNotificationUtility);

            logger.Info("Initialize the classes");
        }

        [HttpGet]
        public IHttpActionResult Get(string assignment_group)
        {
            bool result = false;
            logger.Info($"Get Request=>{Request.RequestUri}" );

            try
            {
                if (assignment_group == null)
                {
                    logger.Info($"Request Id is Null=>{assignment_group}" );
                    return NotFound();

                }
                else
                {
                   var accruentApisList = apiUtility.GetRequestByAssignGroup(Request.Method.ToString(), assignment_group);
                   logger.Info($"Total Work Order reading from JSON => {accruentApisList.Count}");
                   result = accruentRepository.AddRequestAsync(accruentApisList);
                   logger.Info($"Assignment Group Reading and Insertion Result is =>{result}");

                    return Ok(result);
                }
            }
            catch(Exception ex)
            {
                logger.Info($"Exception Occured =>{ex.Message}");
                var msg = $"Api Crashed, {ex.Message}";
                emailNotificationUtility?.SendEmail(msg);
                //emailNotificationUtility?.SendGmailEmail(msg);
                
                return BadRequest(msg);
            }

        }

 
        //[HttpPost]
        //public string Post(string sysid, [FromBody] XElement value)
        //{
        //    string str = "";
        //    var json = JsonConvert.SerializeXNode(value, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented, true);

        //    // InComingStudent json = JsonConvert.DeserializeObject<InComingStudent>(jstr);
        //    //var json = JsonConvert.DeserializeObject<entry>(jstr);

        //    JObject parent = JObject.Parse(json);

        //    var entry = parent.Value<JObject>("entry").Properties();
        //    foreach (var item in entry)
        //    {
        //        str = item.Name + " : " + item.Value;
        //    }
        //    return str;

            

        //}

        [HttpPatch]
        public async Task<bool> Patch(string sysid, [FromBody] XElement value)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            bool result = false;
            try
            {
                if (value != null)
                {

                    logger.Info($"Patch Request=>{Request.RequestUri}");
                    response = await apiUtility.PatchRequestBySysId(sysid, value.ToString());
                    result = response.IsSuccessStatusCode ? true : false;
                }
                //return response;
                return result;
            }
            catch (Exception ex)
            {
                logger.Info($"Exception Occured =>{ex.Message}");
                var msg = $"Api Crashed, {ex.Message}";
                emailNotificationUtility?.SendEmail(msg);
                return false;
            }
        }

        //public async Task<IHttpActionResult> Get(int id)
        //{
        //    string response = "";
        //    //bool result = false;
        //    try
        //    {
        //        if (id != 0)
        //        {

        //            logger.Info($"Patch Request=>{Request.RequestUri}");
        //            response = await apiUtility.GetRequestBySysId(id);
        //           // result = response.IsSuccessStatusCode ? true : false;
        //        }
        //        //return response;
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Info($"Exception Occured =>{ex.Message}");
        //        // return null;
        //        return null;
        //    }
        //}

        //public string Post([FromBody] XElement value)
        //{
        //    if(value != null)
        //    {
        //        return value.ToString();
        //    }

        //    return null;
        //}

        //// DELETE: api/Accruent/5
        //public void Delete(int id)
        //{
        //}
    }
}
