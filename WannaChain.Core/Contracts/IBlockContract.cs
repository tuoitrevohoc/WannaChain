using WannaChain.Models;

namespace WannaChain.Core.Contracts
{

    /// <summary>
    /// A contract that verifies a block in the chain
    /// </summary>
    public interface IBlockContract<TData>
    {

        /// <summary>
        /// Check if the next block in the chain is valid
        /// </summary>
        /// <returns><c>true</c>, if the block is valid, <c>false</c> otherwise.</returns>
        /// <param name="current">Current block.</param>
        /// <param name="next">Next block.</param>
        bool IsValid(Block<TData> current, Block<TData> next);
    }
}
