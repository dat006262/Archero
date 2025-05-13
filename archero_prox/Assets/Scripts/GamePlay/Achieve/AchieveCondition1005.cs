




using Dxx.Util;

public class AchieveCondition1005 : AchieveConditionBase
{
	private int entityid;

	protected override void OnInit()
	{
		if (mArgs.Length != 2)
		{
			SdkManager.Bugly_Report(GetType().ToString(), Utils.FormatString("Args.Length:{0} != 2   !!!", mArgs.Length));
		}
		if (!int.TryParse(mArgs[1], out entityid))
		{
			SdkManager.Bugly_Report(GetType().ToString(), Utils.FormatString("Args[1] is not a int type"));
		}
	}

	protected override void OnExcute()
	{
		int killBoss = GameLogic.Hold.BattleData.GetKillBoss(5001);
		if (killBoss <= 0)
		{
		}
	}

	protected override string OnGetConditionString()
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("monstername{0}", entityid));
		return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", mData.mData.CondType), languageByTID);
	}
}
