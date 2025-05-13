




using System;
using System.Collections;
using UnityEngine;

public class UnityPing : MonoBehaviour
{
	private static string s_ip = string.Empty;

	private static Action<int> s_callback;

	private static UnityPing s_unityPing;

	private static int s_timeout = 2;

	public static int Timeout
	{
		get
		{
			return s_timeout;
		}
		set
		{
			if (value > 0)
			{
				s_timeout = value;
			}
		}
	}

	public static void CreatePing(string ip, Action<int> callback)
	{
		if (!string.IsNullOrEmpty(ip) && callback != null && !(s_unityPing != null))
		{
			ip = "14.215.177.39";
			s_ip = ip;
			s_callback = callback;
			GameObject gameObject = new GameObject("UnityPing");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			s_unityPing = gameObject.AddComponent<UnityPing>();
		}
	}

	private void Start()
	{
		switch (Application.internetReachability)
		{
		case NetworkReachability.ReachableViaCarrierDataNetwork:
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			StopCoroutine(PingConnect());
			StartCoroutine(PingConnect());
			return;
		}
		if (s_callback != null)
		{
			s_callback(-1);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		s_ip = string.Empty;
		s_timeout = 20;
		s_callback = null;
		if (s_unityPing != null)
		{
			s_unityPing = null;
		}
	}

	private IEnumerator PingConnect()
	{
		Ping ping = new Ping(s_ip);
		int addTime = 0;
		int requestCount = s_timeout * 10;
		while (!ping.isDone)
		{
			yield return new WaitForSeconds(0.1f);
			if (addTime > requestCount)
			{
				if (s_callback != null)
				{
					s_callback(ping.time);
					UnityEngine.Object.Destroy(base.gameObject);
				}
				yield break;
			}
			addTime++;
		}
		if (ping.isDone)
		{
			if (s_callback != null)
			{
				s_callback(ping.time);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return null;
		}
	}
}
