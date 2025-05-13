




using Dxx.Util;
using UnityEngine;

public class ColliderEnable : MonoBehaviour
{
	private float mTime;

	private float starttime;

	private bool bExcute;

	private BoxCollider mBox;

	private SphereCollider mSphere;

	private CapsuleCollider mCapsule;

	public float DelayTime;

	[Tooltip("延迟后可否碰撞")]
	public bool bDelayEnable;

	private void Awake()
	{
		mBox = GetComponent<BoxCollider>();
		mSphere = GetComponent<SphereCollider>();
		mCapsule = GetComponent<CapsuleCollider>();
	}

	private void OnEnable()
	{
		mTime = DelayTime;
		starttime = Updater.AliveTime;
		bExcute = false;
		SetColliderEnable(!bDelayEnable);
	}

	private void Update()
	{
		if (!bExcute && Updater.AliveTime - starttime >= mTime)
		{
			bExcute = true;
			SetColliderEnable(bDelayEnable);
		}
	}

	private void SetColliderEnable(bool enable)
	{
		if ((bool)mBox)
		{
			mBox.enabled = enable;
		}
		if ((bool)mSphere)
		{
			mSphere.enabled = enable;
		}
		if ((bool)mCapsule)
		{
			mCapsule.enabled = enable;
		}
	}
}
