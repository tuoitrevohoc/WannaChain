using System;
namespace WannaChain.Core.Contracts
{

    /// <summary>
    /// Contract for data in blockchain
    /// </summary>
    public interface IDataContract<TData>
    {

        /// <summary>
        /// Check if data is valid
        /// </summary>
        /// <returns><c>true</c>, if data is valid, <c>false</c> otherwise.</returns>
        /// <param name="data">Data.</param>
        bool IsValid(TData data);
    }
}
