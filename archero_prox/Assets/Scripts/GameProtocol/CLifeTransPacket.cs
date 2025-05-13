




using Newtonsoft.Json;
using System;
using System.IO;

namespace GameProtocol
{
	[Serializable]
	public sealed class CLifeTransPacket : CProtocolBase
	{
		public enum eLifeTransType
		{
			ETransSpendLife = 1,
			ETransDiamondToLife,
			ETransCoinToPotion,
			ETransDiamondToPotion,
			ETransDiamondToRevival,
			EInvalidType
		}

		public uint m_nTransID;

		public ushort m_nMaterial;

		public byte m_nType;

		[JsonIgnore]
		public override ushort GetMsgType => 13;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_nTransID = reader.ReadUInt32();
			m_nMaterial = reader.ReadUInt16();
			m_nType = reader.ReadByte();
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nTransID);
			writer.Write(m_nMaterial);
			writer.Write(m_nType);
		}
	}
}
