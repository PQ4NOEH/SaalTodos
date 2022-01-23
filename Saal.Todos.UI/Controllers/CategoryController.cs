using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Saal.Todos.Repositories.Base;
using Saal.Todos.Services.Category;
using Saal.Todos.UI.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dto = Saal.Todos.Contracts.Dto;

namespace Saal.Todos.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger _logger;

        public CategoryController(
            [NotNull]ICategoryService categoryService,
            [NotNull]ILogger logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public Task<IPaginatedResult<Dto.Category>> Get(int pageSize = 50, int pageNumber = 1) 
        {
            _logger.LogInformation("requested categories page");
            return _categoryService.Fetch(pageSize, pageNumber);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public Task<Dto.Category> Get(int id)
        {
            _logger.LogInformation($"requested category {id}");
            return _categoryService.FetchById(id);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryCommand command)
        {
            _logger.LogInformation("requested creation of a new category");
            var commandresult = await _categoryService.Handle(command);
            return this.GetActionResult(commandresult);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ChangeCategoryCommand command)
        {
            _logger.LogInformation($"requested change of category {id}");
            command.CategoryId = id;
            var commandresult = await _categoryService.Handle(command);
            return this.GetActionResult(commandresult);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"requested delete of category {id}");
            var command = new DeleteCategoryCommand { CategoryId = id };
            var commandresult = await _categoryService.Handle(command);
            return this.GetActionResult(commandresult);
        }
    }

    
}
