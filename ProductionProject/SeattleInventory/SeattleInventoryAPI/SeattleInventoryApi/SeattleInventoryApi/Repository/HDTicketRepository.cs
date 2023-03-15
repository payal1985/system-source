using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using SeattleInventoryApi.DBContext;
using SeattleInventoryApi.DBModels;
using SeattleInventoryApi.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SeattleInventoryApi.Repository
{
    public class HDTicketRepository : IHDTicketRepository
    {

        SSIRequestContext _dbContext;
        IConfiguration _configuration { get; }
        private readonly IEmailNotificationRepository _emailNotificationRepository;

        public HDTicketRepository(SSIRequestContext dbContext
                                   ,IConfiguration configuration
                                   ,IEmailNotificationRepository emailNotificationRepository
                                  )
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _emailNotificationRepository = emailNotificationRepository;
        }

        public bool CreateHDTicket(InventoryOrderModel model, string mbody)
        {
            bool result = false;
            
            string descr = $"{model.cart_item[0].client_name} Inventory Request from { model.requestoremail}\n Request For  : {model.request_individual_project}\nLocation     : {model.destination_room}" +
                $"\n Install Date : {model.requested_inst_date}\n Return Date  : {System.DateTime.Now} \n Comments: {model.comments}";

            
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Requests requestsModel = new Requests();
                    requestsModel.client_id = 12;
                    //requestsModel.client_id = model.cart_item[0].client_id;
                    requestsModel.request_status_id = 1;
                    requestsModel.sub_status = 1;
                    requestsModel.assignee = GetAssignee(12);
                    //requestsModel.assignee = GetAssignee(model.cart_item[0].client_id);
                    requestsModel.subject = $"{model.cart_item[0].client_name} Inventory Request from {model.requestoremail}";
                    requestsModel.description = descr;
                    requestsModel.request_type = 154;
                    requestsModel.request_date = System.DateTime.Now;
                    requestsModel.bid_in_process = false;
                    requestsModel.fromStandardsSite = true;
                    requestsModel.installStartDateDue = Convert.ToDateTime(model.requested_inst_date);
                    requestsModel.installCompleteDateDue = System.DateTime.Now;
                    requestsModel.eu_name = "";
                    requestsModel.eu_email = model.requestoremail;
                    requestsModel.eu_phone = "";
                    requestsModel.euc_name = model.request_individual_project;
                    requestsModel.euc_email = "";
                    requestsModel.euc_phone = "";
                    requestsModel.eur_name = "";
                    requestsModel.eur_email = "";
                    requestsModel.eur_phone = "";
                    requestsModel.createid = 0;
                    requestsModel.createdate = System.DateTime.Now;
                    requestsModel.createprocess = 4;
                    requestsModel.updateid = 0;
                    requestsModel.updatedate = System.DateTime.Now;
                    requestsModel.updateprocess = 4;

                    _dbContext.Add(requestsModel);
                    _dbContext.SaveChanges();

                    int request_id = requestsModel.request_id;

                    int attachment_id = InsertAttachments(mbody, request_id);
                    bool rq_atta_result = InsertRequestAttachment(attachment_id, request_id);

                    InsertEmailQueue(request_id, requestsModel.subject,requestsModel.assignee, model.cart_item[0].client_id, model.cart_item[0].client_name);

                    transaction.Commit();

                    result = true;
                }

                catch (Exception ex)
                {
                    transaction.RollbackAsync();
                    throw;
                }
            }

            return result;
        }

        private int GetAssignee(int hdclient)
        {
            //var assignee = _dbContext.ClientSettings.Where(cs => cs.client_id == hdclient && cs.name == "REQUEST_DEFAULT_ASSIGNEE").Select(s => s.value).ToString();
            var assignee = _dbContext.ClientSettings.Where(cs => cs.client_id == hdclient && cs.name == "REQUEST_DEFAULT_ASSIGNEE").FirstOrDefault().value;

            return !string.IsNullOrEmpty(assignee) ? Convert.ToInt32(assignee) : 0;
        }

        private int InsertAttachments(string mbody, int request_id)
        {
            string attDir = _configuration.GetValue<string>("AttDir");
            string fn = _configuration.GetValue<string>("FileName");
            string attData = $"req{request_id}/req{request_id }/{fn}";
          
            string sz = string.Format("{0:n1} KB", (mbody.Length / 1024f));
           

            if (!Directory.Exists($"{attDir}/req{request_id}"))
            {
                Directory.CreateDirectory($"{attDir}/req{request_id}");
            }
            if (!Directory.Exists($"{attDir}/req{request_id}/req{request_id}"))
            {               
                Directory.CreateDirectory($"{attDir}/req{request_id}/req{request_id}");
            }

            using (FileStream fs = new FileStream(attDir + attData, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(mbody);
                }
            }

            Attachments attachmentsModel = new Attachments();
            attachmentsModel.fname = fn;
            attachmentsModel.uname = fn;
            attachmentsModel.attachment_type_id = 1;
            attachmentsModel.data = attData;
            attachmentsModel.sizef = sz;
            attachmentsModel.deleted = false;
            attachmentsModel.createid = 0;
            attachmentsModel.createdate = System.DateTime.Now;
            attachmentsModel.createprocess = 4;
            attachmentsModel.updateid = 0;
            attachmentsModel.updatedate = System.DateTime.Now;
            attachmentsModel.updateprocess = 4;


            _dbContext.Add(attachmentsModel);
            _dbContext.SaveChanges();

            int att_id = attachmentsModel.attachment_id;           

            return att_id;
        }

        private bool InsertRequestAttachment(int attachment_id,int request_id)
        {
            try
            {
                RequestAttachment requestAttachmentModel = new RequestAttachment();
                requestAttachmentModel.attachment_id = attachment_id;
                requestAttachmentModel.request_id = request_id;

                _dbContext.Add(requestAttachmentModel);
                _dbContext.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
                      
        }

        private bool InsertEmailQueue(int request_id,string subject,int assignee,int client_id,string client_name)
        {
            DateTime creator_time = System.DateTime.Now;
            //string client_path = "blom";
           // string craetor_name = "System Job";
            string request_subject = subject;
            string user_name = $"{client_name} Standards Site Integration";
            //string link = "https://dev.systemsource.com/" + client_path + "/r/rq/" + request_id;
            string link = _configuration.GetValue<string>("HDTicketUrl") + request_id;

            var userRec = GetUserDetails(assignee);

            string em_body = $"SSIDatabase New Request - Please do not reply\n-----------------------------------------------------------" +
                $"ID      : {request_id}\nLink:{link}\nSubject:{subject}\n------------------------------------------------------------\n" +
                $"A new request {request_id} has been created in SSIDatabase by {user_name}\n\n " +
                $"\n\n------------------------------------------------------------\n" +
                $"Assigned by {user_name} at {System.DateTime.Now.ToString("M/d/yyyy hh:mm tt")}";

         

            EmailQueue emailQueueModel = new EmailQueue();
            emailQueueModel.client_id = 12;
           // emailQueueModel.client_id = client_id;
            emailQueueModel.user_id = 1;
            emailQueueModel.stype = 9;
            emailQueueModel.skey = request_id;//2021-11-11 08:00:02.000
            //emailQueueModel.scheduled_time = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss:000"));

            //DateTime myDateTime = DateTime.Now;
            //string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //DateTime newMyDate = Convert.ToDateTime(sqlFormattedDate);

            emailQueueModel.scheduled_time = System.DateTime.Now;
            emailQueueModel.status = 0;
            //emailQueueModel.sdata = "";
            emailQueueModel.tip = "Hoag Ticket";
            emailQueueModel.em_subject = "SSIDatabase New Request";
            emailQueueModel.em_body = em_body;
            emailQueueModel.em_to = userRec.email;
            //emailQueueModel.em_to = "PPatel.IC@SystemSource.com";
            emailQueueModel.em_cc = "PPatel.IC@SystemSource.com";
            emailQueueModel.createid = 0;
            emailQueueModel.createdate = System.DateTime.Now;
            emailQueueModel.createprocess = 4;
            emailQueueModel.updateid = 0;
            emailQueueModel.updatedate = System.DateTime.Now;
            emailQueueModel.updateprocess = 4;

            _dbContext.Add(emailQueueModel);
            _dbContext.SaveChanges();

            //send email for testing while going to production need to comment it.
           // _emailNotificationRepository.SendGmailEmailQueue(em_body, "SSIDatabase New Request");

            return false;
        }

        private Users GetUserDetails(int user_id)
        {
            var users = _dbContext.Users.Where(u => u.user_id == user_id).FirstOrDefault();

            return users;
 
        }

        public bool CreateWarrentyRequestHDTicket(InventoryOrderModel model, string mbody)
        {
            bool result = false;

            string descr = $"{model.cart_item[0].client_name} Warrenty Request from { model.requestoremail}" + 
                $"\n Description : {model.cart_item[0].description}"+
                $"\n Request For  : {model.request_individual_project}\nPull Info     : {model.cart_item[0].building},{model.cart_item[0].floor},{model.cart_item[0].mploc} " +
                $"\n Condition : {model.cart_item[0].cond}\n Comments: {model.comments}";


            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Requests requestsModel = new Requests();
                    requestsModel.client_id = 12;
                    //requestsModel.client_id = model.cart_item[0].client_id;
                    requestsModel.request_status_id = 1;
                    requestsModel.sub_status = 1;
                    requestsModel.assignee = GetAssignee(12);
                    //requestsModel.assignee = GetAssignee(model.cart_item[0].client_id);
                    requestsModel.subject = $"{model.cart_item[0].client_name} Inventory Request from {model.requestoremail}";
                    requestsModel.description = descr;
                    requestsModel.request_type = 154; //stocking
                    requestsModel.request_date = System.DateTime.Now;
                    requestsModel.bid_in_process = false;
                    requestsModel.fromStandardsSite = true;
                    //requestsModel.installStartDateDue = Convert.ToDateTime(model.requested_inst_date);
                    requestsModel.installCompleteDateDue = System.DateTime.Now;
                    requestsModel.eu_name = "";
                    requestsModel.eu_email = model.requestoremail;
                    requestsModel.eu_phone = "";
                    requestsModel.euc_name = model.request_individual_project;
                    requestsModel.euc_email = "";
                    requestsModel.euc_phone = "";
                    requestsModel.eur_name = "";
                    requestsModel.eur_email = "";
                    requestsModel.eur_phone = "";
                    requestsModel.createid = 0;
                    requestsModel.createdate = System.DateTime.Now;
                    requestsModel.createprocess = 4;
                    requestsModel.updateid = 0;
                    requestsModel.updatedate = System.DateTime.Now;
                    requestsModel.updateprocess = 4;

                    _dbContext.Add(requestsModel);
                    _dbContext.SaveChanges();

                    int request_id = requestsModel.request_id;

                    int attachment_id = InsertWarrantyAttachments(mbody, request_id);
                    bool rq_atta_result = InsertRequestAttachment(attachment_id, request_id);

                    InsertEmailQueue(request_id, requestsModel.subject, requestsModel.assignee, model.cart_item[0].client_id, model.cart_item[0].client_name);

                    transaction.Commit();

                    result = true;
                }

                catch (Exception ex)
                {
                    transaction.RollbackAsync();
                    throw;
                }
            }

            return result;
        }

        private int InsertWarrantyAttachments(string mbody, int request_id)
        {
            string attDir = _configuration.GetValue<string>("AttDir");
            string fn = _configuration.GetValue<string>("WarrantyReqFileName");
            string attData = $"req{request_id}/req{request_id }/{fn}";

            string sz = string.Format("{0:n1} KB", (mbody.Length / 1024f));


            if (!Directory.Exists($"{attDir}/req{request_id}"))
            {
                Directory.CreateDirectory($"{attDir}/req{request_id}");
            }
            if (!Directory.Exists($"{attDir}/req{request_id}/req{request_id}"))
            {
                Directory.CreateDirectory($"{attDir}/req{request_id}/req{request_id}");
            }

            using (FileStream fs = new FileStream(attDir + attData, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(mbody);
                }
            }

            Attachments attachmentsModel = new Attachments();
            attachmentsModel.fname = fn;
            attachmentsModel.uname = fn;
            attachmentsModel.attachment_type_id = 107;
            attachmentsModel.data = attData;
            attachmentsModel.sizef = sz;
            attachmentsModel.deleted = false;
            attachmentsModel.createid = 0;
            attachmentsModel.createdate = System.DateTime.Now;
            attachmentsModel.createprocess = 4;
            attachmentsModel.updateid = 0;
            attachmentsModel.updatedate = System.DateTime.Now;
            attachmentsModel.updateprocess = 4;


            _dbContext.Add(attachmentsModel);
            _dbContext.SaveChanges();

            int att_id = attachmentsModel.attachment_id;

            return att_id;
        }


    }
}


