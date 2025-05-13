




using DG.Tweening;
using Dxx.Util;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenOneCurrencyCtrl : MonoBehaviour
{
	public Image Image_Icon;

	public Text Text_Count;

	public Text Text_Content;

	public Transform child;

	public GameObject fx_open;

	public GameObject gift;

	public Text Text_Gift;

	public Sequence Init(Drop_DropModel.DropData data)
	{
		DeInit();
		gift.SetActive(value: false);
		Text_Gift.text = GameLogic.Hold.Language.GetLanguageByTID("Box_Gift");
		Text_Content.text = string.Empty;
		Image_Icon.gameObject.SetActive(value: false);
		Text_Count.text = Utils.FormatString("x{0}", data.count);
		CurrencyType id = (CurrencyType)data.id;
		if (id == CurrencyType.Reborn)
		{
			Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("currency_reborn");
		}
		else
		{
			Image_Icon.gameObject.SetActive(value: true);
			Image_Icon.sprite = SpriteManager.GetUICommonCurrency(id);
		}
		fx_open.SetActive(value: true);
		return DOTween.Sequence().Append(child.DOScale(1f, 0.3f));
	}

	public void DeInit()
	{
		fx_open.SetActive(value: false);
		child.localScale = Vector3.zero;
	}
}
