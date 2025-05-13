




using System.IO;

namespace GameProtocol
{
	public sealed class CRespInAppPurchase : IProtocol
	{
		public string m_strIAPTransID;

		public uint m_nTotalCoins;

		public uint m_nTotalDiamonds;

		public ushort m_nBattleRebornCount;

		public ushort m_nNormalDiamondItems;

		public ushort m_nLargeDiamondItems;

		public CEquipmentItem[] m_arrEquipInfo;

		public string product_id;

		public ushort GetMsgType => 15;

		public void ReadFromStream(BinaryReader reader)
		{
			m_strIAPTransID = reader.ReadString();
			m_nTotalCoins = reader.ReadUInt32();
			m_nTotalDiamonds = reader.ReadUInt32();
			m_nBattleRebornCount = reader.ReadUInt16();
			m_nNormalDiamondItems = reader.ReadUInt16();
			m_nLargeDiamondItems = reader.ReadUInt16();
			ushort num = reader.ReadUInt16();
			m_arrEquipInfo = new CEquipmentItem[num];
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				m_arrEquipInfo[num2] = new CEquipmentItem();
				m_arrEquipInfo[num2].ReadFromStream(reader);
			}
		}

		public void WriteToStream(BinaryWriter writer)
		{
			writer.Write(m_strIAPTransID);
			writer.Write(m_nTotalCoins);
			writer.Write(m_nTotalDiamonds);
			writer.Write(m_nBattleRebornCount);
			writer.Write(m_nNormalDiamondItems);
			writer.Write(m_nLargeDiamondItems);
			writer.Write((ushort)m_arrEquipInfo.Length);
			for (ushort num = 0; num < (ushort)m_arrEquipInfo.Length; num = (ushort)(num + 1))
			{
				m_arrEquipInfo[num].WriteToStream(writer);
			}
		}

		public byte[] buildPacket()
		{
			BinaryWriter writer = ProtocolBuffer.Writer;
			writer.Write((byte)13);
			writer.Write(GetMsgType);
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer2 = new CustomBinaryWriter(memoryStream);
			WriteToStream(writer2);
			ushort value = (ushort)memoryStream.ToArray().Length;
			writer.Write(value);
			writer.Write(memoryStream.ToArray());
			return ProtocolBuffer.CacheStream.ToArray();
		}
	}
}
