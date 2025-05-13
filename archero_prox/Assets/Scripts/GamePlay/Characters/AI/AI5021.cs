




using Dxx.Util;

public class AI5021 : AIBase
{
	private WeightRandomCount weight = new WeightRandomCount(1, 4);

	private int ran;

	private int randomcount;

	protected override void OnInit()
	{
		randomcount++;
		if (randomcount == 1)
		{
			AddActionWait(1f);
		}
		if (randomcount <= 2)
		{
			ran = weight.GetRandom();
			if (ran == 3)
			{
				ran = weight.GetRandom();
			}
		}
		else
		{
			ran = weight.GetRandom();
		}
		switch (ran)
		{
		case 0:
			AddAction(GetActionAttack(string.Empty, 5049));
			break;
		case 1:
			AddAction(GetActionAttack(string.Empty, 5047));
			break;
		case 2:
			AddAction(new AIMove1041(m_Entity));
			break;
		case 3:
			AddAction(new AIMove1042(m_Entity, 2));
			break;
		}
		AddAction(new AIMove1043(m_Entity, GameLogic.Random(1500, 2500)));
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}
}
