




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopOneDiamondBox : ShopOneBase
{
	public Text Text_Title;

	public List<ShopItemDiamondBoxBase> mList;

	protected override void OnInit()
	{
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			mList[i].Init(i);
		}
		OnLanguageChange();
	}

	public override void OnLanguageChange()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_宝箱标题"));
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			mList[i].LanguageChange();
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

	protected override void OnDeinit()
	{
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			mList[i].Deinit();
		}
	}
}
