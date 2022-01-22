using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Saal.Todos.Services.Core
{
    public interface IServiceValidationResult
    {
        bool IsValid { get; }
        string ErrorsString { get; }
    }
    public interface IServiceValidator<T>
    {
        IServiceValidationResult Validate(T value);
    }

    public class ServiceFluentValidationResult : IServiceValidationResult
    {
        readonly ValidationResult _validationResult;

        public ServiceFluentValidationResult([NotNull]ValidationResult validationResult)
        {
            _validationResult = validationResult ?? throw new ArgumentNullException(nameof(validationResult));
        }

        public bool IsValid => _validationResult.IsValid;

        public string ErrorsString => string.Join(", ", _validationResult.Errors.Select(e => e.ErrorMessage));
    }
}
