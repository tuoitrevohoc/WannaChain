using System.Collections.Generic;
using WannaChain.Models;

namespace WannaChain.Core.Services
{


    /// <summary>
    /// Interface for a node in the network
    /// </summary>
    public interface INode<TData>
    {
        /// <summary>
        /// Event when a block is created
        /// </summary>
        /// <param name="source">a block is created</param>
        /// <param name="newIndex">the new block index</param>
        bool OnBlockCreated(INode<TData> source, int newIndex);

        /// <summary>
        /// Add a new block to the chain
        /// </summary>
        /// <param name="block">the block to add</param>
        void AddBlock(Block<TData> block);

        /// <summary>
        /// Get the chain from index
        /// </summary>
        /// <returns>list of block from index</returns>
        /// <param name="index">index of the first block.</param>
        ICollection<Block<TData>> GetBlocks(int index);

        /// <summary>
        /// Get the list of block hashes.
        /// </summary>
        /// <returns>The block hashes.</returns>
        /// <param name="index">index of the first block</param>
        string GetBlockHash(int index);
    }
}
