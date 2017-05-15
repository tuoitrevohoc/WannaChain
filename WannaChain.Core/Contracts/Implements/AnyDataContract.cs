﻿namespace WannaChain.Core.Contracts.Implement
{

    /// <summary>
    /// Any data contract, not verifying data
    /// </summary>
    public class AnyDataContract<TData> : IDataContract<TData>
    {

        /// <summary>
        /// Always return true. 
        /// Not validating any data
        /// </summary>
        /// <returns>true</returns>
        /// <param name="data">Data.</param>
        public bool IsValid(TData data)
        {
            return true;
        }
    }
}
