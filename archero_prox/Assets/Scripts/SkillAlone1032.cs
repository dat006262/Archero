




using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1032 : SkillAloneBase
{
	protected override void OnInstall()
	{
		EntityBase entity = m_Entity;
		entity.OnMonsterDeadAction = (Action<EntityBase>)Delegate.Combine(entity.OnMonsterDeadAction, new Action<EntityBase>(DeadAction));
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnMonsterDeadAction = (Action<EntityBase>)Delegate.Remove(entity.OnMonsterDeadAction, new Action<EntityBase>(DeadAction));
	}

	private void DeadAction(EntityBase entity)
	{
		if (!(entity == null))
		{
			GameObject gameObject = GameLogic.EffectGet(Utils.GetString("Game/SkillPrefab/", base.ClassName));
			gameObject.transform.position = entity.position;
			SkillAloneAttrGoodBase.Add(m_Entity, gameObject, true);
		}
	}
}
