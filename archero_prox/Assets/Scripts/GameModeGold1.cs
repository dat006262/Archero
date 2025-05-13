




using TableTool;

public class GameModeGold1 : GameModeBase
{
	public override long GetMapStandardDefence()
	{
		return GameLogic.Hold.BattleData.ActiveLevelData.StandardDefence;
	}

	public override string[] GetMapAttributes()
	{
		return GameLogic.Hold.BattleData.ActiveLevelData.MapAttributes;
	}

	public override string[] GetTmxIds(int roomid, int roomcount)
	{
		return GameLogic.Hold.BattleData.GetActiveLevelData(roomid).RoomIDs;
	}

	public override int GetMaxLayer()
	{
		return LocalModelManager.Instance.Stage_Level_activitylevel.GetMaxLayer();
	}

	public override string[] GetMonsterTmxAttributes()
	{
		return GameLogic.Hold.BattleData.ActiveLevelData.Attributes;
	}

	public override DropManager.DropData GetDropData()
	{
		Stage_Level_activity activeData = GameLogic.Hold.BattleData.ActiveData;
		DropManager.DropData dropData = new DropManager.DropData();
		dropData.EquipProb = activeData.EquipProb;
		return dropData;
	}

	public override int GetDropDataGold(Soldier_soldier data)
	{
		return data.GoldDropGold2;
	}

	public override int GetDropDataEquipExp(Soldier_soldier data)
	{
		return data.ScrollDropLevel;
	}
}
