using Saal.Todos.Services.Core;

namespace Saal.Todos.Services.Tests.Fakes
{
    internal class ServiceValidationResultOK : IServiceValidationResult
    {
        
        public bool IsValid => true;

        public string ErrorsString => string.Empty;
    }
}
