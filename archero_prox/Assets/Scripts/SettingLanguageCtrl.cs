




using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingLanguageCtrl : MonoBehaviour
{
	private static List<string> languagelist = new List<string>();

	public Text Text_LanguageContent;

	public Text Text_Language;

	public ButtonCtrl Button_Language;

	public static List<string> GetLanguageList()
	{
		if (languagelist.Count == 0)
		{
			Dictionary<string, string>.Enumerator enumerator = LanguageManager.languagedic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				languagelist.Add(enumerator.Current.Key);
			}
		}
		return languagelist;
	}

	private void Awake()
	{
		Button_Language.onClick = OnClickButton;
		UpdateLanguage();
	}

	private void OnClickButton()
	{
		WindowUI.ShowWindow(WindowID.WindowID_Language);
	}

	public void UpdateLanguage()
	{
		Text_LanguageContent.text = GameLogic.Hold.Language.GetLanguageByTID("设置_语言");
		string languageString = GameLogic.Hold.Language.GetLanguageString();
		if (LanguageManager.languagedic.ContainsKey(languageString))
		{
			Text_Language.text = LanguageManager.languagedic[languageString];
		}
		else
		{
			Text_Language.text = LanguageManager.languagedic["EN"];
		}
	}
}
