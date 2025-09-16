




using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class ActiveUICtrl : MediatorCtrlBase
{
	private const string Ani_Info_Show = "Info_Show";

	private const string Ani_Info_Hide = "Info_Hide";

	public GameObject copyitems;

	public ButtonCtrl Button_Close;

	public ScrollIntActiveCtrl mScrollInt;

	public Transform mScrollChild;

	public GameObject copyActive;

	public GameObject copyDiffcult;

	public ActiveInfoCtrl mInfoCtrl;

	public Animation mInfoAni;

	private int showCount = 10;

	private int count = 40;

	private float allWidth;

	private float itemWidth;

	private float offsetx = 360f;

	private float lastscrollpos;

	private float lastspeed;

	private int mCurrentIndex;

	private List<ActiveOneCtrl> mCaches = new List<ActiveOneCtrl>();

	private List<Stage_Level_activityModel.ActivityTypeData> mDataList;

	private int currentChoose;

	private ActiveOneCtrl mChooseActive;

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_Active);
		};
		mScrollInt.copyItem = copyActive;
		mScrollInt.mScrollChild = mScrollChild;
		mScrollInt.OnUpdateOne = UpdateActiveOne;
		mScrollInt.OnUpdateSize = UpdateActiveSize;
		mScrollInt.OnBeginDragEvent = OnBeginDrag;
		mScrollInt.OnScrollEnd = OnScrollEnd;
		copyitems.SetActive(value: false);
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		mDataList = LocalModelManager.Instance.Stage_Level_activity.GetDifficults();
		currentChoose = 0;
		mScrollInt.Init(mDataList.Count);
	}

	private void UpdateActiveOne(int index, ActiveOneCtrl one)
	{
		one.Init(mDataList[index]);
		if (index == 0 && mChooseActive == null)
		{
			mChooseActive = one;
			mInfoCtrl.Init(one.activedata);
		}
	}

	private void UpdateActiveSize(int index, ActiveOneCtrl one)
	{
		Stage_Level_activityModel.ActivityTypeData activityTypeData = mDataList[index];
	}

	private void OnScrollEnd(int index, ActiveOneCtrl one)
	{
		mInfoCtrl.Init(one.activedata);
		mInfoAni.Play("Info_Show");
		currentChoose = index;
		mChooseActive = one;
		UpdateUI();
	}

	private void OnBeginDrag()
	{
		mInfoAni.Play("Info_Hide");
	}

	private void UpdateUI()
	{
	}

	protected override void OnClose()
	{
		mScrollInt.DeInit();
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
