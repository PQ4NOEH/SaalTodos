using Saal.Todos.Services.Core;

namespace Saal.Todos.Services.Tests.Fakes
{
    internal class ServiceValidationResultNoOK : IServiceValidationResult
    {
        public ServiceValidationResultNoOK(string errorsString)
        {
            ErrorsString = errorsString;
        }

        public bool IsValid => false;

        public string ErrorsString { get; }
    }
}
