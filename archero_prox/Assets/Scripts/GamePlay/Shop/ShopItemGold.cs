




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemGold : MonoBehaviour
{
	public Text Text_Title;

	public ButtonCtrl Button_Get;

	public Image Image_Icon;

	public Text Text_Count;

	public GoldTextCtrl mGoldCtrl;

	public Action<int, ShopItemGold> OnClickButton;

	private Shop_Shop shopdata;

	private int mIndex;

	private void Awake()
	{
		Button_Get.SetDepondNet(value: true);
		Button_Get.onClick = delegate
		{
			if (OnClickButton != null)
			{
				OnClickButton(mIndex, this);
			}
		};
	}

	public void Init(int index)
	{
		mIndex = index;
		shopdata = LocalModelManager.Instance.Shop_Shop.GetBeanById(101 + index);
		Image_Icon.sprite = SpriteManager.GetMain(Utils.FormatString("ic_coin_{0}", index + 1));
		if (LocalSave.Instance.Card_GetHarvestLevel() == 0)
		{
			Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_金币{0}", shopdata.ID));
		}
		else
		{
			Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_equipexp_reward", LocalSave.Instance.mShop.get_gold_time(index));
		}
		Text_Count.text = GetGold().ToString();
		mGoldCtrl.SetValue(shopdata.Price);
	}

	public int GetGold()
	{
		return LocalSave.Instance.mShop.get_buy_golds(mIndex);
	}

	public int GetDiamond()
	{
		return (int)shopdata.Price;
	}

	public void OnLanguageChange()
	{
		Init(mIndex);
	}

	public void UpdateNet()
	{
	}
}
