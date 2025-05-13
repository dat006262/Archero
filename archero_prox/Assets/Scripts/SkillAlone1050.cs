




using System;

public class SkillAlone1050 : SkillAloneShieldValueBase
{
	protected override void OnInstall()
	{
		base.OnInstall();
		EntityBase entity = m_Entity;
		entity.OnMissAngel = (Action)Delegate.Combine(entity.OnMissAngel, new Action(OnMissAngel));
	}

	protected override void OnUninstall()
	{
		base.OnUninstall();
		EntityBase entity = m_Entity;
		entity.OnMissAngel = (Action)Delegate.Remove(entity.OnMissAngel, new Action(OnMissAngel));
	}

	private void OnMissAngel()
	{
		ResetShieldHitValue();
	}
}
