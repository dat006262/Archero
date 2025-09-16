




using Dxx.Util;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
	public float DisableTime = 1f;

	private float pDisableTime;

	private bool bStart;

	private SphereCollider mSphere;

	private BoxCollider mBox;

	private CapsuleCollider mCapsule;

	private void OnEnable()
	{
		pDisableTime = DisableTime;
		bStart = true;
		ColliderEnable(enable: true);
	}

	private void ColliderEnable(bool enable)
	{
		if (mSphere == null)
		{
			mSphere = GetComponent<SphereCollider>();
		}
		if (mSphere != null)
		{
			mSphere.enabled = enable;
		}
	}

	private void Update()
	{
		if (bStart)
		{
			pDisableTime -= Updater.delta;
			if (pDisableTime <= 0f)
			{
				bStart = false;
				ColliderEnable(enable: false);
			}
		}
	}
}
