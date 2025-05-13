




public class AI3075 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1006(m_Entity, 800, 1000));
	}

	protected override void OnAIDeInit()
	{
	}
}
