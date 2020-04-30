using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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
            restClient.UseNewtonsoftJson(new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                    {
                        ProcessDictionaryKeys = true
                    }
                }
            });
            client = restClient;
        }

        //todo more SOLID :)
        public async Task<SearchTracksResultModel> SearchTracks(string query)
        {
            const string uri = "/search/tracks";
            var request = new RestRequest(uri);
            request.AddQueryParameter("q", query);
            request.AddQueryParameter("client_id", appConfiguration.SoundCloud.ClientId);
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("Authorization", appConfiguration.SoundCloud.OAuthToken);
            var response = await client.ExecuteGetAsync<SearchTracksResultModel>(request);
            logger.LogInformation("Successfully got track list.");
            return response.Data;
        }

        //todo decorated restclient or smth like
        public async Task<Stream> DownloadTrackAsync(TrackModel track)
        {
            var redirectUrl = await GetRedirectUrlAsync(track.Media.Transcodings[0].Url);
            var chunkLinksList = await GetChunkLinksListAsync(redirectUrl);
            var chunksList = new Dictionary<int, byte[]>();
            await Task.WhenAll(chunkLinksList.Select(async (x, index) =>
            {
                var request = new RestRequest(x);
                var response = await client.ExecuteGetAsync(request);
                chunksList[index] = response.RawBytes;
            }));
            logger.LogInformation($"Successfully downloaded track by {chunksList.Count} chunks.");
            return new MemoryStream(chunksList
                .OrderBy(x => x.Key)
                .Select(x => x.Value)
                .Aggregate((prev, next) =>
                {
                    var prevLength = prev.Length;
                    var result = new byte[prevLength + next.Length];
                    prev.CopyTo(result, 0);
                    next.CopyTo(result, prevLength);
                    return result;
                }));
        }

        private async Task<string[]> GetChunkLinksListAsync(string redirectUrl)
        {
            var chunksListRequest = new RestRequest(redirectUrl);
            chunksListRequest.AddHeader("Authorization", appConfiguration.SoundCloud.OAuthToken);
            var chunksListResponse = await client.ExecuteGetAsync<object>(chunksListRequest);
            logger.LogInformation("Successfully got chunks list.");
            return Regex
                .Split(chunksListResponse.Content, "(https://cf-hls-media.sndcdn.com.(?(?=\\n#)|.*))")
                .Where((x, i) => i % 2 == 1) // works as needed, made because of low-level knowledge of regex
                .ToArray();
        }

        private async Task<string> GetRedirectUrlAsync(string url)
        {
            var request = new RestRequest(url);
            request.AddQueryParameter("client_id", appConfiguration.SoundCloud.ClientId);
            request.AddHeader("Authorization", appConfiguration.SoundCloud.OAuthToken);
            var response = await client.ExecuteGetAsync<RedirectResponse>(request);
            logger.LogInformation("Successfully got redirect url.");
            return response.Data.Url;
        }

        private class RedirectResponse
        {
            public string Url { get; set; }
        }

        public async Task<TrackModel> ResolveTrackAsync(string trackUrl)
        {
            const string uri = "/resolve";
            var request = new RestRequest(uri);
            request.AddQueryParameter("url", trackUrl);
            request.AddQueryParameter("client_id", appConfiguration.SoundCloud.ClientId);
            request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            request.AddHeader("Authorization", appConfiguration.SoundCloud.OAuthToken);
            var response = await client.ExecuteGetAsync<TrackModel>(request);
            if (response.IsSuccessful)
            {
                logger.LogInformation("Successfully resolved track.");
                return response.Data;
            }
            logger.LogWarning($"Track resolve for url \"{trackUrl}\" failed.");
            return null;
        }
    }
}