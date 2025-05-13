




using System;
using UnityEngine;

public class SkillAlone1004 : SkillAloneBase
{
	protected override void OnInstall()
	{
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
		for (int i = 0; i < 4; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, 3010, m_Entity.position + new Vector3(0f, 1f, 0f), i * 90);
		}
	}
}
