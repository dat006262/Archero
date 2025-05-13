




using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class CardUICtrl : MediatorCtrlBase
{
	private const int LineCount = 3;

	private const int CardWidth = 210;

	private const int CardHeight = 210;

	public GameObject window;

	public Text Text_Title;

	public Text Text_Content;

	public Text Text_Count;

	public ButtonGoldCtrl Button_Upgrade;

	public ButtonCtrl Button_BG;

	public RectTransform cardstitle;

	public Transform cardparent;

	public RectTransform randomobj;

	public CardInfoCtrl mInfoCtrl;

	public CardUpgradeCtrl mUpgradeCtrl;

	private List<LocalSave.CardOne> cards;

	private List<CardOneCtrl> mCardList = new List<CardOneCtrl>();

	private LocalUnityObjctPool mPool;

	private Sequence s;

	private Sequence s_random;

	private int gold;

	private GameObject _carditem;

	private float cardparenty;

	private bool bInitOver;

	private const int SpeedDownCount = 20;

	private int lastrandomindex = -1;

	private int currentcount;

	private int currentrandomid = -1;

	private AnimationCurve curve;

	private LocalSave.CardOne randomcard;

	private bool bOpened;

	private GameObject carditem
	{
		get
		{
			if (_carditem == null)
			{
				GameObject gameObject = _carditem = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/CardUI/CardOne"));
				gameObject.SetParentNormal(base.transform);
				gameObject.SetActive(value: false);
			}
			return _carditem;
		}
	}

	protected override void OnInit()
	{
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(100022);
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<CardOneCtrl>(carditem);
		Button_Upgrade.onClick = OnClickUpgrade;
		Button_BG.onClick = OnClickBG;
		Vector3 localPosition = cardparent.localPosition;
		cardparenty = localPosition.y;
		window.SetActive(value: false);
	}

	protected override void OnOpen()
	{
		bOpened = true;
		if (!window.activeSelf)
		{
			window.SetActive(value: true);
		}
		float fringeHeight = PlatformHelper.GetFringeHeight();
		cardstitle.localPosition = new Vector3(0f, fringeHeight, 0f);
		cardparent.localPosition = new Vector3(0f, cardparenty + fringeHeight, 0f);
		InitUI();
	}

	protected override void OnClose()
	{
		mPool.Collect<CardOneCtrl>();
		OnClickBG();
		if (s != null)
		{
			s.Kill();
		}
		if (s_random != null)
		{
			s_random.Kill();
			s_random = null;
		}
		bOpened = false;
	}

	public override object OnGetEvent(string eventName)
	{
		return base.OnGetEvent(eventName);
	}

	public override void OnHandleNotification(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
		if (name != null && name == "PUB_UI_UPDATE_CURRENCY")
		{
			UpdateButton();
		}
	}

	private void InitUI()
	{
		GameLogic.Hold.Guide.mCard.GoNext(1, Button_Upgrade.transform as RectTransform);
		OnClickBG();
		bInitOver = false;
		ResetRandom();
		randomcard = null;
		randomobj.gameObject.SetActive(value: false);
		mCardList.Clear();
		UpdateButton();
		mPool.Collect<CardOneCtrl>();
		cards = LocalSave.Instance.GetCardsList();
		cards.Sort(Sort);
		int count = cards.Count;
		count = MathDxx.Clamp(count, 0, 16);
		s.Kill();
		s = DOTween.Sequence();
		for (int i = 0; i < count; i++)
		{
			int index = i;
			s.AppendCallback(delegate
			{
				UpdateOne(index);
			});
			s.AppendInterval(0.02f);
		}
		s.AppendCallback(delegate
		{
			bInitOver = true;
		});
	}

	private int Sort(LocalSave.CardOne a, LocalSave.CardOne b)
	{
		if (a.CardID < b.CardID)
		{
			return -1;
		}
		return 1;
	}

	private void UpdateOne(int index)
	{
		CardOneCtrl cardOneCtrl = mPool.DeQueue<CardOneCtrl>();
		LocalSave.CardOne carddata = cards[index];
		cardOneCtrl.InitCard(carddata);
		cardOneCtrl.SetButtonEnable(value: true);
		cardOneCtrl.Event_Click = OnClickCard;
		cardOneCtrl.gameObject.SetParentNormal(cardparent);
		RectTransform rectTransform = cardOneCtrl.transform as RectTransform;
		float num = 420f;
		float x = (float)(index % 3 * 210) - num / 2f;
		float y = (float)(-index / 3 * 210) - 105f + 420f;
		rectTransform.anchoredPosition = new Vector2(x, y);
		mCardList.Add(cardOneCtrl);
	}

	private void UpdateButton()
	{
		mUpgradeCtrl.UpdateUpgrade();
		gold = LocalSave.Instance.Card_GetRandomGold();
	}

	private int GetCardIndex(LocalSave.CardOne one)
	{
		int i = 0;
		for (int count = cards.Count; i < count; i++)
		{
			if (cards[i].CardID == one.CardID)
			{
				return i;
			}
		}
		SdkManager.Bugly_Report("CardUICtrl", Utils.FormatString("GetCardIndex[{0}] is not found.", one.CardID));
		return 0;
	}

	private void OnClickBG()
	{
		mInfoCtrl.Show(value: false);
	}

	private void OnClickCard(CardOneCtrl one)
	{
		mInfoCtrl.Show(value: true);
		mInfoCtrl.Init(one);
	}

	private void OnGoldBuyCallback(int diamond)
	{
		OnClickUpgrade();
	}

	private void OnClickUpgrade()
	{
		OnClickBG();
		if (bInitOver)
		{
			long num = LocalSave.Instance.GetGold() - gold;
			if (num < 0)
			{
				PurchaseManager.Instance.SetOpenSource(ShopOpenSource.ETALENT);
				WindowUI.ShowGoldBuy(CoinExchangeSource.ETALENT, -num, OnGoldBuyCallback);
				return;
			}
			GameLogic.Hold.Guide.mCard.CurrentOver(1);
			WindowUI.ShowMask(value: true);
			Drop_DropModel.DropData drop = LocalSave.Instance.Card_GetRandomOnly();
			currentrandomid = drop.id;
			CReqObtainTreasure cReqObtainTreasure = new CReqObtainTreasure();
			cReqObtainTreasure.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
			cReqObtainTreasure.m_nCoin = (uint)gold;
			cReqObtainTreasure.m_stTreasureItems = new CEquipmentItem();
			cReqObtainTreasure.m_stTreasureItems.m_nUniqueID = Utils.GenerateUUID();
			cReqObtainTreasure.m_stTreasureItems.m_nEquipID = (uint)drop.id;
			cReqObtainTreasure.m_stTreasureItems.m_nLevel = 1u;
			cReqObtainTreasure.m_stTreasureItems.m_nFragment = 1u;
			NetManager.SendInternal(cReqObtainTreasure, SendType.eForceOnce, delegate(NetResponse response)
			{
#if ENABLE_NET_MANAGER
				if (response.IsSuccess)
#endif
				{
					LocalSave.Instance.Modify_Gold(-gold);
					LocalSave.CardOne cardOne = LocalSave.Instance.Card_ReceiveCard(drop);
					randomcard = LocalSave.Instance.GetCardByID(currentrandomid);
					StartPlayRandom();
					LocalSave.Instance.GetCardSucceed();
					int target_level = LocalSave.Instance.Card_GetRandomCount();
					SdkManager.send_event_talent("UPGRADE", randomcard.CardID, target_level, gold);
				}
#if ENABLE_NET_MANAGER
				else
				{
					if (response.error != null)
					{
						CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 1);
					}
					WindowUI.ShowMask(value: false);
				}
#endif
			});
		}
	}

	private void ResetRandom()
	{
		currentcount = 0;
		lastrandomindex = -1;
	}

	private void StartPlayRandom()
	{
		randomobj.gameObject.SetActive(value: true);
		randomobj.SetAsLastSibling();
		SetRandomPosition();
		PlayRandom();
	}

	private void PlayRandom()
	{
		s_random = DOTween.Sequence();
		float num = 0f;
		int num2 = 8;
		if (currentcount < 20 - num2)
		{
			num = 0.07f;
		}
		else
		{
			num = curve.Evaluate((float)(currentcount - (20 - num2)) / (float)num2) * 0.2f + 0.07f;
			num = MathDxx.Clamp(num, 0.07f, num);
		}
		s_random.AppendInterval(num);
		s_random.AppendCallback(delegate
		{
			currentcount++;
			if (currentcount < 20)
			{
				SetRandomPosition();
				PlayRandom();
			}
			else
			{
				CardUICtrl cardUICtrl = this;
				ResetRandom();
				randomobj.anchoredPosition = (mCardList[GetCardIndex(randomcard)].transform as RectTransform).anchoredPosition;
				int index = 0;
				s_random = DOTween.Sequence();
				s_random.AppendInterval(0.2f).OnStepComplete(delegate
				{
					index++;
					cardUICtrl.randomobj.gameObject.SetActive(!cardUICtrl.randomobj.gameObject.activeSelf);
					if (index == 6)
					{
						cardUICtrl.s_random = DOTween.Sequence();
						cardUICtrl.s_random.AppendInterval(0.5f).AppendCallback(delegate
						{
							cardUICtrl.randomobj.gameObject.SetActive(value: false);
							cardUICtrl.mCardList[cardUICtrl.GetCardIndex(cardUICtrl.randomcard)].InitCard(cardUICtrl.randomcard);
							CardLevelUpProxy proxy = new CardLevelUpProxy(cardUICtrl.randomcard)
							{
								Event_Para0 = cardUICtrl.UpdateButton
							};
							Facade.Instance.RegisterProxy(proxy);
							WindowUI.ShowWindow(WindowID.WindowID_CardLevelUp);
							WindowUI.ShowMask(value: false);
						});
					}
				}).SetLoops(6);
			}
		});
	}

	private void SetRandomPosition()
	{
		int num;
		for (num = GameLogic.Random(0, cards.Count); num == lastrandomindex; num = GameLogic.Random(0, cards.Count))
		{
		}
		randomobj.anchoredPosition = (mCardList[num].transform as RectTransform).anchoredPosition;
		GameLogic.Hold.Sound.PlayUI(1000005);
	}

	public override void OnLanguageChange()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Title");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Content");
		mUpgradeCtrl.OnLanguageChange();
	}
}
