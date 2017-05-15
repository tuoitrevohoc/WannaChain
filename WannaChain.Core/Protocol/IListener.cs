namespace WannaChain.Core.Protocol
{

	/// <summary>
	/// On client connected delegate.
	/// </summary>
	public delegate void OnClientConnectedEventHandler(IDataStream dataStream);

    /// <summary>
    /// A server side listener
    /// </summary>
    public interface IListener
    {
		/// <summary>
		/// Handle event when there is a client connected
		/// </summary>
		/// <value>The on client connected.</value>
		OnClientConnectedEventHandler OnClientConnected { get; set; }

        /// <summary>
        /// Close current listener
        /// </summary>
        void Close();
    }
}
