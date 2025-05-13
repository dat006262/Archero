




using UnityEngine;

public class PlatformHelper
{
	public const string IOS_APPID = "1453651052";

	private const float FringeHeight = -55f;

	private const float BottomHeight = 30f;

	public static ushort GetAppVersionCode()
	{
		return 2;
	}

	public static string GetAppVersionName()
	{
		return "1.0.3";
	}

	public static int GetPlatformID()
	{
		return 1;
	}

	private static bool IsFringe()
	{
		return false;
	}

	public static float GetFringeHeight()
	{
		if (IsFringe())
		{
			return -55f;
		}
		return 0f;
	}

	public static float GetBottomHeight()
	{
		if (IsFringe())
		{
			return 30f;
		}
		return 0f;
	}

	public static bool GetFlagShip()
	{
		return false;
	}

	public static string GetUUID()
	{
		return SystemInfo.deviceUniqueIdentifier;
	}

	public static bool IsEditor()
	{
		return false;
	}

	public static bool IsAndroid()
	{
		return true;
	}

	public static bool IsIOS()
	{
		return false;
	}
}
