




using System.IO;

namespace GameProtocol
{
	public sealed class CReqObtainTreasure : CProtocolBase
	{
		public uint m_nTransID;

		public uint m_nCoin;

		public CEquipmentItem m_stTreasureItems;

		public override ushort GetMsgType => 14;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_nTransID = reader.ReadUInt32();
			m_nCoin = reader.ReadUInt16();
			m_stTreasureItems.ReadFromStream(reader);
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nTransID);
			writer.Write(m_nCoin);
			m_stTreasureItems.WriteToStream(writer);
		}
	}
}
