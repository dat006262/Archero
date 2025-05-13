




using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1023 : SkillAloneBase
{
	private float hitratio;

	private ActionBasic action = new ActionBasic();

	protected override void OnInstall()
	{
		EntityBase entity = m_Entity;
		entity.OnMonsterDeadAction = (Action<EntityBase>)Delegate.Combine(entity.OnMonsterDeadAction, new Action<EntityBase>(DeadAction));
		hitratio = float.Parse(base.m_SkillData.Args[0]);
		action.Init();
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnMonsterDeadAction = (Action<EntityBase>)Delegate.Remove(entity.OnMonsterDeadAction, new Action<EntityBase>(DeadAction));
		action.DeInit();
	}

	private void DeadAction(EntityBase entity)
	{
		if (!(entity == null))
		{
			action.AddActionWaitDelegate(0.1f, delegate
			{
				GameObject gameObject = GameLogic.EffectGet(Utils.GetString("Game/SkillPrefab/", base.ClassName));
				gameObject.transform.position = entity.position;
				SkillAloneAttrGoodBase.Add(m_Entity, gameObject, true, hitratio);
			});
		}
	}
}
