




using Dxx.Util;

public class AchieveCondition1004 : AchieveConditionBase
{
	private int count;

	protected override void OnInit()
	{
		count = mData.maxcount;
	}

	protected override void OnExcute()
	{
		int killBoss = GameLogic.Hold.BattleData.GetKillBoss();
		LocalSave.Instance.Achieve_AddProgress(base.ID, killBoss);
	}

	protected override string OnGetConditionString()
	{
		return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", mData.mData.CondType), count);
	}
}
