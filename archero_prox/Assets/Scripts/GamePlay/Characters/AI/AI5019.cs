




public class AI5019 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1039(m_Entity, 8));
		if (GameLogic.Random(0, 100) < 50)
		{
			AddAction(new AIMove1038(m_Entity));
		}
		else
		{
			AddAction(GetActionAttackWait(5040, 1000, 1000));
		}
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}
}
