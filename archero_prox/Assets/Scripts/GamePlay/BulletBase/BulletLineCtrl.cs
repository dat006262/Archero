




using System;
using UnityEngine;

public class BulletLineCtrl : MonoBehaviour
{
	private GameObject mBulletLineStart;

	private LineRenderer mBulletLining;

	private GameObject mBulletLineEnd;

	private ParticleSystem[] mStartP;

	private ParticleSystem[] mEndP;

	private BulletBase mBullet;

	private BulletBase mLastBullet;

	private bool bStart;

	private bool bOverDistance;

	public Action mOverDistanceEvent;

	private const float textureLengthScale = 3f;

	private const float textureScrollSpeed = 8f;

	private void Awake()
	{
		mBulletLineStart = base.transform.Find("BulletLineStart").gameObject;
		mBulletLining = base.transform.Find("BulletLining").GetComponent<LineRenderer>();
		mBulletLineEnd = base.transform.Find("BulletLineEnd").gameObject;
		mStartP = mBulletLineStart.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		mEndP = mBulletLineEnd.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
	}

	public void Init(BulletBase bullet, BulletBase lastbullet)
	{
		mBullet = bullet;
		mLastBullet = lastbullet;
		bOverDistance = false;
		bStart = true;
		base.transform.position = Vector3.zero;
	}

	private void Update()
	{
		if (bStart)
		{
			if ((bool)mBullet && mBullet.GetInit() && (bool)mLastBullet && mLastBullet.GetInit() && !CheckOverDistance())
			{
				UpdateEffect();
			}
			else
			{
				Cache();
			}
		}
	}

	private void UpdateEffect()
	{
		mBulletLineStart.transform.position = mBullet.transform.position;
		mBulletLining.positionCount = 2;
		mBulletLining.SetPosition(0, mBulletLineStart.transform.position);
		mBulletLining.SetPosition(1, mLastBullet.transform.position);
		mBulletLineEnd.transform.position = mLastBullet.transform.position;
		mBulletLineStart.transform.LookAt(mBulletLineEnd.transform.position);
		mBulletLineEnd.transform.LookAt(mBulletLineStart.transform.position);
		float num = Vector3.Distance(mBulletLineStart.transform.position, mLastBullet.transform.position);
		mBulletLining.material.mainTextureScale = new Vector2(num / 3f, 1f);
		mBulletLining.material.mainTextureOffset -= new Vector2(Time.deltaTime * 8f, 0f);
	}

	private bool CheckOverDistance()
	{
		Debugger.Log("CheckOverDistance " + mBullet.transform.position + " last " + mLastBullet.transform.position);
		bOverDistance = ((mBullet.transform.position - mLastBullet.transform.position).magnitude > 1000f);
		return bOverDistance;
	}

	public bool IsOverDistance()
	{
		return bOverDistance;
	}

	private void ParticleClear()
	{
		int i = 0;
		for (int num = mStartP.Length; i < num; i++)
		{
			mStartP[i].Clear();
		}
		int j = 0;
		for (int num2 = mEndP.Length; j < num2; j++)
		{
			mEndP[j].Clear();
		}
	}

	public void Cache()
	{
		if (bStart)
		{
			bStart = false;
			bOverDistance = false;
			if (mOverDistanceEvent != null)
			{
				mOverDistanceEvent();
			}
			mBulletLining.positionCount = 0;
			GameLogic.EffectCache(base.gameObject);
		}
	}
}
