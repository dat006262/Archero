




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainModuleMediator : WindowMediator, IMediator, INotifier
{
	public class PageData
	{
		public int Page;

		public Transform self;

		public UIBase PageCtrl;

		public RectTransform buttonrect;

		public Animation animation;

		public ButtonCtrl buttonctrl;

		public RedNodeCtrl redctrl;

		private bool bInit;

		public PageData(int page, Transform self, UIBase ctrl)
		{
			Page = page;
			this.self = self;
			PageCtrl = ctrl;
			PageCtrl.InitBefore();
			animation = self.Find("child").GetComponent<Animation>();
			buttonctrl = self.Find("child/child/Button").GetComponent<ButtonCtrl>();
			buttonrect = (buttonctrl.transform as RectTransform);
			redctrl = self.GetComponentInChildren<RedNodeCtrl>();
		}

		public void init()
		{
			if (!bInit)
			{
				bInit = true;
				PageCtrl.Init();
			}
		}

		public void AddButtonClick(Action click)
		{
			buttonctrl.onClick = click;
		}

		public void In()
		{
			init();
			RectTransform rectTransform = buttonrect;
			Vector2 sizeDelta = buttonrect.sizeDelta;
			rectTransform.sizeDelta = new Vector2(240f, sizeDelta.y);
			PageCtrl.Open();
			PageCtrl.OnLanguageChange();
		}

		public void Out()
		{
			RectTransform rectTransform = buttonrect;
			Vector2 sizeDelta = buttonrect.sizeDelta;
			rectTransform.sizeDelta = new Vector2(120f, sizeDelta.y);
			PageCtrl.Close();
		}

		public void Play(string name)
		{
			if (animation != null)
			{
				animation.Play(name);
			}
		}
	}

	private static RectTransform mScrollTransform;

	private static ScrollRectBase mScrollRect;

	private static GridLayoutGroup grid;

	private static RectTransform mButtonFront;

	private static PageData[] mPageDatas = new PageData[5];

	private static ButtonCtrl Button_Start;

	private static Transform MiddleTransform;

	private static Image Image_Sound;

	private static BoxRedAniCtrl mBoxCtrl;

	private static ButtonCtrl Button_Setting;

	private static ButtonCtrl Button_Set;

	private static GameObject Obj_Setting;

	private static ButtonCtrl Button_Rate;

	private static MainDownCtrl mDownCtrl;

	private ActionBasic action = new ActionBasic();

	private bool bSettingShow;

	private int currentPage = 2;

	private float scrollpercentx;

	private bool bOnlyMain;

	private Action OnOnlyMainAction;

	public override List<string> OnListNotificationInterests
	{
		get
		{
			List<string> list = new List<string>();
			list.Add("PUB_UI_UPDATE_CURRENCY");
			list.Add("MainUI_GetGold");
			list.Add("MainUI_UpdateExp");
			list.Add("MainUI_GotoShop");
			list.Add("MainUI_MailUpdate");
			list.Add("MainUI_LayerUpdate");
			list.Add("MainUI_TimeBoxUpdate");
			list.Add("MainUI_ShopRedCountUpdate");
			list.Add("MainUI_EquipRedCountUpdate");
			list.Add("MainUI_CardRedCountUpdate");
			list.Add("MainUI_UpdatePage");
			list.Add("MainUI_HarvestUpdate");
			list.Add("ShopUI_Update");
			return list;
		}
	}

	public MainModuleMediator()
		: base("MainUIPanel")
	{
	}

	protected override void OnRegisterOnce()
	{
		Transform parent = _MonoView.transform.Find("ScrollView/ScrollView/Viewport/Content");
		mScrollRect = _MonoView.transform.Find("ScrollView/ScrollView").GetComponent<ScrollRectBase>();
		grid = mScrollRect.content.GetComponent<GridLayoutGroup>();
		mDownCtrl = _MonoView.transform.Find("Down").GetComponent<MainDownCtrl>();
		mDownCtrl.SetScrollRect(mScrollRect);
		mPageDatas[0] = new PageData(0, _MonoView.transform.Find("Down/Button_0"), new MainUIPageShop(parent, mScrollRect));
		mPageDatas[0].init();
		mPageDatas[1] = new PageData(1, _MonoView.transform.Find("Down/Button_1"), new MainUIPageChar(parent, mScrollRect));
		mPageDatas[2] = new PageData(2, _MonoView.transform.Find("Down/Button_2"), new MainUIPageBattle(parent));
		mPageDatas[3] = new PageData(3, _MonoView.transform.Find("Down/Button_3"), new MainUIPage3(parent));
		mPageDatas[4] = new PageData(4, _MonoView.transform.Find("Down/Button_4"), new MainUIPage4(parent));
		mButtonFront = (_MonoView.transform.Find("Down/Front") as RectTransform);
		mButtonFront.parent.localScale = Vector3.one * GameLogic.WidthScaleAll;
		mScrollRect.SetWhole(grid, 5);
	}

	private void OnValueChanged(Vector2 value)
	{
		scrollpercentx = value.x;
		RectTransform rectTransform = mButtonFront;
		float x = scrollpercentx * 480f - 240f;
		Vector2 anchoredPosition = mButtonFront.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition.y);
	}

	private void OnOnlyMain()
	{
		if (false || LocalSave.Instance.mGuideData.CheckDiamondBox(mPageDatas[0].buttonctrl.transform as RectTransform, 0))
		{
		}
		if (OnOnlyMainAction != null)
		{
			OnOnlyMainAction();
			OnOnlyMainAction = null;
		}
	}

	private void EndDragItem(int page)
	{
		TouchPage(page);
	}

	private void TouchPage(int nextpage)
	{
		int num = currentPage;
		currentPage = nextpage;
		if (currentPage == num)
		{
			return;
		}
		mScrollRect.SetPage(currentPage, animate: true);
		bool flag = currentPage < num;
		string text = (!flag) ? "Right" : "Left";
		List<int> list = new List<int>();
		if (flag)
		{
			for (int num2 = num; num2 > currentPage; num2--)
			{
				list.Add(num2);
			}
		}
		else
		{
			for (int i = num; i < currentPage; i++)
			{
				list.Add(i);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			int num3 = list[j];
			mPageDatas[num3].Out();
			if (num3 == num)
			{
				mPageDatas[num3].Play(Utils.FormatString("Button_{0}Out{1}", text, num3));
			}
			else
			{
				mPageDatas[num3].Play(Utils.FormatString("Button_{0}OutMove{1}", text, num3));
			}
		}
		mPageDatas[currentPage].In();
		mPageDatas[currentPage].Play(Utils.FormatString("Button_{0}In{1}", text, currentPage));
	}

	protected override void OnRegisterEvery()
	{
		SdkManager.send_event_page_show(WindowID.WindowID_Main, "SHOW");
		ApplicationEvent.OnOnlyMain += OnOnlyMain;
		OnOnlyMainAction = null;
		bOnlyMain = true;
		GameLogic.ResetMaxResolution();
		DOTween.Sequence().AppendInterval(0.3f).AppendCallback(delegate
		{
			GameLogic.Hold.Sound.PlayBackgroundMusic(SoundManager.BackgroundMusicType.eMain);
		});
		WindowUI.ShowCurrency(WindowID.WindowID_Currency);
		GameLogic.SetInGame(gaming: false);
		LocalSave.GoldUpdateEvent = (Action<long, long>)Delegate.Combine(LocalSave.GoldUpdateEvent, new Action<long, long>(GoldUpdate));
		LocalSave.CardUpdateEvent = (Action)Delegate.Combine(LocalSave.CardUpdateEvent, new Action(UpdateCardRedCount));
		for (int i = 0; i < 5; i++)
		{
			int index = i;
			mPageDatas[i].AddButtonClick(delegate
			{
				if (index == 1)
				{
					GameLogic.Hold.Guide.mEquip.CurrentOver(0);
				}
				if (index == 3)
				{
					GameLogic.Hold.Guide.mCard.CurrentOver(0);
				}
				if (index == 0)
				{
					LocalSave.Instance.mGuideData.SetIndex(1);
				}
				if (currentPage != index)
				{
					TouchPage(index);
				}
			});
		}
		GameLogic.Hold.Guide.mEquip.GoNext(0, mPageDatas[1].buttonctrl.transform as RectTransform);
		GameLogic.Hold.Guide.mCard.GoNext(0, mPageDatas[3].buttonctrl.transform as RectTransform);
		mScrollRect.ValueChanged = OnValueChanged;
		mScrollRect.EndDragItem = EndDragItem;
		currentPage = 2;
		mPageDatas[currentPage].In();
		mScrollRect.SetPage(currentPage, animate: false);
		for (int j = 0; j < 5; j++)
		{
			mPageDatas[j].Play(Utils.FormatString("Button_Init{0}", j));
		}
		update_page();
	}

	private void update_page()
	{
		mDownCtrl.UpdateUI();
		UpdateShopRedCount();
		UpdateEquipRedCount();
		UpdateCardRedCount();
	}

	private void Guide()
	{
		WindowUI.ShowWindow(WindowID.WindowID_Battle);
	}

	private void GoldUpdate(long allgold, long change)
	{
		UpdateCardRedCount();
	}

	private void PlayGetGold(object o)
	{
		int num = (int)o;
		if (num > 0)
		{
			CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, num);
		}
	}

	protected override void OnRemoveAfter()
	{
		ApplicationEvent.OnOnlyMain -= OnOnlyMain;
		WindowUI.CloseCurrency();
	}

	private void UpdateShopRedCount()
	{
		int num = 0;
		int diamondBoxFreeCount = LocalSave.Instance.GetDiamondBoxFreeCount(LocalSave.TimeBoxType.BoxChoose_DiamondLarge);
		int diamondBoxFreeCount2 = LocalSave.Instance.GetDiamondBoxFreeCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal);
		num += diamondBoxFreeCount;
		num += diamondBoxFreeCount2;
		mDownCtrl.SetRedNodeType(0, RedNodeType.eRedCount);
		mDownCtrl.SetRedCount(0, num);
	}

	private void UpdateEquipRedCount()
	{
		mDownCtrl.UpdateLock(1);
		if (GameLogic.Hold.Guide.mEquip.process == 0)
		{
			mDownCtrl.SetRedCount(1, 0);
			return;
		}
		int num = LocalSave.Instance.Equip_GetCanWearCount();
		if (num > 0)
		{
			mDownCtrl.SetRedNodeType(1, RedNodeType.eRedWear);
			mDownCtrl.SetRedCount(1, num);
			return;
		}
		int num2 = LocalSave.Instance.Equip_GetNewCount();
		if (num2 > 0)
		{
			mDownCtrl.SetRedNodeType(1, RedNodeType.eRedNew);
			mDownCtrl.SetRedCount(1, num2);
			return;
		}
		mDownCtrl.SetRedCount(1, 0);
		int count = LocalSave.Instance.Equip_GetCanUpCount();
		mDownCtrl.SetRedNodeType(1, RedNodeType.eGreenCount);
		mDownCtrl.SetRedCount(1, count);
	}

	private void UpdateCardRedCount()
	{
		mDownCtrl.UpdateLock(3);
		int count = 0;
		if (GameLogic.Hold.Guide.mCard.process > 0 && !LocalSave.Instance.Card_GetAllMax() && LocalSave.Instance.Card_GetRandomGold() <= LocalSave.Instance.GetGold())
		{
			int num = LocalSave.Instance.Card_GetNeedLevel();
			if (LocalSave.Instance.GetLevel() >= num)
			{
				count = 1;
			}
		}
		mDownCtrl.SetRedNodeType(3, RedNodeType.eGreenUp);
		mDownCtrl.SetRedCount(3, count);
	}

	public override void OnHandleNotification(INotification notification)
	{
        switch (notification.Name)
        {
            case "MainUI_UpdatePage":
                update_page();
                break;
            case "MainUI_GotoShop":
                if (currentPage != 0)
                {
                    TouchPage(0);
                }
                break;
            case "MainUI_ShopRedCountUpdate":
                UpdateShopRedCount();
                break;
            case "MainUI_EquipRedCountUpdate":
                UpdateEquipRedCount();
                break;
            case "MainUI_CardRedCountUpdate":
                UpdateCardRedCount();
                break;
            case "MainUI_GetGold":
                {
                    //@TODO CANNOT DECODE _003COnHandleNotification_003Ec__AnonStorey1
                    //_003COnHandleNotification_003Ec__AnonStorey1 CS_0024_003C_003E8__locals0;
			        if (bOnlyMain)
			        {
                        //PlayGetGold(CS_0024_003C_003E8__locals0.vo);
                        PlayGetGold(notification.Body);
			        }
			        else
			        {
				        OnOnlyMainAction = (Action)Delegate.Combine(OnOnlyMainAction, (Action)delegate
				        {
                            //PlayGetGold(CS_0024_003C_003E8__locals0.vo);
                            PlayGetGold(notification.Body);
				        });
			        }
                    break;
                }
        }
		for (int i = 0; i < 5; i++)
		{
			PageData pageData = mPageDatas[i];
			if (pageData != null && pageData.PageCtrl != null)
			{
				mPageDatas[i].PageCtrl.HandleNotification(notification);
			}
		}
	}

	public override object GetEvent(string eventName)
	{
		for (int i = 0; i < 5; i++)
		{
			PageData pageData = mPageDatas[i];
			if (pageData != null && pageData.PageCtrl != null)
			{
				object obj = mPageDatas[i].PageCtrl.OnGetEvent(eventName);
				if (obj != null)
				{
					return obj;
				}
			}
		}
		return null;
	}

	private void MiddleShow(bool show)
	{
		Button_Start.gameObject.SetActive(show);
	}

	private void OnButtonClick()
	{
	}

	private void UpdateGold()
	{
	}

	protected override void OnLanguageChange()
	{
		for (int i = 0; i < 5; i++)
		{
			PageData pageData = mPageDatas[i];
			if (pageData != null && pageData.PageCtrl != null)
			{
				mPageDatas[i].PageCtrl.OnLanguageChange();
			}
		}
		mDownCtrl.OnLanguageChange();
	}
}
