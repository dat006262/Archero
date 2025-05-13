




using System.IO;

public interface IProtocol
{
	ushort GetMsgType
	{
		get;
	}

	void ReadFromStream(BinaryReader reader);

	void WriteToStream(BinaryWriter writer);
}
