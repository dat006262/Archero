




using UnityEngine;

public class SkillAlone1015 : SkillAloneBase
{
	private GameObject obj;

	protected override void OnInstall()
	{
		obj = GameLogic.EffectGet("Game/SkillPrefab/SkillAlone1015");
		obj.transform.SetParent(m_Entity.transform);
		obj.transform.localPosition = Vector3.zero;
		EntityParentBase[] componentsInChildren = obj.GetComponentsInChildren<EntityParentBase>(includeInactive: true);
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].SetEntityParent(m_Entity);
		}
		m_Entity.AddNewRotateShield(obj);
	}

	protected override void OnUninstall()
	{
		m_Entity.RemoveRotateShield(obj);
	}
}
