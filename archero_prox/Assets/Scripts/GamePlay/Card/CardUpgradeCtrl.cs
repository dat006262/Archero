




using UnityEngine;
using UnityEngine.UI;

public class CardUpgradeCtrl : MonoBehaviour
{
	public Text Text_UpgradeCount;

	public CardUILevelLimitCtrl mLevelLimitCtrl;

	public ButtonCtrl Button_Upgrade;

	public GoldTextCtrl mGoldCtrl;

	public Text Text_UpgradeContent;

	public void UpdateUpgrade()
	{
		Text_UpgradeCount.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_UpgradeCount", LocalSave.Instance.Card_GetRandomCount());
		if (!LocalSave.Instance.Card_GetAllMax())
		{
			int num = LocalSave.Instance.Card_GetNeedLevel();
			if (LocalSave.Instance.GetLevel() < num)
			{
				mLevelLimitCtrl.Show(value: true);
				mLevelLimitCtrl.Init(num);
				Button_Upgrade.gameObject.SetActive(value: false);
				return;
			}
			mLevelLimitCtrl.Show(value: false);
			Button_Upgrade.gameObject.SetActive(value: true);
			int value = LocalSave.Instance.Card_GetRandomGold();
			mGoldCtrl.SetCurrencyType(CurrencyType.Gold);
			mGoldCtrl.SetValue(value);
			Button_Upgrade.SetEnable(value: true);
		}
		else
		{
			mLevelLimitCtrl.Show(value: false);
			Button_Upgrade.gameObject.SetActive(value: false);
		}
	}

	public void OnLanguageChange()
	{
		Text_UpgradeContent.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Upgrade");
	}
}
