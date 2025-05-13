




public class AI3100 : AIBase
{
	private bool call;

	private int count;

	private int range = 4;

	private int callid = 3002;

	protected override void OnInitOnce()
	{
		InitCallData(callid, 3, int.MaxValue, 1, 1, 10);
	}

	protected override void OnInit()
	{
		int num = GameLogic.Random(1, 3);
		for (int i = 0; i < num; i++)
		{
			AddAction(new AIMove1002(m_Entity, 500, 1000));
		}
		call = (GameLogic.Random(0, 100) < 50);
		if (call && GetCanCall(callid))
		{
			AddActionAddCall(callid, 1081);
		}
		bReRandom = true;
	}

	protected override void OnAIDeInit()
	{
	}
}
