using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using SoundCloudTelegramBot.AppSettings;
using SoundCloudTelegramBot.Common.Extensions;
using SoundCloudTelegramBot.Common.SoundCloud.Models;

namespace SoundCloudTelegramBot.Common.SoundCloud.Interaction
{
    public class SoundCloudInteractor : ISoundCloudInteractor
    {
        private const string url = "https://api-v2.soundcloud.com";
        private readonly IAppConfiguration appConfiguration;
        private readonly ILogger<SoundCloudInteractor> logger;
        private readonly IRestClient client;
        public SoundCloudInteractor(IAppConfiguration appConfiguration, ILogger<SoundCloudInteractor> logger)
        {
            this.appConfiguration = appConfiguration;
            this.logger = logger;
            var restClient = new RestClient(url);
            restClient.UseNewtonsoftJson(/*new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                    {
                        ProcessDictionaryKeys = true
                    }
                }
            }*/);
            client = restClient;
        }

        public async Task<SearchTracksResultModel> SearchTracks(string query)
        {
            const string uri = "/search/tracks";
            var request = new RestRequest(uri);
            request.AddQueryParameter("q", query);
            request.AddQueryParameter("client_id", appConfiguration.SoundCloud.ClientId);
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("Authorization", appConfiguration.SoundCloud.OAuthToken);
            var response = await client.ExecuteGetAsync<SearchTracksResultModel>(request);
            /*var model = JsonConvert.DeserializeObject<SearchTracksResultModel>(response.Content, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                    {
                        ProcessDictionaryKeys = true
                    }
                }
            });*/
            //logger.LogInformation(model.ToJson());
            return response.Data;
        }
    }
}