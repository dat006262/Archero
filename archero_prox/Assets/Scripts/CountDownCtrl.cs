




using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CountDownCtrl : MonoBehaviour
{
	public GameObject child;

	public Image Image_Fill;

	public Text Text_Time;

	public Image Image_Arrow;

	private bool bShow = true;

	private string timestring;

	public void Show(bool show)
	{
		if (bShow != show)
		{
			bShow = show;
			child.SetActive(show);
		}
	}

	public void Refresh(long time, float percent)
	{
		TimeSpan time2 = Utils.GetTime(time);
		Image_Fill.fillAmount = percent;
		Image_Arrow.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - percent * 360f);
		if (time2.Days > 0)
		{
			Text_Time.text = GameLogic.Hold.Language.GetLanguageByTID("倒计时_dh", time2.Days.ToString(), time2.Hours.ToString());
		}
		else if (time2.Hours > 0)
		{
			Text_Time.text = GameLogic.Hold.Language.GetLanguageByTID("倒计时_hm", time2.Hours.ToString(), time2.Minutes.ToString());
		}
		else if (time2.Minutes > 0)
		{
			Text_Time.text = GameLogic.Hold.Language.GetLanguageByTID("倒计时_ms", time2.Minutes.ToString(), time2.Seconds.ToString());
		}
		else
		{
			Text_Time.text = GameLogic.Hold.Language.GetLanguageByTID("倒计时_s", time2.Seconds.ToString());
		}
		timestring = Text_Time.text;
	}

	public string GetTimeString()
	{
		return timestring;
	}
}
