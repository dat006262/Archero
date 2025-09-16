




using DG.Tweening;
using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenBoxAniCtrl : MonoBehaviour
{
	public enum BoxState
	{
		BoxOpenOpen = 101,
		BoxOpenStand,
		BoxOpenShow
	}

	public const string BoxAni_Open = "BoxOpenOpen";

	public const string BoxAni_Stand = "BoxOpenStand";

	public const string BoxAni_Show = "BoxOpenShow";

	public const string BoxAni_Shock = "shock";

	public Image Image_Down;

	public Animator Ani_Box;

	public GameObject effect_light;

	public GameObject effect_open;

	public GameObject boxshowing;

	public GameObject boxshowone;

	public RectTransform child_box;

	public RectTransform child_box2d;

	private Vector2 boxstartpos;

	private Vector2 child_boxpos;

	private Vector2 child_box2dpos;

	private RectTransform mRectTransform;

	private void Awake()
	{
		mRectTransform = (base.transform as RectTransform);
		boxstartpos = mRectTransform.anchoredPosition;
		child_boxpos = child_box.anchoredPosition;
		child_box2dpos = child_box2d.anchoredPosition;
	}

	public void Init()
	{
		mRectTransform.anchoredPosition = boxstartpos;
		child_box.anchoredPosition = child_boxpos;
		child_box2d.anchoredPosition = child_box2dpos;
	}

	public void Play(BoxState state, LocalSave.TimeBoxType type)
	{
		string text = state.ToString();
		string text2 = string.Empty;
		switch (type)
		{
		case LocalSave.TimeBoxType.BoxChoose_DiamondNormal:
			text2 = "eNormal";
			Image_Down.sprite = SpriteManager.GetUICommon("UICommon_Box02_Down");
			break;
		case LocalSave.TimeBoxType.BoxChoose_DiamondLarge:
			text2 = "eLarge";
			Image_Down.sprite = SpriteManager.GetUICommon("UICommon_Box01_Down");
			break;
		}
		string stateName = Utils.FormatString("{0}_{1}", text, text2);
		Ani_Box.Play(stateName);
	}

	public void Play(string str)
	{
		Ani_Box.Play(str);
	}

	public void ShowOpenEffect(bool value)
	{
		effect_open.SetActive(value);
	}

	public void ShowBoxOpeningEffect(bool value)
	{
		if ((bool)boxshowing)
		{
			boxshowing.SetActive(value);
		}
	}

	public void ShowBoxOneEffect(bool value)
	{
		if ((bool)boxshowone)
		{
			boxshowone.SetActive(value);
			if (value)
			{
				DOTween.Sequence().AppendInterval(0.2f).AppendCallback(delegate
				{
					boxshowone.SetActive(value: false);
				});
			}
		}
	}
}
