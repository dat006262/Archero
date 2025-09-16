




using PureMVC.Interfaces;
using System.Collections.Generic;
using UnityEngine.UI;

public class EventRewardTurnUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public ButtonCtrl Button_Start;

	public ButtonCtrl Button_Close;

	public GoldTextCtrl mGoldCtrl;

	public GameTurnTableCtrl mTurnCtrl;

	private TurnTableType resultType;

	private int[] qualities = new int[6]
	{
		1,
		1,
		1,
		2,
		2,
		3
	};

	private TurnTableType[] types = new TurnTableType[6]
	{
		TurnTableType.Reward_Gold2,
		TurnTableType.Reward_Gold3,
		TurnTableType.Reward_Item2,
		TurnTableType.Reward_Item3,
		TurnTableType.Reward_All2,
		TurnTableType.Reward_All3
	};

	protected override void OnInit()
	{
		mTurnCtrl.TurnEnd = delegate(TurnTableData data)
		{
			resultType = data.type;
			WindowUI.CloseWindow(WindowID.WindowID_EventRewardTurn);
		};
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_EventRewardTurn);
		};
	}

	protected override void OnOpen()
	{
		mGoldCtrl.SetCurrencyType(CurrencyType.Diamond);
		mGoldCtrl.SetValue(50);
		GameLogic.Hold.Sound.PlayUI(1000004);
		GameLogic.SetPause(pause: true);
		Button_Start.onClick = delegate
		{
			Button_Start.gameObject.SetActive(value: false);
			mTurnCtrl.Init();
		};
		Button_Start.gameObject.SetActive(value: true);
		InitUI();
	}

	private void InitUI()
	{
		List<TurnTableData> list = new List<TurnTableData>();
		int i = 0;
		for (int num = qualities.Length; i < num; i++)
		{
			TurnTableData turnTableData = new TurnTableData();
			turnTableData.type = types[i];
			turnTableData.quality = qualities[i];
			list.Add(turnTableData);
		}
		mTurnCtrl.InitGood(list);
	}

	protected override void OnClose()
	{
		GameLogic.SetPause(pause: false);
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	public override void OnLanguageChange()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_Title");
	}
}
