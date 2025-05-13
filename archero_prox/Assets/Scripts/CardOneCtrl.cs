




using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class CardOneCtrl : MonoBehaviour
{
	public ButtonCtrl button;

	public Text buttonText;

	public Image Image_Icon;

	public Image Image_Quality;

	public Text Text_LevelContent;

	public Text Text_Level;

	public Text Text_Name;

	public Image Image_Unknow;

	public CanvasGroup mCanvas;

	private Skill_slotout mData;

	public Action<CardOneCtrl> Event_Click;

	private static Dictionary<int, Color> mLevelContentColors = new Dictionary<int, Color>
	{
		{
			1,
			new Color(107f / 255f, 107f / 255f, 107f / 255f)
		},
		{
			2,
			new Color(52f / 255f, 31f / 85f, 131f / 255f)
		},
		{
			3,
			new Color(36f / 85f, 27f / 85f, 44f / 85f)
		}
	};

	private static Dictionary<int, Color> mLevelColors = new Dictionary<int, Color>
	{
		{
			1,
			new Color(11f / 15f, 11f / 15f, 11f / 15f)
		},
		{
			2,
			new Color(76f / 255f, 169f / 255f, 0.9411765f)
		},
		{
			3,
			new Color(40f / 51f, 36f / 85f, 1f)
		}
	};

	public LocalSave.CardOne carddata
	{
		get;
		private set;
	}

	private void Awake()
	{
		button.onClick = OnClick;
	}

	public void InitCard(LocalSave.CardOne carddata)
	{
		this.carddata = carddata;
		UpdateUI();
	}

	public void OnClick()
	{
		if (Event_Click != null)
		{
			Event_Click(this);
		}
	}

	public void SetButtonEnable(bool value)
	{
		if ((bool)button)
		{
			button.enabled = value;
		}
		if ((bool)buttonText)
		{
			buttonText.enabled = value;
		}
	}

	public void SetTextShow(bool value)
	{
		if ((bool)Text_Level)
		{
			Text_Level.gameObject.SetActive(value);
		}
	}

	public void UpdateUI()
	{
		SetNameShow(value: true);
		SetAlpha(1f);
		mData = LocalModelManager.Instance.Skill_slotout.GetBeanById(carddata.CardID);
		Sprite card = SpriteManager.GetCard(carddata.data.GroupID);
		if ((bool)card)
		{
			Image_Icon.sprite = card;
		}
		else
		{
			Image_Icon.sprite = SpriteManager.GetCard(101);
		}
		Image_Quality.sprite = SpriteManager.GetCard(Utils.FormatString("CardUI_Quality{0}", mData.Quality));
		Image_Unknow.sprite = SpriteManager.GetCard(Utils.FormatString("CardUI_Unknow{0}", mData.Quality));
		if (carddata.Unlock)
		{
			Text_LevelContent.enabled = true;
			Image_Unknow.enabled = false;
			Image_Icon.enabled = true;
		}
		else
		{
			Text_LevelContent.enabled = false;
			Image_Unknow.enabled = true;
			Image_Icon.enabled = false;
		}
		SetTextShow(value: true);
		if (!carddata.IsMaxLevel)
		{
			if (carddata.level > 0)
			{
				Text_Level.text = carddata.level.ToString();
			}
			else
			{
				Text_Level.text = string.Empty;
			}
		}
		else
		{
			Text_Level.text = "Max";
		}
		if (!carddata.Unlock)
		{
			Text_Name.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Lock");
		}
		else
		{
			Text_Name.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("宝物名称{0}", carddata.CardID));
		}
		Text_LevelContent.color = mLevelContentColors[carddata.data.Quality];
		Text_Level.color = mLevelColors[carddata.data.Quality];
	}

	public Tweener PlayCanvas(float startalpha, float endalpha, float time)
	{
		SetAlpha(startalpha);
		return mCanvas.DOFade(endalpha, time);
	}

	public void SetAlpha(float alpha)
	{
		mCanvas.alpha = alpha;
	}

	public void SetLock(bool value)
	{
	}

	public void SetNameShow(bool value)
	{
		Text_Name.enabled = value;
	}

	public void OnLanguageChange()
	{
		Text_LevelContent.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Level");
	}
}
