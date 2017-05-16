using System.Collections.Generic;
using System.Threading.Tasks;
using WannaChain.Models;

namespace WannaChain.Core.Services
{

    /// <summary>
    /// Interface for a peer
    /// </summary>
    public interface IPeer<TData>
    {

		/// <summary>
		/// Event when a block is created
		/// </summary>
		/// <param name="source">a block is created</param>
		/// <param name="newIndex">the new block index</param>
        Task<bool> OnBlockCreated(IPeer<TData> source, int newIndex);

		/// <summary>
		/// Get the chain from index
		/// </summary>
		/// <returns>list of block from index</returns>
		/// <param name="index">index of the first block.</param>
		Task<ICollection<Block<TData>>> GetBlocks(int index);

		/// <summary>
		/// Get the list of block hashes.
		/// </summary>
		/// <returns>The block hashes.</returns>
		/// <param name="index">index of the first block</param>
		Task<string> GetBlockHash(int index);
    }
}
