




using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionBasic
{
	public abstract class ActionBase
	{
		public string name = string.Empty;

		public EntityBase m_Entity;

		public Func<bool> ConditionBase;

		public object ConditionBase1Data;

		public Func<object, bool> ConditionBase1;

		public Func<bool> ConditionUpdate;

		public Func<bool> ConditionContinue;

		private int mEndFrame;

		private bool mIsEnd;

		private bool isInit;

		public bool IsEnd
		{
			get
			{
				return mIsEnd;
			}
			private set
			{
				mIsEnd = value;
			}
		}

		public void Init()
		{
			if ((mEndFrame == Time.frameCount && IsEnd) || isInit)
			{
				return;
			}
			IsEnd = false;
			OnInit();
			if (!IsEnd)
			{
				isInit = true;
				if (ConditionBase != null && !ConditionBase())
				{
					End();
				}
				if (ConditionBase1 != null && !ConditionBase1(ConditionBase1Data))
				{
					End();
				}
			}
		}

		protected abstract void OnInit();

		public void Update()
		{
			if (!IsEnd)
			{
				if (ConditionUpdate != null && ConditionUpdate())
				{
					End();
				}
				else if (ConditionContinue == null || ConditionContinue())
				{
					OnUpdate();
				}
			}
		}

		protected virtual void OnUpdate()
		{
		}

		protected void End()
		{
			mEndFrame = Time.frameCount;
			isInit = false;
			IsEnd = true;
			OnEnd();
			OnEnd1();
		}

		public void ForceEnd()
		{
			if (!IsEnd)
			{
				OnForceEnd();
				End();
			}
		}

		protected abstract void OnForceEnd();

		public void Reset()
		{
			IsEnd = false;
			isInit = false;
		}

		protected virtual void OnEnd()
		{
		}

		protected virtual void OnEnd1()
		{
		}
	}

	public class ActionUIBase : ActionBase
	{
		protected override void OnForceEnd()
		{
		}

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
			startTime = Updater.AliveTime;
		}

		protected override void OnUpdate()
		{
			if (Updater.AliveTime - startTime >= waitTime)
			{
				End();
			}
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionWaitIgnoreTime : ActionBase
	{
		private float startTime;

		public float waitTime;

		protected override void OnInit()
		{
			startTime = Updater.unscaleAliveTime;
		}

		protected override void OnUpdate()
		{
			if (Updater.unscaleAliveTime - startTime >= waitTime)
			{
				End();
			}
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionParallel : ActionBase
	{
		public List<ActionBase> list;

		private int endCount;

		protected override void OnInit()
		{
			endCount = 0;
		}

		public void Add(ActionBase a)
		{
			if (list == null)
			{
				list = new List<ActionBase>();
			}
			list.Add(a);
		}

		protected override void OnUpdate()
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				ActionBase actionBase = list[i];
				if (actionBase.IsEnd)
				{
					continue;
				}
				actionBase.Init();
				actionBase.Update();
				if (actionBase.IsEnd)
				{
					endCount++;
					if (endCount == count)
					{
						End();
					}
				}
			}
		}

		protected override void OnForceEnd()
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				ActionBase actionBase = list[i];
				actionBase.ForceEnd();
			}
		}
	}

	public class ActionShowMaskUI : ActionBase
	{
		public bool show;

		protected override void OnInit()
		{
			if (show)
			{
				WindowUI.ShowMask(value: true);
			}
			else
			{
				WindowUI.ShowMask(value: false);
			}
			End();
		}

		protected override void OnForceEnd()
		{
		}
	}

	public class ActionDelegate : ActionBase
	{
		public Action action;

		public Action<bool> actionbool;

		public bool resultbool;

		public Action<int> actionint;

		public int resultint;

		public Action<string> actionstring;

		public string resultstring;

		protected override void OnInit()
		{
			if (action != null)
			{
				action();
			}
			if (actionbool != null)
			{
				actionbool(resultbool);
			}
			if (actionint != null)
			{
				actionint(resultint);
			}
			if (actionstring != null)
			{
				actionstring(resultstring);
			}
			End();
		}

		protected override void OnForceEnd()
		{
		}
	}

	protected List<ActionBase> actionList = new List<ActionBase>();

	protected int actionCount;

	protected int actionIndex;

	private bool mIgnoreTimeScale;

	private ActionBase update_action;

	public void Init(bool IgnoreTimeScale = false)
	{
		mIgnoreTimeScale = IgnoreTimeScale;
		Updater.AddUpdate("ActionBasic", OnUpdate, IgnoreTimeScale);
		OnInit1();
	}

	protected virtual void OnInit1()
	{
	}

	public void DeInit()
	{
		OnDeInit();
		ActionClear();
		Updater.RemoveUpdate("ActionBasic", OnUpdate);
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

	public void AddAction(ActionBase action)
	{
		actionList.Add(action);
		actionCount = actionList.Count;
	}

	public void AddActionWait(float waitTime)
	{
		ActionWait actionWait = new ActionWait();
		actionWait.waitTime = waitTime;
		ActionWait action = actionWait;
		AddAction(action);
	}

	public void AddActionIgnoreWait(float waitTime)
	{
		ActionWaitIgnoreTime actionWaitIgnoreTime = new ActionWaitIgnoreTime();
		actionWaitIgnoreTime.waitTime = waitTime;
		ActionWaitIgnoreTime action = actionWaitIgnoreTime;
		AddAction(action);
	}

	public void AddActionDelegate(Action a)
	{
		ActionDelegate actionDelegate = new ActionDelegate();
		actionDelegate.action = a;
		ActionDelegate action = actionDelegate;
		AddAction(action);
	}

	public void AddActionWaitDelegate(float waitTime, Action a)
	{
		AddActionWait(waitTime);
		AddActionDelegate(a);
	}

	public void AddActionIgnoreWaitDelegate(float waitTime, Action a)
	{
		AddActionIgnoreWait(waitTime);
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
