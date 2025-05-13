




public class AI3076 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1056(m_Entity, 1000, 0f, 0.86f));
		AddAction(GetActionWaitRandom(string.Empty, 800, 800));
	}

	protected override void OnAIDeInit()
	{
	}

	protected override void OnDeadBefore()
	{
		base.OnDeadBefore();
		if (m_Entity.IsElite)
		{
			Divide(3006, 2);
		}
	}
}
