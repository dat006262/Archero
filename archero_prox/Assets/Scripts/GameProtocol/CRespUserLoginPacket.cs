




using System.IO;

namespace GameProtocol
{
	public sealed class CRespUserLoginPacket : IProtocol
	{
		public CEquipmentItem[] m_arrayEquipData;

		public CRestoreItem[] m_arrayRestoreData;

		public CTimestampItem[] m_arrayTimestampData;

		public uint m_nTransID;

		public uint m_nCoins;

		public uint m_nDiamonds;

		public ushort m_nMaxLayer;

		public ushort m_nLayerBoxID;

		public ushort m_nLevel;

		public uint m_nExperince;

		public uint m_nTreasureRandomCount;

		public ushort m_nBattleRebornCount;

		public string m_strUserAccessToken;

		public ulong m_nUserRawId;

		public ushort m_nExtraNormalDiamondItem;

		public ushort m_nExtraLargeDiamondItem;

		public long m_nGameSystemMask;

		public ushort GetMsgType => 8;

		public ulong GetServerTime()
		{
			return GetTime(CTimestampItem.EItemIndex.ECurSrvItemIndex);
		}

		public ulong GetHarvestTime()
		{
			return GetTime(CTimestampItem.EItemIndex.EHarvestItemIndex);
		}

		private ulong GetTime(CTimestampItem.EItemIndex type)
		{
			ulong result = 0uL;
			if (type >= CTimestampItem.EItemIndex.ECurSrvItemIndex && (int)type < m_arrayTimestampData.Length)
			{
				result = m_arrayTimestampData[(int)type].m_i64Timestamp;
			}
			return result;
		}

		public CRestoreItem GetRestore(CRestoreItem.EItemIndex type)
		{
			CRestoreItem result = null;
			if (type >= CRestoreItem.EItemIndex.ELifeItemIndex && (int)type < m_arrayRestoreData.Length)
			{
				result = m_arrayRestoreData[(int)type];
			}
			return result;
		}

		public void ReadFromStream(BinaryReader reader)
		{
			ushort num = reader.ReadUInt16();
			m_arrayEquipData = new CEquipmentItem[num];
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				m_arrayEquipData[num2] = new CEquipmentItem();
				m_arrayEquipData[num2].ReadFromStream(reader);
			}
			num = reader.ReadUInt16();
			m_arrayRestoreData = new CRestoreItem[num];
			for (ushort num3 = 0; num3 < num; num3 = (ushort)(num3 + 1))
			{
				m_arrayRestoreData[num3] = new CRestoreItem();
				m_arrayRestoreData[num3].ReadFromStream(reader);
			}
			num = reader.ReadUInt16();
			m_arrayTimestampData = new CTimestampItem[num];
			for (ushort num4 = 0; num4 < num; num4 = (ushort)(num4 + 1))
			{
				m_arrayTimestampData[num4] = new CTimestampItem();
				m_arrayTimestampData[num4].ReadFromStream(reader);
			}
			m_nTransID = reader.ReadUInt32();
			m_nCoins = reader.ReadUInt32();
			m_nDiamonds = reader.ReadUInt32();
			m_nMaxLayer = reader.ReadUInt16();
			m_nLayerBoxID = reader.ReadUInt16();
			m_nLevel = reader.ReadUInt16();
			m_nExperince = reader.ReadUInt32();
			m_nTreasureRandomCount = reader.ReadUInt32();
			m_nBattleRebornCount = reader.ReadUInt16();
			m_strUserAccessToken = reader.ReadString();
			m_nUserRawId = reader.ReadUInt64();
			m_nExtraNormalDiamondItem = reader.ReadUInt16();
			m_nExtraLargeDiamondItem = reader.ReadUInt16();
			m_nGameSystemMask = reader.ReadInt64();
		}

		public void WriteToStream(BinaryWriter writer)
		{
			ushort num = (ushort)m_arrayEquipData.Length;
			writer.Write(num);
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				m_arrayEquipData[num2].WriteToStream(writer);
			}
			num = (ushort)m_arrayRestoreData.Length;
			writer.Write(num);
			for (ushort num3 = 0; num3 < num; num3 = (ushort)(num3 + 1))
			{
				m_arrayRestoreData[num3].WriteToStream(writer);
			}
			num = (ushort)m_arrayTimestampData.Length;
			writer.Write(num);
			for (ushort num4 = 0; num4 < num; num4 = (ushort)(num4 + 1))
			{
				m_arrayTimestampData[num4].WriteToStream(writer);
			}
			writer.Write(m_nTransID);
			writer.Write(m_nCoins);
			writer.Write(m_nDiamonds);
			writer.Write(m_nMaxLayer);
			writer.Write(m_nLayerBoxID);
			writer.Write(m_nLevel);
			writer.Write(m_nExperince);
			writer.Write(m_nTreasureRandomCount);
			writer.Write(m_nBattleRebornCount);
			writer.Write(m_strUserAccessToken);
			writer.Write(m_nUserRawId);
			writer.Write(m_nExtraNormalDiamondItem);
			writer.Write(m_nExtraLargeDiamondItem);
			writer.Write(m_nGameSystemMask);
		}

		private byte[] buildPacket()
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
