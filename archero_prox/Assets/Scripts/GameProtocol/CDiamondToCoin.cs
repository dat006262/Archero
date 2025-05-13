




using System.IO;

namespace GameProtocol
{
	public sealed class CDiamondToCoin : CProtocolBase
	{
		public uint m_nTransID;

		public uint m_nCoins;

		public uint m_nDiamonds;

		public override ushort GetMsgType => 10;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_nTransID = reader.ReadUInt32();
			m_nCoins = reader.ReadUInt32();
			m_nDiamonds = reader.ReadUInt32();
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nTransID);
			writer.Write(m_nCoins);
			writer.Write(m_nDiamonds);
		}
	}
}
