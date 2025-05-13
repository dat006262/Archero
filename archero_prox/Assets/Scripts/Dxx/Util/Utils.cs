




using Dxx.Net;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Dxx.Util
{
	public static class Utils
	{
		private static Vector3 GetDirection_dir = default(Vector3);

		private static float getAngle_angle;

		private static StringBuilder mStringBudier = new StringBuilder();

		private static StringBuilder mFormatStringBudier = new StringBuilder();

		private static object mFormatLock = new object();

		private static StringBuilder mFormatStringBudierThread = new StringBuilder();

		private static object mFormatThreadLock = new object();

		private static DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local));

		private static StringBuilder stringGetSecond3String = new StringBuilder();

		private static StringBuilder stringGetSecond2String = new StringBuilder();

		private static long _startUpTime = 0L;

		public static long StartUpTime
		{
			get
			{
				if (NetManager.NetTime > 0)
				{
					return NetManager.NetTime;
				}
				if (_startUpTime == 0)
				{
					_startUpTime = GetTimeStamp();
				}
				return _startUpTime;
			}
		}

		public static long CurrentTime => StartUpTime + (int)Time.realtimeSinceStartup;

		public static float getAngle(Vector2 dir)
		{
			return getAngle(dir.x, dir.y);
		}

		public static float getAngle(Vector3 dir)
		{
			return getAngle(dir.x, dir.z);
		}

		public static Vector3 World2Screen(Vector3 worldpos)
		{
			Vector3 vector = GameNode.m_Camera.WorldToViewportPoint(worldpos);
			return new Vector3((float)GameLogic.Width * vector.x, (float)GameLogic.Height * vector.y, 0f);
		}

		public static Vector3 GetDirection(float angle)
		{
			GetDirection_dir.x = MathDxx.Sin(angle);
			GetDirection_dir.z = MathDxx.Cos(angle);
			return GetDirection_dir;
		}

		public static float getAngle(float x, float y)
		{
			getAngle_angle = 90f - Mathf.Atan2(y, x) * 57.29578f;
			getAngle_angle = (getAngle_angle + 360f) % 360f;
			return GetFloat2(getAngle_angle);
		}

		public static float GetFloat1(float f)
		{
			return (float)(int)(f * 10f) / 10f;
		}

		public static float GetFloat2(float f)
		{
			return (float)(int)(f * 100f) / 100f;
		}

		public static float GetFloat3(float f)
		{
			return (float)(int)(f * 1000f) / 1000f;
		}

		public static int Ceil(float value)
		{
			return (int)Math.Ceiling(value);
		}

		public static int Floor(float value)
		{
			return (int)Math.Floor(value);
		}

		public static string GetString(params object[] args)
		{
			mStringBudier.Clear();
			int i = 0;
			for (int num = args.Length; i < num; i++)
			{
				mStringBudier.Append(args[i]);
			}
			return mStringBudier.ToString();
		}

		public static string FormatString(string format, params object[] args)
		{
			lock (mFormatLock)
			{
				try
				{
					mFormatStringBudier.Clear();
					mFormatStringBudier.AppendFormat(format, args);
					return mFormatStringBudier.ToString();
				}
				catch (Exception ex)
				{
					SdkManager.Bugly_Report("Utils.FormatString", "mFormatStringBudier try failure!!! string :" + format, ex.StackTrace);
					return format;
				}
			}
		}

		public static string FormatStringThread(string format, params object[] args)
		{
			lock (mFormatThreadLock)
			{
				try
				{
					mFormatStringBudierThread.Clear();
					mFormatStringBudierThread.AppendFormat(format, args);
					return mFormatStringBudierThread.ToString();
				}
				catch (Exception ex)
				{
					SdkManager.Bugly_Report("Utils.FormatStringThread", "mFormatStringBudierThread try failure!!! string :" + format, ex.StackTrace);
					return format;
				}
			}
		}

		public static float ExcuteReboundWall(float angle, Vector3 pos, GameObject o)
		{
			float x = MathDxx.Sin(angle);
			float z = MathDxx.Cos(angle);
			Vector3 vector = new Vector3(x, 0f, z);
			Vector3 origin = pos - vector;
			RaycastHit[] array = Physics.RaycastAll(origin, vector, 2f, 1 << o.gameObject.layer);
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit raycastHit = array[i];
				if (raycastHit.collider.gameObject == o)
				{
					Vector3 vector2 = raycastHit.point - raycastHit.collider.transform.position;
					Vector3 size = raycastHit.collider.GetComponent<BoxCollider>().size;
					float x2 = size.x;
					Vector3 localScale = raycastHit.transform.localScale;
					float num = x2 * localScale.x;
					float num2 = size.z * 1.23f;
					Vector3 localScale2 = raycastHit.transform.localScale;
					float num3 = num / (num2 * localScale2.z);
					if (vector2.z > 0f && Mathf.Abs(vector2.x) <= vector2.z * num3)
					{
						return 180f - angle;
					}
					if (vector2.z < 0f && Mathf.Abs(vector2.x) <= (0f - vector2.z) * num3)
					{
						return 180f - angle;
					}
					if (vector2.x > 0f && Mathf.Abs(vector2.z) <= vector2.x / num3)
					{
						return 0f - angle;
					}
					return 0f - angle;
				}
			}
			return 0f;
		}

		public static float ExcuteReboundWallRedLine(Transform transform, Collider o)
		{
			Vector3 position = transform.position;
			Vector3 eulerAngles = transform.eulerAngles;
			return ExcuteReboundWall(position, eulerAngles.y, o, 0f);
		}

		public static float ExcuteReboundWallSkill(float angle, Vector3 position, SphereCollider s, Collider o)
		{
			return ExcuteReboundWall(position, angle, o, -3f);
		}

		private static float ExcuteReboundWall(Vector3 position, float angle, Collider o, float offsetdir)
		{
			float x = MathDxx.Sin(angle);
			float z = MathDxx.Cos(angle);
			Vector3 vector = new Vector3(x, 0f, z);
			RaycastHit[] array = Physics.RaycastAll(position + offsetdir * vector, vector, 100f, 1 << o.gameObject.layer);
			float angle2 = getAngle(vector);
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit raycastHit = array[i];
				if (raycastHit.collider.gameObject == o.gameObject)
				{
					Vector3 vector2 = raycastHit.point - raycastHit.collider.transform.position;
					Vector3 size = raycastHit.collider.GetComponent<BoxCollider>().size;
					float x2 = size.x;
					Vector3 localScale = raycastHit.transform.localScale;
					float num = x2 * localScale.x;
					float num2 = size.z * 1.23f;
					Vector3 localScale2 = raycastHit.transform.localScale;
					float num3 = num / (num2 * localScale2.z);
					if (vector2.z > 0f && Mathf.Abs(vector2.x) < vector2.z * num3)
					{
						return 180f - angle;
					}
					if (vector2.z < 0f && Mathf.Abs(vector2.x) < (0f - vector2.z) * num3)
					{
						return 180f - angle;
					}
					if (vector2.x > 0f && Mathf.Abs(vector2.z) < vector2.x / num3)
					{
						return 0f - angle;
					}
					return 0f - angle;
				}
			}
			return 0f - angle;
		}

		public static long GetTimeStamp()
		{
			if (NetManager.NetTime == 0)
			{
				return NetManager.LocalTime;
			}
			return Convert.ToInt64((float)NetManager.NetTime + Time.realtimeSinceStartup - NetManager.unitytime);
		}

		public static long GetLocalTime()
		{
			return Convert.ToInt64((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);
		}

		public static DateTime GetCurrentDataTime()
		{
			return ConvertIntDateTime(GetTimeStamp());
		}

		public static string GetTimeGo(double d)
		{
			DateTime d2 = ConvertIntDateTime(d);
			DateTime currentDataTime = GetCurrentDataTime();
			TimeSpan timeSpan = currentDataTime - d2;
			if (timeSpan.Days >= 365)
			{
				return GameLogic.Hold.Language.GetLanguageByTID("几年前", timeSpan.Days / 365);
			}
			if (timeSpan.Days >= 30)
			{
				return GameLogic.Hold.Language.GetLanguageByTID("几月前", timeSpan.Days / 30);
			}
			if (timeSpan.Days > 0)
			{
				return GameLogic.Hold.Language.GetLanguageByTID("几天前", timeSpan.Days);
			}
			if (timeSpan.Hours > 0)
			{
				return GameLogic.Hold.Language.GetLanguageByTID("几小时前", timeSpan.Hours);
			}
			if (timeSpan.Minutes > 0)
			{
				return GameLogic.Hold.Language.GetLanguageByTID("几分钟前", timeSpan.Minutes);
			}
			return GameLogic.Hold.Language.GetLanguageByTID("几秒前", timeSpan.Seconds);
		}

		public static string NormalizeTimpstamp0(long timpStamp)
		{
			long ticks = timpStamp * 10000000;
			TimeSpan value = new TimeSpan(ticks);
			return dtStart.Add(value).ToString("yyyy-MM-dd");
		}

		public static string GetSecond3String(long second)
		{
			stringGetSecond3String.Remove(0, stringGetSecond3String.Length);
			stringGetSecond3String.AppendFormat("{0:D2}:{1:D2}:{2:D2}", second / 3600, second % 3600 / 60, second % 60);
			return stringGetSecond3String.ToString();
		}

		public static string GetSecond2String(int second)
		{
			stringGetSecond2String.Remove(0, stringGetSecond2String.Length);
			stringGetSecond2String.AppendFormat("{0:D2}:{1:D2}", second / 60, second % 60);
			return stringGetSecond2String.ToString();
		}

		public static TimeSpan GetTime(long second)
		{
			return new TimeSpan(second * 1000 * 10000);
		}

		public static DateTime ConvertIntDateTime(double d)
		{
			DateTime minValue = DateTime.MinValue;
			return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)).AddSeconds(d);
		}

		public static double ConvertDateTimeInt(DateTime time)
		{
			double num = 0.0;
			DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local));
			return (time - d).TotalSeconds;
		}

		public static long DateTimeToUnixTimestamp(DateTime dateTime, bool milliseconds = true)
		{
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
			if (milliseconds)
			{
				return Convert.ToInt64((dateTime - d).TotalMilliseconds);
			}
			return Convert.ToInt64((dateTime - d).TotalSeconds);
		}

		public static DateTime UnixTimestampToDateTime(DateTime target, long timestamp)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, target.Kind).AddSeconds(timestamp);
		}

		public static string CutString(string str, int maxlength)
		{
			if (str.Length > maxlength)
			{
				return FormatString("{0}...", str.Substring(0, maxlength - 3));
			}
			return str;
		}

		public static float GetBulletAngle(int current, int count, float allangle)
		{
			if (allangle >= 360f)
			{
				SdkManager.Bugly_Report("Utils.GetBulletAngle", FormatString("allangle：{0} >= 360f!!!", allangle));
			}
			float num = allangle / (float)(count - 1);
			return (float)current * num - allangle / 2f;
		}

		public static void ClearEvents(this object ctrl)
		{
			if (ctrl == null)
			{
				return;
			}
			BindingFlags bindingAttr = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			EventInfo[] events = ctrl.GetType().GetEvents(bindingAttr);
			if (events != null && events.Length >= 1)
			{
				for (int i = 0; i < events.Length; i++)
				{
					try
					{
						EventInfo eventInfo = events[i];
						FieldInfo field = eventInfo.DeclaringType.GetField("Event" + eventInfo.Name, bindingAttr);
						if (field != null)
						{
							field.SetValue(ctrl, null);
						}
					}
					catch
					{
					}
				}
			}
		}

		public static string GenerateUUID()
		{
			return FormatString("{0}{1:D5}", Guid.NewGuid().ToString("N"), GameLogic.Random(0, 100000));
		}

		public static string ToHexString(byte[] bytes)
		{
			string result = string.Empty;
			if (bytes != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					stringBuilder.Append(bytes[i].ToString("X2"));
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		public static byte[] strToHexByte(string hexString)
		{
			UnityEngine.Debug.Log("strToHexByte start " + hexString);
			hexString = hexString.Replace(" ", string.Empty);
			if (hexString.Length % 2 != 0)
			{
				hexString = hexString.Insert(hexString.Length - 1, 0.ToString());
			}
			byte[] array = new byte[hexString.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
			}
			UnityEngine.Debug.Log("strToHexByte " + hexString + " -> " + ToHexString(array));
			return array;
		}

		public static byte[] UlongToByte(ulong ul)
		{
			return new byte[8]
			{
				(byte)((ul >> 56) & 0xFF),
				(byte)((ul >> 48) & 0xFF),
				(byte)((ul >> 40) & 0xFF),
				(byte)((ul >> 32) & 0xFF),
				(byte)((ul >> 24) & 0xFF),
				(byte)((ul >> 16) & 0xFF),
				(byte)((ul >> 8) & 0xFF),
				(byte)(ul & 0xFF)
			};
		}
	}
}
