




using Dxx.Util;
using UnityEngine;

public class BuffAlone1016 : BuffAloneBase
{
	private float range;

	private float hprecoverratio;

	private float attackratio;

	private int hit;

	private int hprecover;

	private GameObject line;

	private LifeLineCtrl mLineCtrl;

	private GameObject effect;

	private EntityBabyBase m_EntityBaby;

	protected override void OnStart()
	{
		range = buff_data.Args[0];
		hprecoverratio = buff_data.Args[1] / 100f;
		attackratio = buff_data.Args[2] / 100f;
		m_EntityBaby = (m_Entity as EntityBabyBase);
	}

	protected override void OnRemove()
	{
	}

	protected override void ExcuteBuff(BuffData data)
	{
		EntityBase nearEntity = GameLogic.Release.Entity.GetNearEntity(m_Entity, range, sameteam: false);
		if (nearEntity != null)
		{
			CreateLine(nearEntity);
		}
		else
		{
			RemoveLine();
		}
	}

	private void CreateLine(EntityBase entity)
	{
		if (line == null)
		{
			line = GameLogic.EffectGet("Effect/Attributes/LifeLine");
			line.transform.SetParent(GameNode.m_PoolParent.transform);
			mLineCtrl = line.GetComponent<LifeLineCtrl>();
			mLineCtrl.mCacheEvent = CacheEvent;
		}
		mLineCtrl.UpdateEntity(m_Entity, entity);
		hit = MathDxx.CeilToInt(attackratio * (float)m_Entity.m_EntityData.attribute.AttackValue.ValueCount);
		entity.m_EntityData.ExcuteBuffs(m_Entity, base.BuffID, buff_data.Attribute, -hit);
		EntityBase entityBase = (!(m_EntityBaby != null)) ? m_Entity : m_EntityBaby.GetParent();
		hprecover = MathDxx.CeilToInt(hprecoverratio * (float)entityBase.m_EntityData.attribute.GetHPBase());
		entityBase.m_EntityData.ExcuteBuffs(m_Entity, base.BuffID, buff_data.Attribute, hprecover);
	}

	private void CacheEvent()
	{
		line = null;
		mLineCtrl = null;
	}

	private void RemoveLine()
	{
		if (line != null)
		{
			GameLogic.EffectCache(line);
			line = null;
			mLineCtrl.Cache();
		}
	}
}
