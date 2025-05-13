




using System.IO;

namespace GameProtocol
{
	public sealed class CRespMailList : IProtocol
	{
		public const ushort MsgType = 5;

		public CMailInfo[] mailList;

		public ushort GetMsgType => 5;

		public void ReadFromStream(BinaryReader reader)
		{
			ushort num = reader.ReadUInt16();
			mailList = new CMailInfo[num];
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				mailList[num2] = new CMailInfo();
				mailList[num2].ReadFromStream(reader);
			}
		}

		public void WriteToStream(BinaryWriter writer)
		{
			ushort num = (ushort)((mailList != null) ? mailList.Length : 0);
			writer.Write(num);
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				mailList[num2].WriteToStream(writer);
			}
		}

		public byte[] buildPacket()
		{
			BinaryWriter writer = ProtocolBuffer.Writer;
			writer.Write((byte)13);
			writer.Write((ushort)5);
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
