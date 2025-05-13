




using System;

public class SkillAlone1051 : SkillAloneShieldCountBase
{
	protected override void OnInstall()
	{
		base.OnInstall();
		EntityBase entity = m_Entity;
		entity.OnInBossRoom = (Action)Delegate.Combine(entity.OnInBossRoom, new Action(OnInBossRoom));
	}

	protected override void OnUninstall()
	{
		base.OnUninstall();
		EntityBase entity = m_Entity;
		entity.OnInBossRoom = (Action)Delegate.Remove(entity.OnInBossRoom, new Action(OnInBossRoom));
	}

	private void OnInBossRoom()
	{
		ResetShieldCount();
	}
}
