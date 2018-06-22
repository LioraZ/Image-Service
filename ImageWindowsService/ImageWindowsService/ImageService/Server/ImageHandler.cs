using Communications.Channels;
using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Event;
using ImageService.Logging;
using ImageService.Logging.Model;
using ImageWindowsService.ImageService.Model.Event;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageWindowsService.ImageService.Server
{
    public class ImageHandler : IClientHandler
    {
        public event EventHandler<TcpClient> ClientDisconnect;
        /// <summary>
        /// The controller
        /// </summary>
        private IImageController controller;
        /// <summary>
        /// The logger
        /// </summary>
        private ILoggingService logger;
        /// <summary>
        /// The stop
        /// </summary>
        private bool stop;
        /// <summary>
        /// The mutex
        /// </summary>
        private static Mutex mutex;


        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHandler"/> class.
        /// </summary>
        /// <param name="imageController">The image controller.</param>
        /// <param name="imageLogger">The image logger.</param>
        public ImageHandler(IImageController imageController, ILoggingService imageLogger)
        {
            stop = false;
            mutex = new Mutex();
            controller = imageController;
            logger = imageLogger;
        }

        /// <summary>
        /// Handles the client.
        /// </summary>
        /// <param name="client">The client.</param>
        public void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                while (!stop)
                {
                    byte[] readBytes = reader.ReadBytes(4);
                    if (BitConverter.IsLittleEndian) Array.Reverse(readBytes);
                    int dataLength = BitConverter.ToInt32(readBytes, 0);
                    byte[] imageNameBytes = reader.ReadBytes(dataLength);
                    string imageName = System.Text.Encoding.Default.GetString(imageNameBytes);

                    //reading image from bytes
                    readBytes = reader.ReadBytes(4);
                    if (BitConverter.IsLittleEndian) Array.Reverse(readBytes);
                    dataLength = BitConverter.ToInt32(readBytes, 0);
                    byte[] imageBytes = reader.ReadBytes(dataLength);
                    //Image image = byteArrayToImage(imageBytes);

                    if (imageBytes != null && imageName != null)
                    {
                        ImageData imageData = new ImageData() { Name = imageName, ImageBytes = imageBytes };
                        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(imageData);
                        bool result;
                        string logMessage = controller.ExecuteCommand(CommandEnum.NewImageCommand, new string[] { jsonString }, out result);
                        //  if (!result) logger.Log(logMessage, MessageTypeEnum.FAIL);
                        //  else logger.Log(logMessage, MessageTypeEnum.INFO);
                    }
                        // else logger.Log("Unable to receive image from connection", MessageTypeEnum.FAIL);
                }
            }
            catch
            {
                ClientDisconnect?.Invoke(this, client);
            }
        }
        
        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
                ms.Position = 0; // this is important
                return Image.FromStream(ms, true);
            }
            catch { return null; }
        }


        /// <summary>
        /// Updates the mutex.
        /// </summary>
        /// <returns>Mutex.</returns>
        public Mutex UpdateMutex()
        {
            return mutex;
        }
    }
}
