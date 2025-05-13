




using DG.Tweening;
using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeMode102 : ChallengeModeBase
{
	private Text Text_Value;

	protected int currenttime;

	protected int alltime;

	private Sequence seq;

	private Sequence seq_update;

	protected override void OnInit()
	{
		if (!int.TryParse(mData, out alltime))
		{
			SdkManager.Bugly_Report("ChallengeCondition103", Utils.FormatString("[{0}] is not a int value.", mData));
		}
		currenttime = alltime;
	}

	protected override void OnStart()
	{
		seq = DOTween.Sequence();
		seq.AppendInterval(1f);
		seq.SetLoops(alltime);
		seq.OnStepComplete(OnUpdateSecond);
		Transform transform = mParent.Find("Text_Value");
		if (!transform)
		{
			SdkManager.Bugly_Report("ChallengeCondition102", "Text_Value is not found.");
		}
		Text_Value = transform.GetComponent<Text>();
		UpdateText();
	}

	protected override void OnDeInit()
	{
		KillSequence();
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
		}
		if (seq_update != null)
		{
			seq_update.Kill();
		}
	}

	private void OnUpdateSecond()
	{
		currenttime--;
		if (currenttime <= 0)
		{
			KillSequence();
			OnSuccess();
		}
		else
		{
			UpdateText();
			OnUpdate();
		}
	}

	protected virtual void OnUpdate()
	{
	}

	private void UpdateText()
	{
		if ((bool)Text_Value)
		{
			Text_Value.text = Utils.FormatString("{0}", currenttime);
		}
	}

	protected override string OnGetSuccessString()
	{
		return Utils.FormatString("生存{0}秒", currenttime);
	}
}
