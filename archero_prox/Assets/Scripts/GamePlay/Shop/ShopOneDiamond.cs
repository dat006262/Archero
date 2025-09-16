




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopOneDiamond : ShopOneBase
{
	public const float itemwidth = 235f;

	public const float itemheight = 360f;

	public Text Text_Title;

	public Text Text_NotReady;

	public GameObject diamondparent;

	private List<ShopItemDiamond> mList = new List<ShopItemDiamond>();

	private GameObject _itemgdiamond;

	private LocalUnityObjctPool mPool;

	private GameObject itemgdiamond
	{
		get
		{
			if (_itemgdiamond == null)
			{
				_itemgdiamond = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/ShopUI/ShopItemDiamondOne"));
				_itemgdiamond.SetParentNormal(diamondparent);
			}
			return _itemgdiamond;
		}
	}

	protected override void OnAwake()
	{
		if (mPool == null)
		{
			mPool = LocalUnityObjctPool.Create(base.gameObject);
			mPool.CreateCache<ShopItemDiamond>(itemgdiamond);
			itemgdiamond.SetActive(value: false);
		}
	}

	protected override void OnInit()
	{
		mPool.Collect<ShopItemDiamond>();
		mList.Clear();
		Text_NotReady.gameObject.SetActive(value: false);
		int num = 6;
		int num2 = 3;
		float num3 = (float)(num2 - 1) * 235f;
		for (int i = 0; i < num; i++)
		{
			ShopItemDiamond shopItemDiamond = mPool.DeQueue<ShopItemDiamond>();
			shopItemDiamond.gameObject.SetParentNormal(diamondparent);
			RectTransform rectTransform = shopItemDiamond.transform as RectTransform;
			rectTransform.anchoredPosition = new Vector2((0f - num3) / 2f + 235f * (float)(i % num2), (float)(i / num2) * -360f);
			shopItemDiamond.Init(i);
			mList.Add(shopItemDiamond);
		}
	}

	protected override void OnDeinit()
	{
	}

	private void OnClickDiamond(string productID)
	{
	}

	public override void OnLanguageChange()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_钻石标题"));
		Text_NotReady.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_正在准备商品"));
		for (int i = 0; i < mList.Count; i++)
		{
			mList[i].Init(i);
		}
	}

	public override void UpdateNet()
	{
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			mList[i].UpdateNet();
		}
	}
}
