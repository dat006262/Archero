




using Dxx.Util;
using System.Collections.Generic;
using TableTool;

public class DropManager
{
	public class DropData
	{
		public int EquipProb;
	}

	private const int percentnumber = 100000000;

	private long level_dropequip;

	private DropData mDropData;

	private bool equip_talent_enable;

	private bool equipexp_talent_enable;

	private bool equip_must_drop;

	private BattleDropData battle_drop_temp;

	private List<Drop_DropModel.DropData> equiplist;

	private bool can_drop = true;

	public void Reset()
	{
		Reset_Level();
	}

	private void Reset_Level()
	{
		level_dropequip = LocalSave.Instance.SaveExtra.EquipDropRate;
		mDropData = GameLogic.Hold.BattleData.mModeData.GetDropData();
		equip_talent_enable = LocalSave.Instance.SaveExtra.Get_Equip_Drop();
		equipexp_talent_enable = LocalSave.Instance.SaveExtra.Get_EquipExp_Drop();
		equip_must_drop = LocalSave.Instance.GetEquipGuide_mustdrop();
	}

	public void GetRandomLevel(ref List<BattleDropData> list, Soldier_soldier data)
	{
		GetRandomEquipExp(ref list, data);
		GetRandomEquip(ref list, data);
	}

	public void GetRandomGoldHitted(ref List<BattleDropData> list, Soldier_soldier data)
	{
		int goldDropGold = data.GoldDropGold1;
		List<Drop_GoldModel.DropGold> dropList = LocalModelManager.Instance.Drop_Gold.GetDropList(goldDropGold);
		int i = 0;
		for (int count = dropList.Count; i < count; i++)
		{
			list.Add(new BattleDropData(FoodType.eGold, dropList[i].Gold));
		}
	}

	private void GetRandomGold(ref List<BattleDropData> list, Soldier_soldier data)
	{
		int dropDataGold = GameLogic.Hold.BattleData.mModeData.GetDropDataGold(data);
		List<Drop_GoldModel.DropGold> dropList = LocalModelManager.Instance.Drop_Gold.GetDropList(dropDataGold);
		int i = 0;
		for (int count = dropList.Count; i < count; i++)
		{
			int gold = dropList[i].Gold;
			gold = MathDxx.CeilToInt((float)gold * (1f + GameLogic.SelfAttribute.InGameGold.Value) * LocalModelManager.Instance.Stage_Level_stagechapter.GetGoldRate());
			list.Add(new BattleDropData(FoodType.eGold, gold));
		}
	}

	private void GetRandomEquipExp(ref List<BattleDropData> list, Soldier_soldier data)
	{
		if (!equipexp_talent_enable)
		{
			return;
		}
		List<int> equipExpDrop = LocalModelManager.Instance.Stage_Level_stagechapter.GetEquipExpDrop((EntityType)LocalModelManager.Instance.Character_Char.GetBeanById(data.CharID).TypeID);
		int i = 0;
		for (int count = equipExpDrop.Count; i < count; i++)
		{
			int num = equipExpDrop[i];
			if (num > 0)
			{
				int num2 = LocalModelManager.Instance.Equip_equip.RandomEquipExp();
				if (LocalSave.Instance.mEquip.Get_EquipExp_CanDrop(num2))
				{
					LocalSave.EquipOne newEquipByID = LocalSave.Instance.GetNewEquipByID(num2, num);
					list.Add(new BattleDropData(FoodType.eEquip, newEquipByID));
				}
			}
		}
	}

	private void GetRandomEquip(ref List<BattleDropData> list, Soldier_soldier data)
	{
		if (!equip_talent_enable)
		{
			return;
		}
		int num = (int)(data.EquipRate * (float)mDropData.EquipProb * 100f * (1f + GameLogic.Self.m_EntityData.attribute.EquipDrop.Value));
		if (equip_must_drop)
		{
			if (data.CharID <= 5000)
			{
				return;
			}
			List<BattleDropData> dropList = GetDropList();
			int i = 0;
			for (int count = dropList.Count; i < count; i++)
			{
				if (dropList[i].type == FoodType.eEquip && dropList[i].data != null)
				{
					LocalSave.EquipOne equipOne = dropList[i].data as LocalSave.EquipOne;
					if (equipOne != null && equipOne.data.Position != 1)
					{
						list.Add(dropList[i]);
						equip_must_drop = false;
						return;
					}
				}
			}
			GetRandomEquip(ref list, data);
		}
		else
		{
			level_dropequip += num;
			level_dropequip = MathDxx.Clamp(level_dropequip, 0L, (long)(1E+08f * GameConfig.GetEquipDropMaxRate()));
			if (GameLogic.Random(0, 100000000) < level_dropequip)
			{
				level_dropequip = 0L;
				List<BattleDropData> dropList2 = GetDropList();
				list.AddRange(dropList2);
			}
			LocalSave.Instance.SaveExtra.SetEquipDropRate(level_dropequip);
		}
	}

	private List<BattleDropData> GetDropList()
	{
		List<BattleDropData> list = new List<BattleDropData>();
		equiplist = LocalSave.Instance.mFakeStageDrop.GetDropList();
		if (equiplist != null && equiplist.Count > 0)
		{
			int i = 0;
			for (int count = equiplist.Count; i < count; i++)
			{
				Drop_DropModel.DropData dropData = equiplist[i];
				can_drop = true;
				LocalSave.EquipOne newEquipByID = LocalSave.Instance.GetNewEquipByID(dropData.id, dropData.count);
				if (!equipexp_talent_enable)
				{
					if (newEquipByID.Overlying)
					{
						can_drop = false;
					}
				}
				else if (newEquipByID.Overlying && !LocalSave.Instance.mEquip.Get_EquipExp_CanDrop(newEquipByID))
				{
					can_drop = false;
				}
				if (can_drop)
				{
					list.Add(new BattleDropData(FoodType.eEquip, newEquipByID));
				}
			}
		}
		return list;
	}
}
