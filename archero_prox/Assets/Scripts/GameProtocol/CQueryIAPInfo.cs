




using System.IO;

namespace GameProtocol
{
	public sealed class CQueryIAPInfo : CProtocolBase
	{
		public ushort m_nPlatformIndex;

		public string m_strProductID;

		private byte[] m_arrSHA256;

		public override ushort GetMsgType => 16;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_nPlatformIndex = reader.ReadUInt16();
			m_strProductID = reader.ReadString();
			ushort num = reader.ReadUInt16();
			if (num > 0)
			{
				m_arrSHA256 = reader.ReadBytes(num);
			}
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nPlatformIndex);
			writer.Write(m_strProductID);
			writer.Write(m_arrSHA256.Length);
			writer.Write(m_arrSHA256);
		}
	}
}
