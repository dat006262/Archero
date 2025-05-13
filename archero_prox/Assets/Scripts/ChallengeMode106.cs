




using DG.Tweening;
using Dxx.Util;
using PureMVC.Patterns;

public class ChallengeMode106 : ChallengeModeBase
{
	private BattleMatchDefenceTime_ConditionCtrl mCtrl;

	protected int currenttime;

	protected int alltime;

	private Sequence seq;

	private Sequence seq_update;

	private int roomids_index;

	private long mRandomSeed;

	private XRandom mRandom;

	protected override void OnInit()
	{
		if (!int.TryParse(mData, out alltime))
		{
			SdkManager.Bugly_Report("ChallengeMode106", Utils.FormatString("[{0}] is not a int value.", mData));
		}
		currenttime = alltime;
	}

	protected override void OnStart()
	{
		mCtrl = mParent.GetComponent<BattleMatchDefenceTime_ConditionCtrl>();
		seq = DOTween.Sequence();
		seq.AppendInterval(1f);
		seq.SetLoops(alltime);
		seq.OnStepComplete(OnUpdateSecond);
		seq.SetUpdate(isIndependentUpdate: true);
		if (mCtrl != null)
		{
			mCtrl.SetTime(currenttime);
			mCtrl.SetMeScore(0);
			mCtrl.SetOtherScore(0);
		}
		Facade.Instance.SendNotification("MatchDefenceTime_other_updatescore", 50);
	}

	protected override void OnDeInit()
	{
		KillSequence();
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
		}
		if (seq_update != null)
		{
			seq_update.Kill();
		}
	}

	private void OnUpdateSecond()
	{
		currenttime--;
		if (mCtrl != null)
		{
			mCtrl.SetTime(currenttime);
		}
		if (currenttime <= 0)
		{
			KillSequence();
			Singleton<MatchDefenceTimeSocketCtrl>.Instance.Send(MatchMessageType.eGameEnd);
			Singleton<MatchDefenceTimeSocketCtrl>.Instance.Close();
			if (mCtrl != null && mCtrl.isWin())
			{
				OnSuccess();
			}
			else
			{
				OnFailure();
			}
		}
		else
		{
			OnUpdate();
		}
	}

	protected override void OnSendEvent(string eventname, object body)
	{
		switch (eventname)
		{
		case "MatchDefenceTime_set_random_seed":
			mRandomSeed = (long)body;
			mRandom = new XRandom(mRandomSeed);
			roomids_index = mRandom.nextInt(0, 3);
			roomids_index = 0;
			break;
		case "MatchDefenceTime_me_updatescore":
		{
			int meScore = (int)body;
			if ((bool)mCtrl)
			{
				mCtrl.SetMeScore(meScore);
			}
			break;
		}
		case "MatchDefenceTime_other_updatescore":
		{
			int otherScore = (int)body;
			if ((bool)mCtrl)
			{
				mCtrl.SetOtherScore(otherScore);
			}
			break;
		}
		case "MatchDefenceTime_me_updatename":
		{
			string meName = (string)body;
			if ((bool)mCtrl)
			{
				mCtrl.SetMeName(meName);
			}
			break;
		}
		case "MatchDefenceTime_other_updatename":
		{
			string otherName = (string)body;
			if ((bool)mCtrl)
			{
				mCtrl.SetOtherName(otherName);
			}
			break;
		}
		case "MatchDefenceTime_other_dead":
		case "MatchDefenceTime_other_reborn":
		case "MatchDefenceTime_other_learn_skill":
			if (mCtrl != null)
			{
				mCtrl.ShowInfo(eventname, body);
			}
			break;
		}
	}

	protected override object OnGetEvent(string eventname)
	{
		if (eventname != null)
		{
			if (eventname == "MatchDefenceTime_get_random_roomid_row")
			{
				return roomids_index;
			}
			if (!(eventname == "MatchDefenceTime_get_random_int"))
			{
				if (eventname == "MatchDefenceTime_get_xrandom")
				{
					return mRandom;
				}
			}
			else
			{
				if (mRandom != null)
				{
					return mRandom.nextInt();
				}
				SdkManager.Bugly_Report("ChallengeMode106", "OnGetEvent get_random_int mRandom is null.");
			}
		}
		return null;
	}

	protected virtual void OnUpdate()
	{
	}

	protected override string OnGetSuccessString()
	{
		return Utils.FormatString("防守{0}秒", currenttime);
	}
}
