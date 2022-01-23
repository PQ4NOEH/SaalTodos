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
        , ICommandHandlerAsync<ChangeCategoryCommand, CommandResult>
        , ICommandHandlerAsync<DeleteCategoryCommand, CommandResult>
        , ICommandHandlerAsync<CreateTodoCommand, CommandResult<Dto.Todo>>
        , ICommandHandlerAsync<ChangeTodoCommand, CommandResult>
        , ICommandHandlerAsync<DeleteTodoCommand, CommandResult>
    {
        Task<IPaginatedResult<Dto.Category>> Fetch(int pageSize, int pageNumber);
        Task<Dto.Category> FetchById(int categoryId);
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

        /// <summary>
        /// Creates a new Category
        /// </summary>
        /// <param name="command">Command data</param>
        /// <returns>The new Category</returns>
        /// <exception cref="ArgumentNullException">When the command parameter is null throws an exception</exception>
        public async Task<CommandResult<Dto.Category>> Handle([NotNull]CreateCategoryCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var newCategory = new Dto.Category()
            {
                Name = command.CategoryName
            };
            var validationResult = _categoryValidator.Validate(newCategory);
            if(validationResult.IsValid)
            {
                newCategory.Id = await _categoryRepository.Insert(newCategory);
                return new CommandResult<Dto.Category>(newCategory);
            }
            else
            {
                var rejectedReason = new CommandRejectedReason(validationResult.ErrorsString);
                return new CommandResult<Dto.Category>(rejectedReason);
            }
            
        }

        /// <summary>
        /// Changes category properties except todos
        /// </summary>
        /// <param name="command">Command data</param>
        /// <returns>Execution result</returns>
        /// <exception cref="ArgumentNullException">When the command parameter is null throws an exception</exception>
        public async Task<CommandResult> Handle([NotNull]ChangeCategoryCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var category = await _categoryRepository.FetchById(command.CategoryId);
            if(category == null)//if no category is found rejects the command execution to inform client
            {
                var rejectedReason = new CommandRejectedReason();
                return new CommandResult(rejectedReason);
            }
            else
            {
                category.Name = command.CategoryName;
                var validationResult = _categoryValidator.Validate(category);
                if (validationResult.IsValid)
                {
                    await _categoryRepository.Update(category);
                    return new CommandResult();
                }
                else
                {
                    var rejectedReason = new CommandRejectedReason(validationResult.ErrorsString);
                    return new CommandResult(rejectedReason);
                }
            }
        }

        /// <summary>
        /// Deletes am aggregate
        /// </summary>
        /// <param name="command">Command data</param>
        /// <returns>Execution result</returns>
        /// <exception cref="ArgumentNullException">When the command parameter is null throws an exception</exception>
        public async Task<CommandResult> Handle([NotNull] DeleteCategoryCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var affectedRecords = await _categoryRepository.Remove(command.CategoryId);
            if (affectedRecords)//if no category is found rejects the command execution to inform client
            {
                var rejectedReason = new CommandRejectedReason();
                return new CommandResult(rejectedReason);
            }
            else
            {
                return new CommandResult();
            }
        }

        /// <summary>
        /// Fetches a page of categories
        /// </summary>
        /// <param name="pageSize">The number of elements per page</param>
        /// <param name="pageNumber">The page</param>
        /// <returns></returns>
        public Task<IPaginatedResult<Dto.Category>> Fetch(int pageSize, int pageNumber)
        {
            pageSize = pageSize > 50 ? 50 : pageSize;
            return _categoryRepository.Fetch(pageSize, pageNumber);
        }

        /// <summary>
        /// Fetches a category
        /// </summary>
        /// <param name="categoryId">The id of the category</param>
        /// <returns>The matched category or null</returns>
        public Task<Dto.Category> FetchById(int categoryId)
        {
            return _categoryRepository.FetchById(categoryId);
        }


        public Task<CommandResult<Dto.Todo>> Handle([NotNull] CreateTodoCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            throw new NotImplementedException();
        }

        public Task<CommandResult> Handle([NotNull] ChangeTodoCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            throw new NotImplementedException();
        }

        public Task<CommandResult> Handle([NotNull] DeleteTodoCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            throw new NotImplementedException();
        }
    }
}
