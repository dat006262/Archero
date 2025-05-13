




using System.IO;

namespace GameProtocol
{
	public sealed class CRestoreItem
	{
		public enum EItemIndex
		{
			ELifeItemIndex,
			ENormalDiamondItemIndex,
			ELargeDiamondItemIndex,
			EAdGetLifeItemIndex,
			EAdGetLuckyItemIndex,
			EInvalidItemIndex
		}

		public short m_nMin;

		public ushort m_nMax;

		public ulong m_i64Timestamp;

		public void ReadFromStream(BinaryReader reader)
		{
			m_nMin = reader.ReadInt16();
			m_nMax = reader.ReadUInt16();
			m_i64Timestamp = reader.ReadUInt64();
		}

		public void WriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nMin);
			writer.Write(m_nMax);
			writer.Write(m_i64Timestamp);
		}
	}
}
