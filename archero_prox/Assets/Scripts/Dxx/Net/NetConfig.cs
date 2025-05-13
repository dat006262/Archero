




using System.Collections.Generic;

namespace Dxx.Net
{
	public class NetConfig
	{
		public const string HttpPath = "https://api-archero.habby.mobi:12020";

		public const string HttpIAPPath = "https://api-archero.habby.mobi:12020/IAP_Verification";

		public static Dictionary<int, string> mIPs = new Dictionary<int, string>
		{
			{
				0,
				"35.181.18.186"
			},
			{
				1,
				"18.138.14.129"
			}
		};

		public const ushort NetVersion = 1;

		public const ushort AppVersion = 2;

		public const string AppVersionName_Android = "1.0.3";

		public const string AppVersionName_IOS = "1.0.3";

		public static string GetPath(ushort sendcode, string ip)
		{
			string text = "https://api-archero.habby.mobi:12020";
			if (sendcode == 15)
			{
				text = "https://api-archero.habby.mobi:12020/IAP_Verification";
			}
			if (!string.IsNullOrEmpty(ip))
			{
				return text.Replace("api-archero.habby.mobi", ip);
			}
			return "https://api-archero.habby.mobi:12020";
		}

		public static string GetIP(int random)
		{
			string value = string.Empty;
			if (!mIPs.TryGetValue(random, out value))
			{
				return mIPs[0];
			}
			return value;
		}

		public static string RandomIP()
		{
			return GetIP(GameLogic.Random(0, 2));
		}
	}
}
