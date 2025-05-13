




using System.IO;

namespace GameProtocol
{
	public sealed class CUserLoginPacket : CProtocolBase
	{
		public override ushort GetMsgType => 8;

		protected override void OnReadFromStream(BinaryReader reader)
		{
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
		}
	}
}
