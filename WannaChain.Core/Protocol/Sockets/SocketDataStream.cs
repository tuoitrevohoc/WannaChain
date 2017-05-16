using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using WannaChain.Core.Protocol.Data;

namespace WannaChain.Core.Protocol.Sockets
{
	/// <summary>
	/// A data stream that use socket
	/// </summary>
	public class SocketDataStream : IDataStream
	{

		/// <summary>
		/// The stream reader
		/// </summary>
		readonly StreamReader reader;

		/// <summary>
		/// The stream writer
		/// </summary>
		readonly StreamWriter writer;

		/// <summary>
		/// The thread that runs this
		/// </summary>
		readonly Thread thread;

		/// <summary>
		/// is this socket data running
		/// </summary>
		bool isRunning;

		/// <summary>
		/// On packet received event handler
		/// </summary>
		/// <value>The on packet received.</value>
		public OnPacketReceivedEventHandler OnPacketReceived { get; set; }

		/// <summary>
		/// Create a socket data stream with a socket
		/// </summary>
		/// <param name="socket">Socket.</param>
		public SocketDataStream(Socket socket)
		{

			var stream = new NetworkStream(socket);
			reader = new StreamReader(stream);
			writer = new StreamWriter(stream);

			thread = new Thread(Reading);
			thread.Start();
		}

		/// <summary>
		/// Close current stream
		/// </summary>
		public void Close()
		{
			isRunning = false;
		}

		/// <summary>
		/// Reading this instance.
		/// </summary>
		void Reading()
		{
			try
			{
				isRunning = true;

				while (isRunning)
				{
					var commandString = reader.ReadLine();
					var data = reader.ReadLine();

					var command = (CommandType)Enum.Parse(typeof(CommandType), commandString);
					var packet = new Packet(command, data);

                    Console.WriteLine($"Incoming data {command}: {data}");

					OnPacketReceived(packet);
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine("Exception handling thread: {0}", exception.Message);
				isRunning = false;
			}
		}

		/// <summary>
		/// Send a packet
		/// </summary>
		/// <returns>The send.</returns>
		/// <param name="packet">Packet.</param>
		public async Task<bool> Send(Packet packet)
		{
			await writer.WriteLineAsync(packet.Command.ToString());
			await writer.WriteLineAsync(packet.Data);

			return true;
		}
	}
}
