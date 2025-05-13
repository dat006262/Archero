




using Newtonsoft.Json;
using System;
using System.IO;

namespace GameProtocol
{
	[Serializable]
	public sealed class CReqItemPacket : CProtocolBase
	{
		public enum eItemType
		{
			EBattleType = 1,
			ETimeType,
			ELevelType,
			ELayerType,
			EDiamondType,
			EMailType,
			EDiamondToCoinType,
			EItemUpgrade,
			EEquipItemTrans,
			EDiamondToLifeTrans,
			ECoinToPotionTrans,
			EDiamondToPotionTrans,
			EObtainTreasureTrans,
			EBuyDiamondsFromShop,
			EFirstRewardFromShop,
			EEquipCompositeTrans,
			EGameHarvestType,
			EAdGetLifeType,
			EAdGetLuckyType,
			EInvalidType
		}

		public uint m_nTransID;

		public ushort m_nPacketType;

		public ushort m_nFromType;

		public uint m_nExtraInfo;

		public uint m_nCoinAmount;

		public uint m_nDiamondAmount;

		public ushort m_nLife;

		public ushort m_nExperince;

		public CEquipmentItem[] arrayEquipItems;

		public ushort m_nNormalDiamondItem;

		public ushort m_nLargeDiamondItem;

		public ushort m_nRebornCount;

		[JsonIgnore]
		public override ushort GetMsgType => 7;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_nTransID = reader.ReadUInt32();
			m_nPacketType = reader.ReadUInt16();
			m_nFromType = reader.ReadUInt16();
			m_nExtraInfo = reader.ReadUInt32();
			m_nCoinAmount = reader.ReadUInt32();
			m_nDiamondAmount = reader.ReadUInt32();
			m_nLife = reader.ReadUInt16();
			m_nExperince = reader.ReadUInt16();
			ushort num = reader.ReadUInt16();
			arrayEquipItems = new CEquipmentItem[num];
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				arrayEquipItems[num2] = new CEquipmentItem();
				arrayEquipItems[num2].ReadFromStream(reader);
			}
			m_nNormalDiamondItem = reader.ReadUInt16();
			m_nLargeDiamondItem = reader.ReadUInt16();
			m_nRebornCount = reader.ReadUInt16();
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nTransID);
			writer.Write(m_nPacketType);
			writer.Write(m_nFromType);
			writer.Write(m_nExtraInfo);
			writer.Write(m_nCoinAmount);
			writer.Write(m_nDiamondAmount);
			writer.Write(m_nLife);
			writer.Write(m_nExperince);
			ushort num = (ushort)((arrayEquipItems != null) ? arrayEquipItems.Length : 0);
			writer.Write(num);
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				arrayEquipItems[num2].WriteToStream(writer);
			}
			writer.Write(m_nNormalDiamondItem);
			writer.Write(m_nLargeDiamondItem);
			writer.Write(m_nRebornCount);
		}
	}
}
