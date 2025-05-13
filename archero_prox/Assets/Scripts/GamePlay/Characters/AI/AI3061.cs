




public class AI3061 : AIBase
{
	public const int DivideCount = 3;

	private int randomcount;

	protected override void OnInit()
	{
		if (randomcount == 0)
		{
			AddAction(GetActionWait(string.Empty, 1000));
		}
		AddAction(new AIMove1034(m_Entity));
		AddAction(new AIMove1034(m_Entity));
		AddAction(new AIMove1034(m_Entity));
		AddAction(new AIMove1034(m_Entity));
		AddAction(GetActionWait(string.Empty, 500));
		AddAction(GetActionAttackSpecial(string.Empty, 5038, rotate: false));
		randomcount++;
		bReRandom = true;
	}

	protected override void OnDeadBefore()
	{
		Divide(3062, 3);
	}
}
