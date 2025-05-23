




using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class PlayerPrefsEncrypt
{
	private static string sKEY = "ZTdkNTNmNDE2NTM3MWM0NDFhNTEzNzU1";

	private static string sIV = "4rZymEMfa/PpeJ89qY4gyA==";

	public static void SetInt(string key, int val)
	{
		PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
	}

	public static int GetInt(string key, int defaultValue = 0)
	{
		string @string = GetString(key, defaultValue.ToString());
		int result = defaultValue;
		int.TryParse(@string, out result);
		return result;
	}

	public static void SetBool(string key, bool val)
	{
		PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
	}

	public static bool GetBool(string key, bool defaultValue = false)
	{
		string @string = GetString(key, defaultValue.ToString());
		bool result = defaultValue;
		bool.TryParse(@string, out result);
		return result;
	}

	public static void SetUInt(string key, uint val)
	{
		PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
	}

	public static uint GetUInt(string key, uint defaultValue = 0u)
	{
		string @string = GetString(key, defaultValue.ToString());
		uint result = defaultValue;
		uint.TryParse(@string, out result);
		return result;
	}

	public static void SetLong(string key, long val)
	{
		PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
	}

	public static long GetLong(string key, long defaultValue = 0L)
	{
		string @string = GetString(key, defaultValue.ToString());
		long result = defaultValue;
		long.TryParse(@string, out result);
		return result;
	}

	public static void SetULong(string key, ulong val)
	{
		PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
	}

	public static ulong GetULong(string key, ulong defaultValue = 0uL)
	{
		string @string = GetString(key, defaultValue.ToString());
		ulong result = defaultValue;
		ulong.TryParse(@string, out result);
		return result;
	}

	public static void SetFloat(string key, float val)
	{
		PlayerPrefs.SetString(GetHash(key), Encrypt(val.ToString()));
	}

	public static float GetFloat(string key, float defaultValue = 0f)
	{
		string @string = GetString(key, defaultValue.ToString());
		float result = defaultValue;
		float.TryParse(@string, out result);
		return result;
	}

	public static void SetString(string key, string val)
	{
		PlayerPrefs.SetString(GetHash(key), Encrypt(val));
	}

	public static string GetString(string key, string defaultValue = "")
	{
		string text = defaultValue;
		string @string = PlayerPrefs.GetString(GetHash(key), defaultValue.ToString());
		if (!text.Equals(@string))
		{
			text = Decrypt(@string);
		}
		return text;
	}

	public static bool HasKey(string key)
	{
		string hash = GetHash(key);
		return PlayerPrefs.HasKey(hash);
	}

	public static void DeleteKey(string key)
	{
		string hash = GetHash(key);
		PlayerPrefs.DeleteKey(hash);
	}

	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	public static void Save()
	{
		PlayerPrefs.Save();
	}

	private static string Decrypt(string encString)
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Padding = PaddingMode.Zeros;
		rijndaelManaged.Mode = CipherMode.CBC;
		rijndaelManaged.KeySize = 128;
		rijndaelManaged.BlockSize = 128;
		RijndaelManaged rijndaelManaged2 = rijndaelManaged;
		byte[] bytes = Encoding.UTF8.GetBytes(sKEY);
		byte[] rgbIV = Convert.FromBase64String(sIV);
		ICryptoTransform transform = rijndaelManaged2.CreateDecryptor(bytes, rgbIV);
		byte[] array = Convert.FromBase64String(encString);
		byte[] array2 = new byte[array.Length];
		MemoryStream stream = new MemoryStream(array);
		CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		cryptoStream.Read(array2, 0, array2.Length);
		return Encoding.UTF8.GetString(array2).TrimEnd(default(char));
	}

	private static string Encrypt(string rawString)
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Padding = PaddingMode.Zeros;
		rijndaelManaged.Mode = CipherMode.CBC;
		rijndaelManaged.KeySize = 128;
		rijndaelManaged.BlockSize = 128;
		RijndaelManaged rijndaelManaged2 = rijndaelManaged;
		byte[] bytes = Encoding.UTF8.GetBytes(sKEY);
		byte[] rgbIV = Convert.FromBase64String(sIV);
		ICryptoTransform transform = rijndaelManaged2.CreateEncryptor(bytes, rgbIV);
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
		byte[] bytes2 = Encoding.UTF8.GetBytes(rawString);
		cryptoStream.Write(bytes2, 0, bytes2.Length);
		cryptoStream.FlushFinalBlock();
		byte[] inArray = memoryStream.ToArray();
		return Convert.ToBase64String(inArray);
	}

	private static string GetHash(string key)
	{
		MD5 mD = new MD5CryptoServiceProvider();
		byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(key));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}
}
