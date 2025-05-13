




using UnityEngine;
using UnityEngine.UI;

public class ButtonSwitchCtrl : ButtonCtrl
{
	public Image Image_Icon;

	public Text Text_Value;

	public Sprite Sprite_Open;

	public Sprite Sprite_Close;

	public string LanguageTID_Open;

	public string LanguageTID_Close;

	public void SetSwitch(bool value)
	{
		Image_Icon.sprite = ((!value) ? Sprite_Close : Sprite_Open);
		Text_Value.text = GameLogic.Hold.Language.GetLanguageByTID((!value) ? LanguageTID_Close : LanguageTID_Open);
	}
}
