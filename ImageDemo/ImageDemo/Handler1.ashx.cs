using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ImageDemo
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Guid id;
            id = new Guid(context.Request.QueryString["id"].ToString());
            if (id != null)
            {
                string constr = ConfigurationManager.ConnectionStrings["ssiConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string sql = "SELECT FileGuid, FileUrl FROM Documents WHERE FileGuid ='" + id +"'";
                    using (SqlDataAdapter sda = new SqlDataAdapter(sql, con))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        //byte[] bytes = (byte[])dt.Rows[0]["FileUrl"];
                        byte[] bytes = ConvertImageToBase64String(dt.Rows[0]["FileUrl"].ToString());
                        context.Response.ContentType = "image/jpeg";
                        context.Response.BinaryWrite(bytes);
                        context.Response.End();
                    }
                }
            }

            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            //int id = Convert.ToInt32(ctx.Request.QueryString["id"]);

            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ssiConnectionString"].ConnectionString);
            //SqlCommand cmd = new SqlCommand("SELECT Filename, FileGuid, FileUrl FROM Documents  WHERE FileId = @FileID", con);
            //cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add("@FileID", id);

            //con.Open();
            //byte[] pict = (byte[])cmd.ExecuteScalar();
            //con.Close();

            //ctx.Response.ContentType = "image/jpg";
            //ctx.Response.OutputStream.Write(pict, 78, pict.Length - 78);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private byte[] ConvertImageToBase64String(string imgurl)
        {

            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(imgurl);
            return imageBytes;
        }
    }
}