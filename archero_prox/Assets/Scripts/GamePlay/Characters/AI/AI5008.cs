




public class AI5008 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1032(m_Entity, 1f, 1000));
		AddAction(new AIMove1032(m_Entity, 1f, 1000));
		int num = GameLogic.Random(1, 4);
		for (int i = 0; i < num; i++)
		{
			AddAction(new AIMove1032(m_Entity, 1f, 1000));
		}
		int num2 = GameLogic.Random(1, 4);
		for (int j = 0; j < num2; j++)
		{
			AddAction(GetActionAttack(string.Empty, 5016 + GameLogic.Random(0, 2)));
		}
		bReRandom = true;
	}
}
