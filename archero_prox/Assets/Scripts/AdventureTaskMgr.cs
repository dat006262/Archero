




using Dxx.Util;
using System;

public class AdventureTaskMgr : CInstance<AdventureTaskMgr>
{
	private AdventureTaskBase mAdventureTask;

	private int mTaskID = -1;

	public void InitAdventureTask()
	{
		mTaskID = GetRandomTaskID();
		string typeName = Utils.FormatString("AdventureTask{0}", mTaskID);
		Type type = Type.GetType(typeName);
		mAdventureTask = (type.Assembly.CreateInstance(typeName) as AdventureTaskBase);
	}

	private int GetRandomTaskID()
	{
		return 1001;
	}

	public void SetUIUpdateEvent(Action<string> callback)
	{
		if (mAdventureTask != null)
		{
			mAdventureTask.SetUIUpdateEvent(callback);
		}
	}

	public string GetShowTaskString()
	{
		if (mAdventureTask != null)
		{
			return mAdventureTask.GetShowTaskString();
		}
		return Utils.FormatString("ID:{0} is error.", mTaskID);
	}

	public bool IsTaskFinish()
	{
		if (mAdventureTask != null)
		{
			return mAdventureTask.IsTaskFinish();
		}
		return true;
	}

	public void OnHitted(EntityBase source)
	{
		if (mAdventureTask != null && mAdventureTask.Event_OnHitted != null)
		{
			mAdventureTask.Event_OnHitted(source);
		}
	}
}
