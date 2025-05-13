




using System;
using UnityEngine;
using UnityEngine.UI;

public class LanguageOneCtrl : MonoBehaviour
{
	public Text Text_Language;

	public ButtonCtrl Button_Language;

	public GameObject fg;

	public Action<LanguageOneCtrl> OnClickButton;

	public string mLanguage
	{
		get;
		private set;
	}

	private void Awake()
	{
		Button_Language.onClick = delegate
		{
			if (OnClickButton != null)
			{
				OnClickButton(this);
			}
		};
	}

	public void Init(int index, string language)
	{
		mLanguage = language;
		Text_Language.text = LanguageManager.languagedic[language];
		UpdateChoose();
	}

	private void UpdateChoose()
	{
		fg.SetActive(mLanguage == GameLogic.Hold.Language.GetLanguageString());
	}
}
