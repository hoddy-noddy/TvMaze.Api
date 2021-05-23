using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TvMaze.BLL;

namespace TvMaze.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShowController : ControllerBase
    {
        private readonly IShowService showService;
        private readonly ITvMazeService service;

        public ShowController(IShowService showService,ITvMazeService service)
        {
            this.showService = showService;
            this.service = service;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetShowsWithCast(int pageSize, int pageNumber)
        {
            var showList = await showService.GetAllShows(pageSize, pageNumber == 0 ? 0 : (pageNumber - 1));
            
            return new OkObjectResult(showList);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SeedDatabase()
        {
            await service.SeedDatabaseWithShowInformation();
            return Ok();
        }
    }
}
