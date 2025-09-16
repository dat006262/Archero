




using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class BattleModuleData
{
	private int layer;

	private long diamond;

	private int mLevel_CurrentStage;

	private int mRebornCount;

	private float gold;

	private List<LocalSave.EquipOne> mEquips = new List<LocalSave.EquipOne>();

	private int battle_ad_use_count;

	private TurnTableType mRewardType;

	private int reward_layer = -1;

	private long BossMaxHP;

	private long BossCurrentHP;

	private Dictionary<int, int> hittedcounts = new Dictionary<int, int>();

	private Dictionary<int, int> killmonsters = new Dictionary<int, int>();

	private Dictionary<int, int> killboss = new Dictionary<int, int>();

	private float game_starttime;

	private GameMode mMode = GameMode.eLevel;

	private int activeid;

	private ChallengeModeBase mChallenge;

	private bool bWin = true;

	private List<bool> mFirstShopBuy = new List<bool>();

	public int Level_CurrentStage
	{
		get
		{
			if (mLevel_CurrentStage == 0)
			{
                mLevel_CurrentStage = PlayerPrefs.GetInt("Level_CurrentStage_Local", 1);
            }
			int value = LocalSave.Instance.Stage_GetStage();
			value = MathDxx.Clamp(value, 1, LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter());
			if (mLevel_CurrentStage > value)
			{
				mLevel_CurrentStage = value;
			}
			return mLevel_CurrentStage;
		}
		set
		{
			if (value >= 1 && value <= LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter())
			{
				mLevel_CurrentStage = value;
				PlayerPrefs.SetInt("Level_CurrentStage_Local", value);
				Facade.Instance.SendNotification("MainUI_LayerUpdate");
			}
		}
	}

	public BattleSource mEnterSource
	{
		get;
		private set;
	}

	public GameModeBase mModeData
	{
		get;
		private set;
	}

	public int ActiveID
	{
		get
		{
			return activeid;
		}
		set
		{
			activeid = value;
		}
	}

	public Stage_Level_activity ActiveData
	{
		get
		{
			Stage_Level_activity beanById = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(activeid);
			SdkManager.Bugly_Report(beanById != null, "BattleModuleData_Active", Utils.FormatString("ActiveData [{0}] is null", activeid));
			return beanById;
		}
	}

	public Stage_Level_activitylevel ActiveLevelData
	{
		get
		{
			string activeLayer = GetActiveLayer();
			Stage_Level_activitylevel beanById = LocalModelManager.Instance.Stage_Level_activitylevel.GetBeanById(activeLayer);
			SdkManager.Bugly_Report(beanById != null, "BattleModuleData_Active", Utils.FormatString("ActiveLevelData [{0}] is null", activeLayer));
			return beanById;
		}
	}

	public bool Win => bWin;

	public void SetLayer(int layer)
	{
		this.layer = layer;
		if (GameLogic.Hold.Guide.GetNeedGuide())
		{
			return;
		}
		GameMode mode = GameLogic.Release.Mode.GetMode();
		if (mode == GameMode.eLevel)
		{
			if (GameLogic.Hold.Guide.mEquip.process == 0 && LocalSave.Instance.GetHaveEquips(havewear: true).Count == 1)
			{
				LocalSave.Instance.SaveExtra.AddEquipAllLayer();
			}
			LocalSave.Instance.SaveExtra.AddLayerCount(Level_CurrentStage, this.layer);
		}
	}

	public int GetLayer()
	{
		return layer;
	}

	public void AddDiamond(long value)
	{
		diamond += value;
	}

	public long GetDiamond()
	{
		return diamond;
	}

	public void InitState()
	{
		if (!PlayerPrefs.HasKey("Level_CurrentStage_Local"))
		{
			Level_CurrentStage = LocalSave.Instance.Stage_GetStage();
		}
	}

	public void RemoveStageLocal()
	{
		if (PlayerPrefs.HasKey("Level_CurrentStage_Local"))
		{
			PlayerPrefs.DeleteKey("Level_CurrentStage_Local");
		}
	}

	public void SetRebornCount(int value)
	{
		mRebornCount = value;
	}

	public int GetRebornCount()
	{
		return mRebornCount;
	}

	public bool GetCanReborn()
	{
		return mRebornCount > 0;
	}

	public void UseReborn()
	{
		mRebornCount--;
	}

	public void AddGold(float value)
	{
		gold += value;
		Facade.Instance.SendNotification("BATTLE_GET_GOLD", value);
	}

	private void UpdateGoldUI()
	{
	}

	public float GetGold()
	{
		return gold;
	}

	private void ResetGold()
	{
		if (LocalSave.Instance.BattleIn_GetIn())
		{
			gold = LocalSave.Instance.BattleIn_GetGold();
		}
		else
		{
			gold = 0f;
		}
		UpdateGoldUI();
	}

	public void AddEquip(LocalSave.EquipOne one)
	{
		AddEquipInternal(one);
		LocalSave.Instance.BattleIn_AddEquip(one);
	}

	public void AddEquipInternal(LocalSave.EquipOne one)
	{
		UnityEngine.Debug.Log("addequip " + one.EquipID);
		if (one.Overlying)
		{
			bool flag = false;
			int i = 0;
			for (int count = mEquips.Count; i < count; i++)
			{
				if (mEquips[i].EquipID == one.EquipID)
				{
					mEquips[i].Count += one.Count;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				mEquips.Insert(0, one);
			}
		}
		else
		{
			mEquips.Add(one);
		}
	}

	public List<LocalSave.EquipOne> GetEquips()
	{
		return mEquips;
	}

	public void Battle_ad_use()
	{
		battle_ad_use_count++;
	}

	public int GetRewardLayer()
	{
		return reward_layer;
	}

	public void SetRewardType(TurnTableType type)
	{
		mRewardType = type;
	}

	public TurnTableType GetRewardType()
	{
		return mRewardType;
	}

	private void reset_reward()
	{
		SetRewardType(TurnTableType.eInvalid);
		reward_layer = -1;
		if (mMode != GameMode.eLevel)
		{
			return;
		}
		Stage_Level_stagechapter beanByChapter = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(Level_CurrentStage);
		if (beanByChapter != null && beanByChapter.DropAddProb > 0 && GameLogic.Random(0, 100) < beanByChapter.DropAddProb && beanByChapter.DropAddCond.Length == 2)
		{
			int num = beanByChapter.DropAddCond[0];
			int num2 = beanByChapter.DropAddCond[1];
			if (num > num2)
			{
				num2 = num;
			}
			reward_layer = GameLogic.Random(num, num2);
		}
	}

	public void AddBossMaxHP(long hp)
	{
		BossMaxHP += hp;
		BossCurrentHP += hp;
		Facade.Instance.SendNotification("BATTLE_UI_BOSSHP_UPDATE", GetBossHPPercent());
	}

	public void BossChangeHP(long hp)
	{
		BossCurrentHP += hp;
		Facade.Instance.SendNotification("BATTLE_UI_BOSSHP_UPDATE", GetBossHPPercent());
		if (BossCurrentHP <= 0)
		{
			BossHPClear();
		}
	}

	private float GetBossHPPercent()
	{
		if (BossMaxHP == 0)
		{
			return 0f;
		}
		return (float)BossCurrentHP / (float)BossMaxHP;
	}

	public void BossHPClear()
	{
		BossMaxHP = 0L;
		BossCurrentHP = 0L;
	}

	public void AddHittedCount(int roomid)
	{
		if (!hittedcounts.ContainsKey(roomid))
		{
			hittedcounts.Add(roomid, 0);
		}
		Dictionary<int, int> dictionary;
		int key;
		(dictionary = hittedcounts)[key = roomid] = dictionary[key] + 1;
	}

	public int GetHittedCount(int layer)
	{
		int num = 0;
		for (int i = 1; i <= layer; i++)
		{
			int value = 0;
			if (hittedcounts.TryGetValue(i, out value))
			{
				num += value;
			}
		}
		return num;
	}

	public int GetHittedCount()
	{
		int num = 0;
		Dictionary<int, int>.Enumerator enumerator = hittedcounts.GetEnumerator();
		while (enumerator.MoveNext())
		{
			num += enumerator.Current.Value;
		}
		return num;
	}

	public void AddKillMonsters(int entityid)
	{
		if (!killmonsters.ContainsKey(entityid))
		{
			killmonsters.Add(entityid, 0);
		}
		Dictionary<int, int> dictionary;
		int key;
		(dictionary = killmonsters)[key = entityid] = dictionary[key] + 1;
	}

	public int GetKillMonsters(int entityid)
	{
		if (killmonsters.ContainsKey(entityid))
		{
			return killmonsters[entityid];
		}
		return 0;
	}

	public int GetKillMonsters()
	{
		Dictionary<int, int>.Enumerator enumerator = killmonsters.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			num += enumerator.Current.Value;
		}
		return num;
	}

	public void AddKillBoss(int entityid)
	{
		if (!killboss.ContainsKey(entityid))
		{
			killboss.Add(entityid, 0);
		}
		Dictionary<int, int> dictionary;
		int key;
		(dictionary = killboss)[key = entityid] = dictionary[key] + 1;
	}

	public int GetKillBoss(int entityid)
	{
		if (killboss.ContainsKey(entityid))
		{
			return killboss[entityid];
		}
		return 0;
	}

	public int GetKillBoss()
	{
		Dictionary<int, int>.Enumerator enumerator = killboss.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			num += enumerator.Current.Value;
		}
		return num;
	}

	public int GetGameTime()
	{
		return (int)(Updater.AliveTime - game_starttime);
	}

	public bool isEnterSourceMain()
	{
		return mEnterSource == BattleSource.eWorld;
	}

	public bool isEnterSourceMatch()
	{
		return mEnterSource == BattleSource.eMatch;
	}

	public bool isEnterSourceChallenge()
	{
		return mEnterSource == BattleSource.eChallenge;
	}

	public GameMode GetMode()
	{
		return mMode;
	}

	public void SetMode(GameMode mode, BattleSource source)
	{
		mEnterSource = source;
		mMode = mode;
		GameMode gameMode = mMode;
		if (gameMode == GameMode.eLevel)
		{
			mModeData = new GameModeLevel();
		}
		else if (source == BattleSource.eWorld)
		{
			mModeData = new GameModeLevel();
		}
		else
		{
			mModeData = new GameModeGold1();
		}
	}

	public void Reset()
	{
		battle_ad_use_count = 0;
		killmonsters.Clear();
		killboss.Clear();
		hittedcounts.Clear();
		game_starttime = Updater.AliveTime;
		ResetGold();
		reset_reward();
		InitShop();
		Challenge_Init(ActiveID);
		mEquips.Clear();
		BossMaxHP = 0L;
		BossCurrentHP = 0L;
		SetWin(value: true);
		diamond = 0L;
		GameMode gameMode = mMode;
		if (gameMode == GameMode.eLevel)
		{
			SetRebornCount(GameConfig.GetRebornCount() - LocalSave.Instance.BattleIn_GetRebornUI());
		}
		else
		{
			SetRebornCount(0);
		}
	}

	public string GetActiveLayer()
	{
		string stageLevel = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(activeid).StageLevel;
		return GetActiveLayer(stageLevel, GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID());
	}

	private string GetActiveLayer(int layerid)
	{
		string stageLevel = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(activeid).StageLevel;
		return GetActiveLayer(stageLevel, layerid);
	}

	public string GetActiveLayer(string stagelevel, int layerid)
	{
		return Utils.FormatString("{0}{1:D3}", stagelevel, layerid);
	}

	public Stage_Level_activitylevel GetActiveLevelData(int layer)
	{
		string activeLayer = GetActiveLayer(layer);
		Stage_Level_activitylevel beanById = LocalModelManager.Instance.Stage_Level_activitylevel.GetBeanById(activeLayer);
		SdkManager.Bugly_Report(beanById != null, "BattleModuleData_Active", Utils.FormatString("ActiveLevelData [{0}] is null", activeLayer));
		return beanById;
	}

	public void Challenge_Init(int id)
	{
		Challenge_DeInit();
		if (id != 0)
		{
			ActiveID = id;
			string[] args = ActiveData.Args;
			if (args.Length <= 0)
			{
				SdkManager.Bugly_Report("BattleModuleData", Utils.FormatString("InitChallengeData args length == 0."));
			}
			string[] array = args[0].Split(':');
			int num = int.Parse(array[0]);
			Type type = Type.GetType(Utils.GetString("ChallengeMode", num));
			mChallenge = (type.Assembly.CreateInstance(Utils.GetString("ChallengeMode", num)) as ChallengeModeBase);
			mChallenge.Init(ActiveData);
		}
	}

	public void Challenge_UpdateMode(int id)
	{
		Challenge_UpdateMode(id, BattleSource.eActivity);
	}

	public void Challenge_UpdateMode(int id, BattleSource source)
	{
		Challenge_Init(id);
		SetMode(ActiveData.GetMode(), source);
	}

	public void Challenge_MainUpdateMode(int id)
	{
		Challenge_UpdateMode(id, BattleSource.eWorld);
	}

	public bool Challenge_ismainchallenge()
	{
		if (mChallenge != null && mEnterSource == BattleSource.eWorld)
		{
			return true;
		}
		return false;
	}

	public void Challenge_Start()
	{
		if (mChallenge != null)
		{
			mChallenge.Start();
		}
	}

	public void Challenge_SetUIParent(Transform parent)
	{
		if (mChallenge != null)
		{
			mChallenge.SetUIParent(parent);
		}
	}

	public void Challenge_SendEvent(string eventname, object body = null)
	{
		if (mChallenge != null)
		{
			mChallenge.SendEvent(eventname, body);
		}
	}

	public object Challenge_GetEvent(string eventname)
	{
		if (mChallenge != null)
		{
			return mChallenge.GetEvent(eventname);
		}
		return null;
	}

	public string Challenge_GetSuccessString()
	{
		if (mChallenge != null)
		{
			return mChallenge.GetSuccessString();
		}
		return string.Empty;
	}

	public List<string> Challenge_GetConditions()
	{
		if (mChallenge != null)
		{
			return mChallenge.GetConditions();
		}
		return new List<string>();
	}

	public bool Challenge_RecoverHP()
	{
		if (mChallenge != null)
		{
			return mChallenge.RecoverHP;
		}
		return true;
	}

	public bool Challenge_DropExp()
	{
		if (mChallenge != null)
		{
			return mChallenge.DropExp;
		}
		return true;
	}

	public bool Challenge_AttackEnable()
	{
		if (mChallenge != null)
		{
			return mChallenge.AttackEnable;
		}
		return true;
	}

	public bool Challenge_BombermanEnable()
	{
		if (mChallenge != null)
		{
			return mChallenge.BombermanEnable;
		}
		return false;
	}

	public float Challenge_BombermanTime()
	{
		if (mChallenge != null)
		{
			return mChallenge.BombermanTime;
		}
		return 0.5f;
	}

	public void Challenge_MonsterDead()
	{
		if (mChallenge != null)
		{
			mChallenge.MonsterDead();
		}
	}

	public bool Challenge_MonsterHide()
	{
		if (mChallenge != null)
		{
			return mChallenge.GetMonsterHide();
		}
		return false;
	}

	public float Challenge_MonsterHideRange()
	{
		if (mChallenge != null)
		{
			return mChallenge.GetMonsterHideRange();
		}
		return float.MaxValue;
	}

	public void Challenge_GetRewards()
	{
		if (mChallenge != null)
		{
			mChallenge.GetRewards();
		}
	}

	public void Challenge_DeInit()
	{
		if (mChallenge != null)
		{
			mChallenge.DeInit();
			mChallenge = null;
		}
	}

	public bool IsHeroMode()
	{
		return IsHeroMode(Level_CurrentStage);
	}

	public bool IsHeroMode(int stageid)
	{
		return stageid > 10;
	}

	public void SetWin(bool value)
	{
		bWin = value;
	}

	private void InitShop()
	{
		InitFirstShop();
	}

	private void InitFirstShop()
	{
		mFirstShopBuy.Clear();
		for (int i = 0; i < 2; i++)
		{
			mFirstShopBuy.Add(item: false);
		}
	}

	public bool GetFirstShopBuy(int index)
	{
		if (index >= mFirstShopBuy.Count || index < 0)
		{
			SdkManager.Bugly_Report("BattleModuleData_Shop", Utils.FormatString("GetFirstShopBuy index = {0} is out range. mFirstShopBuy.Count = {1}", index, mFirstShopBuy.Count));
			return true;
		}
		return mFirstShopBuy[index];
	}

	public void SetFirstShopBuy(int index)
	{
		if (index >= mFirstShopBuy.Count || index < 0)
		{
			SdkManager.Bugly_Report("BattleModuleData_Shop", Utils.FormatString("SetFirstShopBuy index = {0} is out range. mFirstShopBuy.Count = {1}", index, mFirstShopBuy.Count));
		}
		else
		{
			mFirstShopBuy[index] = true;
		}
	}
}
