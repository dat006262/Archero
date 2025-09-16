




using DG.Tweening;
using Dxx.Util;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.UI;

public class BattleExpCtrl : MonoBehaviour
{
	private const string ExpAnimationName = "HeroExpShow";

	private const string ExpScaleName = "HeroExpScale";

	public Text Text_Level;

	public Animation Ani_Exp;

	public RectTransform Exp_Add;

	public RectTransform Exp_Add1;

	public RectTransform Exp_BG;

	public RectTransform Exp_FG;

	private RectTransform rectTransform;

	private int ExpWidth;

	private int ExpBGWidth;

	private BattleUIBossHPCtrl mBossHPCtrl;

	private ActionUpdateCtrl mActionUpdateCtrl;

	private bool bDropExp = true;

	private SequencePool mSequencePool = new SequencePool();

	private void Awake()
	{
		rectTransform = (base.transform as RectTransform);
		if ((bool)Exp_FG)
		{
			Vector2 sizeDelta = Exp_FG.sizeDelta;
			ExpWidth = (int)sizeDelta.x;
		}
		if ((bool)Exp_BG)
		{
			Vector2 sizeDelta2 = Exp_BG.sizeDelta;
			ExpBGWidth = (int)sizeDelta2.x;
		}
	}

	public void Init()
	{
		Vector2 sizeDelta = Exp_FG.sizeDelta;
		Exp_FG.sizeDelta = new Vector2(0f, sizeDelta.y);
		Exp_Add.sizeDelta = new Vector2(0f, sizeDelta.y);
		Exp_Add1.sizeDelta = new Vector2(0f, sizeDelta.y);
		RectTransform exp_BG = Exp_BG;
		Vector2 sizeDelta2 = Exp_BG.sizeDelta;
		exp_BG.sizeDelta = new Vector2(0f, sizeDelta2.y);
		Exp_FG.localScale = Vector3.zero;
		Text_Level.text = string.Empty;
		Sequence seq = DOTween.Sequence().AppendInterval(0.5f).AppendCallback(delegate
		{
			if ((bool)Exp_BG)
			{
				RectTransform exp_BG2 = Exp_BG;
				float x = ExpBGWidth;
				Vector2 sizeDelta3 = Exp_BG.sizeDelta;
				exp_BG2.sizeDelta = new Vector2(x, sizeDelta3.y);
			}
			if ((bool)Exp_Add)
			{
				RectTransform exp_Add = Exp_Add;
				float x2 = ExpWidth;
				Vector2 sizeDelta4 = Exp_Add.sizeDelta;
				exp_Add.sizeDelta = new Vector2(x2, sizeDelta4.y);
			}
			if ((bool)Exp_Add1)
			{
				RectTransform exp_Add2 = Exp_Add1;
				float x3 = ExpWidth;
				Vector2 sizeDelta5 = Exp_Add1.sizeDelta;
				exp_Add2.sizeDelta = new Vector2(x3, sizeDelta5.y);
			}
			if ((bool)GameLogic.Self && GameLogic.Self.m_EntityData != null)
			{
				SetLevel(GameLogic.Self.m_EntityData.GetLevel());
			}
			if ((bool)Ani_Exp)
			{
				Ani_Exp.Play("HeroExpShow");
			}
			if ((bool)Exp_FG)
			{
				Exp_FG.localScale = Vector3.one;
			}
		});
		mSequencePool.Add(seq);
		mActionUpdateCtrl = new ActionUpdateCtrl();
		mActionUpdateCtrl.Init();
	}

	public void DeInit()
	{
		if (mActionUpdateCtrl != null)
		{
			mActionUpdateCtrl.DeInit();
		}
		mSequencePool.Clear();
	}

	public void SetFringe()
	{
		RectTransform rectTransform = Ani_Exp.transform as RectTransform;
		RectTransform obj = rectTransform;
		Vector2 anchoredPosition = rectTransform.anchoredPosition;
		float x = anchoredPosition.x;
		Vector2 anchoredPosition2 = rectTransform.anchoredPosition;
		obj.anchoredPosition = new Vector2(x, anchoredPosition2.y + PlatformHelper.GetFringeHeight());
	}

	public void SetLevel(int level)
	{
		if ((bool)Text_Level)
		{
			Text_Level.text = Utils.FormatString("Lv.{0}", level);
			if ((bool)GameLogic.Self && GameLogic.Self.m_EntityData != null && level == GameLogic.Self.m_EntityData.MaxLevel)
			{
				Text_Level.text = "Lv.MAX";
			}
		}
	}

	private void set_progress(float value)
	{
		RectTransform exp_FG = Exp_FG;
		float x = value * (float)ExpWidth;
		Vector2 sizeDelta = Exp_FG.sizeDelta;
		exp_FG.sizeDelta = new Vector2(x, sizeDelta.y);
	}

	public void ExpUP(ProgressAniManager vo)
	{
		vo.SetUpdate(update_ui);
	}

	private void update_ui(ProgressAniManager.ProgressTransfer data)
	{
		SetLevel(data.currentlevel);
		set_progress(data.percent);
		if (!data.isend)
		{
			float x = UnityEngine.Random.Range(0, 2) * 2 - 1;
			float y = UnityEngine.Random.Range(0, 2) * 2 - 1;
			rectTransform.anchoredPosition = new Vector2(x, y) * 2f;
		}
		else
		{
			rectTransform.anchoredPosition = Vector2.zero;
		}
		if (data.islevelup)
		{
			Facade.Instance.SendNotification("BATTLE_LEVEL_UP");
		}
	}

	public void SetDropExp(bool drop)
	{
		bDropExp = drop;
	}

	public void Show(bool show)
	{
		if (!bDropExp)
		{
			if (Ani_Exp.gameObject.activeSelf)
			{
				Ani_Exp.gameObject.SetActive(value: false);
			}
		}
		else if (show)
		{
			Ani_Exp.gameObject.SetActive(value: true);
		}
		else if (!show)
		{
			Ani_Exp.gameObject.SetActive(value: false);
			Ani_Exp.Play("HeroExpShow");
		}
	}
}
