﻿using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using WannaChain.Models;

namespace WannaChain.Core.Contracts.Implements
{

    /// <summary>
    /// A simple crypto block contract
    /// </summary>
    public class CryptoBlockContract<TData> : IBlockContract<TData>
    {

        /// <summary>
        /// Check if a block is valid
        /// </summary>
        /// <returns>if the new block is valid</returns>
        /// <param name="current">Current block.</param>
        /// <param name="next">Next block.</param>
        public bool IsValid(Block<TData> current, Block<TData> next)
        {
            var nextBlockHash = Hash(next);

            return (current.Hash == next.PreviousHash)
                && (next.Hash == nextBlockHash)
                && (next.Index == current.Index + 1)
				&& string.CompareOrdinal(nextBlockHash, next.Hash) == -1;
		}

		/// <summary>
		/// Create a block hash
		/// </summary>
		/// <returns>The hash.</returns>
		/// <param name="block">Block.</param>
		string Hash(Block<TData> block)
		{
			var index = block.Index;
			var data = JsonConvert.SerializeObject(block.Data);
			var lastHash = block.PreviousHash;
			var magicNumber = block.MagicNumber;

			var stringToHash = $"{index}|{data}|{lastHash}|{magicNumber}";
			var bytesToHash = Encoding.UTF8.GetBytes(stringToHash);
			var hashAlgorithm = SHA512.Create();
			var hashedData = hashAlgorithm.ComputeHash(bytesToHash);
			return Encoding.ASCII.GetString(hashedData);
		}
    }
}
