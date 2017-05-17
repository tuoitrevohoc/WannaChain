using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using WannaChain.Core.Protocol;
using WannaChain.Core.Protocol.Sockets;
using WannaChain.Core.Services;

namespace WannaChain.Core.Wrappers
{


    /// <summary>
    /// Server node
    /// </summary>
    public class ServerNode<TData>
    {

        /// <summary>
        /// The listener
        /// </summary>
        IListener listener;

        /// <summary>
        /// The node
        /// </summary>
        INode<TData> node;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WannaChain.Core.Wrappers.ServerNode`1"/> class.
        /// </summary>
        /// <param name="listener">Listener.</param>
        public ServerNode(IListener listener, INode<TData> node) 
        {
            this.listener = listener;
            this.node = node;

            listener.OnClientConnected += OnClientConnected;
        }


        /// <summary>
        /// Handle client connected
        /// </summary>
        /// <param name="dataStream">Data stream.</param>
        void OnClientConnected(IDataStream dataStream)
        {
            var peer = new ClientNode<TData>(dataStream, node);
            node.AddPeer(peer);
        }


        /// <summary>
        /// Add a client by ip and address
        /// </summary>
        /// <param name="address">Address.</param>
        /// <param name="port">Port.</param>
        public async Task<bool> AddClient(string address, int port) 
        {
            var client = new TcpClient();
            await client.ConnectAsync(address, port);

            var dataStream = new SocketDataStream(client.Client);
            OnClientConnected(dataStream);

            return true;
        }

        /// <summary>
        /// Start the server
        /// </summary>
        public void Start() 
        {
            listener.Start();
            Console.WriteLine("Server Up and Run");
        }
    }
}
