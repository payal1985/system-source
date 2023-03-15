using GoCanvasAPI.ApiUtilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoCanvasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceDataController : ControllerBase
    {

        ApiUtility apiUtility;
        IConfiguration config;
        IWebHostEnvironment environment;


        public ReferenceDataController(IConfiguration _config, IWebHostEnvironment _environment)
        {
            config = _config;
            environment = _environment;
            apiUtility = new ApiUtility(_config);

        }

        // GET: api/<ReferenceDataController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ReferenceDataController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReferenceDataController>
        [HttpPost]
        public async Task<string> Post(IFormCollection collection)
        {
            string result = "";

            try
            {
                var referenceDatasetName = collection["refdataset"].ToString();
                //string wwwPath = this.Environment.WebRootPath;
                string contentPath = environment.ContentRootPath;

                //string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                string path = Path.Combine(environment.ContentRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (Request.Form.Files.Count > 0)
                {
                    IFormFileCollection formFile = collection.Files;
                    foreach (var file in formFile)
                    {
                        string fileName = Path.GetFileName(file.FileName);

                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        var dt = GetDataTable(Path.Combine(path, fileName));
                        var xml = LoadCsv(dt, referenceDatasetName);
                        bool resultCanvasApi = await apiUtility.ReferenceDataUI(xml);
                        //result = true;

                        if (resultCanvasApi)
                            System.IO.File.Delete(Path.Combine(path, fileName));

                        result = $"Reference Data Send Successfully !!!!!!!!!!";
                    }
                }
                else
                {
                    result = $"Files not uploaded.....";
                }

            }
            catch (Exception ex)
            {
                throw new Exception ($"Exception occured while sending reference data to GoCanvas \n {ex.Message} \n {ex.StackTrace}");
            }

            return result;
        }

        // PUT api/<ReferenceDataController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReferenceDataController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private System.Data.DataTable GetDataTable(string fileName)
        {
            StreamReader SR = null;
            var dt = new System.Data.DataTable();
            try
            {
                SR = new StreamReader(fileName);
                string line = SR.ReadLine();
                var strArray = line.Split(',');

                System.Data.DataRow row;
                foreach (string s in strArray)
                    dt.Columns.Add(new System.Data.DataColumn());
                do
                {
                    line = SR.ReadLine();
                    if (!((line ?? "") == (string.Empty ?? "")))
                    {
                        row = dt.NewRow();
                        row.ItemArray = line.Split(',');
                        dt.Rows.Add(row);
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (SR != null)
                    SR.Close();
            }

            return dt;
        }

        private string LoadCsv(System.Data.DataTable dt, string refDataSetName)
        {
            string strXML = "<List Name=\"" + refDataSetName + "\">";
            strXML += "<Columns>";
            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                strXML += "<c>";
                strXML += dc.ColumnName;
                strXML += "</c>";
            }

            strXML += "</Columns>";
            strXML += "<Rows>";
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                strXML += "<r>";
                foreach (string value in dr.ItemArray)
                {
                    strXML += "<v>";
                    strXML += value;
                    strXML += "</v>";
                }

                strXML += "</r>";
            }

            strXML += "</Rows>";
            strXML += "</List>";
            return strXML;
        }
    }
}
