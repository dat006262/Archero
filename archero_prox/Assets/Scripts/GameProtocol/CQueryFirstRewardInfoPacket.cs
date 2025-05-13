




using System.IO;

namespace GameProtocol
{
	public sealed class CQueryFirstRewardInfoPacket : CProtocolBase
	{
		public override ushort GetMsgType => 17;

		protected override void OnReadFromStream(BinaryReader reader)
		{
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
		}
	}
}
