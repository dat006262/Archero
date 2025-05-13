




using System.Security.Cryptography;

public class NetEncrypt
{
	public static void EncryptTest()
	{
		string text = "test string";
		string privateKey = string.Empty;
		string publicKey = string.Empty;
		RSAGenerateKey(ref privateKey, ref publicKey);
		Debugger.Log("加密前：" + text);
		string text2 = Encrypt_UTF8(text, publicKey);
		string str = DesDecrypt(text2, publicKey);
		Debugger.Log("解密后：" + str);
	}

	public static void RSAGenerateKey(ref string privateKey, ref string publicKey)
	{
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		privateKey = rSACryptoServiceProvider.ToXmlString(includePrivateParameters: true);
		publicKey = rSACryptoServiceProvider.ToXmlString(includePrivateParameters: false);
	}

	public static string Encrypt_UTF8(string text, string password)
	{
		return text.AesStr(password, password);
	}

	public static string DesDecrypt(string text, string password)
	{
		return text.UnAesStr(password, password);
	}
}
