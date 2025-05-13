




using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class PassWordHelper
{
	private const string PublicRsaKey = "pubKey";

	private const string PrivateRsaKey = "priKey";

	public static string Md532(this string source)
	{
		if (source.IsEmpty())
		{
			return null;
		}
		Encoding uTF = Encoding.UTF8;
		MD5 hashAlgorithmObj = MD5.Create();
		return HashAlgorithmBase(hashAlgorithmObj, source, uTF);
	}

	public static string Md532Salt(this string source, string salt)
	{
		return (!salt.IsEmpty()) ? (source + "『" + salt + "』").Md532() : source.Md532();
	}

	public static string Sha1(this string source)
	{
		if (source.IsEmpty())
		{
			return null;
		}
		Encoding uTF = Encoding.UTF8;
		SHA1 hashAlgorithmObj = new SHA1CryptoServiceProvider();
		return HashAlgorithmBase(hashAlgorithmObj, source, uTF);
	}

	public static string Sha256(this string source)
	{
		if (source.IsEmpty())
		{
			return null;
		}
		Encoding uTF = Encoding.UTF8;
		SHA256 hashAlgorithmObj = new SHA256Managed();
		return HashAlgorithmBase(hashAlgorithmObj, source, uTF);
	}

	public static string Sha512(this string source)
	{
		if (source.IsEmpty())
		{
			return null;
		}
		Encoding uTF = Encoding.UTF8;
		SHA512 hashAlgorithmObj = new SHA512Managed();
		return HashAlgorithmBase(hashAlgorithmObj, source, uTF);
	}

	public static string HmacSha1(this string source, string keyVal)
	{
		if (source.IsEmpty())
		{
			return null;
		}
		Encoding uTF = Encoding.UTF8;
		byte[] bytes = uTF.GetBytes(keyVal);
		HMACSHA1 hashAlgorithmObj = new HMACSHA1(bytes);
		return HashAlgorithmBase(hashAlgorithmObj, source, uTF);
	}

	public static string HmacSha256(this string source, string keyVal)
	{
		if (source.IsEmpty())
		{
			return null;
		}
		Encoding uTF = Encoding.UTF8;
		byte[] bytes = uTF.GetBytes(keyVal);
		HMACSHA256 hashAlgorithmObj = new HMACSHA256(bytes);
		return HashAlgorithmBase(hashAlgorithmObj, source, uTF);
	}

	public static string HmacSha384(this string source, string keyVal)
	{
		if (source.IsEmpty())
		{
			return null;
		}
		Encoding uTF = Encoding.UTF8;
		byte[] bytes = uTF.GetBytes(keyVal);
		HMACSHA384 hashAlgorithmObj = new HMACSHA384(bytes);
		return HashAlgorithmBase(hashAlgorithmObj, source, uTF);
	}

	public static string HmacSha512(this string source, string keyVal)
	{
		if (source.IsEmpty())
		{
			return null;
		}
		Encoding uTF = Encoding.UTF8;
		byte[] bytes = uTF.GetBytes(keyVal);
		HMACSHA512 hashAlgorithmObj = new HMACSHA512(bytes);
		return HashAlgorithmBase(hashAlgorithmObj, source, uTF);
	}

	public static bool IsEmpty(this string value)
	{
		return string.IsNullOrEmpty(value);
	}

	public static string HmacMd5(this string source, string keyVal)
	{
		if (source.IsEmpty())
		{
			return null;
		}
		Encoding uTF = Encoding.UTF8;
		byte[] bytes = uTF.GetBytes(keyVal);
		HMACMD5 hashAlgorithmObj = new HMACMD5(bytes);
		return HashAlgorithmBase(hashAlgorithmObj, source, uTF);
	}

	public static string HmacRipeMd160(this string source, string keyVal)
	{
		if (source.IsEmpty())
		{
			return null;
		}
		Encoding uTF = Encoding.UTF8;
		byte[] bytes = uTF.GetBytes(keyVal);
	//	HMACRIPEMD160 hashAlgorithmObj = new HMACRIPEMD160(bytes);
	HashAlgorithm hashAlgorithmObj = new SHA256Managed();
		return HashAlgorithmBase(hashAlgorithmObj, source, uTF);
	}

	public static string AesStr(this string source, string keyVal, string ivVal)
	{
		Encoding uTF = Encoding.UTF8;
		byte[] rgbKey = keyVal.FormatByte(uTF);
		byte[] rgbIV = ivVal.FormatByte(uTF);
		byte[] bytes = uTF.GetBytes(source);
		Rijndael rijndael = Rijndael.Create();
		string result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
			{
				cryptoStream.Write(bytes, 0, bytes.Length);
				cryptoStream.FlushFinalBlock();
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
		}
		rijndael.Clear();
		return result;
	}

	public static string UnAesStr(this string source, string keyVal, string ivVal)
	{
		Encoding uTF = Encoding.UTF8;
		byte[] rgbKey = keyVal.FormatByte(uTF);
		byte[] rgbIV = ivVal.FormatByte(uTF);
		byte[] array = Convert.FromBase64String(source);
		Rijndael rijndael = Rijndael.Create();
		string @string;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
			{
				cryptoStream.Write(array, 0, array.Length);
				cryptoStream.FlushFinalBlock();
				@string = uTF.GetString(memoryStream.ToArray());
			}
		}
		rijndael.Clear();
		return @string;
	}

	public static byte[] AesByte(this byte[] data, string keyVal, string ivVal)
	{
		byte[] array = new byte[32];
		Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(array.Length)), array, array.Length);
		byte[] array2 = new byte[16];
		Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(array2.Length)), array2, array2.Length);
		Rijndael rijndael = Rijndael.Create();
		try
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(array, array2), CryptoStreamMode.Write))
				{
					cryptoStream.Write(data, 0, data.Length);
					cryptoStream.FlushFinalBlock();
					return memoryStream.ToArray();
				}
			}
		}
		catch
		{
			return null;
		}
	}

	public static byte[] UnAesByte(this byte[] data, string keyVal, string ivVal)
	{
		byte[] array = new byte[32];
		Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(array.Length)), array, array.Length);
		byte[] array2 = new byte[16];
		Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(array2.Length)), array2, array2.Length);
		Rijndael rijndael = Rijndael.Create();
		try
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				using (CryptoStream cryptoStream = new CryptoStream(stream, rijndael.CreateDecryptor(array, array2), CryptoStreamMode.Read))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						byte[] array3 = new byte[1024];
						int count;
						while ((count = cryptoStream.Read(array3, 0, array3.Length)) > 0)
						{
							memoryStream.Write(array3, 0, count);
						}
						return memoryStream.ToArray();
					}
				}
			}
		}
		catch
		{
			return null;
		}
	}

	public static string Rsa(this string source)
	{
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.FromXmlString("pubKey");
		byte[] inArray = rSACryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(source), fOAEP: true);
		return Convert.ToBase64String(inArray);
	}

	public static string UnRsa(this string source)
	{
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.FromXmlString("priKey");
		byte[] bytes = rSACryptoServiceProvider.Decrypt(Convert.FromBase64String(source), fOAEP: true);
		return Encoding.UTF8.GetString(bytes);
	}

	public static string Des(this string source, string keyVal, string ivVal)
	{
		try
		{
			byte[] bytes = Encoding.UTF8.GetBytes(source);
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes((keyVal.Length <= 8) ? keyVal : keyVal.Substring(0, 8));
			dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes((ivVal.Length <= 8) ? ivVal : ivVal.Substring(0, 8));
			DESCryptoServiceProvider dESCryptoServiceProvider2 = dESCryptoServiceProvider;
			ICryptoTransform cryptoTransform = dESCryptoServiceProvider2.CreateEncryptor();
			byte[] value = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
			return BitConverter.ToString(value);
		}
		catch
		{
			return "转换出错！";
		}
	}

	public static string UnDes(this string source, string keyVal, string ivVal)
	{
		try
		{
			string[] array = source.Split("-".ToCharArray());
			byte[] array2 = new byte[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = byte.Parse(array[i], NumberStyles.HexNumber);
			}
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes((keyVal.Length <= 8) ? keyVal : keyVal.Substring(0, 8));
			dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes((ivVal.Length <= 8) ? ivVal : ivVal.Substring(0, 8));
			DESCryptoServiceProvider dESCryptoServiceProvider2 = dESCryptoServiceProvider;
			ICryptoTransform cryptoTransform = dESCryptoServiceProvider2.CreateDecryptor();
			byte[] bytes = cryptoTransform.TransformFinalBlock(array2, 0, array2.Length);
			return Encoding.UTF8.GetString(bytes);
		}
		catch
		{
			return "解密出错！";
		}
	}

	public static string Des3(this string source, string keyVal)
	{
		try
		{
			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
			tripleDESCryptoServiceProvider.Key = keyVal.FormatByte(Encoding.UTF8);
			tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
			tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider2 = tripleDESCryptoServiceProvider;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(source);
				try
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDESCryptoServiceProvider2.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytes, 0, bytes.Length);
						cryptoStream.FlushFinalBlock();
					}
					return memoryStream.ToArray().Bytes2Str();
				}
				catch
				{
					return source;
				}
			}
		}
		catch
		{
			return "TripleDES加密出现错误";
		}
	}

	public static string UnDes3(this string source, string keyVal)
	{
		try
		{
			byte[] array = source.Str2Bytes();
			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
			tripleDESCryptoServiceProvider.Key = keyVal.FormatByte(Encoding.UTF8);
			tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
			tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
			TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider2 = tripleDESCryptoServiceProvider;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDESCryptoServiceProvider2.CreateDecryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(array, 0, array.Length);
					cryptoStream.FlushFinalBlock();
					cryptoStream.Close();
					memoryStream.Close();
					return Encoding.UTF8.GetString(memoryStream.ToArray());
				}
			}
		}
		catch
		{
			return "TripleDES解密出现错误";
		}
	}

	public static string Base64(this string source)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(source);
		return Convert.ToBase64String(bytes, 0, bytes.Length);
	}

	public static string UnBase64(this string source)
	{
		byte[] bytes = Convert.FromBase64String(source);
		return Encoding.UTF8.GetString(bytes);
	}

	private static byte[] Str2Bytes(this string source)
	{
		source = source.Replace(" ", string.Empty);
		byte[] array = new byte[source.Length / 2];
		for (int i = 0; i < source.Length; i += 2)
		{
			array[i / 2] = Convert.ToByte(source.Substring(i, 2), 16);
		}
		return array;
	}

	private static string Bytes2Str(this IEnumerable<byte> source, string formatStr = "{0:X2}")
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte item in source)
		{
			stringBuilder.AppendFormat(formatStr, item);
		}
		return stringBuilder.ToString();
	}

	private static byte[] FormatByte(this string strVal, Encoding encoding)
	{
		return encoding.GetBytes(strVal.Base64().Substring(0, 16).ToUpper());
	}

	private static string HashAlgorithmBase(HashAlgorithm hashAlgorithmObj, string source, Encoding encoding)
	{
		byte[] bytes = encoding.GetBytes(source);
		byte[] source2 = hashAlgorithmObj.ComputeHash(bytes);
		return source2.Bytes2Str();
	}
}
