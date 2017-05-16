using System.Collections.Generic;
using WannaChain.Models;

namespace WannaChain.Core.Services
{


    /// <summary>
    /// Interface for a node in the network
    /// </summary>
    public interface INode<TData>: IPeer<TData>
    {

        /// <summary>
        /// A peer is added to this
        /// </summary>
        /// <param name="peer">Peer.</param>
        void AddPeer(IPeer<TData> peer);

		/// <summary>
		/// Add a new block to the chain
		/// </summary>
		/// <param name="block">the block to add</param>
		void AddBlock(Block<TData> block);

    }
}
