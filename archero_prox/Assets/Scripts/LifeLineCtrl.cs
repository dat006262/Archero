




using System;
using UnityEngine;

public class LifeLineCtrl : MonoBehaviour
{
	private GameObject mStartObj;

	private GameObject mEndObj;

	private LineRenderer line;

	private const float textureLengthScale = 3f;

	private const float textureScrollSpeed = 8f;

	private EntityBase m_Entity;

	private EntityBase entity;

	public Action mCacheEvent;

	private bool bStart;

	private void Awake()
	{
		mStartObj = base.transform.Find("LifeBeamStart").gameObject;
		mEndObj = base.transform.Find("LifeBeamEnd").gameObject;
		line = base.transform.Find("Life Beam").GetComponent<LineRenderer>();
		line.sortingLayerName = "Hit";
	}

	public void UpdateEntity(EntityBase m_Entity, EntityBase entity)
	{
		this.m_Entity = m_Entity;
		this.entity = entity;
		bStart = true;
	}

	private void Update()
	{
		if (!bStart)
		{
			return;
		}
		if ((bool)m_Entity && !m_Entity.GetIsDead() && (bool)entity && !entity.GetIsDead())
		{
			Vector3 position = entity.m_Body.EffectMask.transform.position;
			base.transform.LookAt(position);
			mStartObj.transform.position = m_Entity.m_Body.EffectMask.transform.position;
			mEndObj.transform.position = position;
			line.positionCount = 2;
			line.SetPosition(0, mStartObj.transform.position);
			line.SetPosition(1, position);
			float num = Vector3.Distance(mStartObj.transform.position, position);
			line.material.mainTextureScale = new Vector2(num / 3f, 1f);
			line.material.mainTextureOffset -= new Vector2(Time.deltaTime * 8f, 0f);
		}
		else
		{
			if (mCacheEvent != null)
			{
				mCacheEvent();
			}
			Cache();
		}
	}

	public void Cache()
	{
		bStart = false;
		GameLogic.EffectCache(base.gameObject);
	}
}
