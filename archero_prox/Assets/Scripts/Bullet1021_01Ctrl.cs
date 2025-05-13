




using Dxx.Util;
using UnityEngine;

public class Bullet1021_01Ctrl : MonoBehaviour
{
	public GameObject child;

	private float alltime = 1.4f;

	private float downtime = 1.2f;

	private float time;

	private int state;

	private void Start()
	{
	}

	private void OnEnable()
	{
		state = 0;
		time = 0f;
		child.SetActive(value: true);
		child.transform.localPosition = new Vector3(0f, 0f, 15f);
		child.transform.localScale = Vector3.zero;
	}

	private void Update()
	{
		time += Time.deltaTime;
		if (state == 0)
		{
			float d = MathDxx.Clamp(time, 0f, 0.3f) / 0.3f;
			child.transform.localScale = Vector3.one * d;
			if (time > downtime)
			{
				state = 1;
			}
		}
		else if (state == 1)
		{
			if (time > alltime)
			{
				time = alltime;
			}
			float num = (time - downtime) / (alltime - downtime);
			child.transform.localPosition = new Vector3(0f, 0f, 15f * (1f - num));
			if (time >= alltime)
			{
				child.SetActive(value: false);
			}
		}
	}
}
