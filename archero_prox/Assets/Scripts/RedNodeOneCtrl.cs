




using UnityEngine;
using UnityEngine.UI;

public class RedNodeOneCtrl : MonoBehaviour
{
	public RectTransform child;

	public Text text_count;

	public Image image;

	public Image image_icon;

	public Animator ani;

	public int count;

	private RedNodeType mType;

	public int Value
	{
		set
		{
			count = value;
			if (value > 0)
			{
				SetText(value.ToString());
			}
			else
			{
				SetText(string.Empty);
			}
		}
	}

	private bool CanSetText => mType == RedNodeType.eGreenCount || mType == RedNodeType.eRedCount;

	public void SetText(string value)
	{
		if (CanSetText && text_count != null)
		{
			text_count.text = value;
		}
	}

	private void SetAniEnable(bool value)
	{
		ani.enabled = value;
		if (!value)
		{
			child.anchoredPosition = Vector2.zero;
		}
	}

	public void SetType(RedNodeType type)
	{
		if (mType != type)
		{
			mType = type;
			switch (type)
			{
			case RedNodeType.eGreenCount:
				image.enabled = true;
				image_icon.enabled = false;
				text_count.enabled = true;
				SetAniEnable(value: false);
				image.sprite = SpriteManager.GetUICommon("UICommon_GreenNode");
				break;
			case RedNodeType.eGreenEmpty:
				image.enabled = true;
				image_icon.enabled = false;
				text_count.enabled = false;
				SetAniEnable(value: false);
				image.sprite = SpriteManager.GetUICommon("UICommon_GreenNode");
				break;
			case RedNodeType.eGreenUp:
				image.enabled = true;
				image_icon.enabled = true;
				text_count.enabled = false;
				SetAniEnable(value: false);
				image.sprite = SpriteManager.GetUICommon("UICommon_GreenNode");
				image_icon.sprite = SpriteManager.GetUICommon("UICommon_RedNode_Up");
				break;
			case RedNodeType.eRedCount:
				image.enabled = true;
				image_icon.enabled = false;
				text_count.enabled = true;
				SetAniEnable(value: true);
				image.sprite = SpriteManager.GetUICommon("UICommon_RedNode");
				break;
			case RedNodeType.eRedEmpty:
				image.enabled = true;
				image_icon.enabled = false;
				text_count.enabled = false;
				SetAniEnable(value: true);
				image.sprite = SpriteManager.GetUICommon("UICommon_RedNode");
				break;
			case RedNodeType.eRedNew:
				image.enabled = true;
				image_icon.enabled = false;
				text_count.enabled = true;
				SetAniEnable(value: true);
				image.sprite = SpriteManager.GetUICommon("UICommon_RedNode");
				text_count.text = GameLogic.Hold.Language.GetLanguageByTID("red_new");
				break;
			case RedNodeType.eRedUp:
				image.enabled = true;
				image_icon.enabled = true;
				text_count.enabled = false;
				SetAniEnable(value: true);
				image.sprite = SpriteManager.GetUICommon("UICommon_RedNode");
				image_icon.sprite = SpriteManager.GetUICommon("UICommon_RedNode_Up");
				break;
			case RedNodeType.eRedWear:
				image.enabled = true;
				image_icon.enabled = true;
				text_count.enabled = false;
				SetAniEnable(value: true);
				image.sprite = SpriteManager.GetUICommon("UICommon_RedNode");
				image_icon.sprite = SpriteManager.GetUICommon("UICommon_RedNode_Wear");
				break;
			case RedNodeType.eWarning:
				image.enabled = true;
				image_icon.enabled = true;
				text_count.enabled = false;
				SetAniEnable(value: true);
				image.sprite = SpriteManager.GetUICommon("UICommon_RedNode");
				image_icon.sprite = SpriteManager.GetUICommon("UICommon_RedNode_Combine");
				break;
			}
		}
	}
}
