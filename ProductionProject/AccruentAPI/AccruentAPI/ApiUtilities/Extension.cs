using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;

namespace AccruentAPI.ApiUtilities
{
    public static class Extension
    {
        public static Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient client, string requestUri, HttpContent value)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = value };

            return client.SendAsync(request).ContinueWith(responseTask => responseTask.Result);

            
        } 


    }
}