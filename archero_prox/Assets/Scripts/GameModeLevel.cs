




using TableTool;

public class GameModeLevel : GameModeBase
{
	public override long GetMapStandardDefence()
	{
		if (GameLogic.Release.Mode == null || GameLogic.Release.Mode.RoomGenerate == null)
		{
			return 0L;
		}
		int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
		int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
		return LocalModelManager.Instance.Stage_Level_stagechapter.GetStageLevelStandardDefence(level_CurrentStage, currentRoomID);
	}

	public override string[] GetMapAttributes()
	{
		if (GameLogic.Release.Mode == null || GameLogic.Release.Mode.RoomGenerate == null)
		{
			return new string[0];
		}
		int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
		int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
		return LocalModelManager.Instance.Stage_Level_stagechapter.GetStageLevelMapAttributes(level_CurrentStage, currentRoomID);
	}

	public override int GetMaxLayer()
	{
		int num = 0;
		if (GameLogic.Hold.BattleData.Challenge_ismainchallenge())
		{
			return GameLogic.Hold.BattleData.ActiveData.MaxLayer;
		}
		return LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage);
	}

	public override string[] GetTmxIds(int roomid, int roomcount)
	{
		int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
		return LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_RoomIds(level_CurrentStage, roomid, roomcount);
	}

	public override string[] GetMonsterTmxAttributes()
	{
		if (GameLogic.Release.Mode == null || GameLogic.Release.Mode.RoomGenerate == null)
		{
			return new string[0];
		}
		int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
		int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
		return LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_Attributes(level_CurrentStage, currentRoomID);
	}

	public override DropManager.DropData GetDropData()
	{
		Stage_Level_stagechapter currentStageLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentStageLevel();
		DropManager.DropData dropData = new DropManager.DropData();
		dropData.EquipProb = currentStageLevel.EquipProb;
		return dropData;
	}

	public override int GetDropDataGold(Soldier_soldier data)
	{
		return data.GoldDropLevel;
	}

	public override int GetDropDataEquipExp(Soldier_soldier data)
	{
		return data.ScrollDropLevel;
	}
}
