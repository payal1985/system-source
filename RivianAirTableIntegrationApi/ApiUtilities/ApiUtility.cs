using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RivianAirtableIntegrationApi.ApiUtilities
{
    public class ApiUtility
    {
        private IConfiguration _configuration;

        public ApiUtility(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> ApiCall()
        {
            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Authorization
                //             = new AuthenticationHeaderValue("api_key", "keyFowkf0QylIL7R0");
                // client.DefaultRequestHeaders.Add("api_key", "keyFowkf0QylIL7R0");


                string apiurl = $"{_configuration.GetValue<string>("GetAirTableApiUrl")}&api_key={_configuration.GetValue<string>("ApiKey")}";
                var response = await client.GetAsync(apiurl);



                //HttpRequestMessage request = new HttpRequestMessage();
                //request.RequestUri = new Uri("https://api.airtable.com/v0/appb3Jr0Nf9KU6AEb/Service-Active-Pipeline?view=Service%20-%20Active");
                //request.Method = HttpMethod.Get;
                //request.Headers.Add("api_key", "keyFowkf0QylIL7R0");
                //HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                    return response;
                else
                    return null;
            }
        }
    }
}
