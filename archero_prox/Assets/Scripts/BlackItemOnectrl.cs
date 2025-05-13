




using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BlackItemOnectrl : MonoBehaviour
{
	public ButtonCtrl Button_Buy;

	public GameObject equipparent;

	public Image Image_Buy;

	public GoldTextCtrl mGoldCtrl;

	public Text Text_Name;

	public Text Text_Sold;

	public GameObject buyparent;

	public GameObject notbuyparent;

	public Action<BlackItemOnectrl> OnClickButton;

	public Shop_MysticShop mData;

	private PropOneEquip mItem;

	private Equip_equip equipdata;

	private LocalSave.EquipOne mEquipOne = new LocalSave.EquipOne();

	public int mIndex;

	private bool bBuy;

	private void Awake()
	{
		Button_Buy.onClick = delegate
		{
			if (bBuy)
			{
				CInstance<TipsUIManager>.Instance.Show(ETips.Tips_AlreadyBuy);
			}
			else if (OnClickButton != null)
			{
				OnClickButton(this);
			}
		};
	}

	public void Init(int index, Shop_MysticShop data)
	{
		if (mItem == null)
		{
			mItem = CInstance<UIResourceCreator>.Instance.GetPropOneEquip(equipparent.transform);
			mItem.SetButtonEnable(value: false);
		}
		Drop_DropModel.DropData dropData = new Drop_DropModel.DropData();
		dropData.type = PropType.eEquip;
		dropData.id = data.ProductId;
		dropData.count = data.ProductNum;
		mEquipOne.Clear();
		mEquipOne.EquipID = data.ProductId;
		mItem.InitProp(dropData);
		mIndex = index;
		bBuy = false;
		mData = data;
		equipdata = LocalModelManager.Instance.Equip_equip.GetBeanById(mData.ProductId);
		if (equipdata == null)
		{
			SdkManager.Bugly_Report("BlackItemOneCtrl", Utils.FormatString("Init Equip_equip:{0} is null", data.ID));
			return;
		}
		SetBuy(bBuy);
		mGoldCtrl.UseTextRed();
		mGoldCtrl.SetCurrencyType(data.PriceType);
		mGoldCtrl.SetValue(data.Price);
		Text_Sold.text = GameLogic.Hold.Language.GetLanguageByTID("blackshop_sold");
		Image_Buy.transform.localScale = Vector3.one;
		Text_Name.text = mEquipOne.NameOnlyString;
		Text_Name.color = mEquipOne.qualityColor;
	}

	private void SetBuy(bool buy)
	{
		if (buyparent != null)
		{
			buyparent.SetActive(buy);
		}
		if (notbuyparent != null)
		{
			notbuyparent.SetActive(!buy);
		}
		if (buy && Image_Buy != null)
		{
			Image_Buy.transform.localScale = Vector3.zero;
			Image_Buy.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetUpdate(isIndependentUpdate: true);
		}
	}

	public void SetCurrencyShow(bool value)
	{
		mGoldCtrl.gameObject.SetActive(value);
	}

	public void UpdateCurrency()
	{
		mGoldCtrl.SetValue(mData.Price);
	}

	public void Buy()
	{
		bBuy = true;
		SetBuy(bBuy);
	}
}
