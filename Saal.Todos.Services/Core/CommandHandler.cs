using System.Threading.Tasks;

namespace Saal.Todos.Services.Core
{
    /// <summary>
    /// Defines an interface definition for service layer.
    /// </summary>
    /// <typeparam name="TCommand">The incomming command requested by the user</typeparam>
    /// <typeparam name="TCommandResult">The result of executing the Command</typeparam>
    public interface ICommandHandler<TCommand, TCommandResult>
       where TCommand : ICommand
    {
        TCommandResult Handle(TCommand command);
    }

    public interface ICommandHandlerAsync<TCommand, TCommandResult>
        where TCommand : ICommand
    {
        Task<TCommandResult> Handle(TCommand command);
    }
}
