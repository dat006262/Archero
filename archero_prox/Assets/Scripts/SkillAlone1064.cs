




using Dxx.Util;
using System;
using System.Collections.Generic;

public class SkillAlone1064 : SkillAloneBase
{
	private float range;

	private int debuffid;

	protected override void OnInstall()
	{
		if (base.m_SkillData.Args.Length < 2)
		{
			SdkManager.Bugly_Report("SkillAlone1064.cs", Utils.FormatString("SkillAlone1064 m_SkillData.Args.Length = {0}", base.m_SkillData.Args.Length));
		}
		else if (float.TryParse(base.m_SkillData.Args[0], out range) && int.TryParse(base.m_SkillData.Args[1], out debuffid) && (bool)m_Entity)
		{
			EntityBase entity = m_Entity;
			entity.OnMonsterDeadAction = (Action<EntityBase>)Delegate.Combine(entity.OnMonsterDeadAction, new Action<EntityBase>(DeadAction));
		}
	}

	protected override void OnUninstall()
	{
		if ((bool)m_Entity)
		{
			EntityBase entity = m_Entity;
			entity.OnMonsterDeadAction = (Action<EntityBase>)Delegate.Remove(entity.OnMonsterDeadAction, new Action<EntityBase>(DeadAction));
		}
	}

	private void DeadAction(EntityBase deadentity)
	{
		List<EntityBase> roundEntities = GameLogic.Release.Entity.GetRoundEntities(deadentity, range, haveself: false);
		int i = 0;
		for (int count = roundEntities.Count; i < count; i++)
		{
			GameLogic.SendBuff(roundEntities[i], m_Entity, debuffid);
		}
	}
}
