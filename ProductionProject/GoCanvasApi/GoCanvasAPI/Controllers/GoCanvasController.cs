using GoCanvasAPI.ApiUtilities;
using GoCanvasAPI.DBModels;
using GoCanvasAPI.Models;
using GoCanvasAPI.Repository;
using GoCanvasAPI.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoCanvasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoCanvasController : ControllerBase
    {
        private readonly IConfiguration _config;

        ApiUtility apiUtility;
        private readonly IFormRepository _formRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private IHostingEnvironment Environment;
        public GoCanvasController(IConfiguration config, IFormRepository formRepository, ISubmissionRepository submissionRepository, IHostingEnvironment _environment)
        {
            //logger = LogManager.GetCurrentClassLogger();

            _config = config;
            _formRepository = formRepository;
            _submissionRepository = submissionRepository;
            Environment = _environment;
            apiUtility = new ApiUtility(_config);

            //accruentRepository = new AccruentRepository(appConfigs.ConnectionString, emailNotificationUtility);

            //logger.Info("Initialize the classes");
        }

        // GET: api/<GoCanvasController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //bool result = false;
            //logger.Info($"Get Request=>{Request.RequestUri}");
            RootObject formModelList = new RootObject();

            try
            {
                //var canvasResult = await apiUtility.GetFormsData();      
                //return Ok(canvasResult);

                var formModel = await apiUtility.FormDataUI();

                var data = JsonConvert.SerializeObject(formModel);
                formModelList = JsonConvert.DeserializeObject<RootObject>(data);

                var formModelListDBContext = formModel.Forms.Form.Select(x => new FormModel()
                {

                    FormId = x.Id ?? "",
                    GUID = x.GUID,
                    Name = x.Name.text,
                    OriginatingLibraryTemplateId = x.OriginatingLibraryTemplateId ?? "",
                    Status = x.Status.text,
                    Version = x.Version.text ?? ""
                }).ToList();


                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _formRepository.InsertForm(formModelListDBContext);

                    scope.Complete();
                }
                return Ok(formModelList.Forms.Form);

            }
            catch (Exception ex)
            {
                //logger.Info($"Exception Occured =>{ex.Message}");
                var msg = $"Api Crashed, {ex.Message}";

                return BadRequest(msg);
                //return null;
            }

            //return new string[] { "value1", "value2" };
        }

        // GET api/<GoCanvasController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GoCanvasController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GoCanvasController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GoCanvasController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
