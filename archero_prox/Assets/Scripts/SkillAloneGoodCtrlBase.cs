




using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillAloneGoodCtrlBase : PauseObject
{
	protected SkillAloneBase mSkillAlone;

	protected EntityBase m_Entity;

	private float time;

	private float starttime;

	public Action<SkillAloneGoodCtrlBase> OnGoodDeInit;

	private List<EntityBase> mList = new List<EntityBase>();

	private int[] debuffs;

	public void Init(EntityBase entity, SkillAloneBase alone)
	{
		m_Entity = entity;
		mSkillAlone = alone;
		debuffs = mSkillAlone.m_Data.DeBuffs;
		time = float.Parse(mSkillAlone.m_SkillData.Args[1]);
		starttime = Updater.AliveTime;
		mList.Clear();
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	public void DeInit()
	{
		GameLogic.EffectCache(base.gameObject);
	}

	protected override void UpdateProcess()
	{
		if (Updater.AliveTime - starttime > time)
		{
			DeInit();
			if (OnGoodDeInit != null)
			{
				OnGoodDeInit(this);
			}
		}
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			EntityBase target = mList[i];
			int j = 0;
			for (int num = debuffs.Length; j < num; j++)
			{
				GameLogic.SendBuff(target, m_Entity, debuffs[j]);
			}
		}
	}

	protected virtual bool CanHitEntity(EntityBase target)
	{
		return true;
	}

	private void OnTriggerEnter(Collider o)
	{
		if (o.gameObject.layer == LayerManager.Player)
		{
			EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(o.gameObject);
			if (!GameLogic.IsSameTeam(entityByChild, m_Entity) && CanHitEntity(entityByChild))
			{
				mList.Add(entityByChild);
			}
		}
	}

	private void OnTriggerExit(Collider o)
	{
		if (o.gameObject.layer == LayerManager.Player)
		{
			EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(o.gameObject);
			if (mList.Contains(entityByChild))
			{
				mList.Remove(entityByChild);
			}
		}
	}
}
