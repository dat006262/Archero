




using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class MainUIPageBattleCtrl : MediatorCtrlBase
{
	private enum eBattleState
	{
		eBattle,
		eActive
	}

	public GameObject window;

	public ButtonCtrl Button_Start;

	public RectTransform ButtonParent;

	public Text Text_Start;

	public Text Text_BestStage;

	public Text Text_BestScore;

	public Text Text_ChapterIndex;

	public ButtonCtrl Button_Setting;

	public ButtonCtrl Button_Mail;

	public MainUILevelItem mStageItem;

	public MainUIBattleLayerCtrl mLayerCtrl;

	public RedNodeCtrl mMailRedCtrl;

	public RedNodeCtrl mHarvestRedCtrl;

	public GoldTextCtrl mKeyCtrl;

	public ButtonCtrl Button_ModeTest;

	public ButtonCtrl Button_Change;

	public Text Text_Change;

	public ActiveUICtrl mActiveCtrl;

	public GameObject battleui;

	public GameObject activeui;

	public MainChapterCtrl mChapterCtrl;

	public ButtonCtrl Button_Harvest;

	private int currentStage;

	private float maily;

	private int mKeyCount;

	private bool bOpened;

	private eBattleState mState
	{
		get
		{
			return (eBattleState)PlayerPrefsEncrypt.GetInt("mainui_battle_state");
		}
		set
		{
			PlayerPrefsEncrypt.SetInt("mainui_battle_state", (int)value);
		}
	}

	protected override void OnInit()
	{
		mKeyCount = GameConfig.GetModeLevelKey();
		mStageItem.OnButtonClick = delegate
		{
			WindowUI.ShowWindow(WindowID.WindowID_StageList);
		};
		mLayerCtrl.OnLayerClick = delegate
		{
			WindowUI.ShowWindow(WindowID.WindowID_LayerBox);
		};
		Button_Harvest.onClick = delegate
		{
			if (NetManager.NetTime > 0)
			{
				WindowUI.ShowWindow(WindowID.WindowID_AdHarvest);
			}
			else
			{
				CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
			}
		};
		Button_ModeTest.onClick = delegate
		{
			GameLogic.Hold.BattleData.Challenge_UpdateMode(3001);
			WindowUI.ShowWindow(WindowID.WindowID_Battle);
		};
		Button_Change.onClick = delegate
		{
			GameLogic.Hold.BattleData.Challenge_UpdateMode(3002);
			WindowUI.ShowWindow(WindowID.WindowID_Battle);
		};
		float fringeHeight = PlatformHelper.GetFringeHeight();
		RectTransform rectTransform = Button_Mail.transform.parent.parent as RectTransform;
		Vector2 anchoredPosition = rectTransform.anchoredPosition;
		maily = anchoredPosition.y;
		RectTransform rectTransform2 = rectTransform;
		Vector2 anchoredPosition2 = rectTransform.anchoredPosition;
		rectTransform2.anchoredPosition = new Vector2(anchoredPosition2.x, maily + fringeHeight);
		RectTransform rectTransform3 = Button_Setting.transform.parent as RectTransform;
		RectTransform rectTransform4 = rectTransform3;
		Vector2 anchoredPosition3 = rectTransform3.anchoredPosition;
		rectTransform4.anchoredPosition = new Vector2(anchoredPosition3.x, maily + fringeHeight);
		float bottomHeight = PlatformHelper.GetBottomHeight();
		RectTransform rectTransform5 = Button_Start.transform.parent as RectTransform;
		RectTransform rectTransform6 = rectTransform5;
		Vector2 anchoredPosition4 = rectTransform5.anchoredPosition;
		float x = anchoredPosition4.x;
		Vector2 anchoredPosition5 = rectTransform5.anchoredPosition;
		rectTransform6.anchoredPosition = new Vector2(x, anchoredPosition5.y + bottomHeight);
		ButtonParent.anchoredPosition = new Vector2(0f, (float)GameLogic.Height * 0.23f);
		RectTransform rectTransform7 = base.transform as RectTransform;
		(battleui.transform as RectTransform).sizeDelta = rectTransform7.sizeDelta;
		(activeui.transform as RectTransform).sizeDelta = rectTransform7.sizeDelta;
		(activeui.transform as RectTransform).sizeDelta = rectTransform7.sizeDelta;
		mActiveCtrl.Init();
	}

	private void OnChangeState()
	{
		Text_Change.text = ((mState != 0) ? "主线" : "活动");
		switch (mState)
		{
		case eBattleState.eBattle:
			Text_Change.text = "活动";
			break;
		case eBattleState.eActive:
			Text_Change.text = "主线";
			break;
		}
		battleui.SetActive(mState == eBattleState.eBattle);
		activeui.SetActive(mState == eBattleState.eActive);
	}

	protected override void OnOpen()
	{
		bOpened = true;
		InitUI();
		update_harvest_show();
		mActiveCtrl.Open();
	}

	private void InitUI()
	{
        Debug.Log("@LOG MainUIPageBattleCtrl.InitUI");

        mKeyCtrl.SetValue(GameConfig.GetModeLevelKey());
		LocalSave instance = LocalSave.Instance;
		instance.OnMaxLevelUpdate = (Action)Delegate.Combine(instance.OnMaxLevelUpdate, new Action(UpdateBest));
		OnChangeState();
		UpdateLayer();
		Button_Setting.onClick = delegate
		{
			WindowUI.ShowWindow(WindowID.WindowID_Setting);
		};
		Button_Mail.onClick = delegate
		{
			WindowUI.ShowWindow(WindowID.WindowID_Mail);
		};
		Button_Start.onClick = OnClickPlay;
		CheckUnlockStage();
		update_mail();
		UpdateNet();
		update_harvest();
	}

	private void update_mail()
	{
		mMailRedCtrl.SetType(RedNodeType.eRedCount);
		mMailRedCtrl.Value = LocalSave.Instance.Mail.GetRedCount();
	}

	private void update_harvest()
	{
		mHarvestRedCtrl.SetType(RedNodeType.eWarning);
		mHarvestRedCtrl.Value = (LocalSave.Instance.mHarvest.get_can_reward() ? 1 : 0);
	}

	private void OnClickPlay()
	{
		int modeLevelKey = GameConfig.GetModeLevelKey();
        //@TODO OnClickPlay
        Debug.Log("@LOG MainUIPageBattleCtrl.OnClickPlay modeLevelKey:" + modeLevelKey);
		if (!NetManager.IsNetConnect && !LocalSave.Instance.TrustCount_Use((short)modeLevelKey))
		{
            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NeedNet);
		}
		else if (LocalSave.Instance.GetKey() < mKeyCount)
		{
            KeyBuyUICtrl.SetSource(KeyBuySource.EMAIN_BATTLE);
			WindowUI.ShowWindow(WindowID.WindowID_KeyBuy);
		}
		else
		{
            Facade.Instance.SendNotification("UseCurrencyKey");
			WindowUI.ShowMask(value: true);
			DOTween.Sequence().AppendInterval(0.4f).AppendCallback(delegate
			{
				WindowUI.ShowMask(value: false);
				PlayInternal();
			});
		}
	}

	public void PlayInternal()
	{
		GameLogic.PlayBattle_Main();
	}

	private void CheckUnlockStage()
	{
		if (LocalSave.Instance.Stage_GetFirstIn())
		{
			UpdateLayer();
			GameLogic.Hold.BattleData.Level_CurrentStage = LocalSave.Instance.Stage_GetStage();
			LocalSave.Instance.Stage_SetFirstIn();
			Facade.Instance.RegisterProxy(new UnlockStageProxy(new UnlockStageProxy.Transfer
			{
				StageID = GameLogic.Hold.BattleData.Level_CurrentStage
			}));
			WindowUI.ShowWindow(WindowID.WindowID_UnlockStage);
		}
	}

	private void UpdateLayer()
	{
		currentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
		mStageItem.Init(currentStage);
		int maxLevel = LocalSave.Instance.mStage.MaxLevel;
		int nextLevel = LocalModelManager.Instance.Box_ChapterBox.GetNextLevel(LocalSave.Instance.Stage_GetNextID());
		mLayerCtrl.SetLayer(maxLevel, nextLevel);
		UpdateBest();
	}

	private void update_harvest_show()
	{
		Button_Harvest.gameObject.SetActive(LocalSave.Instance.Card_GetHarvestAvailable());
	}

	protected override void OnClose()
	{
		bOpened = false;
		LocalSave instance = LocalSave.Instance;
		instance.OnMaxLevelUpdate = (Action)Delegate.Remove(instance.OnMaxLevelUpdate, new Action(UpdateBest));
		mActiveCtrl.Close();
	}

	public override object OnGetEvent(string eventName)
	{
		object obj = mActiveCtrl.OnGetEvent(eventName);
		if (obj != null)
		{
			return obj;
		}
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
		mActiveCtrl.OnHandleNotification(notification);
		string name = notification.Name;
		object body = notification.Body;
		if (name == null)
		{
			return;
		}
		if (!(name == "PUB_NETCONNECT_UPDATE"))
		{
			if (!(name == "MainUI_MailUpdate"))
			{
				if (!(name == "MainUI_LayerUpdate"))
				{
					if (name == "MainUI_HarvestUpdate")
					{
						update_harvest();
					}
				}
				else
				{
					UpdateLayer();
					OnLanguageChange();
				}
			}
			else
			{
				update_mail();
			}
		}
		else
		{
			UpdateNet();
		}
	}

	private void UpdateNet()
	{
		mLayerCtrl.UpdateNet();
	}

	private void UpdateBest()
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("最高得分");
		Text_BestScore.text = Utils.FormatString("{0} : {1}", languageByTID, LocalSave.Instance.GetScore());
		if (GameLogic.Hold.BattleData.Level_CurrentStage < LocalSave.Instance.mStage.CurrentStage)
		{
			Text_BestStage.text = GameLogic.Hold.Language.GetLanguageByTID("Main_PassStage");
		}
		else
		{
			string languageByTID2 = GameLogic.Hold.Language.GetLanguageByTID("最高层数");
			int currentMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(LocalSave.Instance.mStage.CurrentStage);
			int currentMaxLevel2 = LocalSave.Instance.mStage.GetCurrentMaxLevel();
			Text_BestStage.text = Utils.FormatString("{0} : {1}/{2}", languageByTID2, currentMaxLevel2, currentMaxLevel);
		}
		OnStageUpdate();
	}

	private void OnStageUpdate()
	{
		Text_ChapterIndex.text = LocalModelManager.Instance.Stage_Level_stagechapter.GetChapterFullName(GameLogic.Hold.BattleData.Level_CurrentStage);
	}

	public override void OnLanguageChange()
	{
		Text_Start.text = GameLogic.Hold.Language.GetLanguageByTID("Main_StartGame");
		UpdateBest();
		mLayerCtrl.OnLanguageChange();
		mActiveCtrl.OnLanguageChange();
	}
}
