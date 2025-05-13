




using UnityEngine;
using UnityEngine.UI;

public class StageListSkillsCtrl : MonoBehaviour
{
	public RectTransform imageRect;

	public Text Text_Value;

	public void Refresh(string value)
	{
		Text_Value.text = value;
		Vector2 sizeDelta = imageRect.sizeDelta;
		float num = sizeDelta.x + Text_Value.preferredWidth + 5f;
		float num2 = (0f - num) / 2f;
		float num3 = num2;
		Vector2 sizeDelta2 = imageRect.sizeDelta;
		float num4 = num3 + sizeDelta2.x + 5f;
		RectTransform rectTransform = imageRect;
		float x = num2;
		Vector2 anchoredPosition = imageRect.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition.y);
		RectTransform rectTransform2 = Text_Value.rectTransform;
		float x2 = num4;
		Vector2 anchoredPosition2 = Text_Value.rectTransform.anchoredPosition;
		rectTransform2.anchoredPosition = new Vector2(x2, anchoredPosition2.y);
		RectTransform rectTransform3 = base.transform as RectTransform;
		RectTransform rectTransform4 = rectTransform3;
		float x3 = num;
		Vector2 sizeDelta3 = imageRect.sizeDelta;
		rectTransform4.sizeDelta = new Vector2(x3, sizeDelta3.y);
	}
}
