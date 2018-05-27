using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communications.Channels
{
    /// <summary>
    /// Interface IClientHandler
    /// </summary>
    public interface IClientHandler
    {
        event EventHandler<TcpClient>ClientDisconnect;
        /// <summary>
        /// Updates the mutex.
        /// </summary>
        /// <returns>Mutex.</returns>
        Mutex UpdateMutex();
        /// <summary>
        /// Handles the client.
        /// </summary>
        /// <param name="client">The client.</param>
        void HandleClient(TcpClient client);
    }
}
