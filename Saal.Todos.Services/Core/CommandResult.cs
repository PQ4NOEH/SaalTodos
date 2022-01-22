using System;

namespace Saal.Todos.Services.Core
{
    public interface ICommandResult
    {
        CommandRejectedReason RejectedReason { get; }
    }
    public interface ICommandResult<TPayload> : ICommandResult
    {
        TPayload Result { get; }
    }

    public class CommandResult : ICommandResult
    {
        public CommandResult(CommandRejectedReason rejectedReason)
        {
            RejectedReason = rejectedReason ?? throw new ArgumentNullException(nameof(rejectedReason));
        }

        public CommandResult()
        {

        }
        public CommandRejectedReason RejectedReason { get; }
    }

    public class CommandResult<TPayload> : ICommandResult<TPayload>
    {
        public CommandResult(TPayload result)
        {
            Result = result ?? throw new ArgumentNullException(nameof(TPayload));
        }
        
        public CommandResult(CommandRejectedReason rejectedReason)
        {
            RejectedReason = rejectedReason ?? throw new ArgumentNullException(nameof(rejectedReason));
        }

        public CommandRejectedReason RejectedReason { get; }

        public TPayload Result { get; }

    }
    public class CommandRejectedReason
    {
        public CommandRejectedReason(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException(nameof(reason));
            Reason = reason;
        }

        public string Reason { get; }
    }

}
