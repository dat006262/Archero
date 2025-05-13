




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class TipsUIManager : CInstance<TipsUIManager>
{
	private Dictionary<string, TipsUICtrl> mList = new Dictionary<string, TipsUICtrl>();

	public void ShowPotion(int id, int quality)
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("药水效果描述{0}", id));
		ShowInternal(languageByTID, LocalSave.QualityColors[quality]);
	}

	private TipsUICtrl ShowInternal(string value, Color color)
	{
		if (mList.TryGetValue(value, out TipsUICtrl value2))
		{
			value2.gameObject.SetActive(value: true);
			value2.Init();
			return value2;
		}
		GameObject gameObject = GameLogic.EffectGet("Game/UI/TipsUIOne");
		gameObject.transform.SetParent(GameNode.m_TipsUI);
		RectTransform rectTransform = gameObject.transform as RectTransform;
		rectTransform.localScale = Vector3.one;
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		value2 = gameObject.GetComponent<TipsUICtrl>();
		mList.Add(value, value2);
		value2.Init(value, color);
		return value2;
	}

	public void Show(string value)
	{
		ShowInternal(value, Color.white);
	}

	public void Show(ETips type, Color color, params string[] args)
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(type.ToString(), args);
		ShowInternal(languageByTID, color);
	}

	public void Show(string value, float y)
	{
		GameObject gameObject = GameLogic.EffectGet("Game/UI/TipsUIOne");
		gameObject.transform.SetParent(GameNode.m_TipsUI);
		RectTransform rectTransform = gameObject.transform as RectTransform;
		rectTransform.localScale = Vector3.one;
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		TipsUICtrl component = gameObject.GetComponent<TipsUICtrl>();
		component.InitNotAni(value);
		component.transform.position = new Vector3((float)GameLogic.Width / 2f, y, 0f);
	}

	public void Show(ETips type, params string[] args)
	{
		Show(type, Color.white, args);
	}

	public void ShowCode(ushort errorcode, int type = 0)
	{
		switch (errorcode)
		{
		case 8:
			switch (type)
			{
			case 1:
				Show(ETips.Tips_GoldNotEnough);
				break;
			case 2:
				Show(ETips.Tips_DiamondNotEnough);
				break;
			case 3:
				Show(ETips.Tips_KeyNotEnough);
				break;
			}
			break;
		case 10:
			Show(ETips.Tips_EquipMaterialNotEnough);
			break;
		case 1:
			if (type == 1)
			{
				Show(ETips.Tips_MailAlreadyGot);
			}
			else
			{
				Show(ETips.Tips_NetError);
			}
			break;
		default:
		{
			ETips type2 = (ETips)errorcode;
			if (!int.TryParse(type2.ToString(), out int _))
			{
				Show(type2);
			}
			else
			{
				Show(ETips.Tips_NetError);
			}
			break;
		}
		}
	}

	public void Cache(GameObject o)
	{
		o.gameObject.SetActive(value: false);
	}

	public void Clear()
	{
	}
}
