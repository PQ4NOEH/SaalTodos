using Microsoft.AspNetCore.Mvc;
using Saal.Todos.Services.Core;

namespace Saal.Todos.UI.Core
{
    /// <summary>
    /// Extension methods for the controllerBase
    /// </summary>
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// Transforms a commandresult to a IActionResult
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="commandresult"></param>
        /// <returns></returns>
        public static IActionResult GetActionResult(this ControllerBase controller, CommandResult commandresult)
        {
            if (commandresult.RejectedReason != null) //If the command has been rejected may be for two reasons
            {
                if (commandresult.RejectedReason.AggregateNotFound)//The record the command request a change could not be found
                {
                    return controller.NotFound();
                }
                else // One or more of the command parameters do not pass the validation
                {
                    return controller.BadRequest(commandresult.RejectedReason.Reason);
                }
            }
            else // The command was executed succesfully
            {
                return controller.Ok();
            }
        }

        /// <summary>
        /// Transforms a commandresult to a IActionResult. A generic version of GetActionResult
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="controller"></param>
        /// <param name="commandresult"></param>
        /// <returns></returns>
        public static IActionResult GetActionResult<TResult>(this ControllerBase controller, CommandResult<TResult> commandresult)
        {
            if (commandresult.RejectedReason != null)//If the command has been rejected may be for two reasons
            {
                if (commandresult.RejectedReason.AggregateNotFound)//The record the command request a change could not be found
                {
                    return controller.NotFound();
                }
                else // One or more of the command parameters do not pass the validation
                {
                    return controller.BadRequest(commandresult.RejectedReason.Reason);
                }
            }
            else // The command was executed succesfully
            {
                return controller.Ok(commandresult.Result);
            }
        }
    }
}
