




using Dxx.Util;

public class AI5040 : AIBase
{
	private WeightRandomCount mWeightRandom = new WeightRandomCount(2, 2);

	private int randomcount;

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
	}

	protected override void OnInit()
	{
		randomcount++;
		if (randomcount == 3)
		{
			randomcount = 0;
			AddAttack(5111, 0f);
		}
		else
		{
			switch (mWeightRandom.GetRandom())
			{
			case 0:
				AddAttack(5112, 0f);
				break;
			case 1:
				AddAttack(5113, 0f);
				break;
			}
		}
		AddAction(GetActionWaitRandom(string.Empty, 500, 1200));
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}

	private void AddAttack(int attackid, float slowspeed)
	{
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", slowspeed);
		}));
		AddAction(GetActionAttack(string.Empty, attackid));
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f - slowspeed);
		}));
	}
}
