




using UnityEngine;
using UnityEngine.UI;

public class SettingProducterCtrl : MonoBehaviour
{
	public Text Text_Producter;

	public Text Text_Value;

	public ButtonCtrl Button_producter;

	private void Awake()
	{
		Button_producter.onClick = OnClickButton;
		UpdateLanguage();
	}

	private void OnClickButton()
	{
		WindowUI.ShowWindow(WindowID.WindowID_Producer);
	}

	public void UpdateLanguage()
	{
		Text_Producter.text = GameLogic.Hold.Language.GetLanguageByTID("设置_制作人员");
		Text_Value.text = GameLogic.Hold.Language.GetLanguageByTID("设置_查看");
	}
}
