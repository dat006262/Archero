




#if ENABLE_GZIP
using ICSharpCode.SharpZipLib.GZip;
#endif
using System.IO;

public class GZipHelper
{
	public static byte[] Press(byte[] binary)
	{
#if ENABLE_GZIP
		MemoryStream memoryStream = new MemoryStream();
		GZipOutputStream gZipOutputStream = new GZipOutputStream(memoryStream);
		gZipOutputStream.Write(binary, 0, binary.Length);
		gZipOutputStream.Close();
		return memoryStream.ToArray();
#endif
        return new byte[0];
	}

	public static byte[] Depress(byte[] press)
	{
#if ENABLE_GZIP
		GZipInputStream gZipInputStream = new GZipInputStream(new MemoryStream(press));
		MemoryStream memoryStream = new MemoryStream();
		int num = 0;
		byte[] array = new byte[4096];
		while ((num = gZipInputStream.Read(array, 0, array.Length)) != 0)
		{
			memoryStream.Write(array, 0, num);
		}
		return memoryStream.ToArray();
#endif
        return new byte[0];
    }
}
