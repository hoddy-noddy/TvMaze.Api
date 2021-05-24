using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TvMaze.DAL;

namespace TvMaze.BLL
{
    public class TvMazeService : ITvMazeService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IDatabaseService databaseService;
        private readonly IConfiguration configuration;

        public TvMazeService(IHttpClientFactory clientFactory, IDatabaseService databaseService,IConfiguration configuration)
        {
            this.clientFactory = clientFactory;
            this.databaseService = databaseService;
            this.configuration = configuration;
        }

        public async Task UpdateDatabaseWitShowInformationAsync(CancellationToken cancellationToken)
        {
            var lastShowIdInDatabase = await databaseService.GetLastShowIdInDatabase();
            var pageSize = int.Parse(configuration["TvMaze:PageSize"]);
            var pageNumber = Math.Ceiling((double)lastShowIdInDatabase / pageSize);

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                using var client = clientFactory.CreateClient("TvMazeClient");
                var url = string.Format(configuration["TvMaze:GetShowsByPageNumberUrl"], pageNumber);
                var response = await client.GetAsync(url,cancellationToken);
                pageNumber++;

                if (response.IsSuccessStatusCode)
                {
                    var showListResponse = await response.Content.ReadAsStringAsync();
                    var showList = JsonConvert.DeserializeObject<List<Show>>(showListResponse);
                    var tasks = new List<Task>();
                        
                    foreach (var show in showList)
                    {
                        var fullShow = await GetShowWithCastInformation(show.Id, cancellationToken);
                        //todo: this should be done in the jsonmapping
                        fullShow.Id = 0;
                        fullShow.ShowId = show.Id;
                        await databaseService.AddOrUpdateShowAsync(fullShow, cancellationToken);
                    }  
                }
                else
                {
                    break;
                }             
            }
        }

        private async Task<Show> GetShowWithCastInformation(int id, CancellationToken cancellationToken)
        {
            using var client = clientFactory.CreateClient("TvMazeClient");
            var url = string.Format(configuration["TvMaze:GetShowWithCastInformationUrl"], id);
            var response = await client.GetAsync(url, cancellationToken);

            var json = await response.Content.ReadAsStringAsync();

            JObject jo = JObject.Parse(json);
            Show show = jo.ToObject<Show>();
            var cast = jo.SelectTokens("_embedded.cast").Children().Select( t => t.SelectToken("person"));
            show.Cast = JsonConvert.SerializeObject(cast);
            return show;
        }
    }
}
