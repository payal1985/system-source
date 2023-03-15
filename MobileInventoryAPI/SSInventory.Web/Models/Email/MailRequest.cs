using Microsoft.AspNetCore.Http;
using SSInventory.Web.Models.Files;
using System.Collections.Generic;

namespace SSInventory.Web.Models.Email
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public List<string> CCs { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }

        //FileName - FileType - FileContent
        public List<TempFileInfo> FileAttachments { get; set; }
    }
}
