using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TvMaze.BLL;
using TvMaze.WebApi.Models;

namespace TvMaze.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShowController : ControllerBase
    {
        private readonly IShowService showService;
        private readonly ITvMazeService service;
        private readonly IMapper mapper;

        public ShowController(IShowService showService,ITvMazeService service, IMapper mapper)
        {
            this.showService = showService;
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetShowsWithCast(int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            var zeroBasedPagenumber = (pageNumber == 0 ? 0 : (pageNumber - 1));

            var showList = await showService.GetAllShowsAsync(pageSize,zeroBasedPagenumber,cancellationToken);

            var showModelsList = mapper.Map<List<ShowModel>>(showList);

            return new OkObjectResult(showModelsList);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SeedDatabase(CancellationToken cancellationToken)
        {
            //todo: this probably better in a webJob. For now easy to updateDAtabase via API
            await service.UpdateDatabaseWitShowInformationAsync(cancellationToken);
            return Ok();
        }
    }
}
