




using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine.UI;

public class EventAdTurnTableUICtrl : MediatorCtrlBase, AdsRequestHelper.AdsCallback
{
	public Text Text_Title;

	public ButtonCtrl Button_Cancel;

	public ButtonCtrl Button_Ad;

	public GameTurnTableCtrl mTurnCtrl;

	public Text Text_Turn;

	private bool bStartTurn;

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

	protected override void OnInit()
	{
		mTurnCtrl.TurnEnd = delegate(TurnTableData data)
		{
			resultType = data.type;
			WindowUI.CloseWindow(WindowID.WindowID_EventAdTurnTable);
		};
	}

	protected override void OnOpen()
	{
		bStartTurn = false;
		GameLogic.Hold.Sound.PlayUI(1000004);
		GameLogic.SetPause(pause: true);
		Button_Cancel.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_EventAdTurnTable);
		};
		Button_Ad.onClick = delegate
		{
			LocalSave.Instance.BattleAd_Use();
			Button_Ad.gameObject.SetActive(value: false);
			Button_Cancel.gameObject.SetActive(value: false);
			AdsRequestHelper.getRewardedAdapter().Show(this);
		};
		Button_Ad.gameObject.SetActive(value: true);
		Button_Cancel.gameObject.SetActive(value: true);
		InitUI();
	}

	private void InitUI()
	{
		List<TurnTableData> list = new List<TurnTableData>();
		string[] adTurn = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).AdTurn;
		int i = 0;
		for (int num = adTurn.Length; i < num && i < 6; i++)
		{
			TurnTableData turnTableData = new TurnTableData();
			string[] array = adTurn[i].Split(',');
			int result = 0;
			int.TryParse(array[0], out result);
			long result2 = 0L;
			long.TryParse(array[1], out result2);
			if (result2 > 0)
			{
				if (result == 1)
				{
					turnTableData.type = TurnTableType.Gold;
				}
				else
				{
					turnTableData.type = TurnTableType.Diamond;
				}
				turnTableData.value = result2;
			}
			else
			{
				turnTableData.type = TurnTableType.Empty;
			}
			turnTableData.quality = qualities[i];
			list.Add(turnTableData);
		}
		for (int j = list.Count; j < 6; j++)
		{
			TurnTableData turnTableData2 = new TurnTableData();
			turnTableData2.type = TurnTableType.Empty;
			list.Add(turnTableData2);
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
		Text_Turn.text = GameLogic.Hold.Language.GetLanguageByTID("event_ad_turntable_turn");
	}

	public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
	{
	}

	public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
	{
	}

	public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
	{
	}

	public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
	{
	}

	public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		if (!bStartTurn)
		{
			Button_Ad.gameObject.SetActive(value: true);
			Button_Cancel.gameObject.SetActive(value: true);
		}
	}

	public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
	{
	}

	public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		mTurnCtrl.Init();
		bStartTurn = true;
		Button_Ad.gameObject.SetActive(value: false);
		Button_Cancel.gameObject.SetActive(value: false);
	}
}
