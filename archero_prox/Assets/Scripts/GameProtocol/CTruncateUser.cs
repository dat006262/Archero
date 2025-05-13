




using System.IO;

namespace GameProtocol
{
	public sealed class CTruncateUser : CProtocolBase
	{
		public override ushort GetMsgType => 12;

		protected override void OnReadFromStream(BinaryReader reader)
		{
		}

		protected override void OnWriteToStream(BinaryWriter writer)
		{
		}
	}
}
