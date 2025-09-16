




using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public abstract class ChallengeModeBase
{
	private int _id;

	protected Stage_Level_activity mActivity;

	protected string mData;

	protected Transform mParent;

	private List<ChallengeConditionBase> mConditions = new List<ChallengeConditionBase>();

	private PropType rewardtype;

	private int rewardid;

	private int rewardcount;

	private bool bMonsterHide;

	private float mHideRange = float.MaxValue;

	public int ID => _id;

	public bool RecoverHP
	{
		get;
		set;
	} = true;


	public bool DropExp
	{
		get;
		set;
	} = true;


	public bool AttackEnable
	{
		get;
		set;
	} = true;


	public bool BombermanEnable
	{
		get;
		set;
	}

	public float BombermanTime
	{
		get;
		set;
	} = 0.5f;


	public void Init(Stage_Level_activity activity)
	{
		string[] args = activity.Args;
		if (args.Length <= 0)
		{
			SdkManager.Bugly_Report(GetType().ToString(), Utils.FormatString("Stage_Level_activity[{0}] args.length == 0", activity.ID));
		}
		string[] array = args[0].Split(':');
		int.TryParse(array[0], out _id);
		mActivity = activity;
		if (array.Length == 2)
		{
			mData = array[1];
		}
		if (args.Length > 1)
		{
			int i = 1;
			for (int num = args.Length; i < num; i++)
			{
				int result = 0;
				string[] array2 = args[i].Split(':');
				if (array2.Length > 0 && int.TryParse(array2[0], out result))
				{
					string arg = string.Empty;
					if (array2.Length > 1)
					{
						arg = array2[1];
					}
					Type type = Type.GetType(Utils.GetString("ChallengeCondition", result));
					ChallengeConditionBase challengeConditionBase = type.Assembly.CreateInstance(Utils.GetString("ChallengeCondition", result)) as ChallengeConditionBase;
					challengeConditionBase.Init(result, arg, this);
					mConditions.Add(challengeConditionBase);
				}
			}
		}
		OnInit();
	}

	public void Start()
	{
		OnStart();
		int i = 0;
		for (int count = mConditions.Count; i < count; i++)
		{
			mConditions[i].Start();
		}
	}

	protected abstract void OnStart();

	public void SetUIParent(Transform parent)
	{
		mParent = parent;
	}

	public void SendEvent(string eventname, object body = null)
	{
		OnSendEvent(eventname, body);
	}

	protected virtual void OnSendEvent(string eventname, object body)
	{
	}

	public object GetEvent(string eventname)
	{
		return OnGetEvent(eventname);
	}

	protected virtual object OnGetEvent(string eventname)
	{
		return null;
	}

	protected abstract void OnInit();

	protected void OnFailure()
	{
		GameLogic.Hold.BattleData.SetWin(value: false);
		WindowUI.ShowWindow(WindowID.WindowID_GameOver);
	}

	protected void OnSuccess()
	{
		GameLogic.Hold.BattleData.SetWin(value: true);
		WindowUI.ShowWindow(WindowID.WindowID_GameOver);
	}

	public void DeInit()
	{
		int i = 0;
		for (int count = mConditions.Count; i < count; i++)
		{
			mConditions[i].DeInit();
		}
		mConditions.Clear();
		OnDeInit();
	}

	protected abstract void OnDeInit();

	private void InitRewards()
	{
		int[] reward = mActivity.Reward;
		rewardtype = (PropType)reward[0];
		rewardid = reward[1];
		rewardcount = reward[2];
	}

	public void GetRewards()
	{
		InitRewards();
		switch (rewardtype)
		{
		case PropType.eCurrency:
			if (rewardid == 1)
			{
				LocalSave.Instance.Modify_Gold(rewardcount, updateui: false);
				CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, rewardcount);
			}
			break;
		}
	}

	public string GetSuccessString()
	{
		return OnGetSuccessString();
	}

	protected abstract string OnGetSuccessString();

	public List<string> GetConditions()
	{
		List<string> list = new List<string>();
		int i = 0;
		for (int count = mConditions.Count; i < count; i++)
		{
			list.Add(mConditions[i].GetConditionString());
		}
		return list;
	}

	public void CheckCondition()
	{
		bool flag = true;
		if (mConditions.Count <= 0)
		{
			return;
		}
		int i = 0;
		for (int count = mConditions.Count; i < count; i++)
		{
			if (mConditions[i].Result != 1)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			OnSuccess();
		}
		else
		{
			OnFailure();
		}
	}

	public void MonsterDead()
	{
		OnMonsterDead();
	}

	protected virtual void OnMonsterDead()
	{
	}

	public bool GetMonsterHide()
	{
		return bMonsterHide;
	}

	public void SetMonsterHide(float range)
	{
		bMonsterHide = true;
		mHideRange = range;
	}

	public float GetMonsterHideRange()
	{
		return mHideRange;
	}
}
