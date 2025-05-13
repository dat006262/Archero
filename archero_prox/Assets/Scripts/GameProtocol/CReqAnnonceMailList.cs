




using System.IO;

namespace GameProtocol
{
	public sealed class CReqAnnonceMailList : CProtocolBase
	{
		public uint m_nLastMailID;

		public override ushort GetMsgType => 4;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_nLastMailID = reader.ReadUInt32();
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			writer.Write(m_nLastMailID);
		}
	}
}
