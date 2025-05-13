




using Dxx.UI;
using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class GoldTextCtrl : MonoBehaviour
{
	private bool _iconfront = true;

	private float _interval;

	private Text text;

	private Color textColor;

	private Color redColor = new Color(1f, 77f / 255f, 77f / 255f);

	private RectTransform imageRect;

	private Image image;

	private TextColor3Dxx text3;

	private Color topColor;

	private Color bottomColor;

	private float allwidth;

	private int gold;

	private CurrencyType type;

	private bool useTextRed;

	private bool bInit;

	public bool bIconFront
	{
		get
		{
			return _iconfront;
		}
		set
		{
			_iconfront = value;
		}
	}

	public float Interval
	{
		get
		{
			return _interval;
		}
		set
		{
			_interval = value;
		}
	}

	private void Awake()
	{
		init();
	}

	private void init()
	{
		if (!bInit)
		{
			bInit = true;
			text = base.transform.Find("Text").GetComponent<Text>();
			textColor = text.color;
			image = base.transform.Find("Image").GetComponent<Image>();
			imageRect = (image.transform as RectTransform);
			text3 = text.GetComponent<TextColor3Dxx>();
			if (text3 != null)
			{
				topColor = text3.topColor;
				bottomColor = text3.bottomColor;
			}
		}
	}

	public void SetAdd(int value)
	{
		SetValueInternal(value.ToString(), "+");
	}

	public void SetReduce(int value)
	{
		SetValueInternal(value.ToString(), "-");
	}

	public void SetValue(int value)
	{
		gold = value;
		SetValueInternal(value.ToString(), "x");
		UpdateTextRed();
	}

	public void SetValue(float value)
	{
		SetValueInternal(value.ToString(), string.Empty);
		UpdateTextRed();
	}

	public void SetValue(string value)
	{
		SetValueInternal(value.ToString(), string.Empty);
		UpdateTextRed();
	}

	private void SetValueInternal(string value, string before)
	{
		if (text == null)
		{
			init();
		}
		text.text = Utils.FormatString("{0}{1}", before, value);
		Vector2 sizeDelta = imageRect.sizeDelta;
		allwidth = sizeDelta.x + text.preferredWidth + Interval;
		float num = 0f;
		float num2 = 0f;
		if (bIconFront)
		{
			num = (0f - allwidth) / 2f;
			float num3 = num;
			Vector2 sizeDelta2 = imageRect.sizeDelta;
			num2 = num3 + sizeDelta2.x + Interval;
		}
		else
		{
			num2 = (0f - allwidth) / 2f;
			num = num2 + text.preferredWidth + Interval;
		}
		RectTransform rectTransform = imageRect;
		float x = num;
		Vector2 anchoredPosition = imageRect.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition.y);
		RectTransform rectTransform2 = text.rectTransform;
		float x2 = num2;
		Vector2 anchoredPosition2 = text.rectTransform.anchoredPosition;
		rectTransform2.anchoredPosition = new Vector2(x2, anchoredPosition2.y);
		RectTransform rectTransform3 = base.transform as RectTransform;
		RectTransform rectTransform4 = rectTransform3;
		float x3 = allwidth;
		Vector2 sizeDelta3 = imageRect.sizeDelta;
		rectTransform4.sizeDelta = new Vector2(x3, sizeDelta3.y);
	}

	public void SetCurrencyType(int type)
	{
		SetCurrencyType((CurrencyType)type);
	}

	public void SetCurrencyType(CurrencyType type)
	{
		init();
		this.type = type;
		string name = string.Empty;
		switch (type)
		{
		case CurrencyType.Gold:
			name = "Currency_Gold";
			break;
		case CurrencyType.Diamond:
			name = "Currency_Diamond";
			break;
		case CurrencyType.Key:
			name = "Currency_Key";
			break;
		default:
			SdkManager.Bugly_Report("GoldTextCtrl", Utils.FormatString("CurrencyType[{0}] is dont achieve!!!", type));
			break;
		}
		image.sprite = SpriteManager.GetUICommon(name);
	}

	public void UseTextRed()
	{
		useTextRed = true;
		UpdateTextRed();
	}

	public void SetButtonEnable(bool value)
	{
		if (value)
		{
			SetTextRed(LocalSave.Instance.GetGold() < gold);
		}
		else
		{
			SetTextRed(red: false);
		}
	}

	public void SetTextRed(bool red)
	{
		if (text3 == null)
		{
			Color color = (!red) ? textColor : redColor;
			text.color = color;
		}
		else if (red)
		{
			text3.topColor = redColor;
			text3.bottomColor = redColor;
			text.color = redColor;
		}
		else
		{
			text3.topColor = topColor;
			text3.bottomColor = bottomColor;
			text.color = Color.white;
		}
	}

	private void UpdateTextRed()
	{
		init();
		if (useTextRed)
		{
			if (type == CurrencyType.Gold)
			{
				SetTextRed(LocalSave.Instance.GetGold() < gold);
			}
			else if (type == CurrencyType.Diamond)
			{
				SetTextRed(red: false);
			}
		}
	}

	public float GetWidth()
	{
		return allwidth;
	}
}
