




using System.Collections.Generic;

public class DropDefault : DropBase
{
	protected override void OnInit()
	{
	}

	protected override List<BattleDropData> OnGetDropDead()
	{
		GameLogic.Hold.Drop.GetRandomLevel(ref mList, m_Data);
		return mList;
	}

	protected override List<BattleDropData> OnGetHittedList(long hit)
	{
		return null;
	}
}
