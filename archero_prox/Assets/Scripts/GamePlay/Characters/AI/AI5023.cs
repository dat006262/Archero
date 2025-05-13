




using Dxx.Util;

public class AI5023 : AIBase
{
	private WeightRandomCount mWeightRandom = new WeightRandomCount(2, 3);

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
	}

	protected override void OnInit()
	{
		switch (mWeightRandom.GetRandom())
		{
		case 0:
			AddAttack5054();
			break;
		case 1:
			AddAction(GetActionAttack(string.Empty, 5055, rotate: false));
			break;
		case 2:
			AddAttack5056();
			break;
		}
		AddAction(GetActionWaitRandom(string.Empty, 1000, 2000));
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}

	private void AddAttack5054()
	{
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.6f);
		}));
		AddAction(GetActionAttack(string.Empty, 5054, rotate: false));
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.6f);
		}));
	}

	private void AddAttack5056()
	{
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.4f);
		}));
		AddAction(GetActionAttack(string.Empty, 5056, rotate: false));
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.4f);
		}));
	}
}
