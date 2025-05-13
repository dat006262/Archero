




public class AI5022 : AIBase
{
	protected override void OnInit()
	{
		int num = GameLogic.Random(0, 3);
		for (int i = 0; i < num; i++)
		{
			AddAction(new AIMove1002(m_Entity, GameLogic.Random(500, 1000)));
		}
		switch (GameLogic.Random(0, 2))
		{
		case 0:
			AddAction(GetActionAttack(string.Empty, 5050));
			break;
		case 1:
			AddAttack5052();
			break;
		}
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}

	private void AddAttack5052()
	{
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.6f);
		}));
		AddAction(GetActionAttack(string.Empty, 5052));
		AddAction(GetActionDelegate(delegate
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.6f);
		}));
	}
}
