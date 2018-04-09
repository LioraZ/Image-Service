using ImageService.Infrastructure;
using ImageService.Model;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModel model;

        public NewFileCommand(IImageServiceModel imageModel)
        {
            model = imageModel;            // Storing the Modal
        }

        public string Execute(string[] args, out bool result)
        {
            return model.AddFile(args[0], out result);
           /* string errorMsg = model.AddFile(args[0], out result);
            if (result) return args[0];
            return errorMsg;
			// The String Will Return the New Path if result = true, and will return the error message
            */
        }
    }
}
