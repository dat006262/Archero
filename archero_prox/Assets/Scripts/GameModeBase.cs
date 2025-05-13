




using TableTool;

public abstract class GameModeBase
{
	public abstract string[] GetMonsterTmxAttributes();

	public abstract string[] GetMapAttributes();

	public abstract string[] GetTmxIds(int roomid, int roomcount);

	public abstract int GetMaxLayer();

	public abstract long GetMapStandardDefence();

	public abstract DropManager.DropData GetDropData();

	public abstract int GetDropDataGold(Soldier_soldier data);

	public abstract int GetDropDataEquipExp(Soldier_soldier data);
}
