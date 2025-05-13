




using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class GameFormManager
{
	public class WeightData
	{
		public int EventID;

		public int Weight;
	}

	private Dictionary<string, List<WeightData>> weightList = new Dictionary<string, List<WeightData>>();

	public const string AngelSkill = "AngelSkill";

	public const string DemonSkill = "DemonSkill";

	public const string GameTurntable = "GameTurntable";

	public const string GreedySkillBig = "GreedySkillBig";

	public const string GreedySkillSmall = "GreedySkillSmall";

	public const string LevelDropIn = "LevelDropIn";

	public GameFormManager()
	{
		Init();
	}

	private void Init()
	{
		weightList.Add("AngelSkill", new List<WeightData>());
		weightList.Add("DemonSkill", new List<WeightData>());
		weightList.Add("GameTurntable", new List<WeightData>());
		weightList.Add("GreedySkillBig", new List<WeightData>());
		weightList.Add("GreedySkillSmall", new List<WeightData>());
		weightList.Add("LevelDropIn", new List<WeightData>());
		IList<Room_eventangelskill> allBeans = LocalModelManager.Instance.Room_eventangelskill.GetAllBeans();
		int i = 0;
		for (int count = allBeans.Count; i < count; i++)
		{
			weightList["AngelSkill"].Add(new WeightData
			{
				EventID = allBeans[i].EventID,
				Weight = allBeans[i].Weight
			});
		}
		IList<Room_eventdemontext2skill> allBeans2 = LocalModelManager.Instance.Room_eventdemontext2skill.GetAllBeans();
		int j = 0;
		for (int count2 = allBeans2.Count; j < count2; j++)
		{
			weightList["DemonSkill"].Add(new WeightData
			{
				EventID = allBeans2[j].EventID,
				Weight = allBeans2[j].Weight
			});
		}
		IList<Room_eventgameturn> allBeans3 = LocalModelManager.Instance.Room_eventgameturn.GetAllBeans();
		int k = 0;
		for (int count3 = allBeans3.Count; k < count3; k++)
		{
			weightList["GameTurntable"].Add(new WeightData
			{
				EventID = allBeans3[k].EventID,
				Weight = allBeans3[k].Weight
			});
		}
		IList<Skill_greedyskill> allBeans4 = LocalModelManager.Instance.Skill_greedyskill.GetAllBeans();
		int l = 0;
		for (int count4 = allBeans4.Count; l < count4; l++)
		{
			Skill_greedyskill skill_greedyskill = allBeans4[l];
			if (skill_greedyskill.Type != 2)
			{
				string key = (skill_greedyskill.Type != 0) ? "GreedySkillSmall" : "GreedySkillBig";
				weightList[key].Add(new WeightData
				{
					EventID = skill_greedyskill.SkillID,
					Weight = skill_greedyskill.Weight
				});
			}
		}
		IList<Skill_dropin> allBeans5 = LocalModelManager.Instance.Skill_dropin.GetAllBeans();
		int m = 0;
		for (int count5 = allBeans5.Count; m < count5; m++)
		{
			Skill_dropin skill_dropin = allBeans5[m];
			weightList["LevelDropIn"].Add(new WeightData
			{
				EventID = skill_dropin.ID,
				Weight = skill_dropin.Weight
			});
		}
	}

	public void InitData()
	{
	}

	private int GetSum(List<WeightData> list)
	{
		int num = 0;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			num += list[i].Weight;
		}
		return num;
	}

	public int GetRandomID(string name)
	{
		List<WeightData> list = weightList[name];
		if (list.Count == 0)
		{
			return 0;
		}
		int sum = GetSum(list);
		int num = Random.Range(0, sum);
		int index = 0;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			int weight = list[i].Weight;
			if (num < weight)
			{
				index = i;
				break;
			}
			num -= weight;
		}
		return list[index].EventID;
	}

	public void RemoveID(string name, int EventID)
	{
		List<WeightData> list = weightList[name];
		int num = 0;
		int count = list.Count;
		WeightData weightData;
		while (true)
		{
			if (num < count)
			{
				weightData = list[num];
				if (weightData.EventID == EventID)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		list.Remove(weightData);
	}

	public List<int> GetGreedySkill(string name, int count)
	{
		List<int> list = new List<int>();
		List<WeightData> list2 = new List<WeightData>();
		int i = 0;
		for (int count2 = weightList[name].Count; i < count2; i++)
		{
			list2.Add(weightList[name][i]);
		}
		for (int j = 0; j < count; j++)
		{
			int sum = GetSum(list2);
			int num = Random.Range(0, sum);
			for (int k = 0; k < list2.Count; k++)
			{
				WeightData weightData = list2[k];
				if (num < weightData.Weight)
				{
					list.Add(weightData.EventID);
					sum -= weightData.Weight;
					list2.RemoveAt(k);
					break;
				}
				num -= weightData.Weight;
			}
		}
		return list;
	}

	public void Release()
	{
		weightList.Clear();
		Init();
	}
}
