




using System.IO;

namespace GameProtocol
{
	public sealed class CTimestampItem
	{
		public enum EItemIndex
		{
			ECurSrvItemIndex,
			EHarvestItemIndex,
			EInvalidItemIndex
		}

		public ulong m_i64Timestamp;

		public void ReadFromStream(BinaryReader reader)
		{
			m_i64Timestamp = reader.ReadUInt64();
		}

		public void WriteToStream(BinaryWriter writer)
		{
			writer.Write(m_i64Timestamp);
		}
	}
}
