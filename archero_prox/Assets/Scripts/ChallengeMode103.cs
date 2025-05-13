




using DG.Tweening;
using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeMode103 : ChallengeModeBase
{
	private Text Text_TimeValue;

	private Text Text_CountValue;

	private int currenttime;

	private int alltime;

	private int currentmonster;

	private int allmonster;

	private Sequence seq;

	private bool bSuccess;

	protected override void OnInit()
	{
		string[] array = mData.Split(',');
		if (array.Length < 2)
		{
			SdkManager.Bugly_Report("ChallengeCondition106", Utils.FormatString("[{0}] length < 2.", mData));
		}
		if (!int.TryParse(array[0], out alltime))
		{
			SdkManager.Bugly_Report("ChallengeCondition106", Utils.FormatString("strs[0]:[{0}] is not a int value.", array[0]));
		}
		if (!int.TryParse(array[1], out allmonster))
		{
			SdkManager.Bugly_Report("ChallengeCondition106", Utils.FormatString("strs[1]:[{0}] is not a int value.", array[1]));
		}
		currenttime = alltime;
		currentmonster = 0;
	}

	protected override void OnStart()
	{
		seq = DOTween.Sequence();
		seq.AppendInterval(1f);
		seq.SetLoops(alltime);
		seq.OnStepComplete(OnUpdate);
		Transform transform = mParent.Find("Condition1/Text_Value");
		if (!transform)
		{
			SdkManager.Bugly_Report("ChallengeCondition106", "Condition1/Text_Value is not found.");
		}
		Text_TimeValue = transform.GetComponent<Text>();
		Transform transform2 = mParent.Find("Condition2/Text_Value");
		if (!transform2)
		{
			SdkManager.Bugly_Report("ChallengeCondition106", "Condition2/Text_Value is not found.");
		}
		Text_CountValue = transform2.GetComponent<Text>();
		UpdateText();
		UpdateMonster();
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
		UpdateText();
	}

	private void UpdateText()
	{
		if ((bool)Text_TimeValue)
		{
			Text_TimeValue.text = Utils.FormatString("{0}", currenttime);
		}
	}

	private void UpdateMonster()
	{
		UpdateMonsterText();
		if (currentmonster >= allmonster && !bSuccess)
		{
			bSuccess = true;
			DOTween.Sequence().AppendInterval(0.5f).AppendCallback(delegate
			{
				OnSuccess();
			});
		}
	}

	private void UpdateMonsterText()
	{
		if ((bool)Text_CountValue && currentmonster <= allmonster)
		{
			Text_CountValue.text = Utils.FormatString("{0}/{1}", currentmonster, allmonster);
		}
	}

	protected override string OnGetSuccessString()
	{
		return Utils.FormatString("{0}秒内击杀{1}只怪物", alltime, allmonster);
	}

	protected override void OnMonsterDead()
	{
		currentmonster++;
		UpdateMonster();
	}
}
