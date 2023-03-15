using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ImageDemo
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                DataTable dt = new DataTable();
                string constr = ConfigurationManager.ConnectionStrings["ssiConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter("SELECT FileGuid,FileName FROM Documents", con))
                    {
                        sda.Fill(dt);
                        ddlImages.DataSource = dt;
                        ddlImages.DataTextField = "FileName";
                        ddlImages.DataValueField = "FileGuid";
                        ddlImages.DataBind();
                    }
                }
            }
        }

        protected void FetchImage(object sender, EventArgs e)
        {
            string id = ddlImages.SelectedItem.Value;
            Image1.Visible = id != "0";
            Image1.ImageUrl = string.Format("~/Handler1.ashx?id={0}", id);
        }
    }
}