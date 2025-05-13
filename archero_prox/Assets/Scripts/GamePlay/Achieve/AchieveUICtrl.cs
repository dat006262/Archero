




using PureMVC.Interfaces;
using TableTool;
using UnityEngine;

public class AchieveUICtrl : MediatorCtrlBase
{
	public GameObject copyitems;

	public ButtonCtrl Button_Close;

	public ScrollIntAchieveCtrl mScrollInt;

	public Transform mScrollChild;

	public GameObject copyitem;

	public GameObject copyone;

	private int showCount = 10;

	private int count = 40;

	private float allWidth;

	private float itemWidth;

	private float offsetx = 360f;

	private float lastscrollpos;

	private float lastspeed;

	private int mCurrentIndex;

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_Achieve);
		};
		mScrollInt.copyItem = copyitem;
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
		mScrollInt.Init(LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter_Hero());
	}

	private void UpdateActiveOne(int index, AchieveItemCtrl one)
	{
		one.Init(index + 1);
	}

	private void UpdateActiveSize(int index, AchieveItemCtrl one)
	{
	}

	private void OnScrollEnd(int index, AchieveItemCtrl one)
	{
	}

	private void OnBeginDrag()
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
