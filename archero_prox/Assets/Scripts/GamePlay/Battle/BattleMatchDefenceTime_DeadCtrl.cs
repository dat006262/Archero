




using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleMatchDefenceTime_DeadCtrl : MonoBehaviour
{
	public GameObject child;

	public Text Text_DeadContent;

	public Text Text_DeadTime;

	private int mTime;

	private int mCurrentTime;

	private Action mCallback;

	private Sequence seq;

	public void Show(bool value)
	{
		child.SetActive(value);
	}

	public void SetTime(int time, Action callback)
	{
		mTime = time;
		mCallback = callback;
		mCurrentTime = mTime;
		KillSequence();
		seq = DOTween.Sequence();
		seq.AppendInterval(1f);
		seq.SetLoops(mTime);
		seq.OnStepComplete(OnUpdateSecond);
		seq.SetUpdate(isIndependentUpdate: true);
		SetTime(mCurrentTime);
	}

	private void OnUpdateSecond()
	{
		mCurrentTime--;
		if (mCurrentTime <= 0)
		{
			KillSequence();
			if (mCallback != null)
			{
				mCallback();
			}
			Show(value: false);
		}
		else
		{
			SetTime(mCurrentTime);
		}
	}

	private void SetTime(int time)
	{
		Text_DeadTime.text = time.ToString();
	}

	public void Deinit()
	{
		KillSequence();
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	public void OnLanguageChange()
	{
		Text_DeadContent.text = "复活倒计时";
	}
}
