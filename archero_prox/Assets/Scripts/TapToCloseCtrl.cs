




using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TapToCloseCtrl : MonoBehaviour
{
	public GameObject child;

	public ButtonCtrl Button_Close;

	public Text Text_Content;

	public Action OnClose;

	private void Awake()
	{
		if (Button_Close != null)
		{
			Button_Close.onClick = delegate
			{
				if (OnClose != null)
				{
					OnClose();
				}
			};
		}
		Play();
	}

	private void Play()
	{
		Text_Content.DOKill();
		Text text_Content = Text_Content;
		Color color = Text_Content.color;
		float r = color.r;
		Color color2 = Text_Content.color;
		float g = color2.g;
		Color color3 = Text_Content.color;
		text_Content.color = new Color(r, g, color3.b, 0f);
		Text_Content.DOFade(1f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad)
			.SetUpdate(isIndependentUpdate: true);
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("TapToClose");
	}

	public void Show(bool value)
	{
		child.SetActive(value);
		if (value)
		{
			Play();
		}
	}
}
