




public class AI3086 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1006(m_Entity, 1800, 2500));
	}

	protected override void OnAIDeInit()
	{
	}
}
