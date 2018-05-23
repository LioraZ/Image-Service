using ImageService.Commands;
using ImageService.Infrastructure.Enums;

namespace ImageService.Controller
{
    public interface IImageController
    {
        /// <summary>
        /// The method will call the right command given the command ID, to be executed.
        /// </summary>
        /// <param name="commandID"></param> The given command ID.
        /// <param name="args"></param> THe command's args.
        /// <param name="result"></param> The result of the command, success/failure.
        /// <returns></returns>
        string ExecuteCommand(CommandEnum commandID, string[] args, out bool result);
        
    }
}
