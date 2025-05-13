




using Dxx.Util;
using System;

public class EntityCallBase : EntityBase
{
	protected EntityBase m_Parent;

	protected AIBase m_AIBase;

	public void SetParent(EntityBase entity)
	{
		m_Parent = entity;
	}

	public EntityBase GetParent()
	{
		return m_Parent;
	}

	protected override void OnCreateModel()
	{
		Type type = System.Type.GetType(Utils.GetString("AI", ClassID));
		if (type != null)
		{
			m_AIBase = (type.Assembly.CreateInstance(Utils.GetString("AI", ClassID)) as AIBase);
			m_AIBase.SetEntity(this);
			m_AIBase.Init();
		}
	}

	protected override void OnDeInitLogic()
	{
		if ((bool)m_Parent && m_Parent is EntityMonsterBase)
		{
			EntityMonsterBase entityMonsterBase = m_Parent as EntityMonsterBase;
			if ((bool)entityMonsterBase && entityMonsterBase.m_AIBase != null)
			{
				entityMonsterBase.m_AIBase.RemoveCall(new RemoveCallData
				{
					entityId = ClassID,
					deadpos = base.position
				});
			}
		}
		if (m_AIBase != null)
		{
			m_AIBase.DeInit();
			m_AIBase = null;
		}
	}

	protected override void OnDeadBefore()
	{
		base.OnDeadBefore();
		SetCollider(enable: false);
		if (m_AIBase != null)
		{
			m_AIBase.DeadBefore();
		}
	}

	public override void RemoveMove()
	{
		if (m_AIBase != null)
		{
			m_AIBase.RemoveMove();
		}
	}

	public AIBase GetAI()
	{
		return m_AIBase;
	}
}
