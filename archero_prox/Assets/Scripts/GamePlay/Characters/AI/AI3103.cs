




public class AI3103 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1007(m_Entity));
	}

	protected override void OnAIDeInit()
	{
	}
}
