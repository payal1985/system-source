using GoCanvasAPI.ApiUtilities;
using GoCanvasAPI.Models;
using GoCanvasAPI.Repository;
using GoCanvasAPI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoCanvasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ISubmissionRepository _submissionRepository;

        ApiUtility apiUtility;

        public SubmissionsController(IConfiguration config, ISubmissionRepository submissionRepository)
        {
            //logger = LogManager.GetCurrentClassLogger();

            //appConfigs = new AppConfigs();
            _config = config;
            _submissionRepository = submissionRepository;
            apiUtility = new ApiUtility(_config);

            //accruentRepository = new AccruentRepository(appConfigs.ConnectionString, emailNotificationUtility);

            //logger.Info("Initialize the classes");
        }

        // GET: api/<SubmissionsController>
        [HttpGet]
        public async Task<IActionResult> Get(string formName)
        {
            bool result = false;
            //logger.Info($"Get Request=>{Request.RequestUri}");
            SubmissionRootObject submissionModel = new SubmissionRootObject();

            try
            {
                if (formName == null)
                {
                    // logger.Info($"Request Id is Null=>{assignment_group}");
                      return NotFound();
                    //return null;

                }
                else
                {
                    submissionModel = await apiUtility.SubmissionDataUI(formName);

                    //check for the submission exists or not for this formname
                    var subModel = _submissionRepository.GetLatestSubmission(submissionModel);

                    bool insertSubmissionData = (subModel != null && subModel.Count > 0) ?
                            await _submissionRepository.InsertAllSubmissionData(subModel, apiUtility) :
                            false;

                    //bool insertSubmissionData = (submissionModel.Submissions.Submission != null && submissionModel.Submissions.Submission.Count > 0) ?
                    //        await _submissionRepository.InsertAllSubmissionData(submissionModel, apiUtility) :
                    //        false;

                   // bool insertSubmissionData = true;

                    result = (insertSubmissionData) ? true : false;

                    return Ok(result);

                    //var canvasResult = await apiUtility.GetSubmissionData(formName);
                    //var canvasResult = new List<CanvasResult>();
                    //result = accruentRepository.AddRequestAsync(accruentApisList);
                    //logger.Info($"Result is =>{result}");


                    //return canvasResult;
                }
            }
            catch (Exception ex)
            {
                //logger.Info($"Exception Occured =>{ex.Message}");
                var msg = $"Api Crashed, {ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";

                return BadRequest(msg);
                //return null;
            }

        }

        // GET api/<SubmissionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SubmissionsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SubmissionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SubmissionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
