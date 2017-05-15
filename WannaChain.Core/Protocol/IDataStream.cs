using WannaChain.Core.Protocol.Data;

namespace WannaChain.Core.Protocol
{

    /// <summary>
    /// Event of receiving a delegate
    /// </summary>
    public delegate void OnPacketReceivedEventHandler(Packet packet);

    /// <summary>
    /// Data Stream 
    /// </summary>
    public interface IDataStream
    {

        /// <summary>
        /// event handler for on packet received
        /// </summary>
        /// <value>The on packet received.</value>
        OnPacketReceivedEventHandler OnPacketReceived { get; set; }

        /// <summary>
        /// Send a packet to 
        /// </summary>
        /// <returns>The send.</returns>
        /// <param name="packet">Packet.</param>
        bool Send(Packet packet);

        /// <summary>
        /// Close current datastream
        /// </summary>
        void Close();
    }
}
