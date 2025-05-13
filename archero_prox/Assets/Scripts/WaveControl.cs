




using Dxx.Util;
using UnityEngine;

public class WaveControl : MonoBehaviour
{
	private Transform child;

	private float speed = 5f;

	private float wavetime = 2f;

	private float currenttime;

	private void Start()
	{
		child = base.transform.Find("Plane");
	}

	private void Update()
	{
		currenttime += Updater.delta;
		child.localScale = new Vector3(currenttime * speed, 1f, currenttime * speed);
		if (currenttime >= wavetime)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
