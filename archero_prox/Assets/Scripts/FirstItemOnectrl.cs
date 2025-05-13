




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class FirstItemOnectrl : MonoBehaviour
{
	public ButtonGoldCtrl Button_Buy;

	public GameObject alreadybuy;

	public Image Image_Icon;

	public Text Text_Content;

	public Text Text_Value;

	public Action<FirstItemOnectrl> OnClickButton;

	public Shop_ReadyShop mData;

	private bool bBuy;

	public int mIndex
	{
		get;
		private set;
	}

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
		Text_Value.gameObject.SetActive(value: false);
	}

	public void Init(int index, Shop_ReadyShop data, bool buy)
	{
		mIndex = index;
		bBuy = buy;
		mData = data;
		SetBuy(bBuy);
		Text_Value.text = mData.ProductId.ToString();
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("药水效果描述{0}", mData.ProductId));
		Image_Icon.sprite = SpriteManager.GetBattle(Utils.FormatString("ShopStart_{0}", mData.ProductId));
	}

	public void SetBuy(bool buy)
	{
		if (buy)
		{
			alreadybuy.SetActive(value: true);
			Button_Buy.gameObject.SetActive(value: false);
			return;
		}
		Button_Buy.gameObject.SetActive(value: true);
		Button_Buy.SetCurrency(mData.PriceType);
		Button_Buy.SetGold(mData.Price);
		alreadybuy.SetActive(value: false);
	}

	public void Buy()
	{
		bBuy = true;
		SetBuy(bBuy);
		GameLogic.Hold.BattleData.SetFirstShopBuy(mIndex);
		Shop_item beanById = LocalModelManager.Instance.Shop_item.GetBeanById(mData.ProductId);
		switch (beanById.EffectType)
		{
		case 1:
			GetOneItem(beanById);
			break;
		case 2:
		{
			WeightRandom weightRandom = new WeightRandom();
			int i = 0;
			for (int num = beanById.EffectArgs.Length; i < num; i++)
			{
				string[] array = beanById.EffectArgs[i].Split(',');
				int result = 0;
				int.TryParse(array[0], out result);
				int result2 = 0;
				int.TryParse(array[1], out result2);
				weightRandom.Add(result2, result);
			}
			int random = weightRandom.GetRandom();
			if (random > 0)
			{
				beanById = LocalModelManager.Instance.Shop_item.GetBeanById(random);
				GetOneItem(beanById);
			}
			break;
		}
		}
	}

	private void GetOneItem(Shop_item item)
	{
		GetOnePotion(item);
		LocalSave.Instance.BattleIn_UpdatePotions(item.ItemID);
		CInstance<TipsUIManager>.Instance.ShowPotion(item.ItemID, item.Quality);
	}

	public static void GetOnePotion(Shop_item item)
	{
		int num = item.EffectArgs.Length;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameLogic.Self.m_EntityData.ExcuteAttributes(item.EffectArgs[i]);
			}
		}
	}
}
