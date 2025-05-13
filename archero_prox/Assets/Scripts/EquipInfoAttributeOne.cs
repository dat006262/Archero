




using Dxx.UI;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoAttributeOne : MonoBehaviour
{
	public Text Text_Attr;

	public OutLineDxx outline;

	public Image Image_Icon;

	public void SetText(string value)
	{
		if ((bool)Text_Attr)
		{
			Text_Attr.text = value;
		}
		SetUnlock(value: true);
	}

	public float GetTextHeight()
	{
		if ((bool)Text_Attr)
		{
			return Text_Attr.preferredHeight;
		}
		return 0f;
	}

	public void SetUnlock(bool value)
	{
		if ((bool)Text_Attr)
		{
			Text_Attr.color = ((!value) ? Color.gray : Color.white);
		}
		Image_Icon.gameObject.SetActive(!value);
		ShowOutLine(value);
	}

	private void ShowOutLine(bool value)
	{
		if ((bool)outline)
		{
			outline.enabled = value;
		}
	}
}
