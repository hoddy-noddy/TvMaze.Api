using AutoMapper;
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

        public TvMazeService(IHttpClientFactory clientFactory, IDatabaseService databaseService)
        {
            this.clientFactory = clientFactory;
            this.databaseService = databaseService;
        }

        public async Task UpdateDatabaseWitShowInformationAsync(CancellationToken cancellationToken)
        {
            var lastShowIdInDatabase = await databaseService.GetLastShowIdInDatabase();
            //todo: put the magic number in configuration!
            var pageNumber = Math.Ceiling((double)lastShowIdInDatabase / 250);
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                using var client = clientFactory.CreateClient("TvMazeClient");

                var response = await client.GetAsync($"shows?page={pageNumber}");
                pageNumber++;

                if (response.IsSuccessStatusCode)
                {
                    var showListResponse = await response.Content.ReadAsStringAsync();
                    var showList = JsonConvert.DeserializeObject<List<Show>>(showListResponse);
                    var tasks = new List<Task>();
                        
                    foreach (var show in showList)
                    {
                        var task = AddShow(show.Id, cancellationToken);
                        tasks.Add(task);
                    }
                    Task.WaitAll(tasks.ToArray());
                }
                else
                {
                    break;
                }             
            }
        }
        private async Task AddShow(int id, CancellationToken cancellationToken)
        {
            var show = await GetShowWithCastInformation(id, cancellationToken);
            show.Id = 0;
            show.ShowId = show.Id;
            await databaseService.AddOrUpdateShowAsync(show, cancellationToken);
        }

        private async Task<Show> GetShowWithCastInformation(int id, CancellationToken cancellationToken)
        {
            //todo: Url in configuration
            using var client = clientFactory.CreateClient("TvMazeClient");
           
            var response = await client.GetAsync($"shows/{id}?embed=cast",cancellationToken);

            var json = await response.Content.ReadAsStringAsync();

            JObject jo = JObject.Parse(json);
            Show show = jo.ToObject<Show>();
            var cast = jo.SelectTokens("_embedded.cast").Children().Select( t => t.SelectToken("person"));
            show.Cast = JsonConvert.SerializeObject(cast);
            return show;
        }
    }
}
