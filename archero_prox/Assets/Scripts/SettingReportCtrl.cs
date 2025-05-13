




using UnityEngine;
using UnityEngine.UI;

public class SettingReportCtrl : MonoBehaviour
{
	public ButtonCtrl Button_Report;

	public Text Text_Report;

	private void Awake()
	{
		Button_Report.onClick = OnClickButton;
		UpdateShow();
	}

	private void UpdateShow()
	{
		Text_Report.text = GameLogic.Hold.Language.GetLanguageByTID("setting_report");
	}

	private void OnClickButton()
	{
		WindowUI.ShowWindow(WindowID.WindowID_Report);
	}

	public void UpdateLanguage()
	{
		UpdateShow();
	}
}
