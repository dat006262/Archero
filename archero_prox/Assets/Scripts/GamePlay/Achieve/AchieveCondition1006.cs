




using Dxx.Util;

public class AchieveCondition1006 : AchieveConditionBase
{
	private int entityid;

	private int count;

	protected override void OnInit()
	{
		if (mArgs.Length != 2)
		{
			SdkManager.Bugly_Report(GetType().ToString(), Utils.FormatString("Args.Length:{0} != 2   !!!", mArgs.Length));
		}
		count = mData.maxcount;
		if (!int.TryParse(mArgs[1], out entityid))
		{
			SdkManager.Bugly_Report(GetType().ToString(), Utils.FormatString("Args[0] is not a int type"));
		}
	}

	protected override void OnExcute()
	{
		int killMonsters = GameLogic.Hold.BattleData.GetKillMonsters(entityid);
		LocalSave.Instance.Achieve_AddProgress(base.ID, killMonsters);
	}

	protected override string OnGetConditionString()
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("monstername{0}", entityid));
		return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", mData.mData.CondType), languageByTID, count);
	}
}
