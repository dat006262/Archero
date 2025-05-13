




using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenSingleRetryCtrl : MonoBehaviour
{
	public GameObject child;

	public Text Text_RetryFree;

	public Text Text_RetryExtra;

	public Text Text_RetryNotFree;

	public Image Image_Extra;

	public ButtonCtrl Button_Retry;

	public GoldTextCtrl mGoldNow;

	public GoldTextCtrl mGoldOld;

	public RedNodeCtrl mRedNodeCtrl;

	public GameObject notfreeparent;

	public GameObject freeparent;

	public GameObject extraparent;

	public Text Text_Extra;

	public Action onRetry;

	private float retry_y;

	private float now_y;

	private float old_y;

	private RectTransform rect_now;

	private RectTransform rect_old;

	private void Awake()
	{
		Button_Retry.SetDepondNet(value: true);
		Button_Retry.onClick = delegate
		{
			if (onRetry != null)
			{
				onRetry();
			}
		};
		rect_now = (mGoldNow.transform as RectTransform);
		rect_old = (mGoldOld.transform as RectTransform);
		Vector2 anchoredPosition = rect_now.anchoredPosition;
		now_y = anchoredPosition.y;
		Vector2 anchoredPosition2 = rect_old.anchoredPosition;
		old_y = anchoredPosition2.y;
		Vector2 anchoredPosition3 = Text_RetryNotFree.rectTransform.anchoredPosition;
		retry_y = anchoredPosition3.y;
	}

	public void Init(LocalSave.TimeBoxType type, int now, int old)
	{
		CurrencyType type2 = CurrencyType.DiamondBoxNormal;
		if (type == LocalSave.TimeBoxType.BoxChoose_DiamondLarge)
		{
			type2 = CurrencyType.DiamondBoxLarge;
		}
		Image_Extra.sprite = SpriteManager.GetUICommonCurrency(type2);
		int timeBoxCount = LocalSave.Instance.GetTimeBoxCount(type);
		mRedNodeCtrl.SetType(RedNodeType.eRedCount);
		mRedNodeCtrl.Value = timeBoxCount;
		rect_old.anchoredPosition = new Vector2(0f, old_y);
		rect_now.anchoredPosition = new Vector2(0f, now_y);
		Text_RetryNotFree.rectTransform.anchoredPosition = new Vector2(0f, retry_y);
		freeparent.SetActive(value: false);
		notfreeparent.SetActive(value: false);
		extraparent.SetActive(value: false);
		if (timeBoxCount > 0)
		{
			freeparent.SetActive(value: true);
			return;
		}
		int num = 0;
		num = LocalSave.Instance.GetDiamondExtraCount(type);
		if (num > 0)
		{
			extraparent.SetActive(value: true);
			Text_Extra.text = Utils.FormatString("{0}/1", num);
			return;
		}
		notfreeparent.SetActive(value: true);
		if (now == old)
		{
			Text_RetryNotFree.rectTransform.anchoredPosition = new Vector2(0f, 20f);
			rect_now.anchoredPosition = new Vector2(0f, old_y);
			mGoldOld.gameObject.SetActive(value: false);
		}
		else
		{
			mGoldNow.gameObject.SetActive(value: true);
			mGoldOld.gameObject.SetActive(value: true);
		}
		mGoldNow.SetValue(now);
		mGoldOld.SetValue(old);
	}

	public void Show(bool value)
	{
		child.SetActive(value);
	}

	public void OnLanguageChange()
	{
		Text_RetryFree.text = GameLogic.Hold.Language.GetLanguageByTID("商店_免费抽取");
		Text_RetryExtra.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_buy_again");
		Text_RetryNotFree.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_buy_again");
	}
}
