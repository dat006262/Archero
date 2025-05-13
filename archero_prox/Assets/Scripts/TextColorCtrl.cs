




using Dxx.UI;
using UnityEngine;
using UnityEngine.UI;

public class TextColorCtrl : MonoBehaviour
{
	private string mText;

	private int mFontSize = -1;

	public Color topColor1 = new Color(242f / 255f, 84f / 85f, 227f / 255f, 1f);

	public Color topColor2 = new Color(163f / 255f, 0.882352948f, 103f / 255f, 1f);

	public Color topoutlineColor = new Color(0f, 0f, 0f, 1f);

	public Color middleColor = new Color(86f / 255f, 23f / 51f, 59f / 255f, 1f);

	public Color middleoutlineColor = new Color(0f, 0f, 0f, 1f);

	public Color shadowColor = new Color(37f / 85f, 37f / 85f, 37f / 85f, 1f);

	private Text _text_top;

	private Text _text_middle;

	private Text _text_shadow;

	public string text
	{
		get
		{
			return mText;
		}
		set
		{
			mText = value;
			text_top.text = value;
			text_middle.text = value;
			text_shadow.text = value;
		}
	}

	public int FontSize
	{
		get
		{
			if (mFontSize == -1)
			{
				mFontSize = text_top.fontSize;
			}
			return mFontSize;
		}
		set
		{
			mFontSize = value;
			text_top.fontSize = value;
			text_middle.fontSize = value;
			text_shadow.fontSize = value;
			_text_middle.transform.localPosition = new Vector3(0f, (float)(-value) / 10f, 0f);
			_text_shadow.transform.localPosition = new Vector3(0f, (float)(-value) / 5f, 0f);
		}
	}

	private Text text_top
	{
		get
		{
			if (_text_top == null)
			{
				_text_top = base.transform.Find("Text_Top").GetComponent<Text>();
			}
			return _text_top;
		}
	}

	private Text text_middle
	{
		get
		{
			if (_text_middle == null)
			{
				_text_middle = base.transform.Find("Text_Middle").GetComponent<Text>();
			}
			return _text_middle;
		}
	}

	private Text text_shadow
	{
		get
		{
			if (_text_shadow == null)
			{
				_text_shadow = base.transform.Find("Text_Shadow").GetComponent<Text>();
			}
			return _text_shadow;
		}
	}

	private void Awake()
	{
		TextColorDxx component = text_top.GetComponent<TextColorDxx>();
		component.topColor = topColor1;
		component.bottomColor = topColor2;
		text_top.GetComponent<Outline>().effectColor = topoutlineColor;
		text_middle.GetComponent<Outline>().effectColor = middleoutlineColor;
		text_shadow.color = shadowColor;
	}
}
