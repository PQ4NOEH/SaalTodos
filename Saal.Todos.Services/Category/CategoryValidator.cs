using FluentValidation;
using Saal.Todos.Services.Core;
using Dto = Saal.Todos.Contracts.Dto;

namespace Saal.Todos.Services.Category
{

    /// <summary>
    /// Validates a category DTO
    /// </summary>
    public class CategoryValidator: AbstractValidator<Dto.Category>, IServiceValidator<Dto.Category>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("A category must have name");
            RuleFor(c => c.Name).Length(3, 50).WithMessage("Category name must have between 3 and 50 characteers ");
        }

        IServiceValidationResult IServiceValidator<Dto.Category>.Validate(Dto.Category value)
        {
            var baseValidation = base.Validate(value);
            return new ServiceFluentValidationResult(baseValidation);
        }
    }
}
