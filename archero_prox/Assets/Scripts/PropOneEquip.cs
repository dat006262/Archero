




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class PropOneEquip : PropOneBase
{
	public class Transfer
	{
		public PropType type;

		public object data;
	}

	public class CurrencyData
	{
		public int id;

		public long count;
	}

	public class EquipData
	{
		public int id;

		public int count;
	}

	private bool bAlreadyGot = true;

	private long gold;

	private Vector2 ImageSize;

	protected override void OnAwake()
	{
		ImageSize = Image_Icon.rectTransform.sizeDelta;
	}

	protected override void OnInit()
	{
		if (data == null)
		{
			SdkManager.Bugly_Report("PropOneEquip", Utils.FormatString("OnInit data == null."));
			return;
		}
		Text text_Value = Text_Value;
		Vector2 sizeDelta = (base.transform as RectTransform).sizeDelta;
		text_Value.fontSize = (int)(30f * sizeDelta.x / 150f);
		Text_Value.resizeTextMinSize = Text_Value.fontSize / 2;
		Text_Value.resizeTextMaxSize = Text_Value.fontSize;
		Text_Content.text = string.Empty;
		Image_Icon.enabled = true;
		Image_BG.enabled = true;
		switch (data.type)
		{
		case PropType.eCurrency:
		{
			if (!(data.data is CurrencyData))
			{
				SdkManager.Bugly_Report("PropOneEquip", Utils.FormatString("OnInit data.data is not a CurrencyData."));
				break;
			}
			CurrencyData currencyData = data.data as CurrencyData;
			Image_Type.enabled = false;
			Image_QualityGold.enabled = false;
			gold = currencyData.count;
			Text_Value.text = Utils.FormatString("x{0}", gold);
			Image_BG.sprite = SpriteManager.GetCharUI(Utils.FormatString("CharUI_Quality{0}", 0));
			Image_Icon.enabled = false;
			Text_Content.text = string.Empty;
			CurrencyType id = (CurrencyType)currencyData.id;
			if (id == CurrencyType.Reborn)
			{
				Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("currency_reborn");
				break;
			}
			Image_Icon.enabled = true;
			Image_Icon.sprite = SpriteManager.GetUICommonCurrency(id);
			Image_Icon.rectTransform.sizeDelta = new Vector2(100f, 100f);
			break;
		}
		case PropType.eEquip:
		{
			if (!(data.data is EquipData))
			{
				SdkManager.Bugly_Report("PropOneEquip", Utils.FormatString("OnInit data.data is not a EquipData."));
				break;
			}
			EquipData equipData = data.data as EquipData;
			LocalSave.EquipOne equipOne = new LocalSave.EquipOne();
			equipOne.EquipID = equipData.id;
			if (equipData.count == 0 || !equipOne.Overlying)
			{
				Text_Value.text = string.Empty;
			}
			else
			{
				Text_Value.text = Utils.FormatString("x{0}", equipData.count);
			}
			Image_BG.sprite = SpriteManager.GetCharUI(Utils.FormatString("CharUI_Quality{0}", equipOne.Quality));
			Image_Icon.enabled = true;
			Image_Icon.sprite = equipOne.Icon;
			Image_Icon.rectTransform.sizeDelta = ImageSize;
			Sprite typeIcon = equipOne.TypeIcon;
			Image_Type.enabled = (typeIcon != null);
			Image_Type.sprite = typeIcon;
			Image_QualityGold.enabled = equipOne.ShowQualityGoldImage;
			break;
		}
		}
	}

	public void SetAlreadyGet(bool alreadyget)
	{
		bAlreadyGot = alreadyget;
	}

	public Vector3 GetMiddlePosition()
	{
		return Image_BG.transform.position;
	}

	protected override void OnClicked()
	{
	}
}
