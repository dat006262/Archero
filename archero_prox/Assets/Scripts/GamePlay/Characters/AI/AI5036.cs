




using Dxx.Util;
using UnityEngine;

public class AI5036 : AIBase
{
	private WeightRandomCount weightnear = new WeightRandomCount(1, 3);

	private WeightRandomCount weightfar = new WeightRandomCount(1, 3);

	protected override void OnInit()
	{
		if (Vector3.Distance(GameLogic.Self.position, m_Entity.position) < 10f)
		{
			switch (weightnear.GetRandom())
			{
			case 0:
				AddAction(GetActionDelegate(delegate
				{
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.5f);
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.3f);
				}));
				AddAction(GetActionAttack(string.Empty, 5097));
				AddAction(GetActionDelegate(delegate
				{
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.5f);
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.3f);
				}));
				AddAction(GetActionWaitRandom(string.Empty, 100, 200));
				break;
			case 1:
				AddAction(GetActionDelegate(delegate
				{
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.3f);
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f);
				}));
				AddAction(GetActionAttack(string.Empty, 5098));
				AddAction(GetActionDelegate(delegate
				{
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.3f);
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f);
				}));
				AddAction(GetActionWaitRandom(string.Empty, 100, 200));
				break;
			case 2:
				AddAction(GetActionAttack(string.Empty, 5100));
				AddAction(GetActionWaitRandom(string.Empty, 100, 200));
				break;
			}
		}
		else
		{
			switch (weightfar.GetRandom())
			{
			case 0:
				AddAction(GetActionRotateToEntity(GameLogic.Self));
				AddAction(new AIMove1055(m_Entity));
				AddAction(GetActionWaitRandom(string.Empty, 600, 800));
				break;
			case 1:
				AddAction(GetActionAttack(string.Empty, 5096));
				AddAction(GetActionWaitRandom(string.Empty, 100, 200));
				break;
			case 2:
				AddAction(GetActionDelegate(delegate
				{
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.2f);
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f);
				}));
				AddAction(GetActionAttack(string.Empty, 5099));
				AddAction(GetActionDelegate(delegate
				{
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.2f);
					m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f);
				}));
				AddAction(GetActionWaitRandom(string.Empty, 100, 200));
				break;
			}
		}
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}
}
