




using Newtonsoft.Json;
using System;
using System.IO;

namespace GameProtocol
{
	[Serializable]
	public sealed class CEquipmentItem : IProtocol
	{
		public const ushort MsgType = ushort.MaxValue;

		public string m_nUniqueID;

		public ulong m_nRowID;

		public uint m_nEquipID;

		public uint m_nLevel;

		public uint m_nFragment;

		[JsonIgnore]
		public ushort GetMsgType => ushort.MaxValue;

		public void ReadFromStream(BinaryReader reader)
		{
			m_nUniqueID = reader.ReadString();
			m_nRowID = reader.ReadUInt64();
			m_nEquipID = reader.ReadUInt32();
			m_nLevel = reader.ReadUInt32();
			m_nFragment = reader.ReadUInt32();
			if (m_nUniqueID.Length > 37)
			{
				m_nUniqueID = m_nUniqueID.Substring(0, 37);
			}
		}

		public void WriteToStream(BinaryWriter writter)
		{
			writter.Write(m_nUniqueID);
			writter.Write(m_nRowID);
			writter.Write(m_nEquipID);
			writter.Write(m_nLevel);
			writter.Write(m_nFragment);
		}

		public byte[] buildPacket()
		{
			return null;
		}
	}
}
