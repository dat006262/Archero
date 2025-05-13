




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class BattleConditionUI1002 : BattleConditionUIBase
{
	public Text Text_Content;

	protected override void OnInit()
	{
	}

	protected override void OnRefresh()
	{
		string second2String = Utils.GetSecond2String(GameLogic.Hold.BattleData.GetGameTime());
		Text_Content.text = Utils.FormatString("{0}/{1}", second2String, mData.mCondition.GetBattleMaxString());
		bool flag = mData.mCondition.IsFinish();
		Text_Content.color = ((!flag) ? Color.red : Color.green);
	}
}
