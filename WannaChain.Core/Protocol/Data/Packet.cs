using System;
using Newtonsoft.Json;

namespace WannaChain.Core.Protocol.Data
{
    /// <summary>
    /// Command that send through network
    /// </summary>
    public enum CommandType 
    {
        NewBlockAvailable,
        QueryBlockHash,
        QueryBlockHashResult,
        QueryBlocks,
        QueryBlocksResult
    }

    /// <summary>
    /// A packet of data that send to network
    /// </summary>
    public class Packet
    {

        /// <summary>
        /// The command of this packet
        /// </summary>
        public CommandType Command { get; set; }

        /// <summary>
        /// The data of this package
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Initialize a packet with string and data
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="data">Data.</param>
        public Packet(CommandType command, string data) 
        {
            Command = command;
            Data = data;
        }

        /// <summary>
        /// Get data with type
        /// </summary>
        /// <returns>The get.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Get<T>()
        {
            return JsonConvert.DeserializeObject<T>(Data);
        }

        /// <summary>
        /// Create a packet 
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="command">Command.</param>
        /// <param name="data">Data.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Packet Create<T>(CommandType command, T data) 
        {
            return new Packet(command, JsonConvert.SerializeObject(data));
        }
    }
}
