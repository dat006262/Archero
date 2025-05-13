




using Dxx.Util;
using System;
using System.Collections.Generic;

public class ActionBattle
{
	public class ActionBase : ActionBasic.ActionUIBase
	{
		protected override void OnInit()
		{
		}
	}

	public class ActionWait : ActionBase
	{
		private float startTime;

		public float waitTime;

		public bool ignoreTime;

		protected override void OnInit()
		{
			startTime = m_Entity.AliveTime;
		}

		protected override void OnUpdate()
		{
			if (m_Entity.AliveTime - startTime >= waitTime)
			{
				End();
			}
		}
	}

	protected List<ActionBasic.ActionBase> actionList = new List<ActionBasic.ActionBase>();

	protected int actionCount;

	protected int actionIndex;

	private EntityBase m_Entity;

	private ActionBasic.ActionBase update_action;

	public void Init(EntityBase entity)
	{
		m_Entity = entity;
		Updater.AddUpdate("ActionBattle", OnUpdate);
		OnInits();
	}

	protected virtual void OnInits()
	{
	}

	public void DeInit()
	{
		ActionClear();
		Updater.RemoveUpdate("ActionBattle", OnUpdate);
		OnDeInit();
	}

	protected virtual void OnDeInit()
	{
	}

	protected virtual void OnUpdate(float delta)
	{
		if (actionList.Count <= 0)
		{
			return;
		}
		update_action = actionList[0];
		update_action.Init();
		update_action.Update();
		if (update_action.IsEnd)
		{
			if (actionList.Count > 0)
			{
				actionList.RemoveAt(0);
			}
			actionCount = actionList.Count;
		}
	}

	public void AddAction(ActionBasic.ActionBase action)
	{
		actionList.Add(action);
		actionCount = actionList.Count;
	}

	public void AddActionWait(float waitTime)
	{
		ActionWait actionWait = new ActionWait();
		actionWait.m_Entity = m_Entity;
		actionWait.waitTime = waitTime;
		ActionWait action = actionWait;
		AddAction(action);
	}

	public void AddActionDelegate(Action a)
	{
		ActionBasic.ActionDelegate actionDelegate = new ActionBasic.ActionDelegate();
		actionDelegate.action = a;
		ActionBasic.ActionDelegate action = actionDelegate;
		AddAction(action);
	}

	public void AddActionWaitDelegate(float waitTime, Action a)
	{
		AddActionWait(waitTime);
		AddActionDelegate(a);
	}

	public void ActionClear()
	{
		actionList.Clear();
		actionCount = 0;
		actionIndex = 0;
		OnActionClear();
	}

	protected virtual void OnActionClear()
	{
	}

	public int GetActionCount()
	{
		return actionCount;
	}
}
