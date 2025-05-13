




using System;

public class SkillAlone3002 : SkillAloneBase
{
	private float percent;

	private long speed;

	private bool bUse;

	protected override void OnInstall()
	{
		EntityBase entity = m_Entity;
		entity.OnChangeHPAction = (Action<long, long, float, long>)Delegate.Combine(entity.OnChangeHPAction, new Action<long, long, float, long>(OnChangeHP));
		percent = float.Parse(base.m_SkillData.Args[0]);
		speed = long.Parse(base.m_SkillData.Args[1]);
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnChangeHPAction = (Action<long, long, float, long>)Delegate.Remove(entity.OnChangeHPAction, new Action<long, long, float, long>(OnChangeHP));
		RemoveSpeed();
	}

	private void OnChangeHP(long currentHP, long maxHP, float percent, long change)
	{
		if (percent <= this.percent)
		{
			AddSpeed();
		}
		else
		{
			RemoveSpeed();
		}
	}

	private void AddSpeed()
	{
		if (!bUse)
		{
			bUse = true;
			m_Entity.m_EntityData.ExcuteAttributes("AttackSpeed%", speed);
		}
	}

	private void RemoveSpeed()
	{
		if (bUse)
		{
			bUse = false;
			m_Entity.m_EntityData.ExcuteAttributes("AttackSpeed%", -speed);
		}
	}
}
