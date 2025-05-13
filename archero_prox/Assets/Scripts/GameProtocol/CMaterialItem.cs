




using System.IO;

namespace GameProtocol
{
	public sealed class CMaterialItem : IProtocol
	{
		public const ushort MsgType = ushort.MaxValue;

		public uint m_nEquipID;

		public uint m_nMaterial;

		public ushort GetMsgType => ushort.MaxValue;

		public void ReadFromStream(BinaryReader reader)
		{
			m_nEquipID = reader.ReadUInt32();
			m_nMaterial = reader.ReadUInt32();
		}

		public void WriteToStream(BinaryWriter writter)
		{
			writter.Write(m_nEquipID);
			writter.Write(m_nMaterial);
		}

		public byte[] buildPacket()
		{
			return null;
		}
	}
}
