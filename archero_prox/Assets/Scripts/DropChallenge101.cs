




using System.Collections.Generic;

public class DropChallenge101 : DropBase
{
	protected override void OnInit()
	{
	}

	protected override List<BattleDropData> OnGetDropDead()
	{
		return mList;
	}

	protected override List<BattleDropData> OnGetHittedList(long hit)
	{
		return null;
	}
}
