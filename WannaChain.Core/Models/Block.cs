namespace WannaChain.Models
{

	/// <summary>
	/// A Block in the block chain
	/// </summary>
	public class Block<TData>
	{

		/// <summary>
		/// Index of the Block
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Hash
		/// </summary>
		public string Hash { get; set; }

		/// <summary>
		/// The magic number of this block
		/// </summary>
		/// <value>The magic number.</value>
		public long MagicNumber { get; set; }

		/// <summary>
		/// Previous Hash
		/// </summary>
		/// <value>The previous hash.</value>
		public string PreviousHash { get; set; }

		/// <summary>
		/// Data to store of the block
		/// </summary>
		public TData Data { get; set; }

	}
}
