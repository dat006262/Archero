




using UnityEngine;
using UnityEngine.UI;

public class SettingMusicCtrl : MonoBehaviour
{
	public Text Text_Music;

	public ButtonSwitchCtrl Button_Music;

	private void Awake()
	{
		Button_Music.onClick = OnClickButton;
		UpdateShow();
	}

	private void UpdateShow()
	{
		if ((bool)GameLogic.Hold && (bool)GameLogic.Hold.Sound)
		{
			Button_Music.SetSwitch(GameLogic.Hold.Sound.GetMusic());
		}
		Text_Music.text = GameLogic.Hold.Language.GetLanguageByTID("设置_音乐");
	}

	private void OnClickButton()
	{
		GameLogic.Hold.Sound.ChangeMusic();
		UpdateShow();
	}

	public void UpdateLanguage()
	{
		UpdateShow();
	}
}
