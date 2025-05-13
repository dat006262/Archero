




using PureMVC.Interfaces;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class StageListUICtrl : MediatorCtrlBase
{
	public RectTransform window;

	public Transform titleparent;

	public Animation titleani;

	public Text Text_Title;

	public GameObject title_normal;

	public GameObject title_hero;

	public ButtonCtrl Button_Close;

	public ScrollIntStageListCtrl mScrollInt;

	public Transform mScrollChild;

	public GameObject copyStage;

	public ButtonCtrl Button_Change;

	public Text Text_Change;

	public GameObject lockparent;

	public Text Text_Lock;

	private int mCurrentStage;

	private int mCurrentMaxStage;

	private int MaxStage;

	private bool m_bHeroModeUnlock;

	protected override void OnInit()
	{
		float fringeHeight = PlatformHelper.GetFringeHeight();
		window.anchoredPosition = new Vector2(0f, fringeHeight);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_StageList);
		};
		copyStage.SetActive(value: false);
		mScrollInt.Speed = 2f;
		mScrollInt.copyItem = copyStage;
		mScrollInt.mScrollChild = mScrollChild;
		mScrollInt.OnUpdateOne = UpdateOne;
		mScrollInt.OnUpdateSize = UpdateSize;
		mScrollInt.OnBeginDragEvent = OnBeginDrag;
		mScrollInt.OnScrollEnd = OnScrollEnd;
		Button_Change.onClick = delegate
		{
			GameLogic.Hold.BattleData.Level_CurrentStage = mCurrentStage;
			Button_Close.onClick();
		};
	}

	protected override void OnOpen()
	{
		titleani.Play("Info_Show");
		m_bHeroModeUnlock = (LocalSave.Instance.Stage_GetStage() > 10);
		if (!m_bHeroModeUnlock)
		{
			MaxStage = 10;
		}
		else
		{
			MaxStage = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
		}
		mCurrentMaxStage = LocalSave.Instance.Stage_GetStage();
		WindowUI.ShowCurrency(WindowID.WindowID_Currency);
		mScrollInt.Init(MaxStage + 1);
		mCurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
		mScrollInt.GotoInt(mCurrentStage - 1);
		InitUI();
	}

	private void InitUI()
	{
	}

	private void UpdateOne(int index, StageListOneCtrl one)
	{
		int num = index + 1;
		one.Init(num, num <= mCurrentMaxStage);
	}

	private void UpdateSize(int index, StageListOneCtrl one)
	{
	}

	private void OnScrollEnd(int index, StageListOneCtrl one)
	{
		titleani.Play("Info_Show");
		mCurrentStage = index + 1;
		bool flag = GameLogic.Hold.BattleData.IsHeroMode(mCurrentStage);
		title_normal.SetActive(!flag);
		title_hero.SetActive(flag);
		if (mCurrentStage <= MaxStage)
		{
			if (mCurrentStage > mCurrentMaxStage)
			{
                Button_Change.gameObject.SetActive(value: false);
                lockparent.SetActive(value: true);
                Text_Lock.text = GameLogic.Hold.Language.GetLanguageByTID("Chapter_UnlockGoal", mCurrentStage - 1);
			}
			else
			{
				Button_Change.gameObject.SetActive(value: true);
				lockparent.SetActive(value: false);
			}
			Text_Title.text = LocalModelManager.Instance.Stage_Level_stagechapter.GetChapterFullName(mCurrentStage);
		}
		else
		{
			Button_Change.gameObject.SetActive(value: false);
			lockparent.SetActive(value: false);
			Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("stagelist_hero_title");
		}
		UpdateUI();
	}

	private void OnBeginDrag()
	{
		titleani.Play("Info_Hide");
	}

	private void UpdateUI()
	{
	}

	protected override void OnClose()
	{
		WindowUI.CloseCurrency();
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
		Text_Change.text = GameLogic.Hold.Language.GetLanguageByTID("Chapter_Change");
	}
}
