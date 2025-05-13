




using System;
using System.Collections.Generic;
using TableTool;

public class SkillAlone1027 : SkillAloneBabyBase
{
	private EntityBase mParent;

	private string attackparentatt = string.Empty;

	protected override void OnInstall(object[] args)
	{
		mBabyID = int.Parse(base.m_SkillData.Args[0]);
		OnInstall();
		mParent = baby.GetParent();
		if ((bool)mParent)
		{
			EntityAttributeBase attribute = mParent.m_EntityData.attribute;
			attribute.OnAttackUpdate = (Action<long>)Delegate.Combine(attribute.OnAttackUpdate, new Action<long>(OnAttackUpdate));
			if (mParent.m_EntityData.attribute.BabyCountAttack.Value > 0f)
			{
				mParent.m_EntityData.ExcuteAttributes("Attack%", mParent.m_EntityData.attribute.BabyCountAttack.ValueLong);
			}
			if (mParent.m_EntityData.attribute.BabyCountAttackSpeed.Value > 0f)
			{
				mParent.m_EntityData.ExcuteAttributes("AttackSpeed%", mParent.m_EntityData.attribute.BabyCountAttackSpeed.ValueLong);
			}
		}
		for (int i = 1; i < base.m_SkillData.Args.Length; i++)
		{
			string str = base.m_SkillData.Args[i];
			Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
			if (goodData.goodType == "AttackParentAttack%")
			{
				attackparentatt = str;
			}
			baby.m_EntityData.ExcuteAttributes(goodData);
		}
		if (args.Length == 1 && args[0] != null && args[0] is LocalSave.EquipOne)
		{
			LocalSave.EquipOne equipOne = args[0] as LocalSave.EquipOne;
			List<Goods_goods.GoodData> babyAttributes = equipOne.GetBabyAttributes();
			for (int j = 0; j < babyAttributes.Count; j++)
			{
				baby.m_EntityData.ExcuteAttributes(babyAttributes[j]);
			}
			List<int> babySkills = equipOne.GetBabySkills();
			for (int k = 0; k < babySkills.Count; k++)
			{
				baby.AddSkillBaby(babySkills[k]);
			}
		}
	}

	protected override void OnUninstall()
	{
		base.OnUninstall();
		if ((bool)mParent)
		{
			EntityAttributeBase attribute = mParent.m_EntityData.attribute;
			attribute.OnAttackUpdate = (Action<long>)Delegate.Remove(attribute.OnAttackUpdate, new Action<long>(OnAttackUpdate));
		}
	}

	private void OnAttackUpdate(long value)
	{
		if (!string.IsNullOrEmpty(attackparentatt))
		{
			baby.m_EntityData.ExcuteAttributes(attackparentatt);
		}
	}
}
