




using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingQualityCtrl : MonoBehaviour
{
	private static Dictionary<int, string> mQualityHigh = new Dictionary<int, string>
	{
		{
			1,
			"设置_画面质量_低"
		},
		{
			2,
			"设置_画面质量_中"
		},
		{
			3,
			"设置_画面质量_高"
		}
	};

	private static Dictionary<int, string> mQualityLow = new Dictionary<int, string>
	{
		{
			1,
			"设置_画面质量_低"
		},
		{
			2,
			"设置_画面质量_中"
		}
	};

	private bool bFlagship;

	public ButtonCtrl Button_Quality;

	public Text Text_QualityContent;

	public Text Text_Quality;

	private void Awake()
	{
		bFlagship = PlatformHelper.GetFlagShip();
		Button_Quality.onClick = OnClickButton;
		UpdateShow();
	}

	private string GetQualityString(int qualityid)
	{
		if (bFlagship)
		{
			return mQualityHigh[qualityid];
		}
		return mQualityLow[qualityid];
	}

	private void UpdateShow()
	{
		Text_QualityContent.text = GameLogic.Hold.Language.GetLanguageByTID("设置_画面质量");
		Text_Quality.text = GameLogic.Hold.Language.GetLanguageByTID(GetQualityString(GameLogic.QualityID));
	}

	private void OnClickButton()
	{
		if (bFlagship)
		{
			if (GameLogic.QualityID == 3)
			{
				GameLogic.QualityID = 1;
			}
			else
			{
				GameLogic.QualityID++;
			}
		}
		else
		{
			GameLogic.QualityID = 3 - GameLogic.QualityID;
		}
		UpdateShow();
	}

	public void UpdateLanguage()
	{
		UpdateShow();
	}
}
