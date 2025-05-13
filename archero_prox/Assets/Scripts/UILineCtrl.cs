




using UnityEngine;

public class UILineCtrl : MonoBehaviour
{
	public UILineCtrlOne _one;

	private RectTransform rectTransform;

	private UILineCtrlOne mOne
	{
		get
		{
			if (_one == null)
			{
				_one = CInstance<UIResourceCreator>.Instance.GetUILineOne(base.transform);
				RectTransform rectTransform = _one.transform as RectTransform;
				rectTransform.sizeDelta = (base.transform as RectTransform).sizeDelta;
			}
			return _one;
		}
	}

	private void Awake()
	{
		this.rectTransform = (base.transform as RectTransform);
		if (_one != null)
		{
			RectTransform rectTransform = _one.transform as RectTransform;
			rectTransform.sizeDelta = (base.transform as RectTransform).sizeDelta;
		}
	}

	public void SetInterval(float interval)
	{
		mOne.SetInterval(interval);
	}

	public void SetOutInterval(float outinterval)
	{
		mOne.SetOutInterval(outinterval);
	}

	public void SetY(float y)
	{
		rectTransform.anchoredPosition = new Vector2(0f, y);
	}

	public void SetFontSize(int size)
	{
		mOne.SetFontSize(size);
	}

	public void SetText(string value)
	{
		mOne.SetText(value);
	}

	public void SetColor(Color color)
	{
		mOne.SetColor(color);
	}
}
