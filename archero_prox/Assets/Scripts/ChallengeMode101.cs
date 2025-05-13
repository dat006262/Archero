




using DG.Tweening;
using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeMode101 : ChallengeModeBase
{
	private Text Text_Value;

	private int currenttime;

	private int alltime;

	private Sequence seq;

	protected override void OnInit()
	{
		if (!int.TryParse(mData, out alltime))
		{
			SdkManager.Bugly_Report("ChallengeCondition101", Utils.FormatString("[{0}] is not a int value.", mData));
		}
		currenttime = alltime;
	}

	protected override void OnStart()
	{
		seq = DOTween.Sequence();
		seq.AppendInterval(1f);
		seq.SetLoops(alltime);
		seq.OnStepComplete(OnUpdate);
		Transform transform = mParent.Find("Text_Value");
		if (!transform)
		{
			SdkManager.Bugly_Report("ChallengeCondition101", "Text_Value is not found.");
		}
		Text_Value = transform.GetComponent<Text>();
		UpdateText();
	}

	protected override void OnDeInit()
	{
		if (seq != null)
		{
			seq.Kill();
		}
	}

	private void OnUpdate()
	{
		currenttime--;
		if (currenttime <= 0)
		{
			if (seq != null)
			{
				seq.Kill();
				seq = null;
			}
			OnFailure();
		}
		else
		{
			UpdateText();
		}
	}

	private void UpdateText()
	{
		if ((bool)Text_Value)
		{
			Text_Value.text = Utils.FormatString("{0}", Utils.GetSecond2String(currenttime));
		}
	}

	protected override string OnGetSuccessString()
	{
		return Utils.FormatString("{0}分钟内通关", currenttime / 60);
	}
}
