




using System.IO;

public class CustomBinaryReader : BinaryReader
{
	public CustomBinaryReader(Stream stream)
		: base(stream)
	{
	}

	public override string ReadString()
	{
		ushort count = ReadUInt16();
		byte[] bytes = ReadBytes(count);
		return ProtocolBuffer.Encoding.GetString(bytes, 0, count);
	}
}
