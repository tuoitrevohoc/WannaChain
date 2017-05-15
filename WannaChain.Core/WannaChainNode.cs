using System.Collections.Generic;
using System.Linq;
using WannaChain.Core.Contracts;
using WannaChain.Core.Services;
using WannaChain.Models;

namespace WannaChain.Core
{

    /// <summary>
    /// A Simple implementation of a block chain node
    /// </summary>
    public class WannaChainNode<TData>: INode<TData>
    {

        /// <summary>
        /// The data contract
        /// </summary>
        IDataContract<TData> dataContract;

        /// <summary>
        /// The block contract that contain
        /// </summary>
        IBlockContract<TData> blockContract;

        /// <summary>
        /// Connected peers
        /// </summary>
        List<INode<TData>> peers = new List<INode<TData>>();

        /// <summary>
        /// Gets or sets the chains.
        /// </summary>
        /// <value>The chains.</value>
        protected List<Block<TData>> Chains { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:WannaChain.Core.WannaChainNode`1"/> class.
        /// </summary>
        /// <param name="dataContract">Data contract.</param>
        /// <param name="blockContract">Block contract.</param>
        public WannaChainNode(IDataContract<TData> dataContract,
                            IBlockContract<TData> blockContract) 
        {
            this.dataContract = dataContract;
            this.blockContract = blockContract;
        }


        /// <summary>
        /// Add a block to the chain
        /// </summary>
        /// <param name="block">Block.</param>
        public void AddBlock(Block<TData> block)
        {
            var currentNode = Chains.Last();
            var acceptable = dataContract.IsValid(block.Data)
                                && blockContract.IsValid(currentNode, block);

            if (acceptable) 
            {
                Chains.Add(block);

                BroadCastNewBlockEvent(block.Index);
            }
        }

        /// <summary>
        /// Get a list of block from an index
        /// </summary>
        /// <returns>The block.</returns>
        /// <param name="index">Index.</param>
        public ICollection<Block<TData>> GetBlocks(int index)
        {
            var count = Chains.Count;
            return Chains.GetRange(index, count - index);
        }

        /// <summary>
        /// Get a list of block hashes
        /// </summary>
        /// <returns>The block hashes.</returns>
        /// <param name="index">Index.</param>
        public string GetBlockHash(int index)
        {
            return Chains[index].Hash;
        }

        /// <summary>
        /// When a block is created
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="newIndex">the new block index</param>
        public bool OnBlockCreated(INode<TData> source, int newIndex)
        {
            var currentIndex = Chains.Count;
            var hasNewBlocks = newIndex > currentIndex;

            if (hasNewBlocks)
            {

                while (Chains[currentIndex].Hash != source.GetBlockHash(currentIndex)
                       && currentIndex > 0)
                {
                    currentIndex--;
                }

                currentIndex++;

                // download the new branch for replacement
                var newBlocks = source.GetBlocks(currentIndex);

                Chains.RemoveRange(currentIndex, Chains.Count - currentIndex);
                Chains.AddRange(newBlocks);

                BroadCastNewBlockEvent(newIndex);
            }

            return hasNewBlocks;
        }

        /// <summary>
        /// Broads the cast new block event.
        /// </summary>
        /// <param name="index">Index.</param>
        void BroadCastNewBlockEvent(int index) {
            peers.Select(peer => peer.OnBlockCreated(this, index));
        }
    }
}
