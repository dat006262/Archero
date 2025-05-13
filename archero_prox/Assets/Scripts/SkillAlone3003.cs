




using System;
using UnityEngine;

public class SkillAlone3003 : SkillAloneBase
{
	private long percent;

	private bool bUse;

	private GameObject obj;

	protected override void OnInstall()
	{
		EntityBase entity = m_Entity;
		entity.OnMoveEvent = (Action<bool>)Delegate.Combine(entity.OnMoveEvent, new Action<bool>(OnMoveEvent));
		percent = long.Parse(base.m_SkillData.Args[0]);
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnMoveEvent = (Action<bool>)Delegate.Remove(entity.OnMoveEvent, new Action<bool>(OnMoveEvent));
		RemoveDefence();
	}

	private void OnMoveEvent(bool move)
	{
		if (move)
		{
			RemoveDefence();
		}
		else
		{
			AddDefence();
		}
	}

	private void AddDefence()
	{
		if (!bUse)
		{
			bUse = true;
			m_Entity.m_EntityData.ExcuteAttributes("DamageResist%", percent);
			obj = m_Entity.PlayEffect(3100013);
		}
	}

	private void RemoveDefence()
	{
		if (bUse)
		{
			bUse = false;
			m_Entity.m_EntityData.ExcuteAttributes("DamageResist%", -percent);
			GameLogic.EffectCache(obj);
			obj = null;
		}
	}
}
