using SchIntegrationAPI.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SchIntegrationAPI.Repositories
{
    public class TestSchIntegrationRepository : ITestSchIntegrationRepository
    {
        private static IConnectionUtility ConnectionUtility { get; set; }
        private Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        private Logger LoggerAttachment { get; } = LogManager.GetLogger("logfileattachment");

        //private EmailNotificationUtility _emailNotificationUtility = null;

        public TestSchIntegrationRepository(string connectionString, IConnectionUtility connectionUtility = null)
        {
            ConnectionUtility = connectionUtility ?? new ConnectionUtility(connectionString);
            //_emailNotificationUtility = emailNotificationUtility;

            if (!CheckConnection(false))
            {
                var msg = "Failed to intialize Database Connection Utility";
               // _emailNotificationUtility.SendEmail(msg);
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

        public string GetDescription()
        {
            try
            {
                string number = "WO-00071799";
                var param = new List<IDbDataParameter>();
                string outStrDesc = "";
                param.Add(ConnectionUtility.CreateParameter("@number", 20, number, DbType.String));
                ConnectionUtility.CallStoredProcedureReturnString("SP_RetriveDescription", param.ToArray(), out outStrDesc);

                return outStrDesc;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool CreateAttachment(AttachmentModel attachmentModel, byte[] byteArray, string path, string linkUrl, int request_id)
        {
            bool result = false;
            try
            {
            
            string attachmentStorageLocation = path + request_id; // @"C:\ssi_upload\attachments\req1\reqfu1\";

            if (!Directory.Exists(attachmentStorageLocation))
            {
                Directory.CreateDirectory(attachmentStorageLocation);
                LoggerAttachment.Info($"Create Directory if not exists =>{attachmentStorageLocation}");

            }

            LoggerAttachment.Info($"To Write attachment using byte array on location=>{attachmentStorageLocation}");

            File.WriteAllBytes(attachmentStorageLocation + attachmentModel.file_name, byteArray);

            LoggerAttachment.Info($"Store attachment successfully on location=>{attachmentStorageLocation}");

            result = File.Exists(attachmentStorageLocation + attachmentModel.file_name) ? true : false;
            }
            catch(Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}