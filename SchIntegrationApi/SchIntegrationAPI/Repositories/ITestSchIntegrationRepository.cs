using SchIntegrationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchIntegrationAPI.Repositories
{
    public interface ITestSchIntegrationRepository
    {
        string GetDescription();
        bool CreateAttachment(AttachmentModel attachmentModel, byte[] byteArray, string path, string linkUrl, int request_id);
    }
}
