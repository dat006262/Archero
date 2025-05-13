




using System;

public class SkillAlone1002 : SkillAloneBase
{
	private int buffid;

	protected override void OnInstall()
	{
		buffid = int.Parse(base.m_SkillData.Args[0]);
		EntityBase entity = m_Entity;
		entity.OnMiss = (Action)Delegate.Combine(entity.OnMiss, new Action(OnMiss));
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnMiss = (Action)Delegate.Remove(entity.OnMiss, new Action(OnMiss));
	}

	private void OnMiss()
	{
		GameLogic.SendBuff(m_Entity, m_Entity, buffid);
	}
}
