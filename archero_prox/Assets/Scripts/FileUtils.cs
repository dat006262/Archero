




using Dxx.Util;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class FileUtils
{
	public const string File_Equip = "localequip.txt";

	public const string File_Card = "File_Card";

	public const string File_Active = "File_Active";

	public const string File_Currency = "File_Currency";

	public const string File_Challenge = "File_Challenge";

	public const string File_TimeBox = "File_TimeBox";

	public const string File_Stage = "File_Stage";

	public const string File_Achieve = "File_Achieve1";

	public const string File_MysticShop = "File_MysticShop";

	public const string File_Extra = "File_Extra";

	public const string File_BoxDrop = "File_BoxDrop";

	public const string File_CardDrop = "File_CardDrop";

	public const string File_FakerStageDrop = "File_FakerStageDrop";

	public const string File_FakerEquipDrop = "File_FakerEquipDrop";

	public const string File_FakerCardDrop = "File_FakerCardDrop";

	public const string File_Shop = "File_Shop";

	public const string File_Mail = "mail.txt";

	public const string File_Harvest = "File_Harvest";

	public const string File_LocalSave = "localsave.txt";

	public static string EncryptKey = "4ptjerlkgjlk34jylkej4rgklj4klyj";

	private static string _localpath = string.Empty;

	private static string _FilesDir;

	private static string _CacheDir;

	private static string _ExternalFilesDir;

	private static string _ExternalCacheDir;

	public static string LocalPath
	{
		get
		{
			if (_localpath == string.Empty)
			{
				_localpath = GetPathInternal();
			}
			return _localpath;
		}
	}

	private static string GetPathInternal()
	{
		string empty = string.Empty;
		return Utils.FormatStringThread("{0}/Save", GetDataFolder());
	}

    //@TODO isEncrypt
    private static bool isEncrypt()
	{
        //return true;
        return false;
	}

	public static string GetDataFolder()
	{
		if (_FilesDir == null)
		{
#if ENABLE_ANDROID_NATIVE
            try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						using (AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getFilesDir", Array.Empty<object>()))
						{
							_FilesDir = androidJavaObject2.Call<string>("getCanonicalPath", Array.Empty<object>());
						}
						using (AndroidJavaObject androidJavaObject3 = androidJavaObject.Call<AndroidJavaObject>("getCacheDir", Array.Empty<object>()))
						{
							_CacheDir = androidJavaObject3.Call<string>("getCanonicalPath", Array.Empty<object>());
						}
						using (AndroidJavaObject androidJavaObject4 = androidJavaObject.Call<AndroidJavaObject>("getExternalFilesDir", null))
						{
							_ExternalFilesDir = androidJavaObject4.Call<string>("getCanonicalPath", Array.Empty<object>());
						}
						using (AndroidJavaObject androidJavaObject5 = androidJavaObject.Call<AndroidJavaObject>("getExternalCacheDir", Array.Empty<object>()))
						{
							_ExternalCacheDir = androidJavaObject5.Call<string>("getCanonicalPath", Array.Empty<object>());
						}
					}
				}
			}
			catch (Exception exception)
			{
				_FilesDir = Application.persistentDataPath;
				UnityEngine.Debug.LogException(exception);
			}
#else
            _FilesDir = Application.persistentDataPath;
#endif
        }
		return _FilesDir;
	}

	public static string GetPath()
	{
		return LocalPath;
	}

	public static LocalSave.BattleInBase GetBattleIn()
	{
		string fileName = LocalSave.BattleInBase.GetFileName(LocalSave.Instance.GetServerUserID());
		return ReadXmlFile<LocalSave.BattleInBase>(GetFullPath(fileName));
	}

	public static void WriteBattleIn(LocalSave.BattleInBase data)
	{
		LocalSave.Instance.DoThreadSave(LocalSave.EThreadWriteType.eBattle);
	}

	public static void WriteBattleInThread(LocalSave.BattleInBase data)
	{
		string fileName = LocalSave.BattleInBase.GetFileName(LocalSave.Instance.GetServerUserID());
		CreateWriteXML(GetFullPath(fileName), data);
	}

	public static string GetFullPath(string name)
	{
		string path = GetPath();
		try
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
		catch
		{
		}
		return Utils.FormatStringThread("{0}/{1}", path, name);
	}

	public static T GetXml<T>(string name) where T : new()
	{
		return ReadXmlFile<T>(GetFullPath(name));
	}

	public static void WriteXml<T>(string name, T data)
	{
		LocalSave.Instance.DoThreadSave(LocalSave.EThreadWriteType.eLocal);
	}

	public static void WriteEquip<T>(string name, T data)
	{
		LocalSave.Instance.DoThreadSave(LocalSave.EThreadWriteType.eEquip);
	}

	public static void WriteXmlThread<T>(string name, T data)
	{
		CreateWriteXML(GetFullPath(name), data);
	}

	public static void CreateWriteFile(string path, string info, bool isRelace = true)
	{
		FileInfo fileInfo = new FileInfo(path);
		if (fileInfo.Exists && isRelace)
		{
			File.Delete(path);
		}
		StreamWriter streamWriter = fileInfo.Exists ? fileInfo.AppendText() : fileInfo.CreateText();
		streamWriter.WriteLine(info);
		streamWriter.Close();
		streamWriter.Dispose();
	}

	private static void CreateWriteXML<T>(string path, T t)
	{
		string text = SerializeObject<T>(t);
		string empty = string.Empty;
		empty = (isEncrypt() ? NetEncrypt.Encrypt_UTF8(text, EncryptKey) : text);
		using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
		{
			byte[] bytes = Encoding.UTF8.GetBytes(empty);
			fileStream.Write(bytes, 0, bytes.Length);
			fileStream.Flush();
			fileStream.Close();
			fileStream.Dispose();
		}
	}

	public static string GetXmlFileString(string path)
	{
		path = GetFullPath(path);
		FileInfo fileInfo = new FileInfo(path);
		if (!fileInfo.Exists)
		{
			return string.Empty;
		}
		string text = string.Empty;
		try
		{
			FileStream fileStream = fileInfo.OpenRead();
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			text = Encoding.UTF8.GetString(array);
		}
		catch (Exception)
		{
			SdkManager.Bugly_Report("FileUtils.GetXmlFileString", Utils.FormatString("path : ..... get info failed." + path));
		}
		string result = string.Empty;
		try
		{
			string text2 = text;
			if (isEncrypt())
			{
				text2 = NetEncrypt.DesDecrypt(text, EncryptKey);
			}
			result = text2;
			return result;
		}
		catch
		{
			SdkManager.Bugly_Report("FileUtils.GetXmlFileString", Utils.FormatString("GetXmlFileString failed!!!!!!!!!!!!!!!!!!!!!! path:{0}", path));
			return result;
		}
	}

	private static T ReadXmlFile<T>(string path) where T : new()
	{
		FileInfo fileInfo = new FileInfo(path);
		if (!fileInfo.Exists)
		{
			return new T();
		}
		string text = string.Empty;
		try
		{
			FileStream fileStream = fileInfo.OpenRead();
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			text = Encoding.UTF8.GetString(array);
		}
		catch (Exception)
		{
			SdkManager.Bugly_Report("FileUtils.ReadXmlFile", Utils.FormatString("FileUtils {0} get info failed.", path));
		}
		T val = default(T);
		try
		{
			string empty = string.Empty;
			if (!isEncrypt())
			{
				empty = text;
				try
				{
					val = (T)DeserializeObject<T>(empty);
					return val;
				}
				catch
				{
					UnityEngine.Debug.LogError("DeserializeObject error " + empty);
					return val;
				}
			}
			try
			{
				empty = NetEncrypt.DesDecrypt(text, EncryptKey);
			}
			catch
			{
				empty = text;
			}
			val = (T)DeserializeObject<T>(empty);
			return val;
		}
		catch (Exception)
		{
			SdkManager.Bugly_Report("FileUtils.ReadXmlFile", Utils.FormatString("FileUtils.ReadXmlFile : {0}      DeserializeObject  false", path));
			return val;
		}
		finally
		{
			if (val == null)
			{
				val = new T();
			}
		}
	}

	public static string GetConfig(string name)
	{
		byte[] fileBytes = GetFileBytes("data/config", name);
		string empty = string.Empty;
		if (fileBytes != null)
		{
			try
			{
				return Encoding.Default.GetString(fileBytes);
			}
			catch
			{
				return string.Empty;
			}
		}
		return empty;
	}

	private static string UTF8ByteArrayToString(byte[] characters)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(characters);
	}

	private static byte[] StringToUTF8ByteArray(string pXmlString)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetBytes(pXmlString);
	}

	private static string SerializeObject<T>(object pObject)
	{
		string result = string.Empty;
		try
		{
			result = JsonConvert.SerializeObject(pObject);
			return result;
		}
		catch (Exception ex)
		{
			SdkManager.Bugly_Report("FileUtils.SerializeObject", ex.ToString());
			return result;
		}
	}

	private static object DeserializeObject<T>(string pXmlizedString)
	{
		try
		{
			return JsonConvert.DeserializeObject<T>(pXmlizedString);
		}
		catch
		{
			SdkManager.Bugly_Report("FileUtils.DeserializeObject", Utils.FormatString("string:{0} .... error", pXmlizedString));
			return null;
		}
	}

	public static string Encrypt(string value)
	{
		if (!isEncrypt())
		{
			return value;
		}
		string result = string.Empty;
		try
		{
			result = NetEncrypt.Encrypt_UTF8(value, EncryptKey);
			return result;
		}
		catch
		{
			return result;
		}
	}

	public static string DesDecrypt(string value)
	{
		if (!isEncrypt())
		{
			return value;
		}
		string result = string.Empty;
		try
		{
			result = NetEncrypt.DesDecrypt(value, EncryptKey);
			return result;
		}
		catch
		{
			return result;
		}
	}

	public static void ClearFile(string name)
	{
		FileInfo fileInfo = new FileInfo(GetDataFolder() + "/" + name);
		StreamWriter streamWriter = fileInfo.CreateText();
		streamWriter.Close();
		streamWriter.Dispose();
	}

	public static void CreateFile(string path, string name, string info)
	{
		FileInfo fileInfo = new FileInfo(path + "/" + name);
		StreamWriter streamWriter = fileInfo.Exists ? fileInfo.AppendText() : fileInfo.CreateText();
		streamWriter.Write(info + "\r\n");
		streamWriter.Close();
		streamWriter.Dispose();
	}

	public static void CreateFileOverride(string name, byte[] info)
	{
		string text = name;
		string text2 = name;
		string[] array = name.Split('/');
		name = string.Empty;
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array[i] = Encrypt(array[i]);
			name = ((i >= num - 1) ? (name + array[i]) : (name + Utils.FormatStringThread("{0}/", array[i])));
		}
		if (array.Length > 1)
		{
			int j = 0;
			for (int num2 = array.Length - 1; j < num2; j++)
			{
				string text3 = GetDataFolder();
				int k = 0;
				for (int num3 = j; k <= num3; k++)
				{
					text3 = Utils.FormatStringThread("{0}/{1}", text3, array[k]);
					if (!Directory.Exists(text3))
					{
						Directory.CreateDirectory(text3);
					}
				}
			}
		}
		text2 = Utils.FormatStringThread("{0}/{1}", GetDataFolder(), name);
		FileStream fileStream = new FileStream(text2, FileMode.Create);
		string value = Convert.ToBase64String(info);
		value = Encrypt(value);
		info = Convert.FromBase64String(value);
		fileStream.Write(info, 0, info.Length);
		fileStream.Close();
	}

	public static void CreateFileOverride(string dir, string name, string info)
	{
		string text = Utils.FormatStringThread("{0}/{1}", GetDataFolder(), dir);
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		text = Utils.FormatStringThread("{0}/{1}", text, name);
		using (StreamWriter streamWriter = new StreamWriter(text, append: false))
		{
			streamWriter.Write(info);
		}
	}

	private static string GetEncrpytPath(string path)
	{
		string text = path;
		string[] array = path.Split('/');
		path = string.Empty;
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array[i] = Encrypt(array[i]);
			path = ((i >= num - 1) ? (path + array[i]) : (path + Utils.FormatStringThread("{0}/", array[i])));
		}
		return path;
	}

	private static string GetEncryptFullPath(string path)
	{
		return Utils.FormatStringThread("{0}/{1}", GetDataFolder(), GetEncrpytPath(path));
	}

	public static byte[] GetFileBytes(string dir, string name)
	{
		byte[] array = null;
		try
		{
			string encryptFullPath = GetEncryptFullPath(Utils.FormatStringThread("{0}", dir));
			if (!Directory.Exists(encryptFullPath))
			{
				return null;
			}
			encryptFullPath = GetEncryptFullPath(Utils.FormatStringThread("{0}/{1}", dir, name));
			FileInfo fileInfo = new FileInfo(encryptFullPath);
			if (!fileInfo.Exists)
			{
				return null;
			}
			try
			{
				FileStream fileStream = fileInfo.OpenRead();
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
			}
			catch (Exception)
			{
				SdkManager.Bugly_Report("FileUtils.GetFileBytes", Utils.FormatStringThread("FileUtils {0} get info failed.", encryptFullPath));
			}
		}
		catch
		{
		}
		string value = Convert.ToBase64String(array);
		value = DesDecrypt(value);
		return Convert.FromBase64String(value);
	}

	public static void DeleteFile(string path, string name)
	{
		path = Utils.FormatStringThread("{0}/{1}", path, name);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static void WriteError(object str)
	{
		Write("ErrorLog.txt", str.ToString());
	}

	public static void Write(string name, string str)
	{
		ClearFile(name);
		CreateFile(GetDataFolder(), name, str);
	}
}
