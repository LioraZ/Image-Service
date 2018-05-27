namespace ImageService.Commands
{
    /// <summary>
    /// Interface ICommand
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The method will execute the given command with its arguments.
        /// </summary>
        /// <param name="args"></param> The command's arguments.
        /// <param name="result"></param> The result of the command success/failure.
        /// <returns></returns>
        string Execute(string[] args, out bool result);
    }
}
