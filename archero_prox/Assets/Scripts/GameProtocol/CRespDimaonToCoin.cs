




using System.IO;

namespace GameProtocol
{
	public sealed class CRespDimaonToCoin : IProtocol
	{
		public uint m_nCoins;

		public uint m_nDiamonds;

		public ushort GetMsgType => 10;

		public void ReadFromStream(BinaryReader reader)
		{
			m_nCoins = reader.ReadUInt32();
			m_nDiamonds = reader.ReadUInt32();
		}

		public void WriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nCoins);
			writer.Write(m_nDiamonds);
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
