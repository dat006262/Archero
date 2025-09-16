




using System.Collections.Generic;

public class DropGold : DropBase
{
	private List<BattleDropData> mHittedList = new List<BattleDropData>();

	private int listcount;

	private int allcount;

	protected override void OnInit()
	{
		GameLogic.Hold.Drop.GetRandomGoldHitted(ref mList, m_Data);
		listcount = mList.Count;
		allcount = listcount;
	}

	protected override List<BattleDropData> OnGetHittedList(long hit)
	{
		mHittedList.Clear();
		currentHP += hit;
		float num = (float)currentHP / (float)MaxHP;
		int num2 = (int)(num * (float)listcount);
		int num3 = allcount - num2;
		allcount = num2;
		for (int i = 0; i < num3; i++)
		{
			mHittedList.Add(mList[0]);
			mList.RemoveAt(0);
		}
		return mHittedList;
	}

	protected override List<BattleDropData> OnGetDropDead()
	{
		GameLogic.Hold.Drop.GetRandomLevel(ref mList, m_Data);
		return mList;
	}
}
