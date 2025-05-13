




public class AI3089 : AIBase
{
	public const int DivideCount = 2;

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		if (m_Entity.IsElite)
		{
			m_Entity.m_EntityData.ExcuteAttributes("MoveSpeed%", 7000L);
		}
	}

	protected override void OnInit()
	{
		AddAction(new AIMove1007(m_Entity));
	}

	protected override void OnAIDeInit()
	{
	}

	protected override void OnDeadBefore()
	{
		if (m_Entity.IsElite)
		{
			Divide(3103, 3);
		}
		else
		{
			Divide(3103, 2);
		}
	}
}
