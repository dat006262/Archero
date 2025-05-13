




using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class BuffAloneBase
{
	public class BuffData
	{
		public int symbol;

		public float updatetime;

		public float value;

		public int hurtCount;

		public string hittype;

		public float current_time;
	}

	public const string Hit_Fixed = "FixedDamage";

	public const string Hit_AttackPercent = "Attack%";

	public const string Hit_AttackBasePercent = "AttackBase%";

	public const string Hit_HPMaxFrom = "SourceHPMax%";

	public const string Hit_HPFrom = "SourceHP%";

	public const string Hit_HPMaxTo = "TargetHPMax%";

	public const string Hit_HPTo = "TargetHP%";

	public const string RecoverHPPercent = "HPRecover%";

	public const string BodyHitPercent = "BodyHit%";

	public const string RecoverHPBasePercent = "HPRecoverBase%";

	public const string HPRecover = "HPRecover";

	private int _BuffID;

	protected List<Goods_goods.GoodData> attrList = new List<Goods_goods.GoodData>();

	protected Buff_alone buff_data;

	protected EntityBase m_Entity;

	protected EntityBase m_Target;

	private float startTime;

	private float endTime;

	private bool bForever;

	private bool isEnd;

	private bool bDizzyTrue;

	private bool bCanDizzy;

	private EElementType changeElement;

	protected float[] args;

	public Action<int> RemoveAction;

	private GameObject effect;

	protected List<BuffData> mEffects = new List<BuffData>();

	public const string SelfAttackPercent = "SelfAttack%";

	public const string SelfFixedDamage = "SelfFixedDamage";

	public const string OtherAttackPercent = "OtherAttack%";

	public const string OtherFixedDamage = "OtherFixedDamage";

	public const string ThunderRange = "Range";

	private float thunder_range;

	private float thunder_otherhit;

	private float thunder_selfhit;

	public int BuffID => _BuffID;

	public void Init(EntityBase entity, BattleStruct.BuffStruct data, Buff_alone buff_data)
	{
		this.buff_data = buff_data;
		_BuffID = buff_data.BuffID;
		m_Entity = entity;
		m_Target = data.entity;
		args = data.args;
		bForever = (buff_data.Time == 0);
		ResetBuffTime(force: true);
		InitEffects();
		OnStart();
	}

	protected virtual void OnStart()
	{
	}

	public void ResetBuffTime(bool force = false)
	{
		if (buff_data.BuffType != 1 || force)
		{
			OnResetBuffTime();
			startTime = Updater.AliveTime + (float)buff_data.Delay_time;
			if (buff_data.Attribute == "Att_Ice" && m_Entity.Type == EntityType.Boss)
			{
				endTime = (float)buff_data.Time / 1000f / 2f + startTime;
			}
			else
			{
				endTime = (float)buff_data.Time / 1000f + startTime;
			}
			int i = 0;
			for (int count = mEffects.Count; i < count; i++)
			{
				BuffData buffData = mEffects[i];
				buffData.hurtCount = 0;
			}
		}
	}

	protected virtual void OnResetBuffTime()
	{
	}

	private void InitEffects()
	{
		bool flag = false;
		if (buff_data.BuffType == 1)
		{
			if (GameLogic.Random(0, 100) < buff_data.DizzyChance)
			{
				CreateEffect();
				bDizzyTrue = true;
				bCanDizzy = m_Entity.m_EntityData.GetCanDizzy();
				if (bCanDizzy)
				{
					flag = true;
					m_Entity.m_EntityData.UpdateDizzy(1);
				}
			}
		}
		else
		{
			CreateEffect();
		}
		ExcuteFirstEffects();
		if (flag || buff_data.BuffType != 1)
		{
			ExcuteAttribute();
			ExcuteAttributes();
			ExcuteEffects();
		}
	}

	private BuffData GetGoodDataFirst(string str)
	{
		string[] array = str.Split(' ');
		BuffData buffData = new BuffData();
		buffData.hittype = array[0];
		buffData.symbol = Goods_goods.GetSymbol(array[1]);
		buffData.value = float.Parse(array[2]) * (float)buffData.symbol;
		if (array[0].Contains("%"))
		{
			buffData.value /= 100f;
		}
		return buffData;
	}

	private void ExcuteFirstEffects()
	{
		if (buff_data.Attribute == "Att_Thunder")
		{
			Excute_Thunder();
			return;
		}
		string[] firstEffects = buff_data.FirstEffects;
		int i = 0;
		for (int num = firstEffects.Length; i < num; i++)
		{
			string str = firstEffects[i];
			ExcuteBuff(GetGoodDataFirst(str));
		}
	}

	private void ExcuteAttribute()
	{
		if (buff_data.Attribute == "Att_Fire")
		{
			changeElement = EElementType.eFire;
			m_Entity.m_Body.AddElement(changeElement);
		}
		else if (buff_data.Attribute == "Att_Ice")
		{
			changeElement = EElementType.eIce;
			m_Entity.m_Body.AddElement(changeElement);
		}
	}

	private void ExcuteAttributes()
	{
		string[] attributes = buff_data.Attributes;
		int i = 0;
		for (int num = attributes.Length; i < num; i++)
		{
			string str = attributes[i];
			attrList.Add(Goods_goods.GetGoodData(str));
			Goods_goods.GetAttribute(m_Entity, str);
		}
	}

	private void ExcuteEffects()
	{
		string[] effects = buff_data.Effects;
		int i = 0;
		for (int num = effects.Length; i < num; i++)
		{
			string str = effects[i];
			mEffects.Add(GetGoodDataEvery(str));
		}
	}

	private BuffData GetGoodDataEvery(string str)
	{
		string[] array = str.Split(' ');
		BuffData buffData = new BuffData();
		buffData.hittype = array[0];
		buffData.symbol = Goods_goods.GetSymbol(array[1]);
		string[] array2 = array[2].Split('/');
		buffData.updatetime = (float)int.Parse(array2[0]) / 1000f;
		buffData.value = float.Parse(array2[1]) * (float)buffData.symbol;
		if (array[0].Contains("%"))
		{
			buffData.value /= 100f;
		}
		buffData.current_time = Updater.AliveTime;
		return buffData;
	}

	private void CreateEffect()
	{
		if (!(effect == null))
		{
			return;
		}
		Fx_fx beanById = LocalModelManager.Instance.Fx_fx.GetBeanById(buff_data.FxId);
		if (beanById != null)
		{
			effect = GameLogic.EffectGet(beanById.Path);
			effect.transform.SetParent(m_Entity.GetKetNode(beanById.Node));
			if (beanById.Node != 7)
			{
				effect.transform.localPosition = Vector3.zero;
			}
			else
			{
				RectTransform rectTransform = effect.transform as RectTransform;
				rectTransform.anchoredPosition = Vector3.zero;
			}
			effect.transform.localRotation = Quaternion.identity;
			effect.transform.localScale = Vector3.one;
		}
	}

	private void RemoveEffect()
	{
		if (effect != null)
		{
			GameLogic.EffectCache(effect);
			effect = null;
		}
	}

	public bool IsEnd()
	{
		return !bForever && Updater.AliveTime >= endTime;
	}

	private void RemoveAttributes()
	{
		int i = 0;
		for (int count = attrList.Count; i < count; i++)
		{
			Goods_goods.GoodData goodData = attrList[i];
			goodData.value = -goodData.value;
			Goods_goods.GetAttribute(m_Entity, goodData);
		}
	}

	public void Remove()
	{
		RemoveAttributes();
		if (bDizzyTrue && bCanDizzy)
		{
			m_Entity.m_EntityData.UpdateDizzy(-1);
		}
		RemoveEffect();
		if (changeElement != 0)
		{
			m_Entity.m_Body.RemoveElement(changeElement);
		}
		OnRemove();
	}

	protected virtual void OnRemove()
	{
	}

	public void OnUpdate(float delta)
	{
		if (!(Updater.AliveTime < startTime) && !isEnd)
		{
			DealBuffHit();
			if (!bForever && Updater.AliveTime >= endTime)
			{
				isEnd = true;
			}
		}
	}

	protected virtual float OnValue(float value)
	{
		return value;
	}

	private void DealBuffHit()
	{
		int i = 0;
		for (int count = mEffects.Count; i < count; i++)
		{
			BuffData buffData = mEffects[i];
			if (Updater.AliveTime - buffData.current_time >= buffData.updatetime)
			{
				ExcuteBuff(buffData);
				buffData.current_time += buffData.updatetime;
				buffData.hurtCount++;
			}
		}
	}

	protected virtual void ExcuteBuff(BuffData data)
	{
		if (data.hittype == string.Empty)
		{
			return;
		}
		float num = 0f;
		switch (data.hittype)
		{
		case "FixedDamage":
			num = data.value;
			break;
		case "Attack%":
			if ((bool)m_Target)
			{
				int attack = 0;
				if ((bool)m_Target && m_Target.m_Weapon != null)
				{
					attack = m_Target.m_Weapon.m_Data.Attack;
				}
				num = (float)m_Target.m_EntityData.GetAttackBase(attack) * data.value;
				if (num == 0f)
				{
					attack = m_Target.m_EntityData.GetBodyHit();
					num = (float)attack * data.value;
				}
			}
			break;
		case "AttackBase%":
			if ((bool)m_Target)
			{
				num = (float)m_Target.m_EntityData.attribute.AttackValue.ValueCount * data.value;
			}
			break;
		case "SourceHPMax%":
			if ((bool)m_Target)
			{
				num = (float)m_Target.m_EntityData.MaxHP * data.value;
			}
			break;
		case "SourceHP%":
			if ((bool)m_Target)
			{
				num = (float)m_Target.m_EntityData.CurrentHP * data.value;
			}
			break;
		case "TargetHPMax%":
			num = (float)m_Entity.m_EntityData.MaxHP * data.value;
			break;
		case "TargetHP%":
			num = (float)m_Entity.m_EntityData.CurrentHP * data.value;
			break;
		case "HPRecover%":
			num = (float)m_Entity.m_EntityData.MaxHP * data.value;
			break;
		case "HPRecover":
			num = data.value;
			break;
		case "BodyHit%":
			if ((bool)m_Target)
			{
				num = (float)m_Target.m_EntityData.GetBodyHit() * data.value;
			}
			break;
		case "HPRecoverBase%":
			num = (float)m_Entity.m_EntityData.attribute.HPValue.ValueCount * data.value;
			break;
		}
		num = OnValue(num);
		num = ExcuteElement(data, num);
		if (MathDxx.Abs(num) >= 1f && (bool)m_Entity)
		{
			m_Entity.m_EntityData.ExcuteBuffs(m_Target, buff_data.BuffID, buff_data.Attribute, num);
		}
	}

	private float ExcuteElement(BuffData data, float value)
	{
		switch (buff_data.Attribute)
		{
		case "Att_Fire":
			if ((bool)m_Target)
			{
				value -= (float)m_Target.m_EntityData.attribute.Att_Fire_Add.Value;
				value *= 1f + m_Target.m_EntityData.attribute.Att_Fire_AddPercent.Value;
			}
			break;
		case "Att_Poison":
			if ((bool)m_Target)
			{
				value -= (float)m_Target.m_EntityData.attribute.Att_Poison_Add.Value;
				value *= 1f + m_Target.m_EntityData.attribute.Att_Poison_AddPercent.Value;
			}
			break;
		}
		return value;
	}

	private void Excute_Thunder()
	{
		thunder_range = 0f;
		thunder_otherhit = 0f;
		thunder_selfhit = 0f;
		int i = 0;
		for (int num = buff_data.FirstEffects.Length; i < num; i++)
		{
			Goods_goods.GoodData goodData = Goods_goods.GetGoodData(buff_data.FirstEffects[i]);
			switch (goodData.goodType)
			{
			case "SelfAttack%":
				if (m_Target != null)
				{
					thunder_selfhit = (float)goodData.value / 10000f * (float)m_Target.m_EntityData.GetAttackBase();
				}
				break;
			case "OtherAttack%":
				if (m_Target != null)
				{
					thunder_otherhit = (float)goodData.value / 10000f * (float)m_Target.m_EntityData.GetAttackBase();
				}
				break;
			case "SelfFixedDamage":
				thunder_selfhit = goodData.value;
				break;
			case "OtherFixedDamage":
				thunder_otherhit = goodData.value;
				break;
			case "Range":
				thunder_range += goodData.value;
				break;
			}
		}
		float num2 = 1f;
		if ((float)args.Length > 0f)
		{
			num2 *= args[0];
		}
		thunder_selfhit *= num2;
		thunder_otherhit *= num2;
		if (thunder_selfhit != 0f)
		{
			m_Entity.m_EntityData.ExcuteBuffs(m_Target, buff_data.BuffID, buff_data.Attribute, thunder_selfhit);
		}
		List<EntityBase> roundEntities = GameLogic.Release.Entity.GetRoundEntities(m_Entity, thunder_range, haveself: false);
		int j = 0;
		for (int count = roundEntities.Count; j < count; j++)
		{
			EntityBase entityBase = roundEntities[j];
			entityBase.m_EntityData.ExcuteBuffs(m_Target, buff_data.BuffID, buff_data.Attribute, thunder_otherhit);
			GameObject gameObject = GameLogic.EffectGet("Effect/Attributes/ThunderLine");
			gameObject.GetComponent<ThunderLineCtrl>().UpdateEntity(m_Entity, entityBase);
		}
		if (roundEntities.Count > 0)
		{
			GameLogic.Hold.Sound.PlayBattleSpecial(5000010, m_Entity.position);
		}
	}
}
