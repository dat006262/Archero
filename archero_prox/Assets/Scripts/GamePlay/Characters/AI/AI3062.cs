




public class AI3062 : AIBase
{
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
		AddAction(GetActionAttackSpecial(string.Empty, 1040, rotate: false));
		randomcount++;
		bReRandom = true;
	}
}
