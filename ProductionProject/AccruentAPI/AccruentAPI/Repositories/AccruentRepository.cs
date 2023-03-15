using AccruentAPI.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace AccruentAPI.Repositories
{
    public class AccruentRepository : IAccruentRepository
    {

        private static IConnectionUtility ConnectionUtility { get; set; }
        private Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        private Logger LoggerAttachment { get; } = LogManager.GetLogger("logfileattachment");

        private EmailNotificationUtility _emailNotificationUtility = null;

        public AccruentRepository(string connectionString, EmailNotificationUtility emailNotificationUtility, IConnectionUtility connectionUtility = null)
        {
            ConnectionUtility = connectionUtility ?? new ConnectionUtility(connectionString);
            _emailNotificationUtility = emailNotificationUtility;

            if (!CheckConnection(false))
            {
                var msg = "Failed to intialize Database Connection Utility";
                _emailNotificationUtility.SendEmail(msg);
                throw new Exception(msg);
            }
        }

        public bool CheckConnection(bool showGoodResult)
        {
            bool isCnnectionGood;
            Logger.Info("Checking Connection...");

            string exceptionMsg;

            var success = ConnectionUtility.CheckDbConnection(out exceptionMsg);

            if (!success)
            {
                Logger.Info($"Database connection check failed: { exceptionMsg }"); 
                return isCnnectionGood = false; 
            }


            isCnnectionGood = true;

            if (!showGoodResult) { return isCnnectionGood; }

            return isCnnectionGood;
        }

        public bool AddRequestAsync(List<AccruentApi> accruentApisList)
        {
            bool result = false;

            try
            {
                foreach (var data in accruentApisList)
                {


                    //string description = "Building:" + data.u_building + ",\n" + "Floor:" + data.UFloorModel.ualtfullname + ",\n" + "Room:" + data.URoomModel.ualtfullname +
                    //                    ",\n" + "Location:" + data.x_ctv_sc_location_description + ",\n" + "Short Desc.:" + data.short_description + ",\n" + "Description:" + data.description;

                    
                    string req_subject = data.number + " - " + data.URoomModel.ualtfullname + " " + data.description.Split('.')[0];

                    string description = "Building:" + data.u_building + ",\n" + "Floor:" + data.UFloorModel.ualtfullname + ",\n" + "Room:" + data.URoomModel.ualtfullname +
                    ",\n" + "Location:" + data.x_ctv_sc_location_description + ",\n" + "Description:" + data.description;

                    //Checking Description exist or not and set the flag to insert into rivision log
                    bool descriptionNotExists = false;

                    var param = new List<IDbDataParameter>();
                    string outStrDesc = "";
                    param.Add(ConnectionUtility.CreateParameter("@number", 20, data.number, DbType.String));
                    ConnectionUtility.CallStoredProcedureReturnString("SP_RetriveDescription", param.ToArray(), out outStrDesc);

                    if (!string.IsNullOrEmpty(outStrDesc))
                    {                       
                        if (!outStrDesc.Replace("\n", "").ToLower().Equals(description.Replace("\n", "").ToLower(), StringComparison.OrdinalIgnoreCase))
                               descriptionNotExists = true;
                    }

                    //var dsDesc = ConnectionUtility.GetDataSet("SP_RetriveDescription", CommandType.StoredProcedure, param.ToArray());
                    //if (dsDesc.Tables[0].Rows.Count >= 0)
                    //{
                    //    var dr = dsDesc.Tables[0].Rows[0];
                    //    if (Convert.ToInt32(dr["request_id"]) >= 0)
                    //    {
                    //        if (!dr["description"].ToString().Replace("\n", "").ToLower().Equals(description.Replace("\n", "").ToLower(), StringComparison.OrdinalIgnoreCase))
                    //            descriptionNotExists = true;
                    //    }
                    //}
                                        

                    //insert into request,api-staging,rivison log table.
                    var parameters = new List<IDbDataParameter>();
                    
                    parameters.Add(ConnectionUtility.CreateParameter("@number", 20, (!string.IsNullOrEmpty(data.number) ? data.number : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@eu_name", 50, (!string.IsNullOrEmpty(data.CtvScCallerModel.name ) ? data.CtvScCallerModel.name : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@eu_note", 50, (!string.IsNullOrEmpty(data.CtvScCallerModel.title) ? data.CtvScCallerModel.title : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@eu_email", 50, (!string.IsNullOrEmpty(data.CtvScCallerModel.email) ? data.CtvScCallerModel.email : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@frequest_id5", 20, (!string.IsNullOrEmpty(data.priority) ? data.priority : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@description", 8000, (!string.IsNullOrEmpty(description) ? description : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@requested_due_date", 8, TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(data.due_date), TimeZoneInfo.Local), DbType.DateTime));
                    //parameters.Add(ConnectionUtility.CreateParameter("@requested_due_date", 8, Convert.ToDateTime(data.due_date), DbType.DateTime));

                    //commented below line as per email request to concate subject as of 12/09/21 request. //concatenate wo # "- " Room and Description first sentence until "." 
                    //parameters.Add(ConnectionUtility.CreateParameter("@subject", 200, (!string.IsNullOrEmpty(data.short_description) ? data.short_description : ""), DbType.String)); 
                    parameters.Add(ConnectionUtility.CreateParameter("@subject", 200, (!string.IsNullOrEmpty(req_subject) ? req_subject : ""), DbType.String)); 
                    parameters.Add(ConnectionUtility.CreateParameter("@eu_phone", 200, (!string.IsNullOrEmpty(data.CtvScCallerModel.phone) ? data.CtvScCallerModel.phone : ""), DbType.String)); 
                    parameters.Add(ConnectionUtility.CreateParameter("@state", 200, data.state, DbType.Int32));
                    parameters.Add(ConnectionUtility.CreateParameter("@sys_id", 50, (!string.IsNullOrEmpty(data.sys_id) ? data.sys_id : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@x_ctv_sc_category",50, (!string.IsNullOrEmpty(data.x_ctv_sc_category) ? data.x_ctv_sc_category : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@u_building", 50, (!string.IsNullOrEmpty(data.u_building) ? data.u_building : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@floor_u_alt_full_name", 50, (!string.IsNullOrEmpty(data.UFloorModel.ualtfullname) ? data.UFloorModel.ualtfullname : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@room_u_alt_full_name", 50, (!string.IsNullOrEmpty(data.URoomModel.ualtfullname) ? data.URoomModel.ualtfullname : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@x_ctv_sc_location_description", 50, (!string.IsNullOrEmpty(data.x_ctv_sc_location_description) ? data.x_ctv_sc_location_description : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@description_json", 100, (!string.IsNullOrEmpty(data.description) ? data.description : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@comments", 20, (!string.IsNullOrEmpty(data.comments) ? data.comments : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@comment_json", 8000, (!string.IsNullOrEmpty(data.commentjson) ? data.commentjson : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@work_notes", 50, (!string.IsNullOrEmpty(data.work_notes) ? data.work_notes : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@close_notes", 50, (!string.IsNullOrEmpty(data.close_notes) ? data.close_notes : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@x_ctv_sc_close_code", 50, (!string.IsNullOrEmpty(data.x_ctv_sc_close_code) ? data.x_ctv_sc_close_code : ""), DbType.String));
                    parameters.Add(ConnectionUtility.CreateParameter("@sys_created_by", 50, (!string.IsNullOrEmpty(data.sys_created_by) ? data.sys_created_by : ""), DbType.String));
                    //parameters.Add(ConnectionUtility.CreateParameter("@sys_created_on", 8, Convert.ToDateTime(data.sys_created_on), DbType.DateTime));
                    parameters.Add(ConnectionUtility.CreateParameter("@sys_created_on", 8, TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(data.sys_created_on), TimeZoneInfo.Local), DbType.DateTime));
                    parameters.Add(ConnectionUtility.CreateParameter("@description_not_exists", 1, descriptionNotExists, DbType.Boolean));


                    DataSet ds = new DataSet();
                    ConnectionUtility.Insert("SP_RequestInsert", CommandType.StoredProcedure, parameters.ToArray(), out ds); // prod env line
                    //ConnectionUtility.Insert("SP_RequestInsertBlom", CommandType.StoredProcedure, parameters.ToArray(), out ds); // test/blom env line
                    if (ds.Tables[0].Rows.Count >= 0)
                    {
                        var dr = ds.Tables[0].Rows[0];
                        if (dr["Code"].ToString() == "1")
                        {
                            if (Convert.ToInt32(dr["Id"]) >= 1 && (Convert.ToInt32(data.state) != 3 && Convert.ToInt32(data.state) != 7) && (data.CommentModels != null && data.CommentModels.Count >= 1))
                            {
                                var followupresult = InsertRequestFollowup(data.CommentModels, Convert.ToInt32(dr["Id"]), Convert.ToInt32(data.state));
                                result = followupresult;
                            }
                            else if (Convert.ToInt32(dr["Id"]) >= 1 && (Convert.ToInt32(data.state) == 3 || Convert.ToInt32(data.state) == 7))
                            {
                                var followupresult = InsertRequestFollowup(Convert.ToInt32(dr["Id"]), Convert.ToInt32(data.state));
                                result = followupresult;
                            }
                            else
                                result = true;

                        }
                        else
                        {
                            string errMsg = dr["Code"].ToString() + "\n" + Convert.ToInt32(dr["Id"]) + "\n" + dr["Message"].ToString() + "\n Work Order #-:" + data.number;
                            Logger.Info($"Error Message =>{errMsg}");
                            _emailNotificationUtility.SendEmail(errMsg);
                        }
                    }
                    else
                    {
                        var msg = $"Data set row count is => {ds.Tables[0].Rows.Count}";
                        Logger.Info(msg);
                        _emailNotificationUtility.SendEmail(msg);
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                var exMsg = $"Error Message =>{ex.Message}";
                Logger.Info(exMsg);
                _emailNotificationUtility.SendEmail(exMsg);
                throw;
            }



            return result;
        }

        public bool InsertRequestFollowup(List<CommentModel> commentModelList, int requestid, int state)
        {
            bool result = false;

            try
            {
                commentModelList = commentModelList.OrderBy(c => c.sys_created_on).ToList();
                foreach (var data in commentModelList)
                {
                    var parameters = new List<IDbDataParameter>();

                    if (!data.sys_created_by.Contains("svcSystemSource"))
                    {
                        if (data.value.Contains("________________________________"))
                        {
                            string[] spearator = { "________________________________" };
                            var splitdata = data.value.Split(spearator, StringSplitOptions.None);

                            if (splitdata.Length > 1)
                                data.value = splitdata[0];
                        }

                        //string note = "Created By:" + data.sys_created_by + ",\n" + "Comment:" + data.value; //commented due to new implementation comes form Lisa on Monday Apr, 08, 2022 for the subject RE: WO-00049609 question


                        string note = "";
                        if (data.element.Contains("comments"))
                            note = "Created By:" + data.sys_created_by + ",\n" + "Comment:" + data.value;
                        else if(data.element.Contains("work_notes"))
                            note = "Created By:" + data.sys_created_by + ",\n" + "Work Notes:" + data.value;

                        parameters.Add(ConnectionUtility.CreateParameter("@request_id", 4, requestid, DbType.Int32));
                        parameters.Add(ConnectionUtility.CreateParameter("@note", 8000, note, DbType.String));
                        parameters.Add(ConnectionUtility.CreateParameter("@createdate", 8, TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(data.sys_created_on), TimeZoneInfo.Local), DbType.DateTime));
                        //parameters.Add(ConnectionUtility.CreateParameter("@createdate", 8, Convert.ToDateTime(data.sys_created_on), DbType.DateTime));
                        parameters.Add(ConnectionUtility.CreateParameter("@state", 8, state, DbType.Int32));
                        parameters.Add(ConnectionUtility.CreateParameter("@emailbody", 8000, "", DbType.String));

                        ////DataSet ds = new DataSet();
                        ////ConnectionUtility.Insert("SP_InsertRequestFollowup", CommandType.StoredProcedure, parameters.ToArray(), out ds);
                        
                        ConnectionUtility.Insert("SP_InsertRequestFollowup", CommandType.StoredProcedure, parameters.ToArray()); //prod env line
                        //ConnectionUtility.Insert("SP_InsertRequestFollowupBlom", CommandType.StoredProcedure, parameters.ToArray()); // test/blom env line

                        //if (ds.Tables[0].Rows.Count >= 0)
                        //{
                        //    var dr = ds.Tables[0].Rows[0];
                        //    //if (dr["Code"].ToString() == "1")
                        //    result = (dr["Code"].ToString() == "1") ? true : false;
                        //}
                        result = true;
                        Logger.Info($"Data folloup processed =>{result} for the request_id is-: {requestid} ");

                    }
                    else
                    {
                        result = true;
                        Logger.Info($"Data folloup processed =>{result} for the request id is-: {requestid} which contains sys_created_by is-:{data.sys_created_by}");

                    }
                }
            }
            catch(Exception ex)
            {
                var exMsg = $"Error Message due to follow up process =>{ex.Message}";
                Logger.Info(exMsg);
                _emailNotificationUtility.SendEmail(exMsg);
                throw;
            }
                
            return result;

        }

        public bool InsertRequestFollowup(int requestid, int state)
        {
            bool result = false;

            try
            {                
                var parameters = new List<IDbDataParameter>();
                
                parameters.Add(ConnectionUtility.CreateParameter("@request_id", 4, requestid, DbType.Int32));
                parameters.Add(ConnectionUtility.CreateParameter("@note", 8000, "", DbType.String));
                parameters.Add(ConnectionUtility.CreateParameter("@createdate", 8, Convert.ToDateTime(System.DateTime.Now), DbType.DateTime));
                parameters.Add(ConnectionUtility.CreateParameter("@state", 8, state, DbType.Int32));
                parameters.Add(ConnectionUtility.CreateParameter("@emailbody", 8000, "", DbType.String));

                //DataSet ds = new DataSet();
                //ConnectionUtility.Insert("SP_InsertRequestFollowup", CommandType.StoredProcedure, parameters.ToArray(), out ds);

                //if (ds.Tables[0].Rows.Count >= 0)
                //{
                //    var dr = ds.Tables[0].Rows[0];
                //    //if (dr["Code"].ToString() == "1")
                //    result = (dr["Code"].ToString() == "1") ? true : false;
                //}

                ConnectionUtility.Insert("SP_InsertRequestFollowup", CommandType.StoredProcedure, parameters.ToArray()); //prod env line
                //ConnectionUtility.Insert("SP_InsertRequestFollowupBlom", CommandType.StoredProcedure, parameters.ToArray()); // test/blom env line
                result = true;

            }
            catch (Exception ex)
            {
                var exMsg = $"Error Message due to follow up process =>{ex.Message} for the state is-:{state} and request id is-:{requestid}";
                Logger.Info(exMsg);
                _emailNotificationUtility.SendEmail(exMsg);
                throw;
            }

            return result;
        }

        public bool AddAttachmentRequestAsync(AttachmentModel attachmentModel,byte[] byteArray, string path, string linkUrl,int request_id)
        {
            bool result = false;
            try
            {
                var parameters = new List<IDbDataParameter>();

                parameters.Add(ConnectionUtility.CreateParameter("@request_id", 4,  request_id , DbType.Int32));
                parameters.Add(ConnectionUtility.CreateParameter("@sysid", 50, (!string.IsNullOrEmpty(attachmentModel.table_sys_id) ? attachmentModel.table_sys_id : ""), DbType.String));
                parameters.Add(ConnectionUtility.CreateParameter("@fname", 200, (!string.IsNullOrEmpty(attachmentModel.file_name) ? attachmentModel.file_name : ""), DbType.String));
                parameters.Add(ConnectionUtility.CreateParameter("@uname", 200, (!string.IsNullOrEmpty(attachmentModel.file_name) ? attachmentModel.file_name : ""), DbType.String));
                parameters.Add(ConnectionUtility.CreateParameter("@sizeof", 15, (!string.IsNullOrEmpty(attachmentModel.size_bytes) ? attachmentModel.size_bytes : ""), DbType.String));
                parameters.Add(ConnectionUtility.CreateParameter("@sys_created_on", 8, TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(attachmentModel.sys_created_on), TimeZoneInfo.Local), DbType.DateTime));
                //parameters.Add(ConnectionUtility.CreateParameter("@sys_created_on", 8, Convert.ToDateTime(attachmentModel.sys_created_on), DbType.DateTime));

                DataSet ds = new DataSet();
                ConnectionUtility.Insert("SP_InsertAttachmentFolloup", CommandType.StoredProcedure, parameters.ToArray(), out ds);//production environment line
                //ConnectionUtility.Insert("SP_InsertAttachmentFolloupBlom", CommandType.StoredProcedure, parameters.ToArray(), out ds); //test/blom environment line.
                if (ds.Tables[0].Rows.Count >= 0)
                {
                    var dr = ds.Tables[0].Rows[0];
                    if (dr["Code"].ToString() == "1")
                    {
                        if (request_id >= 1 && Convert.ToInt32(dr["request_followup_id"]) >= 1 && !string.IsNullOrEmpty(dr["locationpath"].ToString()))
                        {
                            //write to localfolder code
                            string attachmentStorageLocation = path + dr["locationpath"].ToString().Replace(dr["locationpath"].ToString().Split('/').Last(), ""); // @"C:\ssi_upload\attachments\req1\reqfu1\";

                            if (!Directory.Exists(attachmentStorageLocation))
                            {
                                Directory.CreateDirectory(attachmentStorageLocation);
                                LoggerAttachment.Info($"Create Directory if not exists =>{attachmentStorageLocation}");

                            }

                            LoggerAttachment.Info($"To Write attachment using byte array on location=>{attachmentStorageLocation}");

                            File.WriteAllBytes(attachmentStorageLocation + attachmentModel.file_name, byteArray);

                            LoggerAttachment.Info($"Store attachment successfully on location=>{attachmentStorageLocation}");

                            result = File.Exists(attachmentStorageLocation + attachmentModel.file_name) ? true : false;


                            if (result)
                            {
                                LoggerAttachment.Info($"File found on location=>{attachmentStorageLocation} ,  next step is start");

                                string body = $"Hello,<br/><br/>Request Follwup for Request_Id-:{request_id} <br/><br/>" +
                                    //$"Url:- <a href={linkUrl}>{linkUrl}</a> <br/></br>" +
                                    $"Url:- {linkUrl}{request_id} <br/><br/>" +
                                    $"Client has attached new document for SSI." +
                                    $" <br/><br/>Thank you,<br/>Team";

                                //string body = "Hello,<br/><br/>Request Follwup for Request_Id-:"+ request_id + "<br/>";
                                //body += "Url:-" + linkUrl + request_id+ "<br/><br/>";
                                //body += "Client has attached new document for SSI.<br/><br/>Thank you,<br/>Team";


                                string subject = $"Attachment Request Follwup for Request_Id: {request_id}";

                                LoggerAttachment.Info($"Email Sending to Business =>{subject}");

                                _emailNotificationUtility?.SendEmailToBusiness(body, subject);

                                LoggerAttachment.Info($"Email Send to Business Successfully =>{subject}");

                            }
                            else
                            {
                                LoggerAttachment.Info($"File not found on location=>{attachmentStorageLocation} ,  next step is not taken to send email to business");
                            }
                        }
                        else
                        {
                            string errMsg = $"Code is=>{dr["Code"].ToString()} \n request_id is=>{request_id} \n Message is=>{dr["Message"].ToString()} \n sys_id #=>{attachmentModel.table_sys_id}";
                            LoggerAttachment.Info($"Info Message =>{errMsg}");
                            _emailNotificationUtility.SendEmail(errMsg);
                        }
                    }
                    else if(dr["Code"].ToString() == "2")
                    {
                        string errMsg = $"Code is=>{dr["Code"].ToString()} \n request_id is=>{request_id} \n Message is=>{dr["Message"].ToString()} \n sys_id #=>{attachmentModel.table_sys_id}";
                        LoggerAttachment.Info($"Info Message =>{errMsg}");
                        result = true;
                    }
                    else
                    {
                        string errMsg = $"Code is=>{dr["Code"].ToString()} \n request_id is=>{request_id} \n Message is=>{dr["Message"].ToString()} \n sys_id #=>{attachmentModel.table_sys_id}";
                        LoggerAttachment.Info($"Info Message =>{errMsg}");
                        _emailNotificationUtility.SendEmail(errMsg);
                    }
                }
                else
                {
                    string errMsg = $"Dataset count is=>{ds.Tables[0].Rows.Count}";
                    LoggerAttachment.Info($"Error Message =>{errMsg}");
                    _emailNotificationUtility.SendEmail(errMsg);
                }
            }
            catch (Exception ex)
            {
                var errMsg = $"Failed to insert attachment record-{ex.Message}";
                 LoggerAttachment.Info($"Error Message =>{errMsg}");
                _emailNotificationUtility.SendEmail(errMsg);
                throw new Exception(errMsg);
            }
            return result;
        }

    }
}