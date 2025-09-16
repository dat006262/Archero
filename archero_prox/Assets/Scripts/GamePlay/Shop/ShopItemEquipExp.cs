




using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemEquipExp : MonoBehaviour
{
	public static Dictionary<int, int> mRewards = new Dictionary<int, int>
	{
		{
			0,
			4
		},
		{
			1,
			8
		},
		{
			2,
			16
		}
	};

	public Text Text_Title;

	public ButtonCtrl Button_Get;

	public Transform itemparent;

	public GoldTextCtrl mGoldCtrl;

	public Action<int, ShopItemEquipExp> OnClickButton;

	private EquipOneCtrl mEquipItem;

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
		if (mEquipItem == null)
		{
			mEquipItem = CInstance<UIResourceCreator>.Instance.GetEquip(itemparent);
		}
		int equipID = LocalModelManager.Instance.Equip_equip.RandomEquipExp();
		int count = GameLogic.Random(5, 15);
		LocalSave.EquipOne equipOne = new LocalSave.EquipOne();
		equipOne.EquipID = equipID;
		equipOne.Count = count;
		mEquipItem.Init(equipOne);
		mIndex = index;
		shopdata = LocalModelManager.Instance.Shop_Shop.GetBeanById(101 + index);
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_equipexp_reward", mRewards[mIndex]);
		mGoldCtrl.SetValue(shopdata.Price);
	}

	public int GetGold()
	{
		return shopdata.ProductNum;
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
