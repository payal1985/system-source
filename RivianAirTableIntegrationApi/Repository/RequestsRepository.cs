using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RivianAirtableIntegrationApi.ApiUtilities;
using RivianAirtableIntegrationApi.DBContext;
using RivianAirtableIntegrationApi.DBModels;
using RivianAirtableIntegrationApi.Models;
using RivianAirtableIntegrationApi.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RivianAirtableIntegrationApi.Repository
{
    public class RequestsRepository : IRequestsRepository
    {
        SSIDBContext _ssidbContext;
        private readonly ApiUtility _apiUtility;
        private IConfiguration _configuration { get; }
        private readonly IEmailNotificationRepository _emailNotificationRepository;
        public RequestsRepository(SSIDBContext ssidbContext
                                ,IConfiguration configuration
                                ,IEmailNotificationRepository emailNotificationRepository)
        {
            _apiUtility = new ApiUtility(configuration);
            _ssidbContext = ssidbContext;   
            _configuration = configuration;
            _emailNotificationRepository = emailNotificationRepository;
        }

        public string GetConnectionString()
        {
            string connString = _configuration.GetConnectionString("SSIDB");
            return connString;
        }
        public async Task<List<RecordsModel>> GetRequestRecords()
        {
            var recordmodel = new List<RecordsModel>();
            try
            {
                var result = await _apiUtility.ApiCall();

                if(result != null)
                {
                    var json = result.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(json))
                    {
                        JObject jObject = JObject.Parse(json);
                        //JToken jUserRecords = jObject["records"];
                        //recordmodel = JsonConvert.DeserializeObject<List<RecordsModel>>(jUserRecords.ToString());

                        foreach (JProperty p in jObject.Properties())
                        {
                            JArray value = (JArray)p.Value;

                            recordmodel = value.ToObject<List<RecordsModel>>();

                            foreach (JObject item in value)
                            {
                                if (item["fields"]["OAC Kickoff"] != null)
                                {
                                    recordmodel.Where(rm => rm.id == item["id"].ToString()).Select(r => r.fields.OAC_Kickoff = item["fields"]["OAC Kickoff"].ToString()).ToList();

                                }
                                if (item["fields"]["Ancillary Install Date"] != null)
                                {
                                    recordmodel.Where(rm => rm.id == item["id"].ToString()).Select(r => r.fields.Ancillary_Install_Date = item["fields"]["Ancillary Install Date"].ToString()).ToList();

                                }
                                if (item["fields"]["Workstation Install Date"] != null)
                                {
                                    recordmodel.Where(rm => rm.id == item["id"].ToString()).Select(r => r.fields.Workstation_Install_Date = item["fields"]["Workstation Install Date"].ToString()).ToList();

                                }
                                if (item["fields"]["SSI Project Team"] != null)
                                {
                                    var ssiprojectteam = item["fields"]["SSI Project Team"].Values<string>().ToArray()[0];
                                    recordmodel.Where(rm => rm.id == item["id"].ToString()).Select(r => r.fields.SSIProjectTeam = ssiprojectteam).ToList();
                                }
                                if (item["fields"]["Rivian PM"] != null)
                                {
                                    var rivianpm = item["fields"]["Rivian PM"].Values<string>().ToArray()[0];
                                    recordmodel.Where(rm => rm.id == item["id"].ToString()).Select(r => r.fields.RivianPM = rivianpm).ToList();
                                }
                                if (item["fields"]["Rivian DM"] != null)
                                {
                                    var riviandm = item["fields"]["Rivian DM"].Values<string>().ToArray()[0];
                                    recordmodel.Where(rm => rm.id == item["id"].ToString()).Select(r => r.fields.RivianDM = riviandm).ToList();
                                }

                            }
                        }


                        //JToken jUserRecords = jObject["records"];
                        //recordmodel = JsonConvert.DeserializeObject<List<RecordsModel>>(jUserRecords.ToString());
                    }

                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return recordmodel;

        }

        public async Task<bool> InsertUpdateRequestRecords(List<RecordsModel> models)
        {
            bool result = false;
            try
            {
                var entity = await _ssidbContext.Requests.Where(r => r.client_id == 140).ToListAsync();

                if(entity != null && entity.Count > 0)
                {
                    var matchUpdateList = models.Where(m => entity.Any(e => e.frequest_id6 == m.id)).ToList();
                    var nonMatchInsertList = models.Where(m => entity.All(e => e.frequest_id6 != m.id)).ToList();

                    //var matchUpdateList = entity.Where(e => models.Any(m => m.fields.id == e.frequest_id6)).ToList();
                    //var nonMatchInsertList = entity.Where(e => models.Any(m => m.fields.id != e.frequest_id6)).ToList();

                    if (matchUpdateList != null && matchUpdateList.Count > 0)
                    {
                        //perform updation
                        //Dictionary<int, List<string>> updatedCols = new Dictionary<int, List<string>>();

                       // var existsEntity = entity.Where(e => matchUpdateList.Any(m => m.fields.id == e.frequest_id6)).ToList();

                        foreach(var matchUpdate in matchUpdateList)
                        {
                            var item = entity.Where(e => e.frequest_id6 == matchUpdate.id).FirstOrDefault();                         

                            if(item != null)
                            {
                                Dictionary<int, List<string>> updatedCols = new Dictionary<int, List<string>>();

                                List<string> columnNames = new List<string>();

                                if (item.eu_name != matchUpdate.fields.RivianPM)
                                {
                                    //columnNames.Add("eu_name");
                                    columnNames.Add("Rivian PM");
                                    item.eu_name = matchUpdate.fields.RivianPM;
                                }
                                if (item.requested_due_date != Convert.ToDateTime(matchUpdate.fields.gate5_date))
                                {
                                    columnNames.Add("Gate 5 / Opening");
                                    item.requested_due_date = Convert.ToDateTime(matchUpdate.fields.gate5_date);
                                    //item.requested_due_date = matchUpdate.fields.gate5_date;
                                }
                                if (item.frequest_id4 != matchUpdate.fields.square_footage)
                                {
                                    columnNames.Add("Square Footage");
                                    item.frequest_id4 = matchUpdate.fields.square_footage;
                                }
                                if (item.frequest_id6 != matchUpdate.id)
                                {
                                    columnNames.Add("frequest_id6");
                                    item.frequest_id6 = matchUpdate.id;
                                }

                                if (item.eup_name != matchUpdate.fields.RivianDM)
                                {
                                    columnNames.Add("Rivian DM");
                                    item.eup_name = matchUpdate.fields.RivianDM;
                                }
                                if (item.romDateDue != Convert.ToDateTime(matchUpdate.fields.gate3_date))
                                {
                                    columnNames.Add("Gate 3");
                                    item.romDateDue = Convert.ToDateTime(matchUpdate.fields.gate3_date);
                                }
                                if (item.installStartDateDue != Convert.ToDateTime(matchUpdate.fields.Workstation_Install_Date))
                                {
                                    columnNames.Add("Workstation Install Date");
                                    item.installStartDateDue = Convert.ToDateTime(matchUpdate.fields.Workstation_Install_Date);
                                }
                                if (item.occupancyDateDue != Convert.ToDateTime(matchUpdate.fields.gate5_date))
                                {
                                    if(!columnNames.Contains("Gate 5 / Opening"))
                                    {
                                        columnNames.Add("Gate 5 / Opening");
                                    }
                                    //columnNames.Add("occupancyDateDue");
                                    item.occupancyDateDue = Convert.ToDateTime(matchUpdate.fields.gate5_date);
                                }
                                //if (item.progressPaymentDateDue != Convert.ToDateTime(matchUpdate.fields.Workstation_Install_Date).Date.AddDays(-(6 * 7)))
                                //{
                                //    columnNames.Add("OAC Call Kickoff");
                                //    item.progressPaymentDateDue = Convert.ToDateTime(matchUpdate.fields.Workstation_Install_Date).Date.AddDays(-(6 * 7));
                                //}
                                if (item.installStartDate2Due != Convert.ToDateTime(matchUpdate.fields.Ancillary_Install_Date))
                                {
                                    columnNames.Add("Ancillary Install Start Date");
                                    item.installStartDate2Due = Convert.ToDateTime(matchUpdate.fields.Ancillary_Install_Date);
                                }

                                if (columnNames.Count > 0)
                                {
                                    updatedCols.Add(item.request_id, columnNames);

                                    //item.updatedate = System.DateTime.Now;

                                    _ssidbContext.Update(item);
                                    await _ssidbContext.SaveChangesAsync();

                                    string changesColumn = string.Join(",", columnNames);

                                    string urllink = $"{_configuration.GetValue<string>("HDLink")}{item.request_id}";
                                    string emailsubject = $"Airtable Update for Request ID: {item.request_id}";
                                    string emailbody = $"Hi SSI Team,<br/><br/>There has been an update from Rivian in the Airtable for Request Id: {item.request_id}<br/>" +
                                        $"Url:-<a href={urllink}>{urllink}</a><br/><br/>Changes occurred in the following fields: {changesColumn} <br/><br/>Thank You,<br/>SSI|Database";

                                    var user = await GetUserDetails(item.ssi_manager);

                                    _emailNotificationRepository.UpdateSendEmail(emailsubject, emailbody, user.email); //production environment need to enable
                                    //_emailNotificationRepository.UpdateSendEmail(emailsubject, emailbody, "PPatel.IC@SystemSource.com"); //staging environment need to enable


                                }
                            }                            

                        }
                    }

                    if (nonMatchInsertList != null && nonMatchInsertList.Count > 0)
                    {
                        //perform new request insertion...

                        foreach(var item in nonMatchInsertList)
                        {
                            var request = new Requests();
                            request.client_id = 140;
                            request.assignee = 58;
                            request.request_type = 16;
                            request.subject = $"{item.fields.city}, {item.fields.state}, {item.fields.id}, {item.fields.project_sub_type}";
                            request.description = $"{item.fields.address1},\n{item.fields.city},\n{item.fields.state},\n{item.fields.zipcode},\n{item.fields.country}";
                            request.eu_name = item.fields.RivianPM;
                            request.requested_due_date = Convert.ToDateTime(item.fields.gate5_date).Date;
                            request.request_date = System.DateTime.Now;
                            request.frequest_id4 = item.fields.square_footage;
                            request.frequest_id6 = item.id;
                            request.eup_name = item.fields.RivianDM;
                            request.romDateDue = Convert.ToDateTime(item.fields.gate3_date).Date;
                            request.installStartDateDue = Convert.ToDateTime(item.fields.Workstation_Install_Date).Date;
                            request.client_branch_id = 362;
                            request.occupancyDateDue = Convert.ToDateTime(item.fields.gate5_date).Date;
                            //request.progressPaymentDateDue = Convert.ToDateTime(item.fields.OAC_Kickoff).Date;
                           // request.progressPaymentDateDue = Convert.ToDateTime(item.fields.Workstation_Install_Date).Date.AddDays(-(6*7));
                            request.installStartDate2Due = Convert.ToDateTime(item.fields.Ancillary_Install_Date).Date;
                            request.createdate = System.DateTime.Now;
                            request.createid = 3;
                            request.createprocess = 5;
                            request.updatedate = System.DateTime.Now;
                            request.updateid = 3;
                            request.updateprocess = 3;
                            request.request_status_id = 1;

                            await _ssidbContext.AddAsync(request);
                            await _ssidbContext.SaveChangesAsync();

                            int request_id = request.request_id;

                            string urllink = $"{_configuration.GetValue<string>("HDLink")}{request_id}";
                            string emailsubject = $"New Request Created for Request Order :{item.id}";
                            string emailbody = $"Hello,<br/><br/>New Request created for Request Id:- {request_id},<br/><br/>" +
                                $"Url:-<a href={urllink}>{urllink}</a> <br/><br/>Thank You,<br/>Team";

                            var user = await GetUserDetails(58);

                            _emailNotificationRepository.InsertSendEmail(emailsubject, emailbody, user.email);//production environment need to enable
                            //_emailNotificationRepository.InsertSendEmail(emailsubject, emailbody, "PPatel.IC@SystemSource.com"); //staging environment need to enable
                            
                        }

                        #region alternate method code
                        //List<Requests> requestlist = new List<Requests>();


                        //requestlist = nonMatchInsertList.Select(x => new Requests()
                        //{
                        //    client_id = 140,
                        //    assignee = 58,
                        //    request_type = 16,
                        //    subject = $"{x.fields.city}, {x.fields.state}, {x.fields.id}, {x.fields.project_sub_type}",
                        //    description = $"{x.fields.address1},\n{x.fields.city},\n{x.fields.state},\n{x.fields.zipcode},\n{x.fields.country}",
                        //    eu_name = x.fields.RivianPM,
                        //    requested_due_date = Convert.ToDateTime(x.fields.gate5_date).Date,
                        //    request_date = System.DateTime.Now,
                        //    frequest_id4 = x.fields.square_footage,
                        //    frequest_id6 = x.id,
                        //    eup_name = x.fields.RivianDM,
                        //    romDateDue = Convert.ToDateTime(x.fields.gate3_date).Date,
                        //    installStartDateDue= Convert.ToDateTime(x.fields.Workstation_Install_Date).Date,
                        //    client_branch_id = 362,
                        //    occupancyDateDue = Convert.ToDateTime(x.fields.gate5_date).Date,
                        //    progressPaymentDateDue = Convert.ToDateTime(x.fields.OAC_Kickoff).Date,
                        //    installStartDate2Due = Convert.ToDateTime(x.fields.Ancillary_Install_Date).Date
                        //}).ToList();

                        //await _ssidbContext.AddRangeAsync(requestlist);
                        //await _ssidbContext.SaveChangesAsync();

                        //var request_ids = requestlist.Select(r => {return r.request_id; }).ToList();

                        //if(request_ids.Count > 0)
                        //{
                        //    foreach(var id in request_ids)
                        //    {
                        //        string urllink = $"{_configuration.GetValue<string>("HDLink")}{id}";
                        //        string emailsubject = $"New Request Created for Request Order :{matchUpdate.fields.id}";
                        //        string emailbody = $"Hello,<br/><br/>New Request created for Request_Id:- {item.request_id},<br/><br/>" +
                        //            $"Url:-<a href={urllink}>{urllink}</a> <br/><br/>Thank You,<br/>Team";

                        //        var user = await GetUserDetails(58);
                        //    }


                        //}
                        #endregion
                    }

                    result = true;
                }
            }
            catch(Exception ex)
            {
                throw;
            }

            return result;
        }

        private async Task<Users> GetUserDetails(int? user_id)
        {
            var users = await _ssidbContext.Users.Where(u => u.user_id == user_id).FirstOrDefaultAsync();

            return users;

        }
    }
}
