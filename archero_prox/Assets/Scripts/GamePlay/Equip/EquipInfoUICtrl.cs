




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

public class EquipInfoUICtrl : MediatorCtrlBase
{
	private const string AniMoveName = "CharEquipInfoMove";

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Upgrade;

	public Transform equipparent;

	public Text Text_Info;

	public GoldTextCtrl mGoldTextCtrl;

	public ButtonCtrl Button_Mask;

	public RectTransform bg;

	public Text Text_IsMax;

	public UILineCtrl mTitleCtrl;

	public Text Text_Quality;

	public Text Text_Upgrade;

	public RectTransform Image_OutLine;

	public GameObject attributeParent;

	public Text Text_Attribute;

	public Animator mAni;

	public GameObject WearParent;

	public ButtonCtrl Button_Wear;

	public ButtonCtrl Button_Buy;

	public Text Text_Buy;

	public Text Text_Wear;

	public GoldTextCtrl mBuyGold;

	public EquipInfoNeedCtrl mNeedCtrl;

	public List<GameObject> typeparent;

	public GameObject equipattparent;

	public Text Text_EquipInfo;

	public Text Text_AttributeTitle;

	public Text Text_MaterialTitle;

	private EquipOneCtrl _equipctrl;

	private BlackItemOnectrl _itemone;

	private Vector2 wearbuttonstartpos;

	private RectTransform mRectTransform;

	private LocalSave.EquipOne mEquipData;

	private EquipInfoModuleProxy.Transfer mTransfer;

	private LocalUnityObjctPool mPool;

	private List<EquipInfoAttributeOne> mTexts = new List<EquipInfoAttributeOne>();

	private int diamondforcoin;

	private bool bGoldBuy;

	private EquipOneCtrl mEquipCtrl
	{
		get
		{
			if (_equipctrl == null)
			{
				_equipctrl = CInstance<UIResourceCreator>.Instance.GetEquip(equipparent);
				_equipctrl.ShowAniEnable(value: false);
				_equipctrl.SetButtonEnable(value: false);
			}
			return _equipctrl;
		}
	}

	private BlackItemOnectrl itemone
	{
		get
		{
			if (_itemone == null)
			{
				_itemone = CInstance<UIResourceCreator>.Instance.GetBlackShopOne(equipparent);
				_itemone.SetCurrencyShow(value: false);
			}
			return _itemone;
		}
	}

	protected override void OnInit()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<EquipInfoAttributeOne>(Text_Attribute.gameObject);
		Text_Attribute.gameObject.SetActive(value: false);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_EquipInfo);
		};
		Button_Mask.onClick = Button_Close.onClick;
		Button_Upgrade.onClick = OnClickUpgrade;
		mRectTransform = (base.transform as RectTransform);
		Button_Wear.onClick = delegate
		{
			GameLogic.Hold.Guide.mEquip.CurrentOver(2);
			mTransfer.wearcallback();
			Button_Close.onClick();
		};
		Button_Buy.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_EquipInfo);
			if (mTransfer != null && mTransfer.buy_callback != null)
			{
				mTransfer.buy_callback(mTransfer.buy_itemone);
			}
		};
		wearbuttonstartpos = (Button_Wear.transform.parent as RectTransform).anchoredPosition;
	}

	protected override void OnOpen()
	{
		Button_Buy.transform.parent.gameObject.SetActive(value: false);
		Button_Wear.transform.parent.gameObject.SetActive(value: false);
		Button_Upgrade.transform.parent.gameObject.SetActive(value: false);
		GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
		IProxy proxy = Facade.Instance.RetrieveProxy("EquipInfoModuleProxy");
		if (proxy == null)
		{
			return;
		}
		mTransfer = (EquipInfoModuleProxy.Transfer)proxy.Data;
		if (mTransfer != null)
		{
			mEquipData = mTransfer.one;
			switch (mTransfer.type)
			{
			case EquipInfoModuleProxy.InfoType.eNormal:
				Button_Wear.transform.parent.gameObject.SetActive(value: true);
				Button_Upgrade.transform.parent.gameObject.SetActive(value: true);
				Button_Buy.transform.parent.gameObject.SetActive(value: false);
				mNeedCtrl.gameObject.SetActive(value: true);
				type_show(1, value: false);
				break;
			case EquipInfoModuleProxy.InfoType.eBuy:
				type_show(1, value: true);
				mNeedCtrl.gameObject.SetActive(value: false);
				Button_Wear.transform.parent.gameObject.SetActive(value: false);
				Button_Upgrade.transform.parent.gameObject.SetActive(value: false);
				Button_Buy.transform.parent.gameObject.SetActive(value: true);
				mBuyGold.SetCurrencyType(mTransfer.buy_itemone.mData.PriceType);
				mBuyGold.UseTextRed();
				mBuyGold.SetValue(mTransfer.buy_itemone.mData.Price);
				break;
			}
			UpdateUI();
		}
	}

	private void UpdateUI()
	{
		mRectTransform.anchoredPosition = Vector2.zero;
		equipparent.localScale = Vector3.one;
		Color color = LocalSave.QualityColors[mEquipData.Quality];
		mTitleCtrl.SetColor(color);
		mTitleCtrl.SetText(mEquipData.NameOnlyString);
		Text_Quality.color = color;
		Text_Quality.text = mEquipData.QualityString;
		Text_Info.text = mEquipData.InfoString;
		equipattparent.SetActive(value: false);
		switch (mEquipData.PropType)
		{
		case EquipType.eEquip:
			InitNormalButton();
			InitAttribute();
			switch (mTransfer.type)
			{
			case EquipInfoModuleProxy.InfoType.eNormal:
			{
				RectTransform rectTransform3 = bg;
				Vector2 sizeDelta3 = bg.sizeDelta;
				rectTransform3.sizeDelta = new Vector2(sizeDelta3.x, 1010f);
				break;
			}
			case EquipInfoModuleProxy.InfoType.eBuy:
			{
				RectTransform rectTransform2 = bg;
				Vector2 sizeDelta2 = bg.sizeDelta;
				rectTransform2.sizeDelta = new Vector2(sizeDelta2.x, 840f);
				break;
			}
			}
			equipattparent.SetActive(value: true);
			break;
		case EquipType.eMaterial:
		{
			RectTransform rectTransform = bg;
			Vector2 sizeDelta = bg.sizeDelta;
			rectTransform.sizeDelta = new Vector2(sizeDelta.x, 480f);
			break;
		}
		default:
			SdkManager.Bugly_Report("EquipInfoUICtrl", Utils.FormatString("UpdateUI. Equip:{0} PropType:{1} is not achieve!", mEquipData.EquipID, mEquipData.PropType));
			break;
		}
		update_equipinfo();
		GameLogic.Hold.Guide.mEquip.GoNext(2, Button_Wear.transform as RectTransform);
	}

	private void update_equipinfo()
	{
		mEquipCtrl.Init(mEquipData);
		mEquipCtrl.SetButtonEnable(value: false);
		mEquipCtrl.ShowLevel(value: false);
		mEquipCtrl.SetCountShow(value: false);
		switch (mEquipData.PropType)
		{
		case EquipType.eEquip:
			type_show(0, value: true);
			Text_EquipInfo.text = Utils.FormatString("{0}: {1}/{2}", GameLogic.Hold.Language.Level, mEquipData.Level, mEquipData.CurrentMaxLevel);
			break;
		case EquipType.eMaterial:
			type_show(0, value: false);
			Text_EquipInfo.text = Utils.FormatString("{0}: {1}", GameLogic.Hold.Language.Count, mEquipData.Count);
			break;
		default:
			SdkManager.Bugly_Report("EquipInfoUICtrl", Utils.FormatString("update_equipinfo. Equip:{0} PropType:{1} is not achieve!", mEquipData.EquipID, mEquipData.PropType));
			break;
		}
	}

	private void type_show(int index, bool value)
	{
		typeparent[index].SetActive(value);
	}

	private void InitAttribute()
	{
		mPool.Collect<EquipInfoAttributeOne>();
		mTexts.Clear();
		float num = 0f;
		string specialInfoString = mEquipData.SpecialInfoString;
		if (!string.IsNullOrEmpty(specialInfoString))
		{
			EquipInfoAttributeOne equipInfoAttributeOne = mPool.DeQueue<EquipInfoAttributeOne>();
			equipInfoAttributeOne.SetText(specialInfoString);
			RectTransform rectTransform = equipInfoAttributeOne.transform as RectTransform;
			rectTransform.SetParentNormal(attributeParent);
			rectTransform.anchoredPosition = new Vector2(0f, num);
			num -= equipInfoAttributeOne.GetTextHeight();
		}
		List<Goods_goods.GoodShowData> equipShowAttrs = LocalModelManager.Instance.Equip_equip.GetEquipShowAttrs(mEquipData);
		List<string> equipAttributesNext = LocalModelManager.Instance.Equip_equip.GetEquipAttributesNext(mEquipData);
		int i = 0;
		for (int count = equipShowAttrs.Count; i < count; i++)
		{
			EquipInfoAttributeOne equipInfoAttributeOne2 = mPool.DeQueue<EquipInfoAttributeOne>();
			mTexts.Add(equipInfoAttributeOne2);
			string attributeBase = GetAttributeBase(i);
			equipInfoAttributeOne2.SetText(attributeBase);
			RectTransform rectTransform2 = equipInfoAttributeOne2.transform as RectTransform;
			rectTransform2.SetParentNormal(attributeParent);
			rectTransform2.anchoredPosition = new Vector2(0f, num);
			num -= equipInfoAttributeOne2.GetTextHeight();
		}
		List<string> equipShowAddAttributes = LocalModelManager.Instance.Equip_equip.GetEquipShowAddAttributes(mEquipData);
		int j = 0;
		for (int count2 = equipShowAddAttributes.Count; j < count2; j++)
		{
			string text = equipShowAddAttributes[j].ToString();
			EquipInfoAttributeOne equipInfoAttributeOne3 = mPool.DeQueue<EquipInfoAttributeOne>();
			equipInfoAttributeOne3.SetText(equipShowAddAttributes[j]);
			RectTransform rectTransform3 = equipInfoAttributeOne3.transform as RectTransform;
			rectTransform3.SetParentNormal(attributeParent);
			rectTransform3.anchoredPosition = new Vector2(0f, num);
			num -= equipInfoAttributeOne3.GetTextHeight();
		}
	}

	private string GetAttributeBase(int index)
	{
		List<Goods_goods.GoodShowData> equipShowAttrs = LocalModelManager.Instance.Equip_equip.GetEquipShowAttrs(mEquipData);
		List<string> equipAttributesNext = LocalModelManager.Instance.Equip_equip.GetEquipAttributesNext(mEquipData);
		string text = equipShowAttrs[index].ToString();
		if (!mEquipData.IsMax && mEquipData.CountEnough)
		{
			text = Utils.FormatString("{0} (<color=#00ff00ff>{1}</color>)", text, equipAttributesNext[index]);
		}
		return text;
	}

	private void InitNormalButton()
	{
		if (mTransfer != null && mTransfer.type == EquipInfoModuleProxy.InfoType.eNormal)
		{
			if (mEquipData.IsWear)
			{
				Text_Wear.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_ButtonUnwear");
			}
			else
			{
				Text_Wear.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_ButtonWear");
			}
			mGoldTextCtrl.SetCurrencyType(CurrencyType.Gold);
			mGoldTextCtrl.UseTextRed();
			mGoldTextCtrl.SetValue(mEquipData.NeedGold);
			if (mEquipData.IsMax)
			{
				Button_Upgrade.gameObject.SetActive(value: false);
				Text_IsMax.gameObject.SetActive(value: true);
				mNeedCtrl.gameObject.SetActive(value: false);
				(Button_Wear.transform.parent as RectTransform).anchoredPosition = new Vector2(0f, wearbuttonstartpos.y);
			}
			else
			{
				(Button_Wear.transform.parent as RectTransform).anchoredPosition = wearbuttonstartpos;
				Button_Upgrade.gameObject.SetActive(value: true);
				mNeedCtrl.gameObject.SetActive(value: true);
				UpdateButtonUpgrade();
				Text_IsMax.gameObject.SetActive(value: false);
			}
			mNeedCtrl.Init(mEquipData);
		}
	}

	protected override void OnClose()
	{
	}

	private void OnGoldBuyCallback(int diamond)
	{
		diamondforcoin = diamond;
		OnClickUpgrade();
	}

	private void OnClickUpgrade()
	{
		if (!mEquipData.CountEnough)
		{
			CInstance<TipsUIManager>.Instance.Show(ETips.Tips_EquipMaterialNotEnough);
			return;
		}
		if (!mEquipData.GoldEnough)
		{
			long num = mEquipData.NeedGold - LocalSave.Instance.GetGold();
			PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EEQUIP_UPGRADE);
			WindowUI.ShowGoldBuy(CoinExchangeSource.EEQUIP_UPGRADE, num, OnGoldBuyCallback);
			diamondforcoin = (int)Formula.GetNeedDiamond(num);
			return;
		}
		if (mEquipData.RowID == 0)
		{
			CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
			SdkManager.Bugly_Report("EquipInfoUICtrl", "equipdata.RowID = 0");
			return;
		}
		CItemUpgarde itemUpgrade = new CItemUpgarde();
		itemUpgrade.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
		itemUpgrade.m_nRowID = mEquipData.RowID;
		itemUpgrade.m_nCoins = (uint)mEquipData.NeedGold;
		itemUpgrade.arrayItems = new CMaterialItem[1];
		itemUpgrade.arrayItems[0] = new CMaterialItem();
		itemUpgrade.arrayItems[0].m_nEquipID = (uint)mEquipData.NeedMatID;
		itemUpgrade.arrayItems[0].m_nMaterial = (uint)mEquipData.NeedMatCount;
		NetManager.SendInternal(itemUpgrade, SendType.eForceOnce, delegate(NetResponse response)
		{
#if ENABLE_NET_MANAGER
			if (response.IsSuccess)
#endif
			{
				LocalSave.Instance.Modify_Gold(-mEquipData.NeedGold);
				LocalSave.EquipOne propByID = LocalSave.Instance.GetPropByID(mEquipData.NeedMatID);
				if (propByID != null)
				{
					propByID.Count -= mEquipData.NeedMatCount;
				}
				LocalSave.Instance.EquipLevelUp(mEquipData);
				if (mTransfer.updatecallback != null)
				{
					mTransfer.updatecallback();
				}
				InitNormalButton();
				PlayLevelUp();
				SdkManager.send_event_equipment("UPGRADE", mEquipData.EquipID, 0, mEquipData.Level, EquipSource.EEquip_page, (int)itemUpgrade.m_nCoins);
				bGoldBuy = false;
				diamondforcoin = 0;
			}
#if ENABLE_NET_MANAGER
			else if (response.error != null)
			{
				if (response.error.m_nStatusCode == 8)
				{
					CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 1);
					SdkManager.Bugly_Report("EquipInfoUICtrl", " gold not enough");
				}
				else if (response.error.m_nStatusCode == 10)
				{
					CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode);
					SdkManager.Bugly_Report("EquipInfoUICtrl", " scroll not enough");
				}
			}
			else
			{
				CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
				SdkManager.Bugly_Report("EquipInfoUICtrl", " error == null");
			}
#endif
		});
	}

	private void PlayLevelUp()
	{
		float duration = 0.24f;
		float duration2 = 0.07f;
		float num = 0.3f;
		int attributeAllCount = LocalModelManager.Instance.Equip_equip.GetAttributeAllCount(mEquipData.EquipID);
		Sequence s = DOTween.Sequence();
		s.Append(equipparent.DOScale(1.3f, duration).SetEase(Ease.OutQuad));
		s.AppendCallback(delegate
		{
			mAni.Play("CharEquipInfoMove");
			update_equipinfo();
		});
		s.Append(equipparent.DOScale(1f, duration2).SetEase(Ease.OutQuad));
		for (int i = 0; i < attributeAllCount; i++)
		{
			int index = i;
			if (index < mTexts.Count)
			{
				s.AppendInterval(0.2f);
				s.Append(mTexts[index].transform.DOScale(1.3f, duration).SetEase(Ease.OutQuad));
				s.AppendCallback(delegate
				{
					if (index < mTexts.Count)
					{
						mTexts[index].SetText(GetAttributeBase(index));
						mAni.Play("CharEquipInfoMove");
					}
				});
				s.Append(mTexts[index].transform.DOScale(1f, duration2).SetEase(Ease.OutQuad));
			}
		}
	}

	public override void OnHandleNotification(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
		if (name == null)
		{
			return;
		}
		if (!(name == "PUB_NETCONNECT_UPDATE"))
		{
			if (name == "PUB_UI_UPDATE_CURRENCY")
			{
				InitNormalButton();
				InitAttribute();
			}
		}
		else
		{
			UpdateNet();
		}
	}

	private void UpdateNet()
	{
		UpdateButtonUpgrade();
	}

	private void UpdateButtonUpgrade()
	{
		bool flag =
            NetManager.IsNetConnect && mEquipData.CountEnough;
		Button_Upgrade.SetGray(flag);
		if (flag)
		{
			mGoldTextCtrl.SetTextRed(!mEquipData.GoldEnough);
		}
		else
		{
			mGoldTextCtrl.SetTextRed(red: false);
		}
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnLanguageChange()
	{
		Text_Upgrade.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Upgrade");
		Text_Buy.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Buy");
		Text_AttributeTitle.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_AttributeTitle");
		Text_MaterialTitle.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_MaterialTitle");
	}
}
