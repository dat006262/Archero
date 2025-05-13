




using DG.Tweening;
using DG.Tweening.Core;
using Dxx.Util;
using System;
using UnityEngine.UI;

public class ProgressTextCtrl : ProgressCtrl
{
	private int _cur;

	private int _max = 10;

	public Action<int, int> OnValueChanged;

	public Text text
	{
		get;
		private set;
	}

	public int current
	{
		get
		{
			return _cur;
		}
		set
		{
			_cur = value;
			UpdateValue();
			if (OnValueChanged != null)
			{
				OnValueChanged(current, max);
			}
		}
	}

	public int currentcount
	{
		get
		{
			return _cur;
		}
		set
		{
			if (value >= 0)
			{
				_cur = value;
				if (_max > 0 && (bool)text)
				{
					text.text = Utils.FormatString("{0}/{1}", _cur, _max);
				}
			}
		}
	}

	public int max
	{
		get
		{
			return _max;
		}
		set
		{
			if (value > 0)
			{
				_max = value;
				UpdateValue();
			}
		}
	}

	protected override void OnAwake()
	{
		text = base.transform.Find("Slider/Text").GetComponent<Text>();
	}

	private void UpdateValue()
	{
		if (_max > 0)
		{
			base.Value = (float)MathDxx.Clamp(_cur, 0, _cur) / (float)_max;
			if ((bool)text)
			{
				text.text = Utils.FormatString("{0}/{1}", _cur, _max);
			}
		}
	}

	public void SetText(string value)
	{
		if ((bool)text)
		{
			text.text = value;
		}
	}

	public void PlayTextScale(float scale, float time)
	{
		if ((bool)text)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(text.transform.DOScale(scale, time));
		}
	}

	public Sequence PlayCount(int after, float alltime)
	{
		int num = MathDxx.Abs(after - current);
		float value = alltime;
		if (num != 0)
		{
			value = alltime / (float)num;
		}
		value = MathDxx.Clamp(value, 0f, 0.15f);
		PlayTextScale(1.4f, 0.15f);
        DOTween.To(() => this.current, (DOSetter<int>)delegate (int x)
          {
              current = x;
          }, after, value * (float)num).SetUpdate(isIndependentUpdate: true);
		Sequence sequence = DOTween.Sequence();
		sequence.AppendInterval(value * (float)(num - 1)).AppendCallback(delegate
		{
			PlayTextScale(1f, 0.15f);
		}).AppendInterval(value)
			.SetUpdate(isIndependentUpdate: true);
		return sequence;
	}

    public void PlayPercent(float after, float time)
    {
        DOTween.To(() => this.Value, (DOSetter<float>)delegate (float x)
          {
              base.Value = x;
          }, after, time).SetUpdate(isIndependentUpdate: true);
    }
}
