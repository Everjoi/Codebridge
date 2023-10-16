using Codebridge.Application.CQRS.Dogs.Commands.CreateDog;
using Codebridge.Application.CQRS.Dogs.Commands.DeleteDog;
using Codebridge.Application.CQRS.Dogs.Commands.UpdateDog;
using Codebridge.Application.CQRS.Dogs.Queries.GetAllDogs;
using Codebridge.Application.CQRS.Dogs.Queries.GetAllDogsByWeight;
using Codebridge.Application.CQRS.Dogs.Queries.GetDogById;
using Codebridge.Application.CQRS.Dogs.Queries.GetDogsWithPagination;
using Codebridge.Application.DTOs;
using Codebridge.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Reflection;

namespace Codebridge.Presentation.Controllers
{
    [ApiController]
    [Route("")]
    [EnableRateLimiting("fixedWindow")]
    public class DogController:ControllerBase
    {
        private readonly ILogger<DogController> _logger;
        private readonly IMediator _mediator;

        public DogController(ILogger<DogController> logger,IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }




        [HttpGet]
        [Route("/ping")]
        public IActionResult Ping()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var title = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
            var version = assembly.GetName().Version?.ToString();

            return Ok($"Product:{title}.Version{version}");
        }


        [HttpGet]
        [Route("dogs")]
        public async Task<ActionResult<Result<List<DogDto>>>> Get()
        {
            return await _mediator.Send(new GetAllDogsQuery());
        }

        [HttpGet("/dog/{id}")]
        public async Task<ActionResult<Result<DogDto>>> GetDogsById(Guid id)
        {
            return await _mediator.Send(new GetDogByIdQuery(id));
        }

        [HttpGet]
        [Route("/dogs/weight/")]
        public async Task<ActionResult<Result<List<DogDto>>>> GetDogsByWeightQuery()
        {
            return await _mediator.Send(new GetAllDogsByWeightQuery());
        }

        [HttpGet]
        [Route("/dogs/paged")]
        public async Task<ActionResult<PaginatedResult<DogDto>>> GetDogsWithPagination([FromQuery] GetDogsWithPaginationQuery query)
        {
            var validator = new GetDogsWithPaginationValidator();


            var result = validator.Validate(query);

            if(result.IsValid)
            {
                return await _mediator.Send(query);
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpPost("/dog/create")]
        public async Task<ActionResult<Result<Guid>>> Create(CreateDogCommnd command)
        {
            if(!ModelState.IsValid)
                return BadRequest("You enter invalid value");

            return await _mediator.Send(command);
        }

        [HttpPut("/dog/edit/{id}")]
        public async Task<ActionResult<Result<Guid>>> Update(Guid id,UpdateDogCommand command)
        {
            if(id != command.Id)
            {
                return BadRequest("You enter invalid value");
            }

            return await _mediator.Send(command);
        }

        [HttpDelete("/dog/delete/{id}")]
        public async Task<ActionResult<Result<Guid>>> Delete(Guid id)
        {
            if(!ModelState.IsValid)
                return BadRequest("You enter invalid value");

            return await _mediator.Send(new DeleteDogCommand(id));
        }


    }
}
