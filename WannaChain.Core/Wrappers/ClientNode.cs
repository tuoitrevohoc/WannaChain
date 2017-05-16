using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WannaChain.Core.Protocol;
using WannaChain.Core.Protocol.Data;
using WannaChain.Core.Services;
using WannaChain.Models;

namespace WannaChain.Core.Wrappers
{

    /// <summary>
    /// The client node
    /// </summary>
    public class ClientNode<TData>: IPeer<TData>
    {
        /// <summary>
        /// The data stream that connect
        /// </summary>
        readonly IDataStream dataStream;

        /// <summary>
        /// The source node
        /// </summary>
        readonly INode<TData> source;

        /// <summary>
        /// Get block hash callback
        /// </summary>
        TaskCompletionSource<string> blockHashSource;

        /// <summary>
        /// get blocks source
        /// </summary>
        TaskCompletionSource<ICollection<Block<TData>>> blocksSource;

        /// <summary>
        /// Initialize a ClientNode
        /// </summary>
        public ClientNode(IDataStream dataStream, INode<TData> source)
        {
            this.dataStream = dataStream;
            this.source = source;

            dataStream.OnPacketReceived += OnPacketReceived;
        }

		/// <summary>
		/// Ons the packet received.
		/// </summary>
		/// <param name="packet">Packet.</param>
		void OnPacketReceived(Packet packet)
		{
            switch(packet.Command) 
            {
                case CommandType.NewBlockAvailable:
	                {
	                    source.OnBlockCreated(this, packet.Get<int>());
	                    break;
	                }
                case CommandType.QueryBlockHash:
	                {
	                    var index = packet.Get<int>();
	                    var hash = source.GetBlockHash(index).Result;
	                    dataStream.Send(Packet.Create(CommandType.QueryBlockHashResult, hash));
	                    break;
	                }
                case CommandType.QueryBlocks:
	                {
	                    var index = packet.Get<int>();
	                    var blocks = source.GetBlocks(index).Result;
	                    dataStream.Send(Packet.Create(CommandType.QueryBlocksResult, blocks));
	                    break;
	                }
                case CommandType.QueryBlockHashResult:
                    {
                        var hash = packet.Get<string>();
                        blockHashSource.SetResult(hash);
                        break;
                    }
                case CommandType.QueryBlocksResult:
                    {
                        var blocks = packet.Get<List<Block<TData>>>();
                        blocksSource.SetResult(blocks);
                        break;
                    }
            }
		}

        /// <summary>
        /// Get a block hash
        /// </summary>
        /// <returns>The block hash.</returns>
        /// <param name="index">Index.</param>
        public async Task<string> GetBlockHash(int index)
        {
            var packet = new Packet(CommandType.QueryBlockHash, index.ToString());
            await dataStream.Send(packet);

            if (blockHashSource != null) 
            {
                throw new Exception("Call is in progress");
            }

            blockHashSource = new TaskCompletionSource<string>();

            return await blockHashSource.Task;
        }

        /// <summary>
        /// Get 
        /// </summary>
        /// <returns>The blocks.</returns>
        /// <param name="index">Index.</param>
        public async Task<ICollection<Block<TData>>> GetBlocks(int index)
        {
            var packet = Packet.Create(CommandType.QueryBlockHash, index);
			await dataStream.Send(packet);

            if (blocksSource != null)
			{
				throw new Exception("Call is in progress");
			}

            blocksSource = new TaskCompletionSource<ICollection<Block<TData>>>();

            return await blocksSource.Task;
        }

        /// <summary>
        /// Notify when block is created
        /// </summary>
        /// <returns><c>true</c>, if block created was oned, <c>false</c> otherwise.</returns>
        /// <param name="source">Source.</param>
        /// <param name="newIndex">New index.</param>
        public Task<bool> OnBlockCreated(IPeer<TData> source, int newIndex)
        {
            var packet = Packet.Create(CommandType.NewBlockAvailable, newIndex);
            dataStream.Send(packet);

            return Task.FromResult(true);
        }

    }
}
