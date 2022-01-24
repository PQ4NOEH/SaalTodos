using FluentValidation;
using Saal.Todos.Services.Core;
using Dto = Saal.Todos.Contracts.Dto;

namespace Saal.Todos.Services.Category
{
    public class TodoValidator : AbstractValidator<Dto.Todo>, IServiceValidator<Dto.Todo>
    {
        public TodoValidator()
        {
            RuleFor(t => t.Title).NotEmpty().WithMessage("A todo must have a title");
            RuleFor(t => t.Title).Length(3, 100).WithMessage("Todo title must have between 3 and 100 characteers ");
        }
        IServiceValidationResult IServiceValidator<Dto.Todo>.Validate(Dto.Todo value)
        {
            var baseValidation = base.Validate(value);
            return new ServiceFluentValidationResult(baseValidation);
        }
    }
}
