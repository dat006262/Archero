




using System.Collections;
using UnityEngine;

public class PingTest : MonoBehaviour
{
	public string IP = "123.206.4.94";

	private Ping ping;

	private float delayTime;

	private void Start()
	{
		StartCoroutine("Pings");
	}

	private IEnumerator Pings()
	{
		while (true)
		{
			ping = new Ping(IP);
			for (int i = 0; i < 10; i++)
			{
				if (ping.isDone)
				{
					delayTime = ping.time;
					ping.DestroyPing();
					ping = null;
					break;
				}
				yield return new WaitForSeconds(0.3f);
			}
			if (ping != null)
			{
				delayTime = 999f;
				Debugger.Log("No network!");
				ping.DestroyPing();
				ping = null;
			}
			else
			{
				Debugger.Log("Network delay : " + delayTime + "ms");
			}
			yield return new WaitForSeconds(1f);
		}
	}
}
