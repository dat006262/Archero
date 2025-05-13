




using Dxx.Util;
using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
	public float DespawnTime = 2f;

	private float pDespawnTime;

	private bool bStart;

	private void OnEnable()
	{
		pDespawnTime = DespawnTime;
		bStart = true;
	}

	public void SetDespawnTime(float value)
	{
		DespawnTime = value;
		pDespawnTime = DespawnTime;
	}

	private void Update()
	{
		if (bStart)
		{
			pDespawnTime -= Updater.delta;
			if (pDespawnTime <= 0f)
			{
				bStart = false;
				GameLogic.EffectCache(base.gameObject);
			}
		}
	}
}
