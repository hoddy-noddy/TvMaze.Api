using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TvMaze.BLL.Dto;
using TvMaze.DAL;

namespace TvMaze.BLL
{
    public class TvMazeService : ITvMazeService
    {
        private readonly HttpClient client;
        private readonly IDatabaseService databaseService;
        private readonly IMapper mapper;

        public TvMazeService(HttpClient client, IDatabaseService databaseService, IMapper mapper)
        {
            this.client = client;
            this.databaseService = databaseService;
            this.mapper = mapper;
        }

        public async Task SeedDatabaseWithShowInformation()
        {
            
            var response = await client.GetAsync("shows");
            var showListResponse = await response.Content.ReadAsStringAsync();

            var showList = JsonConvert.DeserializeObject<List<Show>>(showListResponse);
            var uniqueIDCount = showList.Select(s => s.Id).Distinct().Count() ;
            foreach( var show in showList)
            {
                var fullShow = await GetShowWithCastInformation(show.Id);
                //todo: this should be done in the jsonmapping
                fullShow.Id = 0;
                fullShow.ShowId = show.Id;
                await databaseService.AddOrUpdateShowAsync(fullShow);
            }

        }

        private async Task<Show> GetShowWithCastInformation(int id)
        {
            var response = await client.GetAsync($"shows/{id}?embed=cast");

            var json = await response.Content.ReadAsStringAsync();

            JObject jo = JObject.Parse(json);
            Show show = jo.ToObject<Show>();
            var cast = jo.SelectTokens("_embedded.cast").Children().Select( t => t.SelectToken("person"));
            show.Cast = JsonConvert.SerializeObject(cast);
            return show;
        }
    }
}
