




using Dxx.Util;

public class AI5009 : AIBase
{
	private WeightRandomCount mWeight = new WeightRandomCount(1, 3);

	private int ran;

	protected override void OnInit()
	{
		ran = mWeight.GetRandom();
		switch (ran)
		{
		case 0:
			AddActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.25f);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 1f);
			});
			AddAction(GetActionAttack(string.Empty, 5070));
			AddActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.25f);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -1f);
			});
			AddActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 1f);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 1f);
			});
			AddAction(GetActionAttack(string.Empty, 5071));
			AddActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -1f);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -1f);
			});
			break;
		case 1:
			AddActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.3f);
			});
			AddAction(GetActionAttack(string.Empty, 5072));
			AddActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.3f);
			});
			break;
		case 2:
			AddAction(new AIMove1050(m_Entity));
			break;
		}
		AddAction(GetActionWait(string.Empty, 300));
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}
}
