




public class AI5018 : AIBase
{
	public const int DivideCount = 2;

	protected override void OnInit()
	{
		AddAction(new AIMove1034(m_Entity));
		AddAction(new AIMove1034(m_Entity));
		AddAction(new AIMove1034(m_Entity));
		AddAction(new AIMove1034(m_Entity));
		AddAction(GetActionWait(string.Empty, 500));
		AddAction(GetActionAttackSpecial(string.Empty, 5038, rotate: false));
	}

	protected override void OnDeadBefore()
	{
		Divide(3061, 2);
	}
}
