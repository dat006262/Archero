




using Dxx.Util;

public class AI5020 : AIBase
{
	private WeightRandomCount weight = new WeightRandomCount(2, 4);

	private int ran;

	protected override void OnInit()
	{
		ran = weight.GetRandom();
		switch (ran)
		{
		case 0:
			AddActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.86f);
			});
			AddAction(GetActionAttack(string.Empty, 5044));
			AddActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.86f);
			});
			break;
		case 1:
			AddAction(GetActionAttack(string.Empty, 5041));
			AddAction(GetActionDelegate(delegate
			{
				m_Entity.m_AttackCtrl.SetCanRotate(value: false);
			}));
			AddAction(GetActionAttack(string.Empty, 5042, rotate: false));
			AddAction(GetActionAttack(string.Empty, 5041, rotate: false));
			AddAction(GetActionDelegate(delegate
			{
				m_Entity.m_AttackCtrl.SetCanRotate(value: true);
			}));
			AddAction(GetActionWait(string.Empty, 500));
			break;
		case 2:
			AddAction(new AIMove1040(m_Entity, 2));
			AddAction(GetActionWait(string.Empty, 200));
			break;
		case 3:
			AddActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.55f);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.5f);
			});
			AddAction(GetActionAttack(string.Empty, 5045));
			AddActionDelegate(delegate
			{
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.55f);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.5f);
			});
			AddAction(GetActionWait(string.Empty, 200));
			break;
		}
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}
}
