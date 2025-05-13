




using UnityEngine;
using UnityEngine.UI;

public class UILineCtrlOne : MonoBehaviour
{
	public RectTransform mParent;

	public Text text;

	public RectTransform Image_Left;

	public RectTransform Image_Right;

	private RectTransform rectTransform;

	private string mValue;

	private float interval = 15f;

	private float outinterval;

	private void Awake()
	{
		rectTransform = (base.transform as RectTransform);
	}

	private void Start()
	{
		RefreshUI();
	}

	public void SetInterval(float interval)
	{
		this.interval = interval;
		SetText(mValue);
	}

	public void SetOutInterval(float outinterval)
	{
		this.outinterval = outinterval;
		SetText(mValue);
	}

	public void SetY(float y)
	{
		rectTransform.anchoredPosition = new Vector2(0f, y);
	}

	public void SetFontSize(int size)
	{
		text.fontSize = size;
		RefreshUI();
	}

	public void SetText(string value)
	{
		mValue = value;
		text.text = value;
		RefreshUI();
	}

	public void SetColor(Color color)
	{
		text.color = color;
	}

	public void RefreshUI()
	{
		Vector2 sizeDelta = mParent.sizeDelta;
		float x = sizeDelta.x;
		float preferredWidth = text.preferredWidth;
		float num = (x - preferredWidth - interval * 2f - outinterval * 2f) / 2f;
		RectTransform image_Left = Image_Left;
		float x2 = num;
		Vector2 sizeDelta2 = Image_Left.sizeDelta;
		image_Left.sizeDelta = new Vector2(x2, sizeDelta2.y);
		Image_Right.sizeDelta = Image_Left.sizeDelta;
		Image_Left.anchoredPosition = new Vector2((0f - x) / 2f + num / 2f + outinterval, 0f);
		Image_Right.anchoredPosition = new Vector2(interval + num / 2f + preferredWidth / 2f, 0f);
	}
}
