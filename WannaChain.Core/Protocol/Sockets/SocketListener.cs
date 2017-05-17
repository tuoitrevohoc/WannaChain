using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WannaChain.Core.Protocol.Sockets
{

    /// <summary>
    /// A listener for clients to connect
    /// </summary>
    public class SocketListener: IListener
    {

        /// <summary>
        /// A tcp listener
        /// </summary>
        TcpListener listener;

        /// <summary>
        /// the port to listen on
        /// </summary>
        readonly int port;

        /// <summary>
        /// The running thread
        /// </summary>
        Thread thread;

        /// <summary>
        /// is this listener running
        /// </summary>
        bool isRunning;

        /// <summary>
        /// Handle event when there is a client connected
        /// </summary>
        /// <value>The on client connected.</value>
        public OnClientConnectedEventHandler OnClientConnected { get; set; }

        /// <summary>
        /// Initialize a new NodeListener
        /// </summary>
        public SocketListener(int port)
        {
            this.port = port;
        }

        /// <summary>
        /// Listen to incoming.
        /// </summary>
        void Listen() 
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                isRunning = true;

                while (isRunning)
                {
                    var socket = listener.AcceptSocketAsync().Result;
                    var stream = new SocketDataStream(socket);

                    Console.WriteLine("Client connected!");
                    OnClientConnected(stream);
                }
            }
			catch (Exception exception)
			{
				Console.WriteLine("Exception handling thread: {0}", exception.Message);
				isRunning = false;
			}
        }

        /// <summary>
        /// Close current listener
        /// </summary>
        public void Close() 
        {
            isRunning = false;   
        }

        /// <summary>
        /// Start listening 
        /// </summary>
        public void Start()
        {
            thread = new Thread(Listen);
            thread.Start();
        }
    }
}
