




using System;
using UnityEngine;

public class AutoDespawnSound : MonoBehaviour
{
	public float DespawnTime = 1f;

	public SoundManager.SoundData sounddata;

	public Action<string, SoundManager.SoundData> callback;

	private float pDespawnTime;

	private bool bStart;

	private void OnEnable()
	{
		pDespawnTime = DespawnTime;
		bStart = true;
	}

	public void SetDespawnTime(float time)
	{
		DespawnTime = time;
		OnEnable();
	}

	private void Update()
	{
		if (!bStart)
		{
			return;
		}
		pDespawnTime -= Time.unscaledDeltaTime;
		if (pDespawnTime <= 0f)
		{
			bStart = false;
			if (callback != null)
			{
				callback(base.name, sounddata);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
