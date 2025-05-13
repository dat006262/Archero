




public class AI3011 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1011(m_Entity));
	}

	protected override void OnAIDeInit()
	{
	}
}
