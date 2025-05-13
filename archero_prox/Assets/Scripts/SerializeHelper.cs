




using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

public static class SerializeHelper
{
	public static string ConvertToString(byte[] data)
	{
		return Encoding.UTF8.GetString(data, 0, data.Length);
	}

	public static string ConvertToString(byte[] data, Encoding encoding)
	{
		return encoding.GetString(data, 0, data.Length);
	}

	public static byte[] ConvertToByte(string str)
	{
		return Encoding.UTF8.GetBytes(str);
	}

	public static byte[] ConvertToByte(string str, Encoding encoding)
	{
		return encoding.GetBytes(str);
	}

	public static byte[] SerializeToBinary(object obj)
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(memoryStream, obj);
		byte[] result = memoryStream.ToArray();
		memoryStream.Close();
		return result;
	}

	public static byte[] SerializeToXml(object obj)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
		xmlSerializer.Serialize(memoryStream, obj);
		byte[] result = memoryStream.ToArray();
		memoryStream.Close();
		return result;
	}

	public static object DeserializeWithBinary(byte[] data)
	{
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(data, 0, data.Length);
		memoryStream.Position = 0L;
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		object result = binaryFormatter.Deserialize(memoryStream);
		memoryStream.Close();
		return result;
	}

	public static T DeserializeWithBinary<T>(byte[] data)
	{
		return (T)DeserializeWithBinary(data);
	}

	public static T DeserializeWithXml<T>(byte[] data)
	{
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(data, 0, data.Length);
		memoryStream.Position = 0L;
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		object obj = xmlSerializer.Deserialize(memoryStream);
		memoryStream.Close();
		return (T)obj;
	}
}
