




using System.Collections.Generic;
using TableTool;

public abstract class DropBase
{
	protected Soldier_soldier m_Data;

	protected List<BattleDropData> mList = new List<BattleDropData>();

	protected long MaxHP;

	protected long currentHP;

	public void Init(Soldier_soldier data, long hp)
	{
		m_Data = data;
		MaxHP = hp;
		currentHP = hp;
		OnInit();
	}

	public List<BattleDropData> GetDropDead()
	{
		return OnGetDropDead();
	}

	public List<BattleDropData> GetHittedList(long hit)
	{
		return OnGetHittedList(hit);
	}

	protected abstract void OnInit();

	protected abstract List<BattleDropData> OnGetDropDead();

	protected abstract List<BattleDropData> OnGetHittedList(long hit);
}
