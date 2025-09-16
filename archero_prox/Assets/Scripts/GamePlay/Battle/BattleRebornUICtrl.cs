




using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class BattleRebornUICtrl : MediatorCtrlBase
{
	public Text Text_Content;

	public Text Text_Time;

	public Text Text_Count;

	public GoldTextCtrl mDiamondCtrl;

	public Text Text_Free;

	public Text Text_FreeCount;

	public GameObject FreeParent;

	public ButtonCtrl Button_Buy;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	private const int Max_Second = 5;

	private bool bFree;

	private long needdiamond;

	private bool bStart;

	private float starttime;

	private int second;

	private bool bDealed;

	private Sequence seq;

	protected override void OnInit()
	{
		Button_Close.onClick = CloseWindow;
		Button_Shadow.onClick = Button_Close.onClick;
		Button_Buy.onClick = delegate
		{
			long diamond = LocalSave.Instance.GetDiamond();
			if (diamond < needdiamond && !bFree)
			{
				bStart = false;
				second = 5;
				starttime = 5f;
				Update();
				PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EREBORN);
				WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond, OnDiamondShopClose);
			}
			else
			{
				bStart = false;
				NetManager.SendInternal(new CLifeTransPacket
				{
					m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
					m_nType = 4,
					m_nMaterial = (ushort)needdiamond
				}, SendType.eForceOnce, delegate(NetResponse response)
				{
#if ENABLE_NET_MANAGER
                    if (response.IsSuccess)
#endif
					{
						if (!bFree)
						{
							LocalSave.Instance.Modify_Diamond(-needdiamond);
						}
						if (LocalSave.Instance.GetRebornCount() > 0)
						{
							LocalSave.Instance.Modify_RebornCount(-1);
						}
						DoReborn();
					}
#if ENABLE_NET_MANAGER
					else
					{
						bStart = true;
						if (response.error != null)
						{
							CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
						}
					}
#endif
                });
			}
		};
	}

	protected override void OnOpen()
	{
		WindowUI.ShowCurrency(WindowID.WindowID_Currency);
		GameLogic.SetPause(pause: true);
		bDealed = false;
		InitUI();
	}

	private void InitUI()
	{
		if ((bool)Button_Shadow)
		{
			Button_Shadow.gameObject.SetActive(value: false);
		}
		KillSequence();
		seq = DOTween.Sequence().AppendInterval(0.5f).AppendCallback(delegate
		{
			if ((bool)Button_Shadow)
			{
				Button_Shadow.gameObject.SetActive(value: true);
			}
		})
			.SetUpdate(isIndependentUpdate: true);
		starttime = 5f;
		bStart = true;
		needdiamond = GameConfig.GetRebornDiamond();
		GameLogic.Hold.Sound.PlayUI(SoundUIType.eRebornSecond);
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("rebornui_count");
		Text_Count.text = Utils.FormatString("{0}:{1}", languageByTID, GameLogic.Hold.BattleData.GetRebornCount().ToString());
		Text_Time.text = 5.ToString();
		second = 5;
		mDiamondCtrl.SetCurrencyType(CurrencyType.Diamond);
		mDiamondCtrl.UseTextRed();
		mDiamondCtrl.SetValue((int)needdiamond);
		UpdateButton();
	}

	private void UpdateButton()
	{
		int rebornCount = LocalSave.Instance.GetRebornCount();
		bFree = (rebornCount > 0);
		mDiamondCtrl.gameObject.SetActive(!bFree);
		FreeParent.SetActive(bFree);
		Text_FreeCount.text = Utils.FormatString("x{0}", rebornCount);
	}

	private void CloseWindow()
	{
		if (!bDealed)
		{
			bDealed = true;
			CloseWindowInternal();
			GameLogic.Self.Reborn_DeadEnd();
			SdkManager.send_event_revival("NO", GameLogic.Hold.BattleData.Level_CurrentStage, GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID(), 0);
		}
	}

	private void DoReborn()
	{
		if (!bDealed)
		{
			LocalSave.Instance.BattleIn_AddRebornUI();
			bDealed = true;
			CloseWindowInternal();
			GameLogic.Self.DoReborn();
			SdkManager.send_event_revival("YES", GameLogic.Hold.BattleData.Level_CurrentStage, GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID(), (int)needdiamond);
		}
	}

	private void CloseWindowInternal()
	{
		WindowUI.CloseWindow(WindowID.WindowID_BattleReborn);
		bStart = false;
	}

	private void OnDiamondShopClose()
	{
		bStart = true;
	}

	private void Update()
	{
		if (!bStart)
		{
			return;
		}
		starttime -= Updater.deltaIgnoreTime;
		if (second != MathDxx.CeilToInt(starttime))
		{
			second = MathDxx.CeilToInt(starttime);
			if (second >= 0)
			{
				GameLogic.Hold.Sound.PlayUI(SoundUIType.eRebornSecond);
			}
			Text_Time.text = second.ToString();
		}
		if (second < 0)
		{
			bStart = false;
			CloseWindow();
		}
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	protected override void OnClose()
	{
		KillSequence();
		WindowUI.CloseCurrency();
		GameLogic.SetPause(pause: false);
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
		if (name != null && name == "PUB_UI_UPDATE_CURRENCY")
		{
			mDiamondCtrl.UseTextRed();
		}
	}

	public override void OnLanguageChange()
	{
		Text_Free.text = GameLogic.Hold.Language.GetLanguageByTID("BoxChoose_Free");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("rebornui_title");
	}
}
