




using Dxx.Util;
using UnityEngine;

public class Debugger
{
	public enum Tag
	{
		eHTTP,
		ePurchase,
		eTest
	}

	public static bool enable = true;

	public static void Log(string value)
	{
		if (enable)
		{
			UnityEngine.Debug.Log("Archero:" + value);
		}
	}

	public static void Log(string tag, string value)
	{
		UnityEngine.Debug.Log(Utils.FormatString("Archero {0} : {1}", tag, value));
	}

	public static void LogFormat(string value, params object[] args)
	{
		if (enable)
		{
			UnityEngine.Debug.LogFormat("Archero:" + value, args);
		}
	}

	public static void Log(Tag tag, string value)
	{
		UnityEngine.Debug.Log(Utils.FormatString("Archero:{0} : {1}", tag.ToString(), value));
	}

	public static void Log(EntityBase entity, string value)
	{
	}

	public static void LogEquipGet(string value)
	{
	}
}
