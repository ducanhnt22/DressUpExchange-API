using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Http;

namespace DressUpExchange.Service.Ultilities
{
    public class TwilioClientSetup : ITwilioRestClient
    {
        private readonly ITwilioRestClient _client;
        public TwilioClientSetup(IConfiguration config, System.Net.Http.HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = new TwilioRestClient(config["Twilio:Email"], config["Twilio:Password"], config["Twilio:AccountSid"], config["Twilio:AuthToken"],new SystemNetHttpClient(httpClient));
        }
        public string AccountSid => _client.AccountSid;

        public string Region => _client.Region;

        public Twilio.Http.HttpClient HttpClient => _client.HttpClient;

        public Response Request(Request request) => _client.Request(request);
     

        public Task<Response> RequestAsync(Request request) => _client.RequestAsync(request);
      
    }
}
