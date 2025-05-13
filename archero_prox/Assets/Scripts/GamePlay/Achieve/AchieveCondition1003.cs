




using Dxx.Util;

public class AchieveCondition1003 : AchieveConditionBase
{
	private int count;

	protected override void OnInit()
	{
		count = mData.maxcount;
	}

	protected override void OnExcute()
	{
		int killMonsters = GameLogic.Hold.BattleData.GetKillMonsters();
		LocalSave.Instance.Achieve_AddProgress(base.ID, killMonsters);
	}

	protected override string OnGetConditionString()
	{
		return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", mData.mData.CondType), count.ToString());
	}
}
