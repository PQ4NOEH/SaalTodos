    using System;

namespace Saal.Todos.Core
{
    public interface ICommand
    {
        public DateTime RequestedOn { get; }
        public string CommandName { get; }
    }

    public interface ICommand<TPayload> : ICommand
    {
        TPayload Payload { get; }
    }

    public abstract class Command : ICommand
    {
        public DateTime RequestedOn { get; }
        public string CommandName => nameof(Command);

        protected Command()
        {
            RequestedOn = DateTime.Now;
        }
    }

    public abstract class Command<TPayload> : Command, ICommand<TPayload>
    {
        public TPayload Payload { get; }
        protected Command(TPayload payload) : base()
        {
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
        }
    }
}
