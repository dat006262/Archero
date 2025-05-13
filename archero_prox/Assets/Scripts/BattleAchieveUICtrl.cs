




using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BattleAchieveUICtrl : MediatorCtrlBase
{
	public ScrollRectBase mScrollRect;

	public GridLayoutGroup mGrid;

	public GameObject copyitems;

	public GameObject copyachieve;

	public ButtonCtrl Button_Close;

	private List<BattleAchieveOneCtrl> mList = new List<BattleAchieveOneCtrl>();

	private LocalUnityObjctPool mPool;

	protected override void OnInit()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<BattleAchieveOneCtrl>(copyachieve);
		copyitems.SetActive(value: false);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_BattleAchieve);
		};
	}

	protected override void OnOpen()
	{
		GameLogic.SetPause(pause: true);
		InitUI();
	}

	private void InitUI()
	{
		mPool.Collect<BattleAchieveOneCtrl>();
		mList.Clear();
		List<int> stageList = LocalModelManager.Instance.Achieve_Achieve.GetStageList(GameLogic.Hold.BattleData.Level_CurrentStage, haveglobal: false);
		stageList.Sort(delegate(int a, int b)
		{
			bool flag = LocalSave.Instance.Achieve_IsFinish(a);
			bool flag2 = LocalSave.Instance.Achieve_IsFinish(b);
			return (flag == flag2) ? ((a >= b) ? 1 : (-1)) : (flag ? 1 : (-1));
		});
		int i = 0;
		for (int count = stageList.Count; i < count; i++)
		{
			BattleAchieveOneCtrl battleAchieveOneCtrl = mPool.DeQueue<BattleAchieveOneCtrl>();
			battleAchieveOneCtrl.gameObject.SetParentNormal(mScrollRect.content);
			battleAchieveOneCtrl.Init(stageList[i]);
			battleAchieveOneCtrl.OnButtonClick = delegate(int achieveid)
			{
				LocalSave.Instance.BattleIn_DeInit();
				WindowUI.ShowLoading(delegate
				{
					WindowUI.ShowWindow(WindowID.WindowID_Main);
					GameLogic.Hold.BattleData.Challenge_MainUpdateMode(achieveid);
					WindowUI.ShowWindow(WindowID.WindowID_Battle);
				});
			};
		}
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
	}
}
