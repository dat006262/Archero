




using Dxx.Util;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ActiveDiffCtrl : MonoBehaviour
{
	public Text Text_Diff;

	public Text Text_Attack;

	public Text Text_Unlock;

	public GoldTextCtrl mKeyCtrl;

	public GameObject unlockobj;

	public GameObject lockobj;

	public ButtonCtrl mButton;

	private int mIndex;

	private Stage_Level_activity mData;

	private void Awake()
	{
		mButton.onClick = OnClickButton;
	}

	public void Init(int index, Stage_Level_activity data)
	{
		mIndex = index;
		mData = data;
		bool unlock = data.Unlock;
		unlockobj.SetActive(unlock);
		lockobj.SetActive(!unlock);
		Text_Diff.text = Utils.FormatString("难度:{0}", data.Difficult);
		Text_Unlock.text = Utils.FormatString("开启等级:{0}", data.LevelCondition);
	}

	private void OnClickButton()
	{
		WindowUI.CloseWindow(WindowID.WindowID_Active);
		GameLogic.Hold.BattleData.ActiveID = mData.ID;
		Debugger.Log("ActiveID = " + GameLogic.Hold.BattleData.ActiveID);
		GameLogic.Hold.BattleData.SetMode(mData.GetMode(), BattleSource.eActivity);
		GameLogic.Hold.Sound.PlayUI(1000003);
		WindowUI.ShowWindow(WindowID.WindowID_Battle);
	}
}
