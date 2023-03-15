using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using InventoryApi.DBContext;
using InventoryApi.DBModels;
using InventoryApi.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventoryApi.DBModels.SSIDBModels;
using InventoryApi.Repository.Interfaces;
using System.Collections.Generic;
using InventoryApi.DBModels.InventoryDBModels;

namespace InventoryApi.Repository
{
    public class HDTicketRepository : IHDTicketRepository
    {

        SSIRequestContext _dbContext;
        InventoryContext _dbContextInventory;
        IConfiguration _configuration { get; }
        private readonly IEmailNotificationRepository _emailNotificationRepository;
        private IAws3Repository _aws3Repository;
        public HDTicketRepository(SSIRequestContext dbContext, InventoryContext dbContextInventory
                                   , IConfiguration configuration
                                   ,IEmailNotificationRepository emailNotificationRepository
                                   , IAws3Repository aws3Repository
                                  )
        {
            _dbContext = dbContext;
            _dbContextInventory = dbContextInventory;
            _configuration = configuration;
            _emailNotificationRepository = emailNotificationRepository;
            _aws3Repository = aws3Repository;

        }

        public async Task<int> CreateHDTicket(InventoryOrderModel model, string mbody)
        {
            int result = 0;

            //string descr = $"{model.CartItem[0].ClientName} Inventory Request from { model.RequestorEmail}\n Request For  : {model.RequestorProjectName}\nLocation     : {model.DestRoom}" +
            //    $"\n Install Date : {model.InstDate}\n Return Date  : {System.DateTime.Now} \n Comments: {model.Comments}";



            //string instDate = String.Join(",", model.CartItem.Distinct().Select(d => Convert.ToDateTime(d.InstDate).Date).ToArray());
            //string destRoom = String.Join(",", model.CartItem.Distinct().Select(d => d.DestRoom).ToArray());
            //string comments = String.Join(",", model.CartItem.Distinct().Select(d => d.Comments).ToArray());

            //string descr = $"{model.CartItem[0].ClientName} Inventory Request from { model.RequestorEmail}\n " +
            //    $"Request For  : {model.RequestorProjectName}\nLocation     : {destRoom}" +
            //    $"\n Install Date : {instDate}\n Return Date  : {System.DateTime.Now} \n Comments: {comments}";

            StringBuilder sbDesc = new StringBuilder();

            foreach(var item in model.CartItem)
            {
                string descr = $"{model.CartItem[0].ClientName} Inventory Request from { model.RequestorEmail}\n " +
                    $"Request For  : {model.RequestorProjectName}\n" +
                    $"Location     : {item.DestRoom}" +
                    $"\n Install Date : {Convert.ToDateTime(item.InstDate).Date}\n Return Date  : {System.DateTime.Now} \n " +
                    $"Comments: {item.Comments}";

                sbDesc.AppendLine(descr);
            }

            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Requests requestsModel = new Requests();
                    //requestsModel.client_id = 12;
                    requestsModel.client_id = model.CartItem[0].ClientID;
                    requestsModel.request_status_id = 1;
                    requestsModel.sub_status = 1;
                    //requestsModel.assignee = GetAssignee(12);
                    requestsModel.assignee = await GetAssignee(model.CartItem[0].ClientID);
                    requestsModel.subject = $"{model.CartItem[0].ClientName} Inventory Request from {model.RequestorEmail}";
                   // requestsModel.description = descr;                   
                    requestsModel.description = sbDesc.ToString();
                    requestsModel.request_type = 154;
                    requestsModel.request_date = System.DateTime.Now;
                    requestsModel.bid_in_process = false;
                    requestsModel.fromStandardsSite = true;
                    requestsModel.installStartDateDue = Convert.ToDateTime(model.CartItem.Min(c=>c.InstDate));
                    //requestsModel.installStartDateDue = Convert.ToDateTime(System.DateTime.Now);
                    requestsModel.installCompleteDateDue = System.DateTime.Now;
                    requestsModel.eu_name = "";
                    requestsModel.eu_email = model.RequestorEmail;
                    requestsModel.eu_phone = "";
                    requestsModel.euc_name = model.RequestorProjectName;
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

                    await _dbContext.AddAsync(requestsModel);
                    await _dbContext.SaveChangesAsync();

                    int request_id = requestsModel.request_id;

                    int attachment_id = await InsertAttachments(mbody, request_id);
                    bool rq_atta_result = await InsertRequestAttachment(attachment_id, request_id);

                    bool emailQueueResult = await InsertEmailQueue(request_id, requestsModel.subject,requestsModel.assignee, model.CartItem[0].ClientID, model.CartItem[0].ClientName,model.CartItem[0].ClientPath);

                    await transaction.CommitAsync();

                    result = request_id;
                }

                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return result;
        }

        private async Task<int> GetAssignee(int hdclient)
        {
            //var assignee = _dbContext.ClientSettings.Where(cs => cs.client_id == hdclient && cs.name == "REQUEST_DEFAULT_ASSIGNEE").Select(s => s.value).ToString();
            var assignee = await _dbContext.ClientSettings.Where(cs => cs.client_id == hdclient && cs.name == "REQUEST_DEFAULT_ASSIGNEE").FirstOrDefaultAsync();

            return !string.IsNullOrEmpty(assignee.value) ? Convert.ToInt32(assignee.value) : 0;
        }

        private async Task<int> InsertAttachments(string mbody, int request_id)
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


            await _dbContext.AddAsync(attachmentsModel);
            await _dbContext.SaveChangesAsync();

            int att_id = attachmentsModel.attachment_id;           

            return att_id;
        }

        private async Task<bool> InsertRequestAttachment(int attachment_id,int request_id)
        {
            try
            {
                RequestAttachment requestAttachmentModel = new RequestAttachment();
                requestAttachmentModel.attachment_id = attachment_id;
                requestAttachmentModel.request_id = request_id;

                await _dbContext.AddAsync(requestAttachmentModel);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
                      
        }

        private async Task<bool> InsertEmailQueue(int request_id,string subject,int assignee,int client_id,string client_name,string client_path)
        {
            //DateTime creator_time = System.DateTime.Now;
            //string client_path = "blom";
           // string craetor_name = "System Job";
            //string request_subject = subject;
            string user_name = $"{client_name} Standards Site Integration";
            //string link = "https://dev.systemsource.com/" + client_path + "/r/rq/" + request_id;
            string link = _configuration.GetValue<string>("HDTicketUrl") + client_path + "/r/rq/" + request_id;

            var userRec = GetUserDetails(assignee);

            string em_body = $"SSIDatabase New Request - Please do not reply\n-----------------------------------------------------------" +
                $"ID      : {request_id}\nLink:{link}\nSubject:{subject}\n------------------------------------------------------------\n" +
                $"A new request {request_id} has been created in SSIDatabase by {user_name}\n\n " +
                $"\n\n------------------------------------------------------------\n" +
                $"Assigned by {user_name} at {System.DateTime.Now.ToString("M/d/yyyy hh:mm tt")}";

         

            EmailQueue emailQueueModel = new EmailQueue();
            //emailQueueModel.client_id = 12;
            emailQueueModel.client_id = client_id;
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

            await _dbContext.AddAsync(emailQueueModel);
            await _dbContext.SaveChangesAsync();

            //send email for testing while going to production need to comment it.
           // _emailNotificationRepository.SendGmailEmailQueue(em_body, "SSIDatabase New Request");

            return false;
        }

        private Users GetUserDetails(int user_id)
        {
            var users = _dbContext.Users.Where(u => u.user_id == user_id).FirstOrDefault();

            return users;
 
        }

        //public async Task<int> CreateWarrentyRequestHDTicket(InventoryItemWarrantyModel model, string mbody)
        //{
        //    int result = 0;                  

        //    var inventoryItemWarrantPurchaseDetails = await _dbContextInventory.InventoryItems.FirstOrDefaultAsync(ii => ii.InventoryItemID == model.InventoryItemID);

        //    string purchaseDetailStr = "";

        //    if(inventoryItemWarrantPurchaseDetails.ProposalNumber != null || inventoryItemWarrantPurchaseDetails.PoOrderNo != null || inventoryItemWarrantPurchaseDetails.PoOrderDate != null)
        //    {
        //        purchaseDetailStr = $"Purchase Details:\n Proposal Number-{inventoryItemWarrantPurchaseDetails.ProposalNumber}" +
        //            $"\n PO Order Number-{inventoryItemWarrantPurchaseDetails.PoOrderNo}" +
        //            $"\n PO Order Date-{inventoryItemWarrantPurchaseDetails.PoOrderDate}";
        //    }

        //    string descr = "";


        //    if (!string.IsNullOrEmpty(purchaseDetailStr))
        //    {
        //        descr = $"{model.ClientName} Warrenty Request from { model.Email}" +
        //                $"\n Description : {model.Description}" +
        //                $"\n Request For : {model.ReqName}\nPull Info : {model.Building},{model.Floor},{model.Room} " +
        //                $"\n Condition : {model.Condition}\n Comments: {model.Comment}" +
        //                $"\n {purchaseDetailStr}";
        //    }
        //    else
        //    {
        //        descr = $"{model.ClientName} Warrenty Request from { model.Email}" +
        //                  $"\n Description : {model.Description}" +
        //                  $"\n Request For : {model.ReqName}\nPull Info : {model.Building},{model.Floor},{model.Room} " +
        //                  $"\n Condition : {model.Condition}\n Comments: {model.Comment}";
        //    }
           


        //    using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            Requests requestsModel = new Requests();
        //            //requestsModel.client_id = 12;
        //            requestsModel.client_id = model.ClientID;
        //            requestsModel.request_status_id = 1;
        //            requestsModel.sub_status = 1;
        //            //requestsModel.assignee = GetAssignee(12);
        //            requestsModel.assignee = await GetAssignee(model.ClientID);
        //            requestsModel.subject = $"{model.ClientName} Inventory Request from {model.Email}";
        //            requestsModel.description = descr;
        //            requestsModel.request_type = 154; //stocking
        //            requestsModel.request_date = System.DateTime.Now;
        //            requestsModel.bid_in_process = false;
        //            requestsModel.fromStandardsSite = true;
        //            //requestsModel.installStartDateDue = Convert.ToDateTime(model.requested_inst_date);
        //            requestsModel.installCompleteDateDue = System.DateTime.Now;
        //            requestsModel.eu_name = "";
        //            requestsModel.eu_email = model.Email;
        //            requestsModel.eu_phone = "";
        //            requestsModel.euc_name = model.ReqName;
        //            requestsModel.euc_email = "";
        //            requestsModel.euc_phone = "";
        //            requestsModel.eur_name = "";
        //            requestsModel.eur_email = "";
        //            requestsModel.eur_phone = "";
        //            requestsModel.createid = 0;
        //            requestsModel.createdate = System.DateTime.Now;
        //            requestsModel.createprocess = 4;
        //            requestsModel.updateid = 0;
        //            requestsModel.updatedate = System.DateTime.Now;
        //            requestsModel.updateprocess = 4;

        //            await _dbContext.AddAsync(requestsModel);
        //            await _dbContext.SaveChangesAsync();

        //            int request_id = requestsModel.request_id;

        //            int attachment_id = await InsertWarrantyAttachments(mbody, request_id, model);
        //            bool rq_atta_result = await InsertRequestAttachment(attachment_id, request_id);

        //            await InsertEmailQueue(request_id, requestsModel.subject, requestsModel.assignee, model.ClientID, model.ClientName,model.ClientPath);
                                        
        //            await transaction.CommitAsync();

        //            result = request_id;
        //        }

        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            throw;
        //        }
        //    }

        //    return result;
        //}

        public async Task<int> CreateWarrentyRequestHDTicket(List<InventoryItemWarrantyModel> model)
        {
            int result = 0;

            StringBuilder sbDesc = new StringBuilder();

            foreach (var item in model)
            {
                List<InventoryItem> inventoryItemWarrantyPurchaseDetails = new List<InventoryItem>();

                if (item.PullQty > 1)
                {
                    inventoryItemWarrantyPurchaseDetails = await _dbContextInventory.InventoryItems
                                                           .Where(ii => ii.InventoryItemID >= item.InventoryItemID && ii.ConditionID == item.ConditionId)
                                                           .Take(item.PullQty)
                                                           .ToListAsync();
                }
                else
                {
                    inventoryItemWarrantyPurchaseDetails = await _dbContextInventory.InventoryItems
                                                           .Where(ii => ii.InventoryItemID == item.InventoryItemID)                                                           
                                                           .ToListAsync();
                }

                StringBuilder sbPurchaseDetail = new StringBuilder();

                if(inventoryItemWarrantyPurchaseDetails.Any(po=>po.ProposalNumber != null || po.PoOrderNo != null || po.PoOrderDate != null))
                {
                    foreach(var poitem in inventoryItemWarrantyPurchaseDetails)
                    {
                        string purchaseDetailStr = "";

                        if (poitem.ProposalNumber != null || poitem.PoOrderNo != null || poitem.PoOrderDate != null)
                        {
                            purchaseDetailStr = $"Purchase Details:\n Proposal Number-{poitem.ProposalNumber}" +
                                                $"\n PO Order Number-{poitem.PoOrderNo}" +
                                                $"\n PO Order Date-{poitem.PoOrderDate}";

                            sbPurchaseDetail.AppendLine(purchaseDetailStr);
                        }

                    }
                }

               
                string descr = "";


                if (!string.IsNullOrEmpty(sbPurchaseDetail.ToString()))
                {
                    descr = $"{item.ClientName} Warrenty Request from { item.Email}" +
                            $"\n Description : {item.Description}" +
                            $"\n Request For : {item.ReqName}\nPull Info : {item.Building},{item.Floor},{item.Room} " +
                            $"\n Condition : {item.Condition}\n Comments: {item.Comment}" +
                            $"\n {sbPurchaseDetail.ToString()}";
                }
                else
                {
                    descr = $"{item.ClientName} Warrenty Request from { item.Email}" +
                              $"\n Description : {item.Description}" +
                              $"\n Request For : {item.ReqName}\nPull Info : {item.Building},{item.Floor},{item.Room} " +
                              $"\n Condition : {item.Condition}\n Comments: {item.Comment}";
                }

                sbDesc.AppendLine(descr);
            }




            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Requests requestsModel = new Requests();
                    //requestsModel.client_id = 12;
                    requestsModel.client_id = model[0].ClientID;
                    requestsModel.request_status_id = 1;
                    requestsModel.sub_status = 1;
                    //requestsModel.assignee = GetAssignee(12);
                    requestsModel.assignee = await GetAssignee(model[0].ClientID);
                    requestsModel.subject = $"{model[0].ClientName} Inventory Warranty Request from {model[0].Email}";
                    requestsModel.description = sbDesc.ToString();
                    requestsModel.request_type = 154; //stocking
                    requestsModel.request_date = System.DateTime.Now;
                    requestsModel.bid_in_process = false;
                    requestsModel.fromStandardsSite = true;
                    //requestsModel.installStartDateDue = Convert.ToDateTime(model.requested_inst_date);
                    requestsModel.installCompleteDateDue = System.DateTime.Now;
                    requestsModel.eu_name = "";
                    requestsModel.eu_email = model[0].Email;
                    requestsModel.eu_phone = "";
                    requestsModel.euc_name = model[0].ReqName;
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

                    await _dbContext.AddAsync(requestsModel);
                    await _dbContext.SaveChangesAsync();

                    int request_id = requestsModel.request_id;

                    await InsertWarrantyRequestFollowup(request_id, model);
                    //int attachment_id = await InsertWarrantyAttachments(request_id, model[0]);
                    //bool rq_atta_result = await InsertRequestAttachment(attachment_id, request_id);

                    await InsertEmailQueue(request_id, requestsModel.subject, requestsModel.assignee, model[0].ClientID, model[0].ClientName, model[0].ClientPath);

                    await transaction.CommitAsync();

                    result = request_id;
                }

                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return result;
        }

        private async Task<bool> InsertWarrantyAttachments(int request_id,int req_followup_id, InventoryItemWarrantyModel model)
        {
            try
            {
                string attDir = _configuration.GetValue<string>("AttDir");

                if (!Directory.Exists($"{attDir}/req{request_id}"))
                {
                    Directory.CreateDirectory($"{attDir}/req{request_id}");
                }
                if (!Directory.Exists($"{attDir}/req{request_id}/reqfu{req_followup_id}"))
                {
                    Directory.CreateDirectory($"{attDir}/req{request_id}/reqfu{req_followup_id}");
                }

                foreach (var file in model.FileData)
                {
                    if (file.Length > 0)
                    {
                        string filePath = Path.Combine($"{attDir}/req{request_id}/reqfu{req_followup_id}", file.FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }

                    string sz = string.Format("{0:n1} KB", (file.Length / 1024f));

                    Attachments attachmentsModel = new Attachments();
                    //attachmentsModel.fname = fn;
                    //attachmentsModel.uname = fn;
                    attachmentsModel.fname = file.FileName;
                    attachmentsModel.uname = file.FileName;
                    attachmentsModel.attachment_type_id = 107;
                    //attachmentsModel.data = attData;
                    attachmentsModel.data = $"req{request_id}/reqfu{req_followup_id}/{file.FileName}";
                    attachmentsModel.sizef = sz;
                    attachmentsModel.deleted = false;
                    attachmentsModel.createid = 0;
                    attachmentsModel.createdate = System.DateTime.Now;
                    attachmentsModel.createprocess = 4;
                    attachmentsModel.updateid = 0;
                    attachmentsModel.updatedate = System.DateTime.Now;
                    attachmentsModel.updateprocess = 4;


                    await _dbContext.AddAsync(attachmentsModel);
                    await _dbContext.SaveChangesAsync();

                    int att_id = attachmentsModel.attachment_id;

                    await InsertWarrantyRequsetFollowupAttachment(request_id, req_followup_id, att_id);
                    //requestfollowupids.Add(att_id);
                }


            }
            catch (Exception ex)
            {
                throw;
            }

            return true;

           // List<int> requestfollowupids = new List<int>();

            //string attDir = _configuration.GetValue<string>("AttDir");
            //string fn = _configuration.GetValue<string>("WarrantyReqFileName")+"_"+model.InventoryImageName;
            //string fn = "123.jpg";
            //string attData = $"req{request_id}/reqfu{req_followup_id}/{fn}";

            ////var document = _aws3Repository.DownloadFileAsync(model.BucketName, model.ImagePath).Result;

            ////string sz = string.Format("{0:n1} KB", (document.Length / 1024f));


            //if (!Directory.Exists($"{attDir}/req{request_id}"))
            //{
            //    Directory.CreateDirectory($"{attDir}/req{request_id}");
            //}
            //if (!Directory.Exists($"{attDir}/req{request_id}/reqfu{req_followup_id}"))
            //{
            //    Directory.CreateDirectory($"{attDir}/req{request_id}/reqfu{req_followup_id}");
            //}

            //// File.WriteAllBytes(attDir + attData, document);

            ////current flow code implemented to download from aws and create to specified location
            ////string savepath = $"{attDir}{attData}";
            ////var document = _aws3Repository.DownloadFileAsync(model.BucketName, model.ImagePath, savepath).Result;

            ////string sz = string.Format("{0:n1} KB", (document / 1024f));

            ////using (FileStream fs = new FileStream(attDir + attData, FileMode.Create))
            ////{
            ////    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
            ////    {
            ////        File.WriteAllBytes(attDir + attData, document);
            ////    }
            ////}

           // string sz = string.Format("{0:n1} KB", (model.FileSize / 1024f));

            
            

           // return requestfollowupids;
        }

        private async Task InsertWarrantyRequestFollowup(int request_id, List<InventoryItemWarrantyModel> model)
        {
            try
            {
                foreach(var item in model)
                {
                    RequestFollowup requestFollowupModel = new RequestFollowup();
                    requestFollowupModel.request_id = request_id;
                    requestFollowupModel.note = item.Comment;
                    requestFollowupModel.active = true;
                    requestFollowupModel.client_visible = true;
                    requestFollowupModel.createid = item.UserId;
                    requestFollowupModel.createdate = System.DateTime.Now;
                    requestFollowupModel.createprocess = 5;
                    requestFollowupModel.updateid = 0;
                    requestFollowupModel.updatedate = System.DateTime.Now;
                    requestFollowupModel.updateprocess = 5;

                    await _dbContext.AddAsync(requestFollowupModel);
                    await _dbContext.SaveChangesAsync();

                    int rq_followup_id = requestFollowupModel.request_followup_id;

                    await InsertWarrantyAttachments(request_id, rq_followup_id, item);
                    //int attachment_id = await InsertWarrantyAttachments(request_id, rq_followup_id, item);
                    //bool rq_atta_result = await InsertRequestAttachment(attachment_id, request_id);
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private async Task InsertWarrantyRequsetFollowupAttachment(int request_id,int req_followup_id, int att_id)
        {
            try
            {
                RequestFollowupAttachment requestFollowupAttachment = new RequestFollowupAttachment();
                requestFollowupAttachment.request_followup_id = req_followup_id;
                requestFollowupAttachment.request_id = request_id;
                requestFollowupAttachment.attachment_id = att_id;

                await _dbContext.AddAsync(requestFollowupAttachment);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}


