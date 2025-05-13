




using System;
using TableTool;

public abstract class AIMoveBase : ActionBasic.ActionUIBase
{
	protected Operation_move Data;

	public string ClassName;

	public int ClassID;

	protected JoyData m_MoveData = default(JoyData);

	public AIMoveBase(EntityBase entity)
	{
		m_MoveData.name = "MoveJoy";
		m_MoveData.action = "Run";
		ClassName = GetType().ToString();
		string s = ClassName.Substring(ClassName.Length - 4, 4);
		int.TryParse(s, out ClassID);
		Data = LocalModelManager.Instance.Operation_move.GetBeanById(ClassID);
		name = ClassName;
		m_Entity = entity;
	}

	protected sealed override void OnInit()
	{
		if (!m_Entity)
		{
			End();
			return;
		}
		EntityBase entity = m_Entity;
		entity.OnDizzy = (Action<bool>)Delegate.Combine(entity.OnDizzy, new Action<bool>(OnDizzy));
		OnInitBase();
	}

	private void OnDizzy(bool value)
	{
		if (value)
		{
			End();
		}
	}

	protected override void OnEnd1()
	{
		EntityBase entity = m_Entity;
		entity.OnDizzy = (Action<bool>)Delegate.Remove(entity.OnDizzy, new Action<bool>(OnDizzy));
	}

	protected abstract void OnInitBase();

	public static ConditionBase GetConditionTime(int time)
	{
		ConditionTime conditionTime = new ConditionTime();
		conditionTime.time = (float)time / 1000f;
		return conditionTime;
	}

	public static ConditionBase GetConditionRandomTime(int min, int max)
	{
		ConditionTime conditionTime = new ConditionTime();
		conditionTime.time = GameLogic.Random((float)min / 1000f, (float)max / 1000f);
		return conditionTime;
	}

	protected override void OnForceEnd()
	{
	}
}
