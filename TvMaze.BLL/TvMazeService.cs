using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly HttpClient client;
        private readonly IDatabaseService databaseService;

        public TvMazeService(HttpClient client, IDatabaseService databaseService)
        {
            this.client = client;
            this.databaseService = databaseService;
        }

        public async Task SeedDatabaseWithShowInformationAsync(CancellationToken cancellationToken)
        {            
            var response = await client.GetAsync("shows");
            var showListResponse = await response.Content.ReadAsStringAsync();

            var showList = JsonConvert.DeserializeObject<List<Show>>(showListResponse);

            foreach( var show in showList)
            {
                var fullShow = await GetShowWithCastInformation(show.Id,cancellationToken);
                //todo: this should be done in the jsonmapping
                fullShow.Id = 0;
                fullShow.ShowId = show.Id;
                await databaseService.AddOrUpdateShowAsync(fullShow,cancellationToken);
            }
        }

        private async Task<Show> GetShowWithCastInformation(int id, CancellationToken cancellationToken)
        {
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
