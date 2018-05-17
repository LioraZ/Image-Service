using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServieGUI.Communication
{
    class CommandManager
    {
       // private delegate CommunicationProcedure(CommandEnum, string);
        private Dictionary<CommandEnum, Task> communicationProcedureDict;
        private TCPServerChannel serverChannel;
        private TCPClientChannel clientChannel;
        public CommandManager()
        {
            serverChannel = new TCPServerChannel();
            clientChannel = new TCPClientChannel();
            communicationProcedureDict = new Dictionary<CommandEnum, Delegate>()
            {
                {CommandEnum.GetConfigCommand, callServer(CommandEnum.GetConfigCommand, Noargs)},
                {CommandEnum.RemoveHandlerCommand, callServer(CommandEnum.RemoveHandlerCommand, handlerName)},
                {CommandEnum.LogCommand, callClient(CommandEnum.LogCommand, logMesage) };
           
        }

        public void EstablishConnection(CommandEnum commandID, string args)
        {
            try
            {
                communicationProcedureDict[commandID].Method(commandID, args);
            }
            catch { }
        }

        public void CLientToServer(CommandEnum commandID, string[] args)
        {
            clientChannel.
        }
        public void ServerToClient(CommandEnum commandID, string[] args)
        {

        }
    }
}
