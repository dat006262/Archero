




using Dxx.Util;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenGetOne : MonoBehaviour
{
	public GameObject currencyparent;

	public GameObject equipparent;

	public CanvasGroup mCanvasGroup;

	public Image Currency_ImageIcon;

	public Image Currency_ImageBG;

	public Text Currency_TextCount;

	public GameObject Currency_Gift;

	public Text Text_Gift;

	public Image Equip_ImageIcon;

	public Image Equip_ImageBG;

	public Text Text_Count;

	public Text Text_Content;

	protected Drop_DropModel.DropData mData;

	public void Init(Drop_DropModel.DropData data)
	{
		mData = data;
		currencyparent.SetActive(value: false);
		equipparent.SetActive(value: false);
		Currency_Gift.SetActive(value: false);
		Text_Gift.text = GameLogic.Hold.Language.GetLanguageByTID("Box_Gift");
		switch (data.type)
		{
		case PropType.eCurrency:
		{
			currencyparent.SetActive(value: true);
			Currency_ImageIcon.gameObject.SetActive(value: false);
			Text_Content.text = string.Empty;
			CurrencyType id = (CurrencyType)data.id;
			if (id == CurrencyType.Reborn)
			{
				Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("currency_reborn");
			}
			else
			{
				Currency_ImageIcon.gameObject.SetActive(value: true);
				Currency_ImageIcon.sprite = SpriteManager.GetUICommonCurrency(id);
			}
			Currency_TextCount.text = Utils.FormatString("x{0}", mData.count);
			break;
		}
		case PropType.eEquip:
		{
			Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById(mData.id);
			equipparent.SetActive(value: true);
			Equip_ImageIcon.sprite = SpriteManager.GetEquip(beanById.EquipIcon);
			Equip_ImageBG.sprite = SpriteManager.GetCharUI(Utils.FormatString("CharUI_Quality{0}", beanById.Quality));
			Text_Count.text = Utils.FormatString("x{0}", mData.count);
			break;
		}
		}
	}
}
