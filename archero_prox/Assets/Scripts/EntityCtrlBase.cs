




using System.Collections.Generic;

public abstract class EntityCtrlBase
{
	public List<EBattleAction> mActionsList = new List<EBattleAction>();

	protected EntityBase m_Entity;

	private bool bUseUpdate;

	public bool UseUpdate => bUseUpdate;

	public void SetUseUpdate()
	{
		bUseUpdate = true;
	}

	public void SetEntity(EntityBase entity)
	{
		m_Entity = entity;
	}

	public virtual void OnStart(List<EBattleAction> actIds)
	{
	}

	public virtual void OnRemove()
	{
	}

	public abstract void ExcuteCommend(EBattleAction id, object action);

	public virtual void OnUpdate(float delta)
	{
	}
}
