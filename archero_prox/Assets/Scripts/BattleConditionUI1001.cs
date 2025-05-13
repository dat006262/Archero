




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class BattleConditionUI1001 : BattleConditionUIBase
{
	public Text Text_Content;

	protected override void OnInit()
	{
	}

	protected override void OnRefresh()
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("成就_受伤次数");
		Text_Content.text = Utils.FormatString("{0}:{1}/{2}", languageByTID, GameLogic.Hold.BattleData.GetHittedCount(), mData.mCondition.GetBattleMaxString());
		bool flag = mData.mCondition.IsFinish();
		Text_Content.color = ((!flag) ? Color.red : Color.green);
	}
}
