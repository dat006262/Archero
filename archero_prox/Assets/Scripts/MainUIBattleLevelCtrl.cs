




using DG.Tweening;
using PureMVC.Patterns;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class MainUIBattleLevelCtrl : MonoBehaviour
{
	public Text Text_Level;

	public ProgressCtrl mProgress;

	private float progresstime = 1f;

	public void UpdateLevel()
	{
		SetLevel(LocalSave.Instance.GetLevel());
		int current = (int)LocalSave.Instance.GetExp();
		int exp = LocalModelManager.Instance.Character_Level.GetExp(LocalSave.Instance.GetLevel());
		SetExp(current, exp);
	}

	public void SetLevel(int level)
	{
		Text_Level.text = level.ToString();
	}

	public void SetExp(int current, int max)
	{
		mProgress.Value = (float)current / (float)max;
	}

	public float AddExpAnimation(int addexp, Sequence seqparent)
	{
		int num = LocalSave.Instance.GetLevel();
		if (num < 1)
		{
			num = 1;
		}
		int maxLevel = LocalModelManager.Instance.Character_Level.GetMaxLevel();
		if (num >= maxLevel)
		{
			return 0f;
		}
		Sequence sequence = null;
		if (seqparent != null)
		{
			sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
		}
		int num2 = (int)LocalSave.Instance.GetExp();
		int exp = LocalModelManager.Instance.Character_Level.GetExp(num);
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		while (num2 + addexp >= exp)
		{
			num4 = 1f;
			num3 = (float)num2 / (float)exp;
			num++;
			num6 = (num4 - num3) * progresstime;
			num5 += num6;
			if (sequence != null)
			{
				int levelup = num;
				sequence.Append(DOTween.To(() => mProgress.Value, delegate(float x)
				{
					mProgress.Value = x;
				}, 1f, num6).SetUpdate(isIndependentUpdate: true));
				sequence.AppendCallback(delegate
				{
					seqparent.Pause();
					Facade.Instance.RegisterProxy(new LevelUpProxy(new LevelUpProxy.Transfer
					{
						level = levelup,
						onclose = delegate
						{
							seqparent.Play();
						}
					}));
					WindowUI.ShowWindow(WindowID.WindowID_LevelUp);
				});
				sequence.AppendInterval(0.03f);
				sequence.AppendCallback(delegate
				{
					SetLevel(levelup);
					mProgress.Value = 0f;
				});
				sequence.AppendInterval(0.03f);
			}
			addexp -= exp - num2;
			num2 = 0;
			if (num >= maxLevel)
			{
				break;
			}
			exp = LocalModelManager.Instance.Character_Level.GetExp(num);
		}
		if (num >= maxLevel)
		{
			mProgress.Value = 1f;
		}
		else
		{
			num3 = (float)num2 / (float)exp;
			num4 = (float)(num2 + addexp) / (float)exp;
			num6 = (num4 - num3) * progresstime;
			num5 += num6;
			if (sequence != null)
			{
				sequence.Append(DOTween.To(() => mProgress.Value, delegate(float x)
				{
					mProgress.Value = x;
				}, num4, num6).SetUpdate(isIndependentUpdate: true));
				seqparent.Append(sequence);
			}
		}
		LocalSave.Instance.SetLevel(num);
		LocalSave.Instance.SetExp(num2 + addexp);
		return num5;
	}
}
