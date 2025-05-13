




using UnityEngine;

public class BattleBossHPCtrl : MonoBehaviour
{
	private const string BossAnimationName = "BossHPShow";

	public GameObject child;

	public RectTransform BossHP_FG;

	public RectTransform BossHP_FGReduce;

	public RectTransform BossHP_FGReduce1;

	public Animation Ani_Boss;

	private BattleUIBossHPCtrl mBossHPCtrl;

	private int BossHPWidth;

	private bool bShow = true;

	private void Awake()
	{
		if ((bool)BossHP_FG)
		{
			Vector2 sizeDelta = BossHP_FG.sizeDelta;
			BossHPWidth = (int)sizeDelta.x;
		}
	}

	public void Init()
	{
		if (mBossHPCtrl == null)
		{
			mBossHPCtrl = new BattleUIBossHPCtrl();
			mBossHPCtrl.Init(BossHP_FGReduce1, BossHP_FGReduce, BossHPWidth);
		}
	}

	public void DeInit()
	{
		if (mBossHPCtrl != null)
		{
			mBossHPCtrl.DeInit();
		}
	}

	public void Show(bool show)
	{
		if (bShow != show)
		{
			bShow = show;
			if (show)
			{
				child.SetActive(value: true);
				Ani_Boss.Play("BossHPShow");
			}
			else if (!show)
			{
				child.SetActive(value: false);
			}
		}
	}

	public void UpdateBossHP(float value)
	{
		Init();
		if (value > 0f)
		{
			float num = (float)BossHPWidth * value;
			Vector2 sizeDelta = BossHP_FG.sizeDelta;
			BossHP_FG.sizeDelta = new Vector2(num, sizeDelta.y);
			mBossHPCtrl.Reduce(num);
		}
		else
		{
			Show(show: false);
		}
	}

	public bool IsShow()
	{
		return bShow;
	}
}
