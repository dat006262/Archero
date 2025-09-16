




using System.IO;

public class CustomBinaryWriter : BinaryWriter
{
	public CustomBinaryWriter(Stream stream)
		: base(stream)
	{
	}

	public override void Write(string value)
	{
		ushort value2 = (ushort)ProtocolBuffer.Encoding.GetByteCount(value);
		byte[] bytes = ProtocolBuffer.Encoding.GetBytes(value);
		base.Write(value2);
		if (bytes.Length > 0)
		{
			base.Write(bytes);
		}
	}
}
