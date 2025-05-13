




using System.IO;

namespace GameProtocol
{
	public sealed class CReqSyncGameSystemMask : CProtocolBase
	{
		public CCommonRespMsg m_syncMsg;

		public override ushort GetMsgType => 19;

		protected override void OnReadFromStream(BinaryReader reader)
		{
			m_syncMsg.ReadFromStream(reader);
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
			m_syncMsg.WriteToStream(writer);
		}
	}
}
