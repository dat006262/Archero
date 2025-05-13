




using Dxx.Util;
using UnityEngine;

public class ProgressCtrl : MonoBehaviour
{
	public enum ProgressDirection
	{
		LeftToRight
	}

	public ProgressDirection direction;

	private RectTransform fill;

	private RectTransform tran;

	private float width;

	private float height;

	private float _Value;

	public float Value
	{
		get
		{
			return _Value;
		}
		set
		{
			value = MathDxx.Clamp01(value);
			_Value = value;
			UpdateFill();
		}
	}

	private void Awake()
	{
		InitFill();
		tran = (base.transform as RectTransform);
		OnAwake();
	}

	protected virtual void OnAwake()
	{
	}

	private void InitFill()
	{
		if (!fill)
		{
			fill = (base.transform.Find("Slider/Fill") as RectTransform);
			if ((bool)fill)
			{
				Vector2 sizeDelta = fill.sizeDelta;
				width = sizeDelta.x;
				Vector2 sizeDelta2 = fill.sizeDelta;
				height = sizeDelta2.y;
			}
		}
	}

	private void RefreshSize()
	{
		if (width == 0f && (bool)fill && (bool)fill.parent)
		{
			RectTransform rectTransform = fill.parent as RectTransform;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			width = sizeDelta.x;
			Vector2 sizeDelta2 = rectTransform.sizeDelta;
			height = sizeDelta2.y;
		}
	}

	protected void UpdateFill()
	{
		InitFill();
		RefreshSize();
		if ((bool)fill)
		{
			fill.sizeDelta = new Vector2(Value * width, height);
		}
	}
}
