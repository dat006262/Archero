




using Dxx.Util;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	public float DestroyTime = 1f;

	private float pDestroyTime;

	private bool bStart;

	private void OnEnable()
	{
		pDestroyTime = DestroyTime;
		bStart = true;
	}

	public void SetDestroyTime(float time)
	{
		DestroyTime = time;
		OnEnable();
	}

	private void Update()
	{
		if (bStart)
		{
			pDestroyTime -= Updater.delta;
			if (pDestroyTime <= 0f)
			{
				bStart = false;
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
