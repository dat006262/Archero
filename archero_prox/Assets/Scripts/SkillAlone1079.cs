




using Dxx.Util;
using System.Collections.Generic;

public class SkillAlone1079 : SkillAloneBase
{
	private long clockindex;

	private int debuffid;

	protected override void OnInstall()
	{
		debuffid = int.Parse(base.m_SkillData.Args[0]);
		clockindex = TimeClock.Register("SkillAlone1079", 1f, OnAttack);
	}

	protected override void OnUninstall()
	{
		TimeClock.Unregister(clockindex);
	}

	private void OnAttack()
	{
		List<EntityBase> entities = GameLogic.Release.Entity.GetEntities();
		int i = 0;
		for (int count = entities.Count; i < count; i++)
		{
			GameLogic.SendBuff(entities[i], m_Entity, debuffid);
		}
	}
}
