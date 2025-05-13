




using System.Collections.Generic;
using TableTool;

public class SelfAttributeData
{
	public EntityAttributeBase attribute;

	private List<string> levelups = new List<string>();

	private List<LocalSave.EquipOne> mEquips = new List<LocalSave.EquipOne>();

	public EntityAttributeBase.ValueFloatBase InGameGold = new EntityAttributeBase.ValueFloatBase();

	public EntityAttributeBase.ValueFloatBase InGameExp = new EntityAttributeBase.ValueFloatBase();

	public EntityAttributeBase.ValueFloatBase Up_Weapon = new EntityAttributeBase.ValueFloatBase();

	public EntityAttributeBase.ValueFloatBase Up_Hero = new EntityAttributeBase.ValueFloatBase();

	public EntityAttributeBase.ValueFloatBase Up_Armor = new EntityAttributeBase.ValueFloatBase();

	public EntityAttributeBase.ValueFloatBase Up_Pet = new EntityAttributeBase.ValueFloatBase();

	public EntityAttributeBase.ValueFloatBase Up_Ornament = new EntityAttributeBase.ValueFloatBase();

	public void Init()
	{
		InitAttribute();
	}

	private void InitAttribute()
	{
		attribute = new EntityAttributeBase(1001);
		attribute.Excute("HPMax + 600");
		attribute.Excute("Attack + 150");
		InitCards();
		InitEquips();
	}

	private void InitEquips()
	{
		LocalSave.Instance.Equip_Attribute2(this);
	}

	public void InitBabies()
	{
		if (GameLogic.InGame)
		{
			List<LocalSave.EquipOne> list = LocalSave.Instance.Equip_get_equip_babies();
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				LocalSave.EquipOne equipOne = list[i];
				GameLogic.Self.AddSkillBaby(equipOne.EquipID, equipOne);
			}
		}
	}

	public void ClearBattle()
	{
		InGameGold.InitValue(0L, 0L);
		InGameExp.InitValue(0L, 0L);
		Up_Weapon.InitValue(0L, 0L);
		Up_Hero.InitValue(0L, 0L);
		Up_Armor.InitValue(0L, 0L);
		Up_Pet.InitValue(0L, 0L);
		Up_Ornament.InitValue(0L, 0L);
	}

	private void ClearCards()
	{
		levelups.Clear();
	}

	private void InitCards()
	{
		ClearCards();
		List<LocalSave.CardOne> wearCards = LocalSave.Instance.GetWearCards();
		int i = 0;
		for (int count = wearCards.Count; i < count; i++)
		{
			List<string> attributes = LocalModelManager.Instance.Skill_slotout.GetAttributes(wearCards[i]);
			int j = 0;
			for (int count2 = attributes.Count; j < count2; j++)
			{
				Excute(attributes[j]);
			}
		}
	}

	public void Attribute2LevelUp(EntityData data)
	{
		int i = 0;
		for (int count = levelups.Count; i < count; i++)
		{
			data.ExcuteAttributes(levelups[i]);
		}
	}

	public float GetUpPercent(int position)
	{
		switch (position)
		{
		case 0:
			return Up_Hero.Value;
		case 1:
			return Up_Weapon.Value;
		case 2:
			return Up_Armor.Value;
		case 5:
			return Up_Ornament.Value;
		case 6:
			return Up_Pet.Value;
		default:
			return 0f;
		}
	}

	public void Excute(string att)
	{
		Goods_goods.GoodData goodData = Goods_goods.GetGoodData(att);
		if (!Excute(goodData.goodType, goodData.value) && !attribute.Excute(goodData) && att.Contains("LevelUp:"))
		{
			levelups.Add(att);
		}
	}
    //@TODO CANNOT DECODE _003C_003Ef__switch_0024map5
    Dictionary<string, int> _003C_003Ef__switch_0024map5;
    public bool Excute(string type, long value)
	{
		bool result = true;
		if (type != null)
		{
			if (_003C_003Ef__switch_0024map5 == null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
				dictionary.Add("Global_HarvestLevel", 0);
				dictionary.Add("Global_InGameGold%", 1);
				dictionary.Add("Global_InGameExp%", 2);
				dictionary.Add("Global_UP_Weapon%", 3);
				dictionary.Add("Global_UP_Hero%", 4);
				dictionary.Add("Global_UP_Armor%", 5);
				dictionary.Add("Global_UP_Pet%", 6);
				dictionary.Add("Global_UP_Ornament%", 7);
				dictionary.Add("Global_UP_EquipAll%", 8);
				_003C_003Ef__switch_0024map5 = dictionary;
			}
			if (_003C_003Ef__switch_0024map5.TryGetValue(type, out int value2))
			{
				switch (value2)
				{
				case 1:
					InGameGold.UpdateValuePercent(value);
					goto IL_0189;
				case 2:
					InGameExp.UpdateValuePercent(value);
					goto IL_0189;
				case 3:
					Up_Weapon.UpdateValuePercent(value);
					goto IL_0189;
				case 4:
					Up_Hero.UpdateValuePercent(value);
					goto IL_0189;
				case 5:
					Up_Armor.UpdateValuePercent(value);
					goto IL_0189;
				case 6:
					Up_Pet.UpdateValuePercent(value);
					goto IL_0189;
				case 7:
					Up_Ornament.UpdateValuePercent(value);
					goto IL_0189;
				case 8:
					Up_Weapon.UpdateValuePercent(value);
					Up_Armor.UpdateValuePercent(value);
					Up_Ornament.UpdateValuePercent(value);
					Up_Pet.UpdateValuePercent(value);
					goto IL_0189;
				case 0:
					goto IL_0189;
				}
			}
		}
		result = false;
		goto IL_0189;
		IL_0189:
		return result;
	}
}
