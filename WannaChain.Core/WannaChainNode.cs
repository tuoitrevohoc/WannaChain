using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        readonly IDataContract<TData> dataContract;

        /// <summary>
        /// The block contract that contain
        /// </summary>
        readonly IBlockContract<TData> blockContract;


        /// <summary>
        /// Connected peers
        /// </summary>
        readonly List<IPeer<TData>> peers = new List<IPeer<TData>>();

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

            Chains = new List<Block<TData>>();
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
        public Task<ICollection<Block<TData>>> GetBlocks(int index)
        {
            var count = Chains.Count;
            return Task.FromResult((ICollection<Block<TData>>)
                Chains.GetRange(index, count - index)
            );
        }

        /// <summary>
        /// Get a list of block hashes
        /// </summary>
        /// <returns>The block hashes.</returns>
        /// <param name="index">Index.</param>
        public Task<string> GetBlockHash(int index)
        {
            return Task.FromResult(Chains[index].Hash);
        }

        /// <summary>
        /// When a block is created
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="newIndex">the new block index</param>
        public async Task<bool> OnBlockCreated(IPeer<TData> source, int newIndex)
        {
            var currentIndex = Chains.Count - 1;
            var hasNewBlocks = newIndex > currentIndex;

            if (hasNewBlocks)
            {

                while (currentIndex > 0 
                        && Chains[currentIndex].Hash != await source.GetBlockHash(currentIndex))
                {
                    currentIndex--;
                }

                currentIndex++;

                // download the new branch for replacement
                var newBlocks = await source.GetBlocks(currentIndex);

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

        /// <summary>
        /// Add a peer to this noode
        /// </summary>
        /// <param name="peer">Peer.</param>
        public void AddPeer(IPeer<TData> peer)
        {
            peers.Add(peer);
        }
    }
}
