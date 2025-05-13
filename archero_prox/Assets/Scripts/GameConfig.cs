




using Dxx.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TableTool;

public class GameConfig
{
	public class MapGoodData
	{
		private string[] attributes;

		private EntityAttributeBase.ValueFloatBase HPAddPercent = new EntityAttributeBase.ValueFloatBase();

		private EntityAttributeBase.ValueBase TrapHit = new EntityAttributeBase.ValueBase();

		private long StandardDefence;

		public void Init()
		{
			TrapHit.InitValueCount(GetTrapHitBase());
			int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
			if (currentRoomID > 0)
			{
				attributes = GameLogic.Hold.BattleData.mModeData.GetMapAttributes();
				StandardDefence = GameLogic.Hold.BattleData.mModeData.GetMapStandardDefence();
			}
			else
			{
				attributes = new string[0];
				StandardDefence = 0L;
			}
			AddAttributes();
		}

		private void AddAttributes()
		{
			int i = 0;
			for (int num = attributes.Length; i < num; i++)
			{
				Goods_goods.GoodData goodData = Goods_goods.GetGoodData(attributes[i]);
				switch (goodData.goodType)
				{
				case "HPAdd%":
					HPAddPercent.UpdateValuePercent(goodData.value);
					break;
				case "TrapHit%":
					TrapHit.UpdateValuePercent(goodData.value);
					break;
				}
			}
		}

		public float GetHPAddPercent()
		{
			return HPAddPercent.Value;
		}

		public long GetTrapHit()
		{
			return TrapHit.Value;
		}

		public long GetStandardDefence()
		{
			return StandardDefence;
		}
	}

	public const int HeroModeStage = 10;

	private static JObject config_game;

	private static int maxkeycount = -1;

	private static int adkeycount = -1;

	public const int HPItemCount = 4;

	private static MapGoodData _MapGood;

	public static int BattleAdUnlockTalentLevel => LocalModelManager.Instance.Config_config.GetValue<int>(3003);

	public static MapGoodData MapGood
	{
		get
		{
			if (_MapGood == null)
			{
				_MapGood = new MapGoodData();
			}
			return _MapGood;
		}
	}

	public static void Init()
	{
	}

	private static void Init_Game()
	{
		if (config_game == null)
		{
			string config = FileUtils.GetConfig("game_config.json");
			if (!string.IsNullOrEmpty(config))
			{
				try
				{
					config_game = (JObject)JsonConvert.DeserializeObject(config);
				}
				catch
				{
					config_game = null;
					IAMazonS3Manager.Instance.ClearFileName(Utils.FormatString("{0}/{1}", "data/config", "game_config.json"));
				}
			}
		}
	}

	private static bool TryGet_Game<T>(string name, out T result)
	{
		Init_Game();
		if (config_game != null)
		{
			JToken value = null;
			if (config_game.TryGetValue(name, out value))
			{
				result = value.ToObject<T>();
				return true;
			}
		}
		result = default(T);
		return false;
	}

	public static bool GetCanOpenRateUI()
	{
		return LocalSave.Instance.SaveExtra.overopencount == LocalModelManager.Instance.Config_config.GetValue<int>(10);
	}

	public static int GetMapStyleID(int RoomID)
	{
		RoomID--;
		return RoomID / 10 % 4 + 1;
	}

	public static bool GetFirstDeadRecover()
	{
		return GameLogic.Random(0, 100) < LocalModelManager.Instance.Config_config.GetValue<int>(20);
	}

	public static float GetArrowEject()
	{
		return LocalModelManager.Instance.Config_config.GetValue<float>(30);
	}

	public static float GetReboundHit()
	{
		return LocalModelManager.Instance.Config_config.GetValue<float>(40);
	}

	public static float GetReboundSpeed()
	{
		return LocalModelManager.Instance.Config_config.GetValue<float>(41);
	}

	public static int GetDemonMin()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(60);
	}

	public static int GetDemonPerHit()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(61);
	}

	public static int Get100AttackValue()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(70);
	}

	public static int Get100HPValue()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(71);
	}

	public static int GetFirstShopStartStage()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(90);
	}

	public static int GetEquipGuide_alllayer()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(120);
	}

	public static float GetEquipDropMaxRate()
	{
		return LocalModelManager.Instance.Config_config.GetValue<float>(130);
	}

	public static int GetHarvestMaxDay()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(140);
	}

	public static short GetKeyTrustCount()
	{
		return LocalModelManager.Instance.Config_config.GetValue<short>(150);
	}

	public static int GetMaxKeyCount()
	{
		if (maxkeycount > 0)
		{
			return maxkeycount;
		}
		int result = maxkeycount = LocalModelManager.Instance.Config_config.GetValue<int>(1001);
		if (TryGet_Game("key_max", out int result2))
		{
			result = (maxkeycount = result2);
		}
		return result;
	}

	public static int GetMaxKeyStartCount()
	{
		int result = LocalModelManager.Instance.Config_config.GetValue<int>(1001) * 2;
		if (TryGet_Game("key_max_start", out int result2))
		{
			result = result2;
		}
		return result;
	}

	public static int GetKeyRecoverTime()
	{
		int result = LocalModelManager.Instance.Config_config.GetValue<int>(1002);
		if (TryGet_Game("key_recover_second", out int result2))
		{
			result = result2;
		}
		return result;
	}

	public static int GetAdKeyCount()
	{
		if (adkeycount > 0)
		{
			return adkeycount;
		}
		int result = adkeycount = LocalModelManager.Instance.Config_config.GetValue<int>(1004);
		if (TryGet_Game("ad_key_max", out int result2))
		{
			result = (adkeycount = result2);
		}
		return result;
	}

	public static int GetModeLevelKey()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(1003);
	}

	public static float GetCoin1Wave()
	{
		return LocalModelManager.Instance.Config_config.GetValue<float>(80);
	}

	public static int GetKeyBuyDiamond()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(1043);
	}

	public static int GetRebornCount()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(91);
	}

	public static int GetRebornDiamond()
	{
		int rebornCount = GameLogic.Hold.BattleData.GetRebornCount();
		int rebornCount2 = GetRebornCount();
		return LocalModelManager.Instance.Config_config.GetValue<int>(92 + rebornCount2 - rebornCount);
	}

	public static int GetBoxDropGold()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(1011);
	}

	public static T GetValue<T>(int id)
	{
		return LocalModelManager.Instance.Config_config.GetValue<T>(id);
	}

	public static int GetBoxChooseGold(LocalSave.TimeBoxType type)
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>((int)type);
	}

	public static long GetBoxChooseTime(LocalSave.TimeBoxType type)
	{
		return LocalModelManager.Instance.Config_config.GetValue<long>((int)(type + 10));
	}

	public static float GetEquipUpgradeSellRatio()
	{
		return LocalModelManager.Instance.Config_config.GetValue<float>(1041);
	}

	public static float GetEquipOneRatio()
	{
		return LocalModelManager.Instance.Config_config.GetValue<float>(1042);
	}

	public static int GetTimeBoxTime(LocalSave.TimeBoxType type)
	{
		return GetValue<int>((int)(type + 10));
	}

	public static int GetEquipUnlockTalentLevel()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(3001);
	}

	public static int GetEquipExpUnlockTalentLevel()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(3002);
	}

	public static int GetTrapHitBase()
	{
		return LocalModelManager.Instance.Config_config.GetValue<int>(2001);
	}
}
