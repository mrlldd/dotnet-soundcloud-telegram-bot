using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
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
using SoundCloudTelegramBot.Common.SoundCloud.Models.Search;

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
                    },
                },
                Error = (_, args) => Console.WriteLine(args.ErrorContext.Error)
            });
            client = restClient;
        }

        //todo more SOLID :)
        public async Task<ISearchResult<ITypedEntity>> SearchAsync(string query)
        {
            const string uri = "/search";
            var request = new RestRequest(uri);
            request.AddQueryParameter("q", query);
            request.AddQueryParameter("client_id", appConfiguration.SoundCloud.ClientId);
            request.AddHeader("Accept", "application/json; q=0.01");
            var response = await client.ExecuteGetAsync<SearchResult<CombinedEntity>>(request);
            logger.LogInformation($"Successfully got list of {response.Data.Collection.Length} tracks.");
            var result = response.Data.ToAbstractLevelEntity();
            return result;
        }

        //todo decorated restclient or smth like
        public async Task<Stream> DownloadTrackAsync(ITrack track)
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
            var chunksListResponse = await client.ExecuteGetAsync(chunksListRequest);
            logger.LogInformation("Successfully got chunks list.");
            return Regex
                .Split(chunksListResponse.Content, "(https://cf-hls-media.sndcdn.com.(?(?=\\n#)|.*))")
                .Where((x, i) => i % 2 == 1) // works as needed, made because of low knowledge level of regex
                .ToArray();
        }

        private async Task<string> GetRedirectUrlAsync(string url)
        {
            var request = new RestRequest(url);
            request.AddQueryParameter("client_id", appConfiguration.SoundCloud.ClientId);
            var response = await client.ExecuteGetAsync<RedirectResponse>(request);
            logger.LogInformation("Successfully got redirect url.");
            return response.Data.Url;
        }

        private class RedirectResponse
        {
            public string Url { get; set; }
        }

        public async Task<ITypedEntity> ResolveAsync(string trackUrl)
        {
            const string uri = "/resolve";
            var request = new RestRequest(uri);
            request.AddQueryParameter("url", trackUrl);
            request.AddQueryParameter("client_id", appConfiguration.SoundCloud.ClientId);
            request.AddHeader("Accept", "application/json; q=0.01");
            var response = await client.ExecuteGetAsync<CombinedEntity>(request);
            if (response.IsSuccessful)
            {
                logger.LogInformation("Successfully resolved track.");
                return response.Data;
            }

            logger.LogWarning($"Track resolve for url \"{trackUrl}\" failed.");
            return null;
        }

        public async Task<ITrack[]> SearchTracksByIds(IEnumerable<long> ids)
        {
            var request = new RestRequest("/tracks");
            request.AddQueryParameter(nameof(ids), string.Join(',', ids));
            request.AddQueryParameter("client_id", appConfiguration.SoundCloud.ClientId);
            var response = await client.ExecuteGetAsync<IEnumerable<Track>>(request);
            return response.Data
                .OfType<ITrack>()
                .ToArray();
        }
    }
}