




using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyKeyCtrl : MonoBehaviour
{
	[NonSerialized]
	public ProgressCtrl mProgressCtrl;

	[NonSerialized]
	public ProgressTextCtrl mProgressTextCtrl;

	private int mBeforeKey = 1;

	private Image Image_Key;

	private void Awake()
	{
		Transform transform = base.transform.Find("fg/ProgressTextBar");
		if ((bool)transform)
		{
			mProgressTextCtrl = transform.GetComponent<ProgressTextCtrl>();
		}
		Transform transform2 = base.transform.Find("fg/ProgressBar");
		if ((bool)transform2)
		{
			mProgressCtrl = transform2.GetComponent<ProgressCtrl>();
		}
		Image_Key = base.transform.Find("fg/Image/Image").GetComponent<Image>();
	}

	public void SetProgress(int current, int max)
	{
		if (max <= 0)
		{
			SdkManager.Bugly_Report("CurrencyKeyCtrl.cs", Utils.FormatString("SetProgress({0}, {1}) is invalid!", current, max));
		}
		else
		{
			if (!mProgressTextCtrl)
			{
				return;
			}
			mProgressTextCtrl.max = max;
			if (current > 0)
			{
				mProgressTextCtrl.current = current;
				mProgressCtrl.Value = 0f;
			}
			else
			{
				mProgressTextCtrl.current = 0;
				mProgressTextCtrl.SetText((-current).ToString());
				if ((bool)mProgressCtrl)
				{
					mProgressCtrl.Value = (float)(-current) / (float)max;
				}
			}
			ChangeImage(current);
		}
	}

	private void ChangeImage(int current)
	{
		if (mBeforeKey * current <= 0)
		{
			mBeforeKey = current;
			Image_Key.sprite = SpriteManager.GetUICommon((current < 0) ? "Currency_Key2" : "Currency_Key");
		}
	}

	public void SetProgress(string text)
	{
	}
}
