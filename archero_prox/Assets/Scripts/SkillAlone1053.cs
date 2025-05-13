




using System;
using UnityEngine;

public class SkillAlone1053 : SkillAloneBase
{
	private float time;

	protected override void OnInstall()
	{
		time = float.Parse(base.m_SkillData.Args[0]);
		EntityBase entity = m_Entity;
		entity.OnHitted = (Action<EntityBase, long>)Delegate.Combine(entity.OnHitted, new Action<EntityBase, long>(OnHitted));
		m_Entity.m_EntityData.Modify_HittedInterval(time);
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnHitted = (Action<EntityBase, long>)Delegate.Remove(entity.OnHitted, new Action<EntityBase, long>(OnHitted));
		m_Entity.m_EntityData.Modify_HittedInterval(0f - time);
	}

	private void OnHitted(EntityBase entity, long value)
	{
		GameObject gameObject = GameLogic.EffectGet("Game/SkillPrefab/SkillAlone1053");
		gameObject.transform.SetParent(m_Entity.m_Body.EffectMask.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		AutoDespawn component = gameObject.GetComponent<AutoDespawn>();
		component.SetDespawnTime(m_Entity.m_EntityData.HittedInterval);
	}
}
