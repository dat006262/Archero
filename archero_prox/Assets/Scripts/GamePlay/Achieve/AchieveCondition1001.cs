




using Dxx.Util;

public class AchieveCondition1001 : AchieveConditionBase
{
	private int maxlevel;

	private int hittedcount;

	protected override void OnInit()
	{
		if (mArgs.Length != 3)
		{
			SdkManager.Bugly_Report("AchieveCondition1001", Utils.FormatString("Args.Length:{0} != 3   !!!", mArgs.Length));
		}
		if (!int.TryParse(mArgs[1], out maxlevel))
		{
			SdkManager.Bugly_Report("AchieveCondition1001", Utils.FormatString("Args[1] is not a int type"));
		}
		if (!int.TryParse(mArgs[2], out hittedcount))
		{
			SdkManager.Bugly_Report("AchieveCondition1001", Utils.FormatString("Args[2] is not a int type"));
		}
	}

	protected override void OnExcute()
	{
		if (GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID() >= maxlevel && GameLogic.Hold.BattleData.GetHittedCount(maxlevel) <= hittedcount)
		{
			LocalSave.Instance.Achieve_AddProgress(base.ID, 1);
		}
	}

	protected override bool OnIsFinish()
	{
		return GameLogic.Hold.BattleData.GetHittedCount() <= hittedcount;
	}

	protected override string OnGetBattleMaxString()
	{
		return hittedcount.ToString();
	}

	protected override string OnGetConditionString()
	{
		return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", mData.mData.CondType), maxlevel.ToString(), hittedcount.ToString());
	}
}
