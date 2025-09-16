




using UnityEngine;
using UnityEngine.UI;

public class CardUILevelLimitCtrl : MonoBehaviour
{
	public GameObject child;

	public RectTransform levelrect;

	public Text Text_Level;

	public Text Text_Info1;

	public Text Text_Info2;

	private float interval = 10f;

	public void Init(int level)
	{
		Text_Level.text = level.ToString();
		Text_Info1.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_UpgradeLevel1");
		Text_Info2.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_UpgradeLevel2");
		float preferredWidth = Text_Info1.preferredWidth;
		Vector2 sizeDelta = levelrect.sizeDelta;
		float x = sizeDelta.x;
		float preferredWidth2 = Text_Info2.preferredWidth;
		float num = (0f - (preferredWidth + x + preferredWidth2 + interval * 2f)) / 2f;
		Text_Info1.rectTransform.anchoredPosition = new Vector2(num + preferredWidth / 2f, 0f);
		RectTransform rectTransform = levelrect;
		Vector2 anchoredPosition = Text_Info1.rectTransform.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(anchoredPosition.x + preferredWidth / 2f + x / 2f + interval, 0f);
		RectTransform rectTransform2 = Text_Info2.rectTransform;
		Vector2 anchoredPosition2 = levelrect.anchoredPosition;
		rectTransform2.anchoredPosition = new Vector2(anchoredPosition2.x + x / 2f + preferredWidth2 / 2f + interval, 0f);
	}

	public void Show(bool value)
	{
		child.SetActive(value);
	}
}
