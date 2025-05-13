




public class AI5014 : AIBase
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
		Divide(3058, 2);
	}
}
