




public class AI3009 : AIBase
{
	public const int DivideCount = 2;

	protected override void OnInit()
	{
		AddAction(new AIMove1007(m_Entity));
	}

	protected override void OnAIDeInit()
	{
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}

	protected override void OnDeadBefore()
	{
		Divide(3010, 2);
	}
}
