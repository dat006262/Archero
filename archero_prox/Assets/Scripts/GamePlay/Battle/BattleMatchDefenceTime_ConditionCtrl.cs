




using DG.Tweening;
using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class BattleMatchDefenceTime_ConditionCtrl : MonoBehaviour
{
	public Text Text_Time;

	public Text Text_Me_Name;

	public Text Text_Me_Score;

	public Text Text_Other_Name;

	public Text Text_Other_Score;

	public RectTransform Progress_BG;

	public RectTransform Progress_Me;

	public RectTransform Progress_Other;

	public RectTransform Progress_Light;

	public BattleMatchDefenceTime_InfoCtrl mInfoCtrl;

	private float allwidth;

	private float height;

	private int score_me;

	private int score_other;

	private Transform t_name;

	private Sequence seq_name;

	private void Awake()
	{
		if ((bool)Progress_BG)
		{
			Vector2 sizeDelta = Progress_BG.sizeDelta;
			allwidth = sizeDelta.x;
		}
		if ((bool)Progress_Me)
		{
			Vector2 sizeDelta2 = Progress_Me.sizeDelta;
			height = sizeDelta2.y;
		}
		update_progress();
	}

	public void SetTime(int time)
	{
		if ((bool)Text_Time)
		{
			Text_Time.text = Utils.GetSecond2String(time);
		}
	}

	public void SetMeName(string name)
	{
		if ((bool)Text_Me_Name)
		{
			Text_Me_Name.text = name;
		}
	}

	public void SetMeScore(int value)
	{
		score_me = value;
		if ((bool)Text_Me_Score)
		{
			Text_Me_Score.text = value.ToString();
		}
		update_progress();
	}

	public void SetOtherName(string name)
	{
		if ((bool)Text_Other_Name)
		{
			Text_Other_Name.text = name;
		}
	}

	public void SetOtherScore(int value)
	{
		score_other = value;
		if ((bool)Text_Other_Score)
		{
			Text_Other_Score.text = value.ToString();
		}
		update_progress();
	}

	public bool isWin()
	{
		return score_me > score_other;
	}

	private void update_progress()
	{
		if (Progress_Me == null || Progress_Other == null || Progress_Light == null)
		{
			return;
		}
		if (score_me == 0 && score_other == 0)
		{
			Progress_Me.sizeDelta = new Vector2(allwidth / 2f, height);
			Progress_Other.sizeDelta = new Vector2(allwidth / 2f, height);
			Progress_Light.anchoredPosition = new Vector2(0f, 0f);
			return;
		}
		if (score_other == 0)
		{
			Progress_Me.sizeDelta = new Vector2(allwidth, height);
			Progress_Other.sizeDelta = new Vector2(0f, height);
			Progress_Light.anchoredPosition = new Vector2(allwidth / 2f, 0f);
			return;
		}
		float num = score_me + score_other;
		float num2 = (float)score_me / num;
		float num3 = num2 * allwidth;
		Progress_Me.sizeDelta = new Vector2(num3, height);
		Progress_Other.sizeDelta = new Vector2(allwidth - num3, height);
		Progress_Light.anchoredPosition = new Vector2(num3 - allwidth / 2f, 0f);
		Transform x = null;
		if (score_other > score_me)
		{
			x = Text_Other_Name.transform;
		}
		else if (score_me > score_other)
		{
			x = Text_Me_Name.transform;
		}
		if (x != null)
		{
			if (!(x == t_name))
			{
				KillSeq();
				if ((bool)t_name)
				{
					t_name.localScale = Vector3.one;
					t_name.localRotation = Quaternion.identity;
				}
				t_name = x;
				t_name.localScale = Vector3.one * 1.35f;
				seq_name = DOTween.Sequence();
				seq_name.Append(t_name.DOScale(Vector3.one * 1.5f, 0.1f));
				seq_name.Join(t_name.DOShakeRotation(0.1f, 5f));
				seq_name.SetEase(Ease.OutQuad);
				seq_name.SetLoops(-1, LoopType.Yoyo);
				seq_name.SetUpdate(isIndependentUpdate: true);
			}
		}
		else
		{
			KillSeq();
		}
	}

	private void KillSeq()
	{
		if (seq_name != null)
		{
			seq_name.Kill();
			seq_name = null;
		}
	}

	public void ShowInfo(string eventname, object body)
	{
		if (mInfoCtrl != null)
		{
			mInfoCtrl.ShowInfo(eventname, body);
		}
	}
}
