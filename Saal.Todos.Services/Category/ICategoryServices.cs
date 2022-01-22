using Saal.Todos.Repositories;
using Saal.Todos.Repositories.Base;
using Saal.Todos.Services.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dto = Saal.Todos.Contracts.Dto;

namespace Saal.Todos.Services.Category
{
    public interface ICategoryService
        : ICommandHandlerAsync<CreateCategoryCommand, CommandResult<Dto.Category>>
    {
        Task<IPaginatedResult<Dto.Category>> Fetch(int pageSize, int pageNumber);
    }

    /// <summary>
    /// Category Api
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IServiceValidator<Dto.Category> _categoryValidator;

        public CategoryService(
            [NotNull] ICategoryRepository categoryRepository,
            [NotNull] IServiceValidator<Dto.Category> categoryValidator)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _categoryValidator = categoryValidator ?? throw new ArgumentNullException(nameof(categoryValidator));
        }

        public async Task<CommandResult<Dto.Category>> Handle(CreateCategoryCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var newCategory = new Dto.Category()
            {
                Name = command.CategoryName
            };
            var validationResult = _categoryValidator.Validate(newCategory);
            if(validationResult.IsValid)
            {
                throw new NotImplementedException();
                newCategory.Id = await _categoryRepository.Insert(newCategory);
                return new CommandResult<Dto.Category>(newCategory);
            }
            else
            {
                var rejectedReason = new CommandRejectedReason(validationResult.ErrorsString);
                return new CommandResult<Dto.Category>(rejectedReason);
            }
            
        }

        public Task<IPaginatedResult<Dto.Category>> Fetch(int pageSize, int pageNumber)
        {
            return _categoryRepository.Fetch(pageSize, pageNumber);
        }
    }
}
