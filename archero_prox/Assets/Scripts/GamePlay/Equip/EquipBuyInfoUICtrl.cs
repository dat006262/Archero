




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EquipBuyInfoUICtrl : MediatorCtrlBase
{
	private const string AniMoveName = "CharEquipInfoMove";

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Buy;

	public Transform equipparent;

	public Text Text_Info;

	public GoldTextCtrl mGoldTextCtrl;

	public ButtonCtrl Button_Mask;

	public Text Text_SkillName;

	public Text Text_Buy;

	public GameObject attributeParent;

	public Text Text_Attribute;

	private BlackItemOnectrl _itemone;

	private EquipBuyInfoProxy.Transfer mTransfer;

	private LocalUnityObjctPool mPool;

	private LocalSave.EquipOne mEquipData;

	private List<Text> mTexts = new List<Text>();

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
		mPool.CreateCache<Text>(Text_Attribute.gameObject);
		Text_Attribute.gameObject.SetActive(value: false);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_EquipBuyInfo);
		};
		Button_Mask.onClick = Button_Close.onClick;
		Button_Buy.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_EquipBuyInfo);
			if (mTransfer != null && mTransfer.callback != null)
			{
				mTransfer.callback(mTransfer.itemone);
			}
		};
	}

	protected override void OnOpen()
	{
		GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
		IProxy proxy = Facade.Instance.RetrieveProxy("EquipBuyInfoProxy");
		if (proxy != null)
		{
			mTransfer = (EquipBuyInfoProxy.Transfer)proxy.Data;
			if (mTransfer != null)
			{
				mEquipData = new LocalSave.EquipOne
				{
					EquipID = mTransfer.itemone.mData.ProductId,
					Level = 1,
					Count = 0,
					bNew = false,
					WearIndex = -1
				};
				itemone.Init(0, mTransfer.itemone.mData);
				itemone.Button_Buy.enabled = false;
				UpdateUI();
			}
		}
	}

	private void UpdateUI()
	{
		equipparent.localScale = Vector3.one;
		Text_SkillName.text = mEquipData.NameString;
		Text_Info.text = mEquipData.InfoString;
		InitButton();
		InitAttribute();
	}

	private void InitAttribute()
	{
		mPool.Collect<Text>();
		mTexts.Clear();
		float num = 0f;
		List<Goods_goods.GoodShowData> equipShowAttrs = LocalModelManager.Instance.Equip_equip.GetEquipShowAttrs(mEquipData);
		List<string> equipAttributesNext = LocalModelManager.Instance.Equip_equip.GetEquipAttributesNext(mEquipData);
		int i = 0;
		for (int count = equipShowAttrs.Count; i < count; i++)
		{
			string text = equipShowAttrs[i].ToString();
			Text text2 = mPool.DeQueue<Text>();
			mTexts.Add(text2);
			text2.text = text;
			RectTransform rectTransform = text2.transform as RectTransform;
			rectTransform.SetParentNormal(attributeParent);
			rectTransform.anchoredPosition = new Vector2(0f, num);
			num -= text2.preferredHeight;
		}
		List<string> equipShowAddAttributes = LocalModelManager.Instance.Equip_equip.GetEquipShowAddAttributes(mEquipData);
		int j = 0;
		for (int count2 = equipShowAddAttributes.Count; j < count2; j++)
		{
			Text text3 = mPool.DeQueue<Text>();
			mTexts.Add(text3);
			text3.text = equipShowAddAttributes[j];
			RectTransform rectTransform2 = text3.transform as RectTransform;
			rectTransform2.SetParentNormal(attributeParent);
			rectTransform2.anchoredPosition = new Vector2(0f, num);
			num -= text3.preferredHeight;
		}
	}

	private void InitButton()
	{
		mGoldTextCtrl.SetCurrencyType(mTransfer.itemone.mData.PriceType);
		mGoldTextCtrl.UseTextRed();
		mGoldTextCtrl.SetValue(mTransfer.itemone.mData.Price);
	}

	protected override void OnClose()
	{
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
				InitButton();
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
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnLanguageChange()
	{
		Text_Buy.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Buy");
	}
}
