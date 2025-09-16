




using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class EntityData
{
	private class StaticAddData
	{
		public string goodType;

		public long value;

		public bool bAdded;

		public void Update(EntityBase entity, bool move, long value)
		{
			if (this.value == value)
			{
				if (move && bAdded)
				{
					bAdded = false;
					entity.m_EntityData.ExcuteAttributes(goodType, -value);
				}
				else if (!move && !bAdded)
				{
					bAdded = true;
					entity.m_EntityData.ExcuteAttributes(goodType, value);
				}
			}
			else
			{
				if (bAdded)
				{
					bAdded = false;
					entity.m_EntityData.ExcuteAttributes(goodType, -this.value);
				}
				this.value = value;
				Update(entity, move, value);
			}
		}
	}

	private class MoveAddData
	{
		public string goodType;

		public long value;

		public bool bAdded;

		public void Update(EntityBase entity, bool move, long value)
		{
			if (this.value == value)
			{
				if (!move && bAdded)
				{
					bAdded = false;
					entity.m_EntityData.ExcuteAttributes(goodType, -value);
				}
				else if (move && !bAdded)
				{
					bAdded = true;
					entity.m_EntityData.ExcuteAttributes(goodType, value);
				}
			}
			else
			{
				if (bAdded)
				{
					bAdded = false;
					entity.m_EntityData.ExcuteAttributes(goodType, -this.value);
				}
				this.value = value;
				Update(entity, move, value);
			}
		}
	}

	public class BuffAttrData
	{
		public int count;

		public float attack = 1f;

		public float resistance;
	}

	public int CharID;

	private EntityBase m_Entity;

	public int mDeadRecover;

	private long mHP2AttackSpeed;

	private long mHP2Miss;

	private int mHitCreate2;

	public float mHitCreate2Percent;

	private int mFlyStoneCount;

	private int mFlyWaterCount;

	private int mBulletThroughCount;

	private int DizzyCount;

	private float mDizzyTime;

	[NonSerialized]
	public int ExtraSkillCount;

	public long CurrentHP;

	[Header("最大血量")]
	public long MaxHP;

	private HitStruct Attack = default(HitStruct);

	public int InvincibleCount;

	[NonSerialized]
	public float BulletSpeed = 1f;

	private int MissHP_Count;

	private WeightRandom<AttackCallData> mAttackMeteorite = new WeightRandom<AttackCallData>();

	public EntityAttributeBase attribute;

	private float mHP2AttackRatio;

	private int mThroughEnemy;

	private float mThroughRatio = 1f;

	private int mBulletLine;

	public BulletBase mLastBullet;

	private int mBulletSputter;

	private int mBulletSpeedHittedCount;

	private float mBulletSpeedHitted = 1f;

	private float mBulletSpeedHittedTime;

	private float mBulletSpeed1Ratio = 1f;

	private float mBulletSpeed1Range;

	private float mBulletSpeed = 1f;

	public float HittedInterval;

	public int TurnTableCount;

	private int mBulletScaleCount;

	private int mOnlyDemonCount;

	private int mBabyResistBulletCount;

	private int mFrontShieldCount;

	private int mLight45;

	private StaticAddData mStaticReduce;

	private MoveAddData mMoveAdd;

	public Dictionary<EElementType, BuffAttrData> mBuffAttrList = new Dictionary<EElementType, BuffAttrData>
	{
		{
			EElementType.eNone,
			new BuffAttrData()
		},
		{
			EElementType.eThunder,
			new BuffAttrData()
		},
		{
			EElementType.eFire,
			new BuffAttrData()
		},
		{
			EElementType.eIce,
			new BuffAttrData()
		},
		{
			EElementType.ePoison,
			new BuffAttrData()
		}
	};

	public static Dictionary<EElementType, ElementDataClass> ElementData = new Dictionary<EElementType, ElementDataClass>
	{
		{
			EElementType.eNone,
			new ElementDataClass()
		},
		{
			EElementType.eThunder,
			new ElementDataClass
			{
				TrailPriority = 0,
				TrailPath = string.Empty,
				HeadPriority = 20,
				HeadPath = "Effect/Attributes/ThunderHead",
				color = new Color(1f, 1f, 49f / 255f)
			}
		},
		{
			EElementType.eFire,
			new ElementDataClass
			{
				TrailPriority = 0,
				TrailPath = string.Empty,
				HeadPriority = 10,
				HeadPath = "Effect/Attributes/FireHead",
				color = new Color(1f, 0.3529412f, 0f)
			}
		},
		{
			EElementType.eIce,
			new ElementDataClass
			{
				TrailPriority = 10,
				TrailPath = "Effect/Attributes/IceTrail",
				HeadPriority = 0,
				HeadPath = string.Empty,
				color = new Color(0f, 244f / 255f, 1f)
			}
		},
		{
			EElementType.ePoison,
			new ElementDataClass
			{
				TrailPriority = 20,
				TrailPath = "Effect/Attributes/PoisonTrail",
				HeadPriority = 0,
				HeadPath = string.Empty,
				color = new Color(0.647058845f, 0f, 67f / 85f)
			}
		},
		{
			EElementType.eThunderFire,
			new ElementDataClass
			{
				TrailPriority = 0,
				TrailPath = string.Empty,
				HeadPriority = 20,
				HeadPath = "Effect/Attributes/ThunderFireHead",
				color = new Color(1f, 1f, 49f / 255f)
			}
		}
	};

	public EElementType ArrowTrailType;

	public EElementType ArrowHeadType;

	private List<EntityBabyBase> mBabies = new List<EntityBabyBase>();

	public List<string> mBabyAttributes = new List<string>();

	public List<string> mSelfAttributes = new List<string>();

	public List<int> mBabySkillIds = new List<int>();

	public List<int> mSelfSkillIds = new List<int>();

	private WeightRandom<DeadCallData> mCallWeight = new WeightRandom<DeadCallData>();

	public int MaxLevel;

	private int Level = 1;

	private float Exp;

	private ProgressAniManager exp_data = new ProgressAniManager();

	private Dictionary<int, float> explist = new Dictionary<int, float>();

	private bool bInitHeadShot;

	private bool bHeadShot;

	private long Shield_Count;

	private long Shield_CurrentCount;

	public long Shield_CurrentHitValue;

	private GameObject mShieldObj;

	private float hittedSoundTime;

	private float mRebornStartTime;

	private AnimationCurve mRebornCurve;

	private const float mRebornAllTime = 1.5f;

	private float mTrapHitTime;

	public int HitCreate2 => mHitCreate2;

	public int BulletThroughCount => mBulletThroughCount;

	public float HP2AttackRatio => mHP2AttackRatio;

	public int ThroughEnemy => mThroughEnemy;

	public float ThroughRatio => mThroughRatio;

	public int BulletLineCount => mBulletLine;

	public int BulletSputter => mBulletSputter;

	public void Init(EntityBase entity, int CharID)
	{
		m_Entity = entity;
		this.CharID = CharID;
		mDeadRecover = 1;
		InitAttribute();
		MaxHP = attribute.HPValue.Value;
		CurrentHP = MaxHP;
		MaxLevel = 11;
		if (GameLogic.Hold.BattleData.GetMode() == GameMode.eMatchDefenceTime)
		{
			MaxLevel = 20;
		}
		attribute.Reborn_Refresh_Count(LocalSave.Instance.BattleIn_GetRebornSkill());
		EntityBase entity2 = m_Entity;
		entity2.OnCrit = (Action<long>)Delegate.Combine(entity2.OnCrit, new Action<long>(OnCritEvent));
		EntityBase entity3 = m_Entity;
		entity3.OnMonsterDeadAction = (Action<EntityBase>)Delegate.Combine(entity3.OnMonsterDeadAction, new Action<EntityBase>(OnMonsterDead));
		EntityBase entity4 = m_Entity;
		entity4.Event_OnAttack = (Action)Delegate.Combine(entity4.Event_OnAttack, new Action(OnAttackCreate));
		EntityBase entity5 = m_Entity;
		entity5.OnMoveEvent = (Action<bool>)Delegate.Combine(entity5.OnMoveEvent, new Action<bool>(OnMoveEvent));
		OnMoveEvent(value: false);
	}

	public void InitAfter()
	{
		EntityAttributeBase entityAttributeBase = attribute;
		entityAttributeBase.Shield_ValueAction = (Action<long>)Delegate.Combine(entityAttributeBase.Shield_ValueAction, new Action<long>(UpdateShieldValueChange));
		Shield_CurrentHitValue = attribute.Shield.Value;
		UpdateShieldValue();
	}

	public void DeInit()
	{
		EntityBase entity = m_Entity;
		entity.OnCrit = (Action<long>)Delegate.Remove(entity.OnCrit, new Action<long>(OnCritEvent));
		Updater.RemoveUpdate("EntityData_Update.RebornUpdate", OnRebornUpdate);
	}

	public void UseDeadRecover()
	{
		mDeadRecover--;
	}

	public long ChangeHP(EntityBase entity, long HP)
	{
		if (m_Entity.Type == EntityType.Baby)
		{
			HP = 0L;
		}
		if (m_Entity.IsSelf)
		{
			if (!GameLogic.Hold.BattleData.Challenge_RecoverHP() && HP > 0)
			{
				HP = 0L;
			}
			if (CurrentHP + HP <= 0 && mDeadRecover > 0)
			{
				float num = MathDxx.Abs((float)(CurrentHP + HP) / (float)MaxHP);
				if (num < 0.3f)
				{
					if (GameConfig.GetFirstDeadRecover())
					{
						HP = GameLogic.Random(1, 50) - CurrentHP;
					}
					UseDeadRecover();
				}
			}
		}
		long num2 = HP;
		CurrentHP += HP;
		if (CurrentHP > MaxHP)
		{
			CurrentHP = MaxHP;
		}
		else if (CurrentHP < 0)
		{
			num2 -= CurrentHP;
			CurrentHP = 0L;
		}
		if (CurrentHP == 0 && m_Entity.OnWillDead != null)
		{
			m_Entity.OnWillDead();
		}
		if (CurrentHP == 0 && attribute.GetCanReborn())
		{
			LocalSave.Instance.BattleIn_AddRebornSkill();
			long num3 = attribute.RebornHP.Value + MathDxx.CeilToInt(attribute.RebornHPPercent.Value * (float)MaxHP);
			ChangeHP(entity, num3);
			CurrentHP = num3;
			RebornUpdate();
		}
		if (HP > 0)
		{
			m_Entity.PlayEffect(3100010);
			if (m_Entity.IsSelf)
			{
				GameLogic.Hold.Sound.PlayBattleSpecial(5000006, m_Entity.position);
			}
		}
		if ((bool)m_Entity.m_HPSlider && m_Entity.m_HPSlider.gameObject.activeInHierarchy)
		{
			m_Entity.m_HPSlider.UpdateHP();
		}
		if (m_Entity.GetIsDead())
		{
			m_Entity.ExcuteCommend(EBattleAction.EBattle_Action_Dead_Before, new BattleStruct.DeadStruct
			{
				entity = m_Entity
			});
			m_Entity.ExcuteCommend(EBattleAction.EBattle_Action_Dead, new BattleStruct.DeadStruct
			{
				entity = m_Entity
			});
			if (m_Entity.m_Data.Divide == 0 && entity != null)
			{
				DoDeadCommand(entity);
			}
		}
		if (m_Entity.OnChangeHPAction != null)
		{
			m_Entity.OnChangeHPAction(CurrentHP, MaxHP, GetHPPercent(), num2);
		}
		if (GetHPPercent() == 1f && m_Entity.OnFullHP != null)
		{
			m_Entity.OnFullHP();
		}
		m_Entity.CurrentHPUpdate();
		if (mHP2AttackSpeed > 0)
		{
			ExcuteAttributes("AttackSpeed%", -mHP2AttackSpeed);
		}
		mHP2AttackSpeed = (long)(attribute.HP2AttackSpeed.Value * 10000f * (1f - GetHPPercent()));
		if (mHP2AttackSpeed > 0)
		{
			ExcuteAttributes("AttackSpeed%", mHP2AttackSpeed);
		}
		if (mHP2Miss > 0)
		{
			ExcuteAttributes("MissRate%", -mHP2Miss);
		}
		mHP2Miss = (long)(attribute.HP2Miss.Value * 10000f * (1f - GetHPPercent()));
		if (mHP2Miss > 0)
		{
			ExcuteAttributes("MissRate%", mHP2Miss);
		}
		return num2;
	}

	public float GetHPPercent()
	{
		if (MaxHP == 0)
		{
			return 1f;
		}
		return (float)CurrentHP / (float)MaxHP;
	}

	public float GetSpeed()
	{
		return (float)attribute.MoveSpeed.Value / 100f;
	}

	public void ExcuteAttributes(Goods_goods.GoodData data)
	{
		ExcuteAttributes(data.goodType, data.value);
	}

	public void ExcuteAttributes(string str)
	{
		Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
		ExcuteAttributes(goodData.goodType, goodData.value);
	}

	public void ExcuteAttributes(string name, long value)
	{
		if (name.Length > "Baby:".Length && name.Substring(0, "Baby:".Length) == "Baby:")
		{
			if (name.Contains("%"))
			{
				value /= 100;
			}
			ExcuteBabyAttributes(name.Substring("Baby:".Length, name.Length - "Baby:".Length), value);
		}
		else
		{
			if (name.Length > "EquipBaby:".Length && name.Substring(0, "EquipBaby:".Length) == "EquipBaby:")
			{
				return;
			}
			if (name.Length > "LevelUp:".Length && name.Substring(0, "LevelUp:".Length) == "LevelUp:" && m_Entity is EntityHero)
			{
				(m_Entity as EntityHero).ExcuteLevelUpAttributes(name.Substring("LevelUp:".Length, name.Length - "LevelUp:".Length), value);
				return;
			}
			bool flag = attribute.Excute(name, value);
			if (!flag && value > 0)
			{
				flag = GameLogic.SelfAttribute.Excute(name, value);
			}
			if (name != null)
			{
				if (!(name == "BodyScale%"))
				{
					if (!(name == "AttackSpeed%"))
					{
						if (!(name == "AttackSpeed%_Buff"))
						{
							if (!(name == "BabyCountAttack%"))
							{
								if (name == "BabyCountAttackSpeed%")
								{
									int count = mBabies.Count;
									if (count > 0)
									{
										ExcuteAttributes("AttackSpeed%", value * count);
									}
								}
							}
							else
							{
								int count2 = mBabies.Count;
								if (count2 > 0)
								{
									ExcuteAttributes("Attack%", value * count2);
								}
							}
						}
						else
						{
							m_Entity.mAniCtrlBase.UpdateWeaponSpeed((float)value / 10000f);
						}
					}
					else
					{
						if (m_Entity.m_AniCtrl == null)
						{
							Debugger.Log("m_Entity.m_AniCtrl is null");
						}
						else if (m_Entity.mAniCtrlBase == null)
						{
							Debugger.Log("m_Entity.m_AniCtrl。mAniCtrlBase is null");
						}
						m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", (float)value / 10000f);
						m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", (float)value / 10000f);
					}
				}
				else
				{
					UpdateBodyScale();
				}
			}
			if (flag)
			{
				return;
			}
			switch (name)
			{
			case "Gold":
				break;
			case "Exp":
			{
				float num = (float)value * (1f + GameLogic.SelfAttribute.InGameExp.Value);
				if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel)
				{
					num *= LocalModelManager.Instance.Stage_Level_stagechapter.GetScoreRate();
				}
				GameLogic.Hold.BattleData.AddGold(num);
				float exp = (float)value * (1f + attribute.ExpGet.Value);
				AddExp(exp);
				break;
			}
			case "HPRecoverFixed":
				GameLogic.Send_Recover(m_Entity, value);
				break;
			case "HPRecoverFixed%":
			{
				long hPBase = attribute.GetHPBase();
				long value3 = MathDxx.CeilToInt((float)(hPBase * value) / 10000f);
				GameLogic.Send_Recover(m_Entity, value3);
				break;
			}
			case "HPRecover":
				Attribute_HP(value);
				break;
			case "HPRecover%":
				Attribute_HPPercent(value);
				break;
			case "HPRecoverBase%":
				Attribute_HPBasePercent(value);
				break;
			case "AllSpeed%":
				Modify_AllSpeed(value);
				break;
			case "BulletSpeed%":
				Modify_BulletSpeed(value);
				break;
			case "MaxLevel":
				MaxLevel += (int)value;
				break;
			case "BabyArgs":
				m_Entity.SetBabyArgs(value);
				break;
			case "Invincible":
				Modify_Invincible(value > 0);
				break;
			case "AttackParentAttack%":
			{
				if (!(m_Entity is EntityCallBase))
				{
					break;
				}
				EntityCallBase entityCallBase2 = m_Entity as EntityCallBase;
				if ((bool)entityCallBase2)
				{
					EntityBase parent2 = entityCallBase2.GetParent();
					if (parent2 != null)
					{
						long value2 = parent2.m_EntityData.attribute.AttackValue.Value;
						long count3 = (long)((float)(value2 * value) / 10000f);
						attribute.AttackValue.InitValueCount(count3);
					}
				}
				break;
			}
			case "BodyHitParentAttack%":
			{
				if (!(m_Entity is EntityCallBase))
				{
					break;
				}
				EntityCallBase entityCallBase = m_Entity as EntityCallBase;
				if ((bool)entityCallBase)
				{
					EntityBase parent = entityCallBase.GetParent();
					if (parent != null)
					{
						long valueCount = parent.m_EntityData.attribute.AttackValue.ValueCount;
						attribute.BodyHit.InitValueCount((long)((float)(valueCount * value) / 10000f));
					}
				}
				break;
			}
			}
		}
	}

	public void Modify_AllSpeed(long value)
	{
		m_Entity.mAniCtrlBase.SetAllSpeed((float)value / 10000f);
		attribute.MoveSpeed.UpdateValuePercent(value);
	}

	public void Modify_BulletSpeed(float value)
	{
		BulletSpeed += value;
	}

	public void Modify_HitCreate2(int count, float percent)
	{
		mHitCreate2 += count;
		mHitCreate2Percent = percent;
	}

	public void Modify_FlyStone(int count)
	{
		mFlyStoneCount += count;
		m_Entity.SetFlyStone(IsFlyStone());
	}

	public bool IsFlyStone()
	{
		return mFlyStoneCount > 0;
	}

	public void Modify_FlyWater(int count)
	{
		mFlyWaterCount += count;
		m_Entity.SetFlyWater(IsFlyWater());
	}

	public bool IsFlyWater()
	{
		return mFlyWaterCount > 0;
	}

	public void Modify_BulletThroughCount(int count)
	{
		mBulletThroughCount += count;
	}

	public bool GetCanDizzy()
	{
		if (Updater.AliveTime - mDizzyTime >= attribute.Monster_DizzyDelay.Value)
		{
			mDizzyTime = Updater.AliveTime;
			return true;
		}
		return false;
	}

	public void UpdateDizzy(int count)
	{
		if (DizzyCount == 0 && count == 1)
		{
			m_Entity.m_AniCtrl.SendEvent("Dizzy");
			if (m_Entity.OnDizzy != null)
			{
				m_Entity.OnDizzy(obj: true);
			}
		}
		else if (DizzyCount == 1 && count == -1)
		{
			m_Entity.mAniCtrlBase.DizzyEnd();
			if (m_Entity.OnDizzy != null)
			{
				m_Entity.OnDizzy(obj: false);
			}
		}
		DizzyCount += count;
	}

	public bool IsDizzy()
	{
		return DizzyCount > 0;
	}

	public bool GetInvincible()
	{
		return InvincibleCount > 0;
	}

	public void Modify_Invincible(bool value)
	{
		if (value)
		{
			InvincibleCount++;
		}
		else
		{
			InvincibleCount--;
		}
	}

	public void UpdateBodyScale()
	{
		m_Entity.SetBodyScale(attribute.BodyScale.Value);
	}

	private float GetBuffBulletValue(EntityBase source, BulletBase bullet, float value)
	{
		if (bullet.m_Data.DebuffID != 0)
		{
			string name = LocalModelManager.Instance.Buff_alone.GetBeanById(bullet.m_Data.DebuffID).Attribute;
			return GetBuffValueInternal(source, name, value);
		}
		return value;
	}

	private float GetBuffValueInternal(EntityBase source, string name, float value)
	{
		float num = 0f;
		switch (name)
		{
		case "Att_Fire":
			value += (float)attribute.Att_Fire_Resist.Value;
			value *= 1f - attribute.Att_Fire_ResistPercent.Value;
			return value;
		case "Att_Thunder":
			return value;
		case "Att_Ice":
			return value;
		case "Att_Poison":
			value += (float)attribute.Att_Poison_Resist.Value;
			value *= 1f - attribute.Att_Poison_ResistPercent.Value;
			return value;
		default:
			return value;
		}
	}

	public void ExcuteBuffs(EntityBase source, int buffid, string name, float value)
	{
		value = MathDxx.CeilBig(value);
		int num = (int)GetBuffValueInternal(source, name, value);
		GameLogic.SendHit_Buff(m_Entity, source, num, GameLogic.GetElement(name), buffid);
	}

	public void AddDeBuff(EElementType element)
	{
	}

	public bool GetMissHP()
	{
		return MissHP_Count > 0;
	}

	public void Modify_MissHP(bool value)
	{
		if (value)
		{
			MissHP_Count++;
		}
		else
		{
			MissHP_Count--;
		}
		if (m_Entity.m_HPSlider != null)
		{
			m_Entity.m_HPSlider.ShowHP(MissHP_Count <= 0);
		}
	}

	private void OnCritEvent(long value)
	{
		if (attribute.CritAddHP.Value > 0f)
		{
			long value2 = (long)(attribute.CritAddHP.Value * (float)MaxHP);
			GameLogic.Send_Recover(m_Entity, value2);
		}
	}

	public long GetAttackBase()
	{
		return GetAttackBase(0);
	}

	public long GetAttackBase(int attack)
	{
		float num = 0f;
		if (m_Entity.IsSelf)
		{
			attribute.AttackValue.UpdateValueCount(attack);
			num = attribute.AttackValue.Value;
			attribute.AttackValue.UpdateValueCount(-attack);
		}
		else
		{
			attribute.AttackValue.UpdateValueCount(attack);
			num = attribute.AttackValue.Value;
			attribute.AttackValue.UpdateValueCount(-attack);
		}
		num *= 1f + attribute.Attack_Value.Value;
		return (long)num;
	}

	public long GetAttack(int attack)
	{
		float num = 1f;
		num *= 1f + HP2AttackRatio * (1f - GetHPPercent());
		num *= attribute.AttackModify.Value;
		long attackBase = GetAttackBase(attack);
		return (long)((float)attackBase * num);
	}

	private bool GetMiss(EntityBase source)
	{
		if (source == null)
		{
			return false;
		}
		float value = source.m_EntityData.attribute.HitRate.Value - attribute.MissRate.Value;
		value = MathDxx.Clamp(value, 0.05f, 1f);
		float num = GameLogic.Random(0f, 1f);
		if (!(num < value))
		{
			return true;
		}
		return false;
	}

	public HitStruct GetHurt(HitStruct otherhs)
	{
		float num = otherhs.before_hit;
		if (otherhs.sourcetype == HitSourceType.eBullet)
		{
			if (otherhs.source != null)
			{
				float num2 = 1f;
				float value = otherhs.source.m_EntityData.attribute.AttackModify.Value;
				if (m_Entity.GetFlying())
				{
					num -= (float)otherhs.source.m_EntityData.attribute.HitToFly.Value * value;
					num2 *= 1f + otherhs.source.m_EntityData.attribute.HitToFlyPercent.Value;
				}
				else
				{
					num -= (float)otherhs.source.m_EntityData.attribute.HitToGround.Value * value;
					num2 *= 1f + otherhs.source.m_EntityData.attribute.HitToGroundPercent.Value;
				}
				if (m_Entity.m_Data.Attackrangetype == 1)
				{
					num -= (float)otherhs.source.m_EntityData.attribute.HitToNear.Value * value;
					num2 *= 1f + otherhs.source.m_EntityData.attribute.HitToNearPercent.Value;
				}
				else if (m_Entity.m_Data.Attackrangetype == 2)
				{
					num -= (float)otherhs.source.m_EntityData.attribute.HitToFar.Value * value;
					num2 *= 1f + otherhs.source.m_EntityData.attribute.HitToFarPercent.Value;
				}
				if (m_Entity.Type == EntityType.Boss)
				{
					num -= (float)otherhs.source.m_EntityData.attribute.HitToBoss.Value * value;
					num2 *= 1f + otherhs.source.m_EntityData.attribute.HitToBossPercent.Value;
				}
				num *= num2;
			}
			if ((bool)otherhs.source && otherhs.bulletdata != null && otherhs.bulletdata.bullet != null)
			{
				num = GetBuffBulletValue(otherhs.source, otherhs.bulletdata.bullet, num);
				if (otherhs.bulletdata.bullet.m_Data.DebuffID != 0)
				{
					GameLogic.SendBuff(m_Entity, otherhs.source, otherhs.bulletdata.bullet.m_Data.DebuffID, otherhs.bulletdata.bullet.GetBuffArgs());
				}
			}
		}
		if (otherhs.sourcetype == HitSourceType.eBullet || otherhs.sourcetype == HitSourceType.eBody)
		{
			if (GetMiss(otherhs.source))
			{
				otherhs.real_hit = 0L;
				otherhs.type = HitType.Miss;
				if (m_Entity.OnMiss != null)
				{
					m_Entity.OnMiss();
				}
				return otherhs;
			}
			if (otherhs.sourcetype == HitSourceType.eBody)
			{
				num += (float)attribute.BodyHittedCount.Value;
				num = MathDxx.Clamp(num, num, -1f);
				num *= 1f - attribute.BodyHitted.Value;
			}
			else
			{
				num *= 1f - attribute.BulletDef.Value;
			}
			float num3 = GameLogic.Random(0f, 1f);
			if (num3 < attribute.BlockRate.Value)
			{
				num *= 0.5f;
				otherhs.type = HitType.Block;
			}
		}
		if (otherhs.sourcetype == HitSourceType.eBullet || otherhs.sourcetype == HitSourceType.eBody)
		{
			if ((bool)otherhs.source)
			{
				float num4 = 0f;
				num4 = ((otherhs.sourcetype != 0 || otherhs.bulletdata == null) ? otherhs.source.m_EntityData.attribute.CritSuperRate.Value : otherhs.bulletdata.bullet.mBulletTransmit.CritSuperRate);
				float num5 = GameLogic.Random(0f, 1f);
				if (num5 < num4)
				{
					otherhs.type = HitType.Crit;
					num *= otherhs.source.m_EntityData.attribute.CritSuperValue.Value;
				}
				else
				{
					num4 = ((otherhs.sourcetype != 0 || otherhs.bulletdata == null) ? (otherhs.source.m_EntityData.attribute.CritRate.Value - attribute.CritDefRate.Value) : (otherhs.bulletdata.bullet.mBulletTransmit.CritRate - attribute.CritDefRate.Value));
					num5 = GameLogic.Random(0f, 1f);
					if (num5 < num4)
					{
						otherhs.type = HitType.Crit;
						num *= otherhs.source.m_EntityData.attribute.CritValue.Value;
					}
				}
			}
			if ((bool)otherhs.source && otherhs.source.Type == EntityType.Boss)
			{
				num *= 1f - attribute.HitFromBoss.Value;
			}
			if (m_Entity.Type == EntityType.Boss)
			{
				num *= 1f + (float)attribute.HitToBoss.Value;
			}
			if ((bool)otherhs.source && otherhs.source.GetFlying())
			{
				num *= 1f - attribute.HitFromFly.Value;
			}
			if (m_Entity.IsSelf)
			{
				num *= 1f - Formula.GetDefence(attribute.DefenceValue.Value);
			}
			num *= 1f - attribute.Damage_Resistance.Value;
		}
		else if (otherhs.sourcetype == HitSourceType.eTrap)
		{
			num += (float)attribute.TrapDefCount.Value;
			num = MathDxx.Clamp(num, num, -1f);
			num *= 1f - attribute.TrapDef.Value;
		}
		otherhs.real_hit = MathDxx.CeilBig(num);
		if (otherhs.real_hit > -1)
		{
			otherhs.real_hit = -1L;
		}
		return otherhs;
	}

	private void OnAttackCreate()
	{
		OnAttackMeteorite();
	}

	private void OnAttackMeteorite()
	{
		int allWeight = mAttackMeteorite.GetAllWeight();
		if (allWeight != 0 && GameLogic.Random(0f, 100f) <= (float)allWeight)
		{
			AttackCallData random = mAttackMeteorite.GetRandom();
			Vector3 endpos = GameLogic.Release.MapCreatorCtrl.RandomPositionRange(m_Entity, 7);
			BulletSlopeBase bulletSlopeBase = GameLogic.Release.Bullet.CreateSlopeBullet(m_Entity, random.id, m_Entity.position + new Vector3(0f, 21f, 0f), endpos);
			bulletSlopeBase.mBulletTransmit.SetAttack(MathDxx.CeilToInt(random.hitratio * (float)m_Entity.m_EntityData.GetAttackBase()));
		}
	}

	public void AddAttackMeteorite(AttackCallData data)
	{
		mAttackMeteorite.Add(data, data.weight);
	}

	public void InitAttribute()
	{
		if (m_Entity.IsSelf)
		{
			attribute = new EntityAttributeBase();
			GameLogic.SelfAttribute.attribute.AttributeTo(attribute);
			GameLogic.SelfAttribute.Attribute2LevelUp(this);
		}
		else
		{
			attribute = new EntityAttributeBase(m_Entity.m_Data.CharID);
		}
		attribute.OnHPUpdate = OnHPUpdate;
	}

	private void OnHPUpdate(long beforemaxhp)
	{
		MaxHP = attribute.HPValue.Value;
		if (MaxHP <= 0)
		{
			MaxHP = 1L;
		}
		long num = MaxHP - beforemaxhp;
		if (num > 0)
		{
			CurrentHP += num;
		}
		else if (CurrentHP > MaxHP)
		{
			CurrentHP = MaxHP;
		}
		if (m_Entity.OnMaxHpUpdate != null)
		{
			m_Entity.OnMaxHpUpdate(beforemaxhp, MaxHP);
		}
	}

	public void Attribute_HP(long value)
	{
		long num = (long)(((float)value * (1f + GameConfig.MapGood.GetHPAddPercent()) + (float)attribute.HPAdd.Value + (float)MathDxx.CeilBig((float)attribute.GetHPBase() * 0.05f)) * (1f + attribute.HPAddPercent.Value) * (1f + GetHP2HPAddPercent()));
		if (num < 0 && CurrentHP + num <= 0)
		{
			num = -(CurrentHP - 1);
		}
		GameLogic.Send_Recover(m_Entity, num);
	}

	public void Attribute_HPPercent(long value)
	{
		long num = (long)(((float)(attribute.HPValue.Value * value) / 10000f * (1f + GameConfig.MapGood.GetHPAddPercent()) + (float)attribute.HPAdd.Value) * (1f + attribute.HPAddPercent.Value) * (1f + GetHP2HPAddPercent()));
		if (num < 0 && CurrentHP + num <= 0)
		{
			num = -(CurrentHP - 1);
		}
		GameLogic.Send_Recover(m_Entity, num);
	}

	public void Attribute_HPBasePercent(long value)
	{
		long num = (long)(((float)(attribute.GetHPBase() * value) / 10000f * (1f + GameConfig.MapGood.GetHPAddPercent()) + (float)attribute.HPAdd.Value) * (1f + attribute.HPAddPercent.Value));
		if (num < 0 && CurrentHP + num <= 0)
		{
			num = -(CurrentHP - 1);
		}
		GameLogic.Send_Recover(m_Entity, num);
	}

	public void Modify_HP2Attack(float value)
	{
		mHP2AttackRatio += value;
	}

	public void ExcuteKillAdd()
	{
		if (attribute.KillVampireResult > 0)
		{
			GameLogic.Send_Recover(m_Entity, attribute.KillVampireResult);
		}
	}

	public void ExcuteHitAdd()
	{
		if (attribute.HitVampireResult > 0)
		{
			GameLogic.Send_Recover(m_Entity, attribute.HitVampireResult);
		}
	}

	public void Modify_ThroughEnemy(int count, float ratio)
	{
		mThroughEnemy += count;
		if (count < 0 && mThroughEnemy == 0)
		{
			mThroughRatio = 1f;
		}
		else if (count > 0 && mThroughEnemy == 1)
		{
			mThroughRatio = ratio;
		}
	}

	public int GetBodyHit()
	{
		return (int)attribute.BodyHit.Value;
	}

	public void Modify_BulletLineCount(int count)
	{
		mBulletLine += count;
	}

	public bool GetBulletLine()
	{
		return mBulletLine > 0;
	}

	public void Modify_ButtetSputter(int count)
	{
		mBulletSputter += count;
	}

	public void Modify_BulletSpeedHitted(int value, float ratio, float time)
	{
		mBulletSpeedHittedCount += value;
		mBulletSpeedHitted += ratio;
		mBulletSpeedHittedTime += time;
	}

	public void Modify_BulletSpeedRatio(float value, float range)
	{
		mBulletSpeed1Ratio = value;
		mBulletSpeed1Range = range;
	}

	public float GetBulletSpeedRatio(BulletBase bullet)
	{
		mBulletSpeed = 1f;
		if (mBulletSpeed1Range > 0f && (bullet.transform.position - m_Entity.position).magnitude < mBulletSpeed1Range)
		{
			mBulletSpeed *= mBulletSpeed1Ratio;
		}
		if (mBulletSpeedHittedCount > 0 && Updater.AliveTime - m_Entity.HittedLastTime < mBulletSpeedHittedTime)
		{
			mBulletSpeed *= mBulletSpeedHitted;
		}
		return mBulletSpeed;
	}

	public void Modify_HittedInterval(float value)
	{
		HittedInterval += value;
	}

	public void Modify_TurnTableCount(int value)
	{
		TurnTableCount += value;
	}

	public void Modify_BulletScale(int count)
	{
		mBulletScaleCount += count;
	}

	public bool GetBulletScale()
	{
		return mBulletScaleCount > 0;
	}

	public void Modify_OnlyDemon(int count)
	{
		mOnlyDemonCount += count;
	}

	public bool GetOnlyDemon()
	{
		return mOnlyDemonCount > 0;
	}

	public void Modify_BabyResistBullet(int count)
	{
		mBabyResistBulletCount += count;
		if (mBabyResistBulletCount == 0 && count < 0)
		{
			BabyResistBullet(value: false);
		}
		else if (mBabyResistBulletCount == count && count > 0)
		{
			BabyResistBullet(value: true);
		}
	}

	public bool GetBabyResistBullet()
	{
		return mBabyResistBulletCount > 0;
	}

	public void Modify_FrontShield(int count)
	{
		mFrontShieldCount += count;
	}

	public bool GetFrontShield()
	{
		return mFrontShieldCount > 0;
	}

	public void Modify_Light45(int count)
	{
		mLight45 += count;
	}

	public bool GetLight45()
	{
		return mLight45 > 0;
	}

	public float GetHP2HPAddPercent()
	{
		return attribute.HP2HPAddPercent.Value * (1f - GetHPPercent());
	}

	private void OnMoveEvent(bool value)
	{
		if (mStaticReduce == null)
		{
			mStaticReduce = new StaticAddData
			{
				goodType = "DamageResist%",
				value = attribute.StaticReducePercent.ValuePercent
			};
		}
		mStaticReduce.Update(m_Entity, value, attribute.StaticReducePercent.ValuePercent);
	}

	public int GetElementCount()
	{
		int num = 0;
		Dictionary<EElementType, BuffAttrData>.Enumerator enumerator = mBuffAttrList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.count > 0)
			{
				num++;
			}
		}
		return num;
	}

	public void AddElement(EElementType type)
	{
		mBuffAttrList[type].count++;
		ArrowTrailType = GetTrailType();
		ArrowHeadType = GetHeadType();
	}

	public void RemoveElement(EElementType type)
	{
		mBuffAttrList[type].count--;
		ArrowTrailType = GetTrailType();
		ArrowHeadType = GetHeadType();
	}

	private EElementType GetTrailType()
	{
		EElementType result = EElementType.eNone;
		int num = 0;
		Dictionary<EElementType, BuffAttrData>.Enumerator enumerator = mBuffAttrList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			EElementType key = enumerator.Current.Key;
			ElementDataClass elementDataClass = ElementData[key];
			int num2 = 0;
			if (enumerator.Current.Value.count > 0)
			{
				num2 = elementDataClass.TrailPriority;
				if (num2 > num && elementDataClass.TrailPath != string.Empty)
				{
					result = key;
					num = num2;
				}
			}
		}
		return result;
	}

	private EElementType GetHeadType()
	{
		EElementType result = EElementType.eNone;
		int num = 0;
		int num2 = 0;
		Dictionary<EElementType, BuffAttrData>.Enumerator enumerator = mBuffAttrList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			EElementType key = enumerator.Current.Key;
			ElementDataClass elementDataClass = ElementData[key];
			int num3 = 0;
			if (enumerator.Current.Value.count > 0)
			{
				num3 = elementDataClass.HeadPriority;
				if (num3 > num2 && elementDataClass.HeadPath != string.Empty)
				{
					result = key;
					num2 = num3;
				}
				if (key == EElementType.eFire)
				{
					num |= 1;
				}
				if (key == EElementType.eThunder)
				{
					num |= 2;
				}
			}
		}
		if (num == 3)
		{
			UnityEngine.Debug.Log("雷火");
			result = EElementType.eThunderFire;
		}
		return result;
	}

	public void AddBaby(EntityBabyBase entity)
	{
		mBabies.Add(entity);
	}

	public void RemoveBaby(EntityBabyBase entity)
	{
		mBabies.Remove(entity);
	}

	public void AddBabyAttribute(string value)
	{
		mBabyAttributes.Add(value);
		BabyUpdateAttributes();
	}

	public void RemoveBabyAttribute(string value)
	{
		mBabyAttributes.Remove(value);
		BabyUpdateAttributes();
	}

	public void ExcuteBabyAttributes(string name, long value)
	{
		AddBabyAttribute(Utils.FormatString("{0} + {1}", name, value));
	}

	public void BabyUpdateAttributes()
	{
		int i = 0;
		for (int count = mBabies.Count; i < count; i++)
		{
			mBabies[i].UpdateAttributes();
		}
	}

	public void AddBabyLearnSkillId(int skillid)
	{
		mBabySkillIds.Add(skillid);
		BabyUpdateSkillIds();
	}

	public void BabyUpdateSkillIds()
	{
		int i = 0;
		for (int count = mBabies.Count; i < count; i++)
		{
			mBabies[i].UpdateSkillIds();
		}
	}

	public void BabyResistBullet(bool value)
	{
		int i = 0;
		for (int count = mBabies.Count; i < count; i++)
		{
			mBabies[i].SetCollider(value);
		}
	}

	private void DoDeadCommand(EntityBase entity)
	{
		if (entity.OnMonsterDeadAction != null)
		{
			entity.OnMonsterDeadAction(m_Entity);
		}
		if (entity.IsSelf)
		{
			GameLogic.Hold.BattleData.Challenge_MonsterDead();
		}
		if (entity is EntityBabyBase)
		{
			EntityBase parent = (entity as EntityBabyBase).GetParent();
			if ((bool)parent && parent.OnMonsterDeadAction != null)
			{
				parent.OnMonsterDeadAction(m_Entity);
			}
		}
		else if (entity is EntityPartBodyBase)
		{
			EntityBase parent2 = (entity as EntityPartBodyBase).GetParent();
			if ((bool)parent2 && parent2.OnMonsterDeadAction != null)
			{
				parent2.OnMonsterDeadAction(m_Entity);
			}
		}
		if (entity.OnKillAction != null)
		{
			entity.OnKillAction(m_Entity, m_Entity.HittedDirection);
		}
		if (entity is EntityBabyBase)
		{
			EntityBase parent3 = (entity as EntityBabyBase).GetParent();
			if ((bool)parent3 && parent3.OnKillAction != null)
			{
				parent3.OnKillAction(m_Entity, m_Entity.HittedDirection);
			}
		}
		else if (entity is EntityPartBodyBase)
		{
			EntityBase parent4 = (entity as EntityPartBodyBase).GetParent();
			if ((bool)parent4 && parent4.OnKillAction != null)
			{
				parent4.OnKillAction(m_Entity, m_Entity.HittedDirection);
			}
		}
	}

	private void OnMonsterDead(EntityBase entity)
	{
		OnMonsterDeadCall(entity);
	}

	public void Reborn()
	{
		CurrentHP = MaxHP;
	}

	private void OnMonsterDeadCall(EntityBase entity)
	{
		int allWeight = mCallWeight.GetAllWeight();
		if (GameLogic.Random(0, 100) < allWeight)
		{
			DeadCallData random = mCallWeight.GetRandom();
			if (random.OnDead != null)
			{
				random.OnDead(entity);
			}
		}
	}

	public void AddDeadCall(DeadCallData data)
	{
		mCallWeight.Add(data, data.weight);
	}

	public void InitExp()
	{
		Exp = 0f;
		explist.Clear();
		IList<Exp_exp> allBeans = LocalModelManager.Instance.Exp_exp.GetAllBeans();
		IEnumerator<Exp_exp> enumerator = allBeans.GetEnumerator();
		while (enumerator.MoveNext())
		{
			explist.Add(enumerator.Current.LevelID, enumerator.Current.Exp);
		}
		UpdateExp();
	}

	private void UpdateExp()
	{
		exp_data.Init(Level, Exp, explist);
		Facade.Instance.SendNotification("BATTLE_EXP_UP", exp_data);
	}

	public void AddExp(float exp)
	{
		if (Level != MaxLevel)
		{
			exp_data.Play(exp);
			Level = exp_data.currentlevel;
			Exp = exp_data.currentvalue;
		}
	}

	public float GetCurrentExp()
	{
		return Exp;
	}

	public int GetLevel()
	{
		return Level;
	}

	public bool IsMaxLevel()
	{
		return Level >= MaxLevel;
	}

	public void SetCurrentExpLevel(float exp, int level)
	{
		Exp = exp;
		Level = level;
		UpdateExp();
	}

	public void DeinitExp()
	{
		exp_data.Deinit();
	}

	public bool GetHeadShot()
	{
		if (m_Entity.Type != EntityType.Soldier)
		{
			return false;
		}
		if (!GameLogic.Self)
		{
			return false;
		}
		if (!bInitHeadShot)
		{
			bInitHeadShot = true;
			bHeadShot = (GameLogic.Random(0f, 1f) < GameLogic.Self.m_EntityData.attribute.HeadShot.Value);
		}
		return bHeadShot;
	}

	public void AddShieldCount(long count)
	{
		Shield_Count += count;
		Shield_CurrentCount += count;
		UpdateShieldCount();
	}

	public void AddShieldCountAction(Action<long> action)
	{
		EntityBase entity = m_Entity;
		entity.Shield_CountAction = (Action<long>)Delegate.Combine(entity.Shield_CountAction, action);
	}

	public void RemoveShieldCountAction(Action<long> action)
	{
		EntityBase entity = m_Entity;
		entity.Shield_CountAction = (Action<long>)Delegate.Remove(entity.Shield_CountAction, action);
	}

	public void ResetShieldCount()
	{
		Shield_CurrentCount = Shield_Count;
		UpdateShieldCount();
	}

	public bool GetCanShieldCount()
	{
		if (Shield_CurrentCount > 0)
		{
			Shield_CurrentCount--;
			UpdateShieldCount();
			return true;
		}
		return false;
	}

	private void UpdateShieldCount()
	{
		if (m_Entity.Shield_CountAction != null)
		{
			m_Entity.Shield_CountAction(Shield_CurrentCount);
		}
	}

	public void UpdateShieldValueChange(long change)
	{
		Shield_CurrentHitValue += change;
		UpdateShieldValue();
	}

	private void UpdateShieldValue()
	{
		if ((bool)m_Entity && (bool)m_Entity.m_HPSlider)
		{
			m_Entity.m_HPSlider.UpdateHP();
		}
		OnShieldObjUpdate();
	}

	public void ResetShieldHitValue()
	{
		Shield_CurrentHitValue = attribute.Shield.Value;
		UpdateShieldValue();
	}

	public long GetShieldHitValue(long value)
	{
		if (Shield_CurrentHitValue >= value)
		{
			Shield_CurrentHitValue -= value;
			UpdateShieldValue();
			return value;
		}
		if (Shield_CurrentHitValue > 0)
		{
			long shield_CurrentHitValue = Shield_CurrentHitValue;
			Shield_CurrentHitValue = 0L;
			UpdateShieldValue();
			return shield_CurrentHitValue;
		}
		return 0L;
	}

	private void OnShieldObjUpdate()
	{
		if (Shield_CurrentHitValue > 0 && mShieldObj == null)
		{
			mShieldObj = GameLogic.EffectGet("Effect/Battle/Shield");
			mShieldObj.transform.SetParent(m_Entity.m_Body.EffectMask.transform);
			mShieldObj.transform.localPosition = Vector3.zero;
			mShieldObj.transform.localRotation = Quaternion.identity;
			mShieldObj.transform.localScale = Vector3.one;
		}
		else if (Shield_CurrentHitValue == 0 && mShieldObj != null)
		{
			GameLogic.EffectCache(mShieldObj);
			mShieldObj = null;
		}
	}

	public bool GetPlayHittedSound()
	{
		if (Updater.AliveTime - hittedSoundTime > 0.2f)
		{
			hittedSoundTime = Updater.AliveTime;
			return true;
		}
		return false;
	}

	private void RebornUpdate()
	{
		m_Entity.PlayEffect(3100012);
		mRebornCurve = LocalModelManager.Instance.Curve_curve.GetCurve(100018);
		mRebornStartTime = Updater.unscaleAliveTime;
		Updater.AddUpdate("EntityData_Update.RebornUpdate", OnRebornUpdate, IgnoreTimeScale: true);
	}

	private void OnRebornUpdate(float delta)
	{
		if (Updater.unscaleAliveTime - mRebornStartTime < 1.5f)
		{
			Time.timeScale = mRebornCurve.Evaluate((Updater.unscaleAliveTime - mRebornStartTime) / 1.5f);
			return;
		}
		Time.timeScale = 1f;
		Updater.RemoveUpdate("EntityData_Update.RebornUpdate", OnRebornUpdate);
	}

	public bool GetCanTrapHit()
	{
		if (Updater.AliveTime - mTrapHitTime > 0.5f)
		{
			mTrapHitTime = Updater.AliveTime;
			return true;
		}
		return false;
	}
}
