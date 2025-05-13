




using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillAlone1014Ctrl
{
	private EntityBase m_Entity;

	private SkillAloneBase mSkillAlone;

	private float createdis;

	private float currentdis;

	private List<SkillAloneGoodCtrlBase> mList = new List<SkillAloneGoodCtrlBase>();

	private GameObject good;

	public void Init(EntityBase entity, SkillAloneBase alone)
	{
		m_Entity = entity;
		mSkillAlone = alone;
		createdis = float.Parse(mSkillAlone.m_SkillData.Args[0]);
		m_Entity.Event_PositionBy += OnPositionBy;
	}

	public void DeInit()
	{
		m_Entity.Event_PositionBy -= OnPositionBy;
	}

	private void OnPositionBy(Vector3 p)
	{
		currentdis += p.magnitude;
		if (currentdis >= createdis)
		{
			currentdis -= createdis;
			CreateOne();
		}
	}

	private void CreateOne()
	{
		GameObject gameObject = GameLogic.EffectGet(Utils.GetString("Game/SkillPrefab/", mSkillAlone.ClassName));
		gameObject.transform.SetParent(GameNode.m_PoolParent);
		SkillAlone1014GoodCtrl component = gameObject.GetComponent<SkillAlone1014GoodCtrl>();
		component.Init(m_Entity, mSkillAlone);
		gameObject.transform.position = m_Entity.transform.position;
		mList.Add(component);
		SkillAlone1014GoodCtrl skillAlone1014GoodCtrl = component;
		skillAlone1014GoodCtrl.OnGoodDeInit = (Action<SkillAloneGoodCtrlBase>)Delegate.Combine(skillAlone1014GoodCtrl.OnGoodDeInit, new Action<SkillAloneGoodCtrlBase>(OnGoodDeInit));
		good = gameObject;
	}

	private void OnGoodDeInit(SkillAloneGoodCtrlBase ctrl)
	{
		mList.Remove(ctrl);
	}

	public void RemoveGoods()
	{
		for (int num = mList.Count - 1; num >= 0; num--)
		{
			mList[num].DeInit();
		}
		mList.Clear();
	}
}
