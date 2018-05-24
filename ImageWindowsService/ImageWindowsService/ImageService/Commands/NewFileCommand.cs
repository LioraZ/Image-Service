using ImageService.Model;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModel model;       // The Image Model

        /// <summary>
        /// The NewfileCommand's constructor.
        /// </summary>
        /// <param name="imageModel"></param> The image model.
        public NewFileCommand(IImageServiceModel imageModel) { model = imageModel; }

        /// <summary>
        /// The method implemtes the Execute method from ICommand class
        /// </summary>
        /// <param name="args"></param> The command's arguments contain the file's path.
        /// <param name="result"></param> The command's result.
        /// <returns></returns>
        public string Execute(string[] args, out bool result) {
            string msg = model.AddFile(args[0], out result);
            if (result) return "Image" + args[0] + "moved to output directory";
            else { return "Couldn't move image" + args[0] + "to output dir"; }
        }
    }
}
