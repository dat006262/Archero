




using System;

public class SkillAlone1057 : SkillAloneBase
{
	protected override void OnInstall()
	{
		EntityBase entity = m_Entity;
		entity.OnHitted = (Action<EntityBase, long>)Delegate.Combine(entity.OnHitted, new Action<EntityBase, long>(OnHitted));
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnHitted = (Action<EntityBase, long>)Delegate.Remove(entity.OnHitted, new Action<EntityBase, long>(OnHitted));
	}

	private void OnHitted(EntityBase entity, long hp)
	{
		if ((bool)entity && !entity.GetIsDead())
		{
			GameLogic.SendBuff(entity, m_Entity, 1022);
		}
	}
}
