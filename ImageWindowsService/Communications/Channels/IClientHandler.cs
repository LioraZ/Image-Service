using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communications.Channels
{
    public interface IClientHandler
    {
        event EventHandler<TcpClient>ClientDisconnect;
        Mutex UpdateMutex(); 
        void HandleClient(TcpClient client);
    }
}
