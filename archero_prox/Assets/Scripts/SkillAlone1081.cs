




using Dxx.Util;
using System;
using TableTool;

public class SkillAlone1081 : SkillAloneBase
{
	private float hppercent;

	private string addatt;

	private Goods_goods.GoodData data_add;

	private Goods_goods.GoodData data_remove;

	private bool bAdded;

	private bool bInit;

	protected override void OnInstall()
	{
		if (base.m_SkillData.Args.Length != 2)
		{
			SdkManager.Bugly_Report("SkillAlone1081", Utils.FormatString("SkillID:{0} args.length:{1} != 2", base.m_SkillData.SkillID, base.m_SkillData.Args.Length));
			return;
		}
		if (!float.TryParse(base.m_SkillData.Args[0], out hppercent))
		{
			SdkManager.Bugly_Report("SkillAlone1081", Utils.FormatString("SkillID:{0} args[0] is not a float type.", base.m_SkillData.SkillID));
			return;
		}
		hppercent /= 100f;
		addatt = base.m_SkillData.Args[1];
		try
		{
			data_add = Goods_goods.GetGoodData(addatt);
			data_remove = Goods_goods.GetGoodData(addatt);
			data_remove.value *= -1L;
			EntityBase entity = m_Entity;
			entity.OnChangeHPAction = (Action<long, long, float, long>)Delegate.Combine(entity.OnChangeHPAction, new Action<long, long, float, long>(OnChangeHP));
			bInit = true;
		}
		catch
		{
			SdkManager.Bugly_Report("SkillAlone1081", Utils.FormatString("SkillID:{0} args[1] is invalid.", base.m_SkillData.SkillID));
		}
	}

	private void OnChangeHP(long currentHP, long maxHP, float percent, long change)
	{
		if (percent < hppercent)
		{
			if (!bAdded)
			{
				m_Entity.m_EntityData.ExcuteAttributes(data_add);
				bAdded = true;
			}
		}
		else if (bAdded)
		{
			m_Entity.m_EntityData.ExcuteAttributes(data_remove);
			bAdded = false;
		}
	}

	protected override void OnUninstall()
	{
		if (bInit)
		{
			EntityBase entity = m_Entity;
			entity.OnChangeHPAction = (Action<long, long, float, long>)Delegate.Remove(entity.OnChangeHPAction, new Action<long, long, float, long>(OnChangeHP));
		}
	}
}
