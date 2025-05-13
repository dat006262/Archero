




using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;

public class SkillBase
{
	protected EntityBase m_Entity;

	protected Skill_skill m_Data;

	private List<SkillAloneBase> effects = new List<SkillAloneBase>();

	public void Install(EntityBase entity, Skill_skill data, params object[] args)
	{
		m_Entity = entity;
		m_Data = data;
		InstallEffects(args);
		OnInstall(args);
	}

	private void InstallEffects(params object[] args)
	{
		UpdateAttributes(1);
		int i = 0;
		for (int num = m_Data.Effects.Length; i < num; i++)
		{
			int num2 = m_Data.Effects[i];
			Type type = Type.GetType(Utils.GetString("SkillAlone", num2));
			SkillAloneBase skillAloneBase = type.Assembly.CreateInstance(Utils.GetString("SkillAlone", num2)) as SkillAloneBase;
			skillAloneBase.Install(m_Entity, m_Data, LocalModelManager.Instance.Skill_alone.GetBeanById(num2), args);
			effects.Add(skillAloneBase);
		}
		int j = 0;
		for (int num3 = m_Data.Buffs.Length; j < num3; j++)
		{
			GameLogic.SendBuff(m_Entity, m_Data.Buffs[j]);
		}
		int k = 0;
		for (int num4 = m_Data.Debuffs.Length; k < num4; k++)
		{
			m_Entity.AddDebuff(m_Data.Debuffs[k]);
		}
		if (m_Data.LearnEffectID != 0)
		{
			m_Entity.PlayEffect(m_Data.LearnEffectID);
		}
	}

	private void UpdateAttributes(int symbol)
	{
		int i = 0;
		for (int num = m_Data.Attributes.Length; i < num; i++)
		{
			Goods_goods.GoodData goodData = Goods_goods.GetGoodData(m_Data.Attributes[i]);
			m_Entity.m_EntityData.ExcuteAttributes(goodData.goodType, goodData.value * symbol);
		}
	}

	public void Uninstall()
	{
		UpdateAttributes(-1);
		for (int num = effects.Count - 1; num >= 0; num--)
		{
			effects[num].Uninstall();
		}
		int i = 0;
		for (int num2 = m_Data.Debuffs.Length; i < num2; i++)
		{
			m_Entity.RemoveDebuff(m_Data.Debuffs[i]);
		}
		int j = 0;
		for (int num3 = m_Data.Buffs.Length; j < num3; j++)
		{
			m_Entity.ExcuteCommend(EBattleAction.EBattle_Action_Remove_Buff, new BattleStruct.BuffStruct
			{
				entity = m_Entity,
				buffId = m_Data.Buffs[j]
			});
		}
		effects.Clear();
		OnUninstall();
	}

	protected virtual void OnInstall(params object[] args)
	{
	}

	protected virtual void OnUninstall()
	{
	}
}
