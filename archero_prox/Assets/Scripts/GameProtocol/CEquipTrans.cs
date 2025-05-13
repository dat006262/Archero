




using System.IO;

namespace GameProtocol
{
	public sealed class CEquipTrans : CProtocolBase
	{
		public enum eEquipTransType
		{
			ETransBuyType = 1,
			ETransSellType,
			ETransInvalidType
		}

		public uint m_nTransID;

		public CEquipmentItem m_stEquipItem;

		public ushort m_nCoins;

		public ushort m_nDiamonds;

		public byte m_nType;

		public override ushort GetMsgType => 11;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_nTransID = reader.ReadUInt32();
			m_stEquipItem = new CEquipmentItem();
			m_stEquipItem.ReadFromStream(reader);
			m_nCoins = reader.ReadUInt16();
			m_nDiamonds = reader.ReadUInt16();
			m_nType = reader.ReadByte();
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nTransID);
			m_stEquipItem.WriteToStream(writer);
			writer.Write(m_nCoins);
			writer.Write(m_nDiamonds);
			writer.Write(m_nType);
		}
	}
}
