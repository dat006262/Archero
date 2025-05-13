




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

namespace TableTool
{
	public class Stage_Level_stagechapterModel : LocalModel<Stage_Level_stagechapter, int>
	{
		public class EquipExpDropDataOne : WeightRandomDataBase
		{
			public int count;

			public int min;

			public int max;

			public EquipExpDropDataOne(int id)
				: base(id)
			{
				count = id;
			}

			public List<int> GetRandom()
			{
				List<int> list = new List<int>();
				for (int i = 0; i < count; i++)
				{
					list.Add(GameLogic.Random(min, max + 1));
				}
				return list;
			}
		}

		public class EquipExpDropData
		{
			public Dictionary<int, WeightRandom<EquipExpDropDataOne>> soldiers = new Dictionary<int, WeightRandom<EquipExpDropDataOne>>();

			public Dictionary<int, WeightRandom<EquipExpDropDataOne>> bosss = new Dictionary<int, WeightRandom<EquipExpDropDataOne>>();

			private EquipExpDropDataOne one;

			private List<int> list = new List<int>();

			public void add(int stage, string[] data_soldiers, string[] data_bosss)
			{
				WeightRandom<EquipExpDropDataOne> weightRandom = new WeightRandom<EquipExpDropDataOne>();
				int i = 0;
				for (int num = data_soldiers.Length; i < num; i++)
				{
					EquipExpDropDataOne equipExpDropDataOne = get(data_soldiers[i]);
					weightRandom.Add(equipExpDropDataOne, equipExpDropDataOne.weight);
				}
				soldiers.Add(stage, weightRandom);
				WeightRandom<EquipExpDropDataOne> weightRandom2 = new WeightRandom<EquipExpDropDataOne>();
				int j = 0;
				for (int num2 = data_bosss.Length; j < num2; j++)
				{
					EquipExpDropDataOne equipExpDropDataOne2 = get(data_bosss[j]);
					weightRandom2.Add(equipExpDropDataOne2, equipExpDropDataOne2.weight);
				}
				bosss.Add(stage, weightRandom2);
			}

			public List<int> random(int stage, EntityType type)
			{
				list.Clear();
				if (type == EntityType.Boss)
				{
					one = bosss[stage].GetRandom();
				}
				else
				{
					one = soldiers[stage].GetRandom();
				}
				list.AddRange(one.GetRandom());
				return list;
			}

			private EquipExpDropDataOne get(string str)
			{
				string[] array = str.Split(',');
				if (array.Length != 4)
				{
					SdkManager.Bugly_Report("Stage_Level_stagechapterModel_Extra", "some equipexp rate is invalid != 4 ! EquipExpDropData");
				}
				int num = 0;
				num = int.Parse(array[0]);
				EquipExpDropDataOne equipExpDropDataOne = new EquipExpDropDataOne(num);
				equipExpDropDataOne.weight = int.Parse(array[1]);
				equipExpDropDataOne.min = int.Parse(array[2]);
				equipExpDropDataOne.max = int.Parse(array[3]);
				return equipExpDropDataOne;
			}
		}

		private const string _Filename = "Stage_Level_stagechapter";

		private Dictionary<int, int> mMaxLayers = new Dictionary<int, int>();

		private int maxChapter;

		private int styleid;

		private EquipExpDropData mEquipExp = new EquipExpDropData();

		protected override string Filename => "Stage_Level_stagechapter";

		protected override int GetBeanKey(Stage_Level_stagechapter bean)
		{
			return bean.ID;
		}

		public void Init()
		{
			maxChapter = 1;
			for (int i = 1; i < 100; i++)
			{
				Stage_Level_stagechapter beanByChapter = GetBeanByChapter(i);
				if (beanByChapter == null)
				{
					break;
				}
				maxChapter = i;
			}
			init_equipexp();
		}

		public int GetMaxChapter()
		{
			return maxChapter;
		}

		public int GetMaxChapter_Hero()
		{
			if (LocalSave.Instance.Stage_GetStage() <= 10)
			{
				return 10;
			}
			return maxChapter;
		}

		public int GetAllMaxLevel()
		{
			int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
			if (level_CurrentStage < 1)
			{
				return 0;
			}
			return GetAllMaxLevel(level_CurrentStage);
		}

		public int GetAllMaxLevel(int chapterId)
		{
			int value = 0;
			if (mMaxLayers.TryGetValue(chapterId, out value))
			{
				return value;
			}
			for (int i = 1; i <= chapterId; i++)
			{
				int currentMaxLevel = GetCurrentMaxLevel(i);
				value += currentMaxLevel;
			}
			mMaxLayers.Add(chapterId, value);
			return value;
		}

		public int GetCurrentMaxLevel(int chapter)
		{
			return LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_MaxLevel(chapter);
		}

		public string GetChapterFullName(int stageid)
		{
			if (!GameLogic.Hold.BattleData.IsHeroMode(stageid))
			{
				return Utils.FormatString("{0}.{1}", stageid, GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("ChapterName_{0}", stageid)));
			}
			string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("Chapter_HeroMode");
			return Utils.FormatString("{0}{1}.{2}", languageByTID, stageid, GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("ChapterName_{0}", stageid)));
		}

		public Color GetChapterColor(int stageid)
		{
			return (!GameLogic.Hold.BattleData.IsHeroMode(stageid)) ? Color.white : new Color(1f, 59f / 255f, 83f / 255f);
		}

		public int GetStartLevel()
		{
			int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
			return GetAllMaxLevel(level_CurrentStage) + 1;
		}

		public bool IsMaxLevel(int roomid)
		{
			return IsMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage, roomid);
		}

		public bool IsMaxLevel(int chapterId, int roomid)
		{
			int currentMaxLevel = GetCurrentMaxLevel(chapterId);
			return roomid >= currentMaxLevel;
		}

		public Stage_Level_stagechapter GetBeanByChapter(int chapter)
		{
			return GetBeanById(chapter + 100);
		}

		public int GetTiledID(int chapter)
		{
			return GetBeanByChapter(chapter).TiledID;
		}

		public Stage_Level_stagechapter GetCurrentStageLevel()
		{
			return GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage);
		}

		public string[] GetStageLevelTmxIds(int stage, int layer)
		{
			return GetStageLevelTmxIds(stage, layer);
		}

		public string[] GetStageLevelTmxIds(int layer)
		{
			return GetStageLevelTmxIds(GameLogic.Hold.BattleData.Level_CurrentStage, layer);
		}

		public string[] GetStageLevelAttributes(int layer)
		{
			return GetStageLevelAttributes(GameLogic.Hold.BattleData.Level_CurrentStage, layer);
		}

		public string[] GetStageLevelAttributes(int stage, int layer)
		{
			return LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_Attributes(stage, layer);
		}

		public string[] GetStageLevelMapAttributes(int stage, int layer)
		{
			return LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_MapAttributes(stage, layer);
		}

		public long GetStageLevelStandardDefence(int stage, int layer)
		{
			return LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_Defence(stage, layer);
		}

		public string GetStartTmx()
		{
			return string.Empty;
		}

		public float GetGoldDropPercent(int layer)
		{
			float result = 1f;
			string[] stageLevelAttributes = GetStageLevelAttributes(layer);
			int i = 0;
			for (int num = stageLevelAttributes.Length; i < num; i++)
			{
				Goods_goods.GoodData goodData = Goods_goods.GetGoodData(stageLevelAttributes[i]);
				if (goodData.goodType == "GoldDrop%")
				{
					result = 1f + (float)goodData.value / 100f;
					break;
				}
			}
			return result;
		}

		public int GetMonsterDropID()
		{
			return 0;
		}

		public int GetBossDropID()
		{
			return 0;
		}

		public int GetEquipDropID(int chapterid)
		{
			return GetBeanByChapter(chapterid).EquipDropID;
		}

		public int GetEquipDropRate(int chapterid)
		{
			return GetBeanByChapter(chapterid).EquipProb;
		}

		public float GetScoreRate()
		{
			return GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).IntegralRate;
		}

		private void init_equipexp()
		{
			IList<Stage_Level_stagechapter> allBeans = GetAllBeans();
			int i = 0;
			for (int count = allBeans.Count; i < count; i++)
			{
				Stage_Level_stagechapter stage_Level_stagechapter = allBeans[i];
				mEquipExp.add(stage_Level_stagechapter.ID - 100, stage_Level_stagechapter.ScrollRate, stage_Level_stagechapter.ScrollRateBoss);
			}
		}

		public List<int> GetEquipExpDrop(EntityType type)
		{
			return mEquipExp.random(GameLogic.Hold.BattleData.Level_CurrentStage, type);
		}

		public float GetGoldRate()
		{
			return GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).GoldRate;
		}

		public int GetStyleID()
		{
			int result = 1;
			string text = "0101";
			if (GameLogic.Release.Mode != null && GameLogic.Release.Mode.RoomGenerate != null && GameLogic.Hold != null && GameLogic.Hold.BattleData != null)
			{
				int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
				text = GetStyleString(currentRoomID);
				text = GetStyleString(currentRoomID).Substring(0, 2);
				int.TryParse(text, out result);
			}
			return result;
		}

		public string GetStyleString(int roomid)
		{
			string[] array = new string[0];
			array = ((GameLogic.Hold.BattleData.GetMode() != GameMode.eLevel) ? LocalModelManager.Instance.Stage_Level_activity.GetBeanById(GameLogic.Hold.BattleData.ActiveID).StyleSequence : GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).StyleSequence);
			return GetStyleStringInternal(array, roomid);
		}

		private string GetStyleStringInternal(string[] seq, int roomid)
		{
			int num = seq.Length;
			roomid--;
			if (roomid <= 0)
			{
				roomid = 1;
			}
			roomid = roomid / 10 % num;
			return seq[roomid];
		}

		public int GetExp()
		{
			int result = 0;
			if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel)
			{
				Stage_Level_stagechapter beanByChapter = GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage);
				int layer = GameLogic.Hold.BattleData.GetLayer();
				result = beanByChapter.ExpBase + layer * beanByChapter.ExpAdd;
			}
			return result;
		}

		public bool is_wave_room()
		{
			return is_wave_room(GameLogic.Hold.BattleData.Level_CurrentStage);
		}

		public bool is_wave_room(int chapter)
		{
			return GetBeanByChapter(chapter).GameType == 1;
		}

		public int waveroom_get_monsterwave()
		{
			int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
			return GetBeanByChapter(level_CurrentStage).GameArgs[0];
		}

		public int waveroom_get_monsterwave_time()
		{
			int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
			return GetBeanByChapter(level_CurrentStage).GameArgs[1];
		}

		public int waveroom_get_bosswave()
		{
			int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
			return GetBeanByChapter(level_CurrentStage).GameArgs[2];
		}

		public int waveroom_get_bosswave_time()
		{
			int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
			return GetBeanByChapter(level_CurrentStage).GameArgs[3];
		}
	}
}
