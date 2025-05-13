




public class AI5017 : AIBase
{
	private int attackcount;

	private int[] attackids = new int[3]
	{
		5035,
		5125,
		5126
	};

	protected override void OnInit()
	{
		AddAction(new AIMove1029(m_Entity, 3));
		int num = GameLogic.Random(0, attackids.Length);
		AddAction(GetActionAttack(string.Empty, attackids[num]));
		bReRandom = true;
	}
}
