




using UnityEngine;

public class EntityParentBase : MonoBehaviour
{
	protected EntityBase m_Entity;

	public void SetEntityParent(EntityBase entity)
	{
		m_Entity = entity;
	}

	public EntityBase GetEntityParent()
	{
		return m_Entity;
	}

	public bool IsSelf(EntityBase entity)
	{
		if ((bool)m_Entity && m_Entity == entity)
		{
			return true;
		}
		return false;
	}
}
