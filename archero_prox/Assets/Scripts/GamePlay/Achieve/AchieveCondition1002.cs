




using Dxx.Util;

public class AchieveCondition1002 : AchieveConditionBase
{
	private int alltime;

	protected override void OnInit()
	{
		if (mArgs.Length != 2)
		{
			SdkManager.Bugly_Report(GetType().ToString(), Utils.FormatString("Args.Length:{0} != 2   !!!", mArgs.Length));
		}
		if (!int.TryParse(mArgs[1], out alltime))
		{
			SdkManager.Bugly_Report(GetType().ToString(), Utils.FormatString("Args[1] is not a int type"));
		}
	}

	protected override void OnExcute()
	{
		if (GameLogic.Hold.BattleData.Win && GameLogic.Hold.BattleData.GetGameTime() <= alltime)
		{
			LocalSave.Instance.Achieve_AddProgress(base.ID, 1);
		}
	}

	protected override bool OnIsFinish()
	{
		return GameLogic.Hold.BattleData.GetGameTime() <= alltime;
	}

	protected override string OnGetBattleMaxString()
	{
		return Utils.GetSecond2String(alltime);
	}

	protected override string OnGetConditionString()
	{
		int num = alltime / 60;
		int num2 = alltime % 60;
		string empty = string.Empty;
		empty = ((num2 == 0) ? GameLogic.Hold.Language.GetLanguageByTID("成就_时间分", num) : GameLogic.Hold.Language.GetLanguageByTID("成就_时间分秒", num, num2));
		return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", mData.mData.CondType), empty);
	}
}
