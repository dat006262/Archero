




using Dxx.Util;
using System.Collections.Generic;
using TableTool;

public class BulletTransmit
{
	public EntityAttributeBase attribute;

	private EntityBase m_Entity;

	private Weapon_weapon weapondata;

	public EElementType trailType;

	public EElementType headType;

	private long attack;

	private float attackratio;

	public float CritRate;

	public float CritSuperRate;

	private HitStruct m_AttackStruct = default(HitStruct);

	public int mThroughEnemy;

	public float mThroughRatio;

	public int mHitCreate2;

	public float mHitCreate2Percent;

	public int mHitSputter;

	private float mThunderRatio;

	private List<int> mDebuffList = new List<int>();

	public BulletTransmit(EntityBase entity, int bulletid, bool clear = false)
	{
		m_Entity = entity;
		weapondata = LocalModelManager.Instance.Weapon_weapon.GetBeanById(bulletid);
		mThunderRatio = 1f;
		attackratio = 1f;
		if (m_Entity == null)
		{
			attribute = new EntityAttributeBase();
			return;
		}
		if (!clear)
		{
			attribute = new EntityAttributeBase(m_Entity.m_Data.CharID);
			m_Entity.m_EntityData.attribute.AttributeTo(attribute);
			CritRate = attribute.CritRate.Value;
			CritSuperRate = attribute.CritSuperRate.Value;
			mThroughEnemy = m_Entity.m_EntityData.ThroughEnemy;
			mThroughRatio = m_Entity.m_EntityData.ThroughRatio;
			mDebuffList = m_Entity.GetDebuffList();
			mHitCreate2 = m_Entity.m_EntityData.HitCreate2;
			mHitCreate2Percent = m_Entity.m_EntityData.mHitCreate2Percent;
			mHitSputter = m_Entity.m_EntityData.BulletSputter;
			trailType = m_Entity.m_EntityData.ArrowTrailType;
			headType = m_Entity.m_EntityData.ArrowHeadType;
		}
		else
		{
			Clear();
		}
		attack = MathDxx.CeilToInt(m_Entity.m_EntityData.GetAttack(weapondata.Attack));
	}

	public void SetAttack(long attack)
	{
		this.attack = attack;
	}

	public long GetAttack()
	{
		return attack;
	}

	public HitStruct GetAttackStruct()
	{
		long num = attack;
		num = MathDxx.CeilToInt((float)num * attackratio);
		m_AttackStruct.before_hit = -num;
		m_AttackStruct.type = HitType.Normal;
		return m_AttackStruct;
	}

	public void ArrowEjectAction(float value)
	{
		mThunderRatio *= value;
	}

	public bool GetThroughEnemy()
	{
		return mThroughEnemy > 0;
	}

	public bool GetHitCreate2()
	{
		return mHitCreate2 > 0;
	}

	public bool GetHitSputter()
	{
		return mHitSputter > 0;
	}

	public void AddDebuffs(params int[] buffs)
	{
		int i = 0;
		for (int num = buffs.Length; i < num; i++)
		{
			mDebuffList.Add(buffs[i]);
		}
	}

	public void AddDebuffsToTarget(EntityBase target)
	{
		int i = 0;
		for (int count = mDebuffList.Count; i < count; i++)
		{
			int buffid = mDebuffList[i];
			GameLogic.SendBuff(target, m_Entity, buffid, mThunderRatio);
		}
	}

	public void AddAttackRatio(float value)
	{
		attackratio *= value;
	}

	private void Clear()
	{
		attribute = new EntityAttributeBase(m_Entity.m_Data.CharID);
		mThroughEnemy = 0;
		mHitCreate2 = 0;
		mDebuffList.Clear();
		trailType = EElementType.eNone;
		headType = EElementType.eNone;
	}
}
