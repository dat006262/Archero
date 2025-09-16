




using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine.UI;

public class EventFirstGoldUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public ButtonCtrl Button_Start;

	public Text Text_Start;

	public GameTurnTableCtrl mTurnCtrl;

	private TurnTableType resultType;

	private int[] qualities = new int[6]
	{
		1,
		1,
		1,
		3,
		3,
		4
	};

	protected override void OnInit()
	{
		mTurnCtrl.TurnEnd = delegate(TurnTableData data)
		{
			resultType = data.type;
			WindowUI.CloseWindow(WindowID.WindowID_EventFirstGold);
		};
	}

	protected override void OnOpen()
	{
		GameLogic.Hold.Sound.PlayUI(1000004);
		LocalSave.Instance.BattleIn_UpdateGoldTurn();
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
		string[] goldTurn = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).GoldTurn;
		int i = 0;
		for (int num = goldTurn.Length; i < num && i < 6; i++)
		{
			TurnTableData turnTableData = new TurnTableData();
			string[] array = goldTurn[i].Split(',');
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
		Text_Start.text = GameLogic.Hold.Language.GetLanguageByTID("开始");
	}
}
