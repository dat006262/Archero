




using UnityEngine;
using UnityEngine.UI;

public class SettingSoundCtrl : MonoBehaviour
{
	public Text Text_Sound;

	public ButtonSwitchCtrl Button_Sound;

	private void Awake()
	{
		Button_Sound.onClick = OnClickButton;
		UpdateShow();
	}

	private void UpdateShow()
	{
		if ((bool)GameLogic.Hold && (bool)GameLogic.Hold.Sound)
		{
			Button_Sound.SetSwitch(GameLogic.Hold.Sound.GetSound());
		}
		Text_Sound.text = GameLogic.Hold.Language.GetLanguageByTID("设置_音效");
	}

	private void OnClickButton()
	{
		GameLogic.Hold.Sound.ChangeSound();
		UpdateShow();
	}

	public void UpdateLanguage()
	{
		UpdateShow();
	}
}
