




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class ShopOneStageDiscountOneCtrl : MonoBehaviour
{
	public Text Text_Count;

	public Image Image_Icon;

	public Text Text_Content;

	private int type;

	private int count;

	public void Init(int type, int count)
	{
		this.type = type;
		this.count = count;
		Image_Icon.enabled = false;
		Text_Content.text = string.Empty;
		Text_Count.text = Utils.FormatString("x{0}", count);
		switch (type)
		{
		case 1:
		case 2:
		case 3:
		case 21:
		case 22:
		case 101:
		case 102:
		case 103:
		case 104:
		case 105:
		case 106:
		case 107:
		case 108:
		case 201:
		case 301:
			Image_Icon.enabled = true;
			Image_Icon.sprite = SpriteManager.GetUICommonCurrency((CurrencyType)type);
			break;
		case 4:
			Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("currency_reborn");
			break;
		}
	}

	public void OnLanguageUpdate()
	{
		Init(type, count);
	}
}
