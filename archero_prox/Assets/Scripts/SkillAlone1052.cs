




using System;
using UnityEngine;

public class SkillAlone1052 : SkillAloneBase
{
	private ActionBasic action = new ActionBasic();

	protected override void OnInstall()
	{
		EntityBase entity = m_Entity;
		entity.OnHitted = (Action<EntityBase, long>)Delegate.Combine(entity.OnHitted, new Action<EntityBase, long>(OnHitted));
		action.Init();
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnHitted = (Action<EntityBase, long>)Delegate.Remove(entity.OnHitted, new Action<EntityBase, long>(OnHitted));
		action.DeInit();
	}

	private void OnHitted(EntityBase entity, long value)
	{
		CreateBullets();
	}

	private void CreateBullets()
	{
		ActionBasic.ActionParallel actionParallel = new ActionBasic.ActionParallel();
		for (int i = 0; i < 8; i++)
		{
			int index = i;
			AIBase.ActionSequence actionSequence = new AIBase.ActionSequence();
			ActionBasic.ActionWait actionWait = new ActionBasic.ActionWait();
			actionWait.waitTime = GameLogic.Random(0f, 0.5f);
			ActionBasic.ActionWait actionWait2 = actionWait;
			actionSequence.AddAction(actionWait2);
			ActionBasic.ActionDelegate actionDelegate = new ActionBasic.ActionDelegate();
			actionDelegate.action = delegate
			{
				GameLogic.Release.Bullet.CreateBullet(m_Entity, 3009, m_Entity.position + new Vector3(0f, 1f, 0f), (float)index * 45f + GameLogic.Random(-30f, 30f));
			};
			actionSequence.AddAction(actionDelegate);
			actionParallel.Add(actionSequence);
		}
		action.AddAction(actionParallel);
	}
}
