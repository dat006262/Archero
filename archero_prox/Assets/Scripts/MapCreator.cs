




using DG.Tweening;
using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TableTool;
using UnityEngine;

public class MapCreator
{
	public class CreateData
	{
		public EntityBase parent;

		public int entityid;

		public Vector2Int v;

		public float x;

		public float y;

		public bool m_bElite;

		public bool bDivide;

		public bool bCall;

		public string path => Utils.GetString(mResList[GetGoodType(entityid)], entityid);
	}

	public class Transfer
	{
		public RoomControlBase roomctrl;

		public int roomid;

		public int resourcesid;

		public string tmxid;

		public bool delay;

		public RoomGenerateBase.RoomType roomtype;

		public override string ToString()
		{
			return Utils.FormatString("RoomID:{0}, Res:{1}, TmxID:{2}, Delay:{3}, RoomType:{4}", roomid, resourcesid, tmxid, delay, roomtype);
		}
	}

	public enum MapGoodType
	{
		Empty,
		Goods,
		Soldier,
		Boss,
		Event,
		Tower
	}

	private class HeroModeData
	{
		public Vector2Int roompos;

		public int entityid;

		public bool m_bElite;
	}

	public Action Event_Button1101;

	private static Dictionary<MapGoodType, string> mResList = new Dictionary<MapGoodType, string>
	{
		{
			MapGoodType.Soldier,
			"Game/Soldier/SoldierNode"
		},
		{
			MapGoodType.Boss,
			"Game/Boss/BossNode"
		},
		{
			MapGoodType.Tower,
			"Game/Tower/TowerNode"
		}
	};

	private Dictionary<string, string> mMapStrings = new Dictionary<string, string>();

	private const int TrapUpStart = 2001;

	private const int TrapUpEnd = 2002;

	private const int TrapDownStart = 2003;

	private const int TrapDownEnd = 2004;

	private const int TrapLeftStart = 2005;

	private const int TrapLeftEnd = 2006;

	private const int TrapRightStart = 2007;

	private const int TrapRightEnd = 2008;

	private const int WaterID = 1006;

	private SequencePool mSequencePool = new SequencePool();

	private string MapID;

	private RoomGenerateBase.RoomType mRoomType;

	private RoomControlBase mRoomCtrl;

	public int width;

	public int height;

	private Vector2 CombineOffset;

	private Dictionary<int, Goods_goods> GoodsList = new Dictionary<int, Goods_goods>();

	private Dictionary<int, int> TileMap2Goods = new Dictionary<int, int>();

	private int[,] tiledata;

	private int[,] RoomRealRect;

	private int[,] findpathRect;

	private TMXGoodsData[,] TmxGoodsList;

	private int roomid;

	private List<GameObject> mGoodsList = new List<GameObject>();

	private Transfer mTransfer;

	private static int count = 0;

	public FindPath Bomberman_path;

	private int[,] bomberman_rect;

	private int[,] bomberman_danger;

	private int[,] mCallRect;

	public const int Good_StoneID = 1001;

	public const int Good_GrassID = 1007;

	public const int Good_WoodID = 1009;

	public const int Good_FenceID = 1008;

	private Dictionary<int, Dictionary<int, WeightRandom>> mElementData = new Dictionary<int, Dictionary<int, WeightRandom>>();

	private Dictionary<int, string> mWeightStrings = new Dictionary<int, string>
	{
		{
			1,
			"1,7;2,1|1,7;2,1|1,1;2,1|1,1"
		},
		{
			2,
			"1,15;2,1;3,1|1,9;2,1|1,1|1,1"
		},
		{
			3,
			"1,7;2,1|1,7;2,1|1,1|1,1"
		},
		{
			4,
			"1,7;2,1|1,7;2,1|1,1|1,1"
		},
		{
			5,
			"1,15;2,1|1,9;2,1|1,1|1,1"
		},
		{
			6,
			"1,7;2,1|1,7;2,1|1,1|1,1"
		},
		{
			7,
			"1,7;2,1|1,7;2,1|1,1;2,1|1,1"
		},
		{
			8,
			"1,15;2,1;3,1|1,9;2,1|1,1|1,1"
		},
		{
			9,
			"1,7;2,1|1,7;2,1|1,1|1,1"
		},
		{
			10,
			"1,7;2,1|1,7;2,1|1,1;2,1|1,1"
		},
		{
			11,
			"1,15;2,1|1,9;2,1|1,1|1,1"
		},
		{
			12,
			"1,7;2,1|1,7;2,1|1,1|1,1"
		}
	};

	private Dictionary<Vector2Int, HeroModeData> elitelist = new Dictionary<Vector2Int, HeroModeData>();

	private Vector2Int GetRoundEmpty_v;

	private List<Vector2Int> GetRoundEmpty_list = new List<Vector2Int>();

	private List<Vector2Int> sides_resultlist = new List<Vector2Int>();

	private List<Vector2Int> sides_list = new List<Vector2Int>();

	private List<Vector2Int> sides_listtemp;

	private List<Vector2Int> line_list = new List<Vector2Int>();

	private List<Vector2Int> line_listtemp;

	private bool[,] waterchecks = new bool[3, 3];

	private Dictionary<RoomGenerateBase.RoomType, int> waveroom_maxwave = new Dictionary<RoomGenerateBase.RoomType, int>();

	private Dictionary<RoomGenerateBase.RoomType, int> waveroom_time = new Dictionary<RoomGenerateBase.RoomType, int>();

	private int waveroom_currentwave;

	private bool waveroom_currentwave_createend;

	private bool waveroom_startwave;

	private List<XmlNode> waveroom_nodelist = new List<XmlNode>();

	private SequencePool waveroom_pool = new SequencePool();

	private BattleLevelWaveData mWaveData = new BattleLevelWaveData();

	public int RoomID => roomid;

	public void Deinit()
	{
		mMapStrings.Clear();
		mSequencePool.Clear();
		waveroom_deinit();
	}

	public void CreateMap(Transfer t)
	{
		mTransfer = t;
		mGoodsList.Clear();
		Event_Button1101 = null;
		mRoomCtrl = mTransfer.roomctrl;
		roomid = mTransfer.roomid;
		int resourcesid = mTransfer.resourcesid;
		MapID = mTransfer.tmxid;
		mRoomType = mTransfer.roomtype;
		if (mRoomType == RoomGenerateBase.RoomType.eInvalid)
		{
			mRoomType = CheckTmxID(mTransfer.tmxid);
		}
		Facade.Instance.SendNotification("BATTLE_ROOM_TYPE", mRoomType);
		float x = LocalModelManager.Instance.Room_room.GetBeanById(resourcesid).GoodsOffset[0];
		float y = LocalModelManager.Instance.Room_room.GetBeanById(resourcesid).GoodsOffset[1];
		CombineOffset = new Vector2(x, y);
		InitTiledMap2Goods();
		bool flag = LocalModelManager.Instance.Stage_Level_stagechapter.is_wave_room();
		if (flag && mRoomType != RoomGenerateBase.RoomType.eEvent)
		{
			waveroom_init();
		}
		else
		{
			readTileMap();
		}
		InitHeroMode();
		if (flag)
		{
			waveroom_create_good();
		}
		else if (mTransfer.delay)
		{
			Sequence seq = DOTween.Sequence().AppendCallback(delegate
			{
				List<Vector2Int> createPositionList = GetCreatePositionList();
				int i = 0;
				for (int num = createPositionList.Count; i < num; i++)
				{
					Vector3 worldPosition = GetWorldPosition(createPositionList[i]);
					GameLogic.PlayEffect(3100023, worldPosition);
				}
			}).AppendInterval(0.9f)
				.AppendCallback(CreateAllGoods);
			mSequencePool.Add(seq);
		}
		else
		{
			CreateAllGoods();
		}
	}

	public RoomGenerateBase.RoomType CheckTmxID(string TmxID)
	{
		RoomGenerateBase.RoomType result = RoomGenerateBase.RoomType.eNormal;
		if (TmxID.Length > 6)
		{
			string a = TmxID.Substring(6, 1);
			if (a == "B")
			{
				result = RoomGenerateBase.RoomType.eBoss;
			}
			else if (a == "E")
			{
				result = RoomGenerateBase.RoomType.eEvent;
			}
		}
		return result;
	}

	private int[,] GetTileData(string tmxid)
	{
        Debug.Log("@LOG GetTileData tmxid:" + tmxid);
        XmlDocument xmlDocument = new XmlDocument();
		string tmxString = GetTmxString(MapID);
		xmlDocument.LoadXml(tmxString);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("map/tileset");
		XmlAttributeCollection attributes = xmlDocument.SelectSingleNode("map").Attributes;
		int num = int.Parse(attributes["width"].Value);
		int num2 = int.Parse(attributes["height"].Value);
		int[,] array = new int[num, num2];
		string innerText = xmlDocument.SelectSingleNode("map/layer/data").InnerText;
		string[] array2 = innerText.Split('\n');
		for (int i = 1; i < array2.Length - 1; i++)
		{
			string[] array3 = array2[i].Split(',');
			for (int j = 0; j < num; j++)
			{
				array[j, i - 1] = int.Parse(array3[j]);
			}
		}
		return array;
	}

	public int GetRoomResourceID(string tmxid)
	{
		switch (GetRoomHeight("GetRoomResourceID", tmxid))
		{
		case 11:
			return 3;
		case 15:
			return 2;
		case 21:
			return 1;
		default:
			return 1;
		}
	}

	public int GetRoomHeight(string source, string tmxid)
	{
        Debug.Log("@LOG GetRoomHeight tmxid:" + tmxid);
        XmlDocument xmlDocument = new XmlDocument();
		string tmxString = GetTmxString(tmxid);
		if (string.IsNullOrEmpty(tmxString))
		{
			SdkManager.Bugly_Report("GetRoomHeight", Utils.FormatString("source : [{0}] the tmxpath:[{1}] is not found!", source, tmxid));
			return 11;
		}
		int result = 11;
		try
		{
			xmlDocument.LoadXml(tmxString);
			XmlAttributeCollection attributes = xmlDocument.SelectSingleNode("map").Attributes;
			result = int.Parse(attributes["height"].Value);
			return result;
		}
		catch
		{
			SdkManager.Bugly_Report("GetRoomHeight", Utils.FormatString("source : {0} GetTmxString try the tmxpath:[{1}] stage:{2} is error!", source, tmxid, GameLogic.Hold.BattleData.Level_CurrentStage));
			string tmxPath = GetTmxPath(tmxid);
            Debug.Log("@LOG GetRoomHeight tmxPath:" + tmxPath);
			if (mMapStrings.ContainsKey(tmxPath))
			{
				mMapStrings.Remove(tmxPath);
				return GetRoomHeight("GetRoomHeight", tmxid);
			}
			return result;
		}
	}

	private string GetTmxPath(string tmxid)
	{
        Debug.Log("@LOG GetTmxPath tmxid:" + tmxid);
        if (tmxid == string.Empty)
		{
			SdkManager.Bugly_Report("MapCreator.GetTmxPath", Utils.FormatString("stage:{0} mode:{1} roomid:{2} random a empty tmxid!!!", GameLogic.Hold.BattleData.Level_CurrentStage, GameLogic.Release.Mode.GetMode().ToString(), GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID()));
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		if (tmxid == "firstroom" || tmxid.Contains("emptyroom"))
		{
			return Utils.FormatString("Game/Map/Tiled/{0}", tmxid);
		}
		switch (GameLogic.Release.Mode.GetMode())
		{
		case GameMode.eLevel:
		{
			int tiledID2 = LocalModelManager.Instance.Stage_Level_stagechapter.GetTiledID(GameLogic.Hold.BattleData.Level_CurrentStage);
			return Utils.FormatString("Game/Map/Tiled/{0}/Main{1}/{2}", GameLogic.Release.Mode.GetMode().ToString(), tiledID2, tmxid);
		}
		case GameMode.eGold1:
		case GameMode.eChest1:
		{
			int difficult = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(GameLogic.Hold.BattleData.ActiveID).Difficult;
			return Utils.FormatString("Game/Map/Tiled/{0}/{1}/{2}", GameLogic.Release.Mode.GetMode().ToString(), difficult, tmxid);
		}
		default:
			if (GameLogic.Hold.BattleData.Challenge_ismainchallenge())
			{
				int tiledID = LocalModelManager.Instance.Stage_Level_stagechapter.GetTiledID(GameLogic.Hold.BattleData.Level_CurrentStage);
				return Utils.FormatString("Game/Map/Tiled/{0}/Main{1}/{2}", GameMode.eLevel.ToString(), tiledID, tmxid);
			}
			return Utils.FormatString("Game/Map/Tiled/{0}/{1}/{2}", GameLogic.Release.Mode.GetMode().ToString(), 1, tmxid);
		}
	}

	private string GetTmxString(string tmxid)
	{
		string tmxPath = GetTmxPath(tmxid);
        Debug.Log("@LOG GetTmxString tmxPath:" + tmxPath);
        string value = string.Empty;
		if (mMapStrings.TryGetValue(tmxPath, out value))
		{
			if (!string.IsNullOrEmpty(value))
			{
				return value;
			}
			SdkManager.Bugly_Report("MapCreator_GetTmxString", Utils.FormatString("mMapStrings.Try first [{0}] is null!", tmxPath));
		}
		string text = Utils.FormatString("{0}/{1}/Main{2}", "data/tiledmap", GameLogic.Release.Mode.GetMode().ToString(), GameLogic.Hold.BattleData.Level_CurrentStage);
		string text2 = Utils.FormatString("{0}/{1}.txt", text, tmxid);
		byte[] fileBytes = FileUtils.GetFileBytes(text, Utils.FormatString("{0}.txt", tmxid));
		if (fileBytes != null)
		{
			value = Encoding.Default.GetString(fileBytes);
			if (!string.IsNullOrEmpty(value))
			{
				if (!mMapStrings.ContainsKey(tmxPath))
				{
					mMapStrings.Add(tmxPath, value);
				}
				return value;
			}
		}
		TextAsset textAsset = ResourceManager.Load<TextAsset>(tmxPath);
		if (textAsset != null)
		{
			value = textAsset.text;
			if (!mMapStrings.ContainsKey(tmxPath))
			{
				mMapStrings.Add(tmxPath, value);
			}
			return value;
		}
		SdkManager.Bugly_Report("MapCreator_GetTmxString", Utils.FormatString("ResourceManager.Load[{0}] is null!", tmxPath));
		return string.Empty;
	}

	public bool HaveTmx(string tmxid)
	{
        string tmxString = GetTmxString(tmxid);
		return !tmxString.Equals(string.Empty);
	}

	private void CreateAllGoods()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (tiledata[i, j] > 0)
				{
					int goodID = GetGoodID(i, j);
					float num = (float)i + CombineOffset.x;
					float num2 = (float)(-j) + CombineOffset.y;
					CreateOneGoods(goodID, i, j);
				}
			}
		}
		DealTrap();
		DealWater();
		heromode_end();
	}

	private List<Vector2Int> GetCreatePositionList()
	{
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (tiledata[i, j] > 0)
				{
					int goodID = GetGoodID(i, j);
					if (IsMonster(goodID))
					{
						list.Add(new Vector2Int(i, j));
					}
				}
			}
		}
		return list;
	}

	public void CreateOneGoods(int GoodsID, int x, int y)
	{
		switch (GetGoodType(GoodsID))
		{
		case MapGoodType.Event:
			if (!LocalSave.Instance.BattleIn_GetIn())
			{
				CreateGoodNotTrap(GoodsID, x, y);
			}
			break;
		case MapGoodType.Goods:
			CreateGoodNotTrap(GoodsID, x, y);
			break;
		case MapGoodType.Soldier:
		case MapGoodType.Boss:
		case MapGoodType.Tower:
			if (!LocalSave.Instance.BattleIn_GetIn())
			{
				CreateData createData = new CreateData();
				createData.m_bElite = heromode_is_elite(x, y);
				createData.entityid = GoodsID;
				Vector3 worldPosition = GetWorldPosition(x, y);
				createData.x = worldPosition.x;
				createData.y = worldPosition.z;
				createData.v = new Vector2Int(x, y);
				CreateEntity(createData);
			}
			break;
		}
	}

	private GameObject CreateGood(int GoodsID, int x, int y)
	{
		GameObject gameObject = ResourceManager.Load<GameObject>(Utils.GetString("Game/Goods/", GoodsID));
		if (!gameObject)
		{
			return null;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		mGoodsList.Add(gameObject2);
		gameObject2.transform.parent = mRoomCtrl.GoodsParent;
		gameObject2.transform.localPosition = GetWorldPositionUnscale(x, y);
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localRotation = Quaternion.identity;
		Transform transform = gameObject2.transform.Find("child/good");
		if ((bool)transform && LocalModelManager.Instance.Goods_goods.GetBeanById(GoodsID).Ground == 0)
		{
			Vector3 position = gameObject2.transform.position;
			int sortingOrder = (int)(0f - position.z);
			transform.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
		}
		return gameObject2;
	}

	private GameObject CreateGoodNotTrap(int GoodsID, int x, int y)
	{
		if (GoodsID >= 2001 && GoodsID <= 2008)
		{
			return null;
		}
		if (GoodsID == 1006)
		{
			return null;
		}
		return CreateGood(GoodsID, x, y);
	}

	public GameObject CreateGoodExtra(int GoodsID, int x, int y)
	{
		return CreateGoodNotTrap(GoodsID, x, y);
	}

	public EntityBase CreateEntityOneWorld(CreateData data)
	{
		EntityBase entityBase = CreateEntity(data);
		entityBase.SetPosition(new Vector3(data.x, 0f, data.y));
		return entityBase;
	}

	public EntityBase CreateEntity(CreateData data)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(data.path));
		gameObject.transform.SetParent(GameNode.m_Monster.transform);
		gameObject.transform.position = new Vector3(data.x, 0f, data.y);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		EntityBase component = gameObject.GetComponent<EntityBase>();
		component.SetElite(data.m_bElite);
		component.bCall = data.bCall;
		component.bDivide = data.bDivide;
		component.SetRoomType(mRoomType);
		component.Init(data.entityid);
		return component;
	}

	public EntityBase CreateDivideEntity(EntityBase parent, int entityid, float x, float y)
	{
		CreateData createData = new CreateData();
		createData.entityid = entityid;
		createData.x = x;
		createData.y = y;
		createData.v = GetRoomXY(x, y);
		createData.bDivide = true;
		CreateData data = createData;
		EntityBase entityBase = CreateEntityOneWorld(data);
		entityBase.SetEntityDivide(mRoomType);
		return entityBase;
	}

	private Goods_goods GetGoods(int GoodsID)
	{
		if (!GoodsList.ContainsKey(GoodsID))
		{
			GoodsList.Add(GoodsID, LocalModelManager.Instance.Goods_goods.GetBeanById(GoodsID));
		}
		if (GoodsList.ContainsKey(GoodsID))
		{
			return GoodsList[GoodsID];
		}
		return null;
	}

	public static MapGoodType GetGoodType(int goodid)
	{
		if (goodid > 0 && goodid < 2000)
		{
			return MapGoodType.Goods;
		}
		if (goodid >= 2009 && goodid <= 2015)
		{
			return MapGoodType.Goods;
		}
		if (goodid >= 3000 && goodid < 5000)
		{
			return MapGoodType.Soldier;
		}
		if (goodid >= 5000 && goodid < 6000)
		{
			return MapGoodType.Boss;
		}
		if (goodid >= 8000 && goodid < 9000)
		{
			return MapGoodType.Tower;
		}
		if (goodid > 9000)
		{
			return MapGoodType.Event;
		}
		return MapGoodType.Empty;
	}

	private bool IsMonster(int goodsid)
	{
		MapGoodType goodType = GetGoodType(goodsid);
		return goodType == MapGoodType.Soldier || goodType == MapGoodType.Boss || goodType == MapGoodType.Tower;
	}

	public EntityMonsterBase CreateEntityCall(int entityid, float x, float y)
	{
		EntityBase entityBase = CreateEntity(new CreateData
		{
			entityid = entityid,
			x = x,
			y = y,
			v = GetRoomXY(x, y),
			bCall = true
		});
		EntityMonsterBase entityMonsterBase = entityBase as EntityMonsterBase;
		if ((bool)entityMonsterBase)
		{
			entityMonsterBase.StartCall();
		}
		return entityMonsterBase;
	}

	private int GetGoodID(int i, int j)
	{
		int key = tiledata[i, j] % 10000 - 1;
		int value = -1;
		TileMap2Goods.TryGetValue(key, out value);
		return value;
	}

	public void ClearGoods()
	{
		for (int num = mGoodsList.Count - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(mGoodsList[num]);
		}
		mGoodsList.Clear();
	}

	private void DealTrap()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (tiledata[i, j] <= 0)
				{
					continue;
				}
				int goodID = GetGoodID(i, j);
				if (goodID == 2001 || goodID == 2003 || goodID == 2005 || goodID == 2007)
				{
					GameObject gameObject = CreateGood(goodID, i, j);
					int x = i;
					int y = j;
					switch (goodID)
					{
					case 2001:
						y = FindTrapUpEnd(i, j);
						CreateGood(2012, i, j);
						CreateGood(2011, i, y);
						break;
					case 2003:
						y = FindTrapDownEnd(i, j);
						CreateGood(2011, i, j);
						CreateGood(2012, i, y);
						break;
					case 2005:
						x = FindTrapLeftEnd(i, j);
						CreateGood(2014, i, j);
						CreateGood(2013, x, j);
						break;
					case 2007:
						x = FindTrapRightEnd(i, j);
						CreateGood(2013, i, j);
						CreateGood(2014, x, j);
						break;
					}
					gameObject.GetComponent<Goods2001>().SetEndPosition(GetWorldPositionUnscale(x, y));
				}
			}
		}
	}

	private int FindTrapUpEnd(int x, int y)
	{
		for (int num = y - 1; num >= 0; num--)
		{
			int num2 = tiledata[x, num];
			if (num2 > 0)
			{
				int num3 = TileMap2Goods[num2 % 10000 - 1];
				if (num3 == 2002)
				{
					return num;
				}
			}
		}
		return -1;
	}

	private int FindTrapDownEnd(int x, int y)
	{
		for (int i = y + 1; i < height; i++)
		{
			int num = tiledata[x, i];
			if (num > 0)
			{
				int num2 = TileMap2Goods[num % 10000 - 1];
				if (num2 == 2004)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private int FindTrapLeftEnd(int x, int y)
	{
		for (int num = x - 1; num >= 0; num--)
		{
			int num2 = tiledata[num, y];
			if (num2 > 0)
			{
				int num3 = TileMap2Goods[num2 % 10000 - 1];
				if (num3 == 2006)
				{
					return num;
				}
			}
		}
		return -1;
	}

	private int FindTrapRightEnd(int x, int y)
	{
		for (int i = x + 1; i < width; i++)
		{
			int num = tiledata[i, y];
			if (num > 0)
			{
				int num2 = TileMap2Goods[num % 10000 - 1];
				if (num2 == 2008)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private void Bomberman_Init()
	{
		bomberman_rect = (int[,])tiledata.Clone();
		bomberman_danger = (int[,])tiledata.Clone();
		Bomberman_path = new FindPath();
		Bomberman_danger_reset();
	}

	public int[,] Bomberman_GetDanger()
	{
		return bomberman_danger;
	}

	public int[,] Bomberman_GetRect()
	{
		return bomberman_rect;
	}

	private void Bomberman_danger_reset()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (bomberman_danger[i, j] <= 10)
				{
					bomberman_danger[i, j] = 0;
				}
			}
		}
	}

	public bool Bomberman_is_empty(Vector3 pos)
	{
		Vector2Int roomXY = GetRoomXY(pos);
		return bomberman_rect[roomXY.x, roomXY.y] == 0;
	}

	public void Bomberman_Use(Vector3 pos, int length)
	{
		Vector2Int roomXY = GetRoomXY(pos);
		bomberman_rect[roomXY.x, roomXY.y] = length;
	}

	public void Bomberman_Cache(Vector3 pos)
	{
		Vector2Int roomXY = GetRoomXY(pos);
		bomberman_rect[roomXY.x, roomXY.y] = 0;
	}

	public bool Bomberman_is_danger(Vector2Int v)
	{
		return Bomberman_is_danger(v.x, v.y);
	}

	public bool Bomberman_is_danger(Grid.NodeItem item)
	{
		return Bomberman_is_danger(item.x, item.y);
	}

	public bool Bomberman_is_danger(int x, int y)
	{
		Bomberman_update_danger();
		return bomberman_danger[x, y] > 0;
	}

	private void Bomberman_update_danger()
	{
		Bomberman_danger_reset();
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (bomberman_rect[i, j] > 0)
				{
					Bomberman_set_danger(bomberman_rect[i, j], i, j);
				}
			}
		}
		Bomberman_path.Init(bomberman_danger);
	}

	public Vector2Int Bomberman_get_safe_near(Vector3 pos)
	{
		Vector2Int selfpos = GetRoomXY(pos);
		Bomberman_update_danger();
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (bomberman_danger[i, j] == 0)
				{
					list.Add(new Vector2Int(i, j));
				}
			}
		}
		list.Sort((Vector2Int a, Vector2Int b) => (MathDxx.Pow(a.x - selfpos.x, 2f) + MathDxx.Pow(a.y - selfpos.y, 2f) < MathDxx.Pow(b.x - selfpos.x, 2f) + MathDxx.Pow(b.y - selfpos.y, 2f)) ? (-1) : 1);
		if (list.Count > 0)
		{
			int num = MathDxx.Clamp(list.Count, 1, 3);
			int num2 = GameLogic.Random(0, num);
			return list[num];
		}
		return RandomRoomXY();
	}

	private void Bomberman_set_danger(int length, int x, int y)
	{
		for (int i = x - length; i <= x + length; i++)
		{
			if (i >= 0 && i < width)
			{
				bomberman_danger[i, y] = 1;
			}
		}
		for (int j = y - length; j <= y + length; j++)
		{
			if (j >= 0 && j < height)
			{
				bomberman_danger[x, j] = 1;
			}
		}
	}

	public List<Grid.NodeItem> Bomberman_find_path(Vector3 startpos, Vector3 endpos)
	{
		return Bomberman_path.FindingPath(startpos, endpos);
	}

	public bool GetCanCall(EntityBase entity, int radiusmin, int radiusmax)
	{
		for (int i = radiusmin; i <= radiusmax; i++)
		{
			List<Vector2Int> roundSideEmpty = GetRoundSideEmpty(mCallRect, entity.position, i);
			if (roundSideEmpty.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool RandomCallSide(EntityBase entity, int range, out float endx, out float endz)
	{
		List<Vector2Int> roundSideEmpty = GetRoundSideEmpty(mCallRect, entity.position, range);
		if (roundSideEmpty.Count == 0)
		{
			RandomItem(entity, range, out endx, out endz);
			return false;
		}
		int index = GameLogic.Random(0, roundSideEmpty.Count);
		Vector3 worldPosition = GetWorldPosition(roundSideEmpty[index]);
		endx = worldPosition.x;
		endz = worldPosition.z;
		Vector2Int roomXY = GetRoomXY(worldPosition);
		mCallRect[roomXY.x, roomXY.y] = 1;
		return true;
	}

	public bool RandomCallSide(EntityBase entity, int radiusmin, int radiusmax, out float endx, out float endz)
	{
		for (int i = radiusmin; i <= radiusmax; i++)
		{
			if (RandomCallSide(entity, i, out endx, out endz))
			{
				return true;
			}
		}
		endx = 0f;
		endz = 0f;
		return true;
	}

	public void CallPositionRecover(Vector3 pos)
	{
		Vector2Int roomXY = GetRoomXY(pos);
		mCallRect[roomXY.x, roomXY.y] = 0;
	}

	private void InitElementData(int id)
	{
		int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
		int tiledID = LocalModelManager.Instance.Stage_Level_stagechapter.GetTiledID(level_CurrentStage);
		if (mElementData.ContainsKey(tiledID))
		{
			return;
		}
		Dictionary<int, WeightRandom> dictionary = new Dictionary<int, WeightRandom>();
		string[] array = mWeightStrings[tiledID].Split('|');
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			WeightRandom weightRandom = new WeightRandom();
			string[] array2 = array[i].Split(';');
			int j = 0;
			for (int num2 = array2.Length; j < num2; j++)
			{
				string[] array3 = array2[j].Split(',');
				int id2 = int.Parse(array3[0]);
				int weight = int.Parse(array3[1]);
				weightRandom.Add(id2, weight);
			}
			dictionary.Add(i + 1, weightRandom);
		}
		mElementData.Add(tiledID, dictionary);
	}

	private int GetRandomElementID(int elementid)
	{
		InitElementData(elementid);
		int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
		int tiledID = LocalModelManager.Instance.Stage_Level_stagechapter.GetTiledID(level_CurrentStage);
		return mElementData[tiledID][elementid].GetRandom();
	}

	public Sprite GetRandomElement(int elementid)
	{
		int randomElementID = GetRandomElementID(elementid);
		return SpriteManager.GetMap(Utils.FormatString("element{0:D2}{1:D2}", elementid, randomElementID));
	}

	public Sprite GetElementShadow(int elementid)
	{
		return SpriteManager.GetMap(Utils.FormatString("elementshadow{0:D2}", elementid));
	}

	public bool GetStage3MiddleWater()
	{
		int num = width / 2;
		int i = num - 1;
		for (int num2 = num + 1; i <= num2; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (findpathRect[i, j] == 1006)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void InitHeroMode()
	{
		if (!GameLogic.Hold.BattleData.IsHeroMode())
		{
			return;
		}
		elitelist.Clear();
		List<bool> list = new List<bool>();
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (tiledata[i, j] > 0)
				{
					int goodID = GetGoodID(i, j);
					MapGoodType goodType = GetGoodType(goodID);
					if (goodType == MapGoodType.Soldier && heromode_can_add(goodID))
					{
						Vector2Int vector2Int = new Vector2Int(i, j);
						elitelist.Add(vector2Int, new HeroModeData
						{
							roompos = vector2Int,
							entityid = goodID
						});
						list.Add(item: false);
					}
				}
			}
		}
		int value = GameLogic.Random(1, 3);
		value = MathDxx.Clamp(value, 0, elitelist.Count);
		for (int k = 0; k < value; k++)
		{
			list[k] = true;
		}
		list.RandomSort();
		Dictionary<Vector2Int, HeroModeData>.Enumerator enumerator = elitelist.GetEnumerator();
		int num = 0;
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.m_bElite = list[num];
			num++;
		}
	}

	private bool heromode_can_add(int entityid)
	{
		if (entityid == 3016)
		{
			return false;
		}
		return true;
	}

	private bool heromode_is_elite(int x, int y)
	{
		Vector2Int key = new Vector2Int(x, y);
		if (elitelist.TryGetValue(key, out HeroModeData value))
		{
			return value.m_bElite;
		}
		return false;
	}

	private void heromode_end()
	{
		elitelist.Clear();
	}

	private void InitTiledMap2Goods()
	{
		if (TileMap2Goods.Count <= 1)
		{
			ReadTiledMapTSX("map_boss");
			ReadTiledMapTSX("map_monster");
			ReadTiledMapTSX("map_good");
		}
	}

	private void ReadTiledMapTSX(string name)
	{
        Debug.Log("@LOG ReadTiledMapTSX name:" + name);
        XmlDocument xmlDocument = new XmlDocument();
		string xml = ResourceManager.Load<TextAsset>(Utils.FormatString("Game/Map/Tiled/{0}", name)).ToString();
		xmlDocument.LoadXml(xml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("tileset/terraintypes");
		XmlNodeList xmlNodeList = xmlNode.SelectNodes("terrain");
		IEnumerator enumerator = xmlNodeList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			XmlNode xmlNode2 = (XmlNode)enumerator.Current;
			int value = int.Parse(xmlNode2.Attributes["name"].Value);
			int key = int.Parse(xmlNode2.Attributes["tile"].Value);
			if (!TileMap2Goods.ContainsKey(key))
			{
				TileMap2Goods.Add(key, value);
			}
		}
	}

	private void readTileMap()
	{
        Debug.Log("@LOG readTileMap");
        XmlDocument xmlDocument = new XmlDocument();
		string tmxString = GetTmxString(MapID);
		if (tmxString.Equals(string.Empty))
		{
			SdkManager.Bugly_Report("MapCreator_Init", Utils.FormatString("readTileMap [{0}] mode:{1} stage:{2} is not found!!!", MapID, GameLogic.Hold.BattleData.GetMode().ToString(), GameLogic.Hold.BattleData.Level_CurrentStage));
			MapID = "firstroom";
			tmxString = GetTmxString(MapID);
		}
		xmlDocument.LoadXml(tmxString);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("map/tileset");
		XmlAttributeCollection attributes = xmlDocument.SelectSingleNode("map").Attributes;
		width = int.Parse(attributes["width"].Value);
		height = int.Parse(attributes["height"].Value);
		tiledata = new int[width, height];
		XmlNodeList nodes = xmlDocument.SelectNodes("map/layer");
		XmlNode xmlNode2 = get_node(nodes);
		string innerText = xmlNode2.SelectSingleNode("data").InnerText;
		string[] array = innerText.Split('\n');
		for (int i = 1; i < array.Length - 1; i++)
		{
			string[] array2 = array[i].Split(',');
			for (int j = 0; j < width; j++)
			{
				tiledata[j, i - 1] = int.Parse(array2[j]);
			}
		}
		initRoomRealRectGoods();
		Bomberman_Init();
	}

	private XmlNode get_node(XmlNodeList nodes)
	{
		XmlNode xmlNode = null;
		if (xmlNode == null)
		{
			int i = GameLogic.Random(0, nodes.Count);
			xmlNode = nodes[i];
		}
		return xmlNode;
	}

	private void initRoomRealRectGoods()
	{
		RoomRealRect = (int[,])tiledata.Clone();
		TmxGoodsList = new TMXGoodsData[width, height];
		int value;
		for (int i = 0; i < height; i++)
		{
			for (int num = width - 1; num >= 0; num--)
			{
				TmxGoodsList[num, i] = new TMXGoodsData();
				if (RoomRealRect[num, i] > 0)
				{
					int key = RoomRealRect[num, i] % 10000 - 1;
					if (TileMap2Goods.TryGetValue(key, out value))
					{
						TmxGoodsList[num, i].SetGoodsId(value);
						if (value != 0)
						{
							MapGoodType goodType = GetGoodType(value);
							if (goodType == MapGoodType.Goods || goodType == MapGoodType.Event)
							{
								Goods_goods goods = GetGoods(value);
								if (goods == null)
								{
									SdkManager.Bugly_Report("MapCreator_Rect.initRoomRealRectGoods", Utils.FormatString("goods is null {0}", value));
								}
								TmxGoodsList[num, i].Init(goods.GoodsType);
								for (int num2 = 0; num2 > -goods.SizeY; num2--)
								{
									for (int j = 0; j < goods.SizeX; j++)
									{
										RoomRealRect[num + j, i + num2] = RoomRealRect[num, i];
										TmxGoodsList[num + j, i + num2].Init(goods.GoodsType);
									}
								}
							}
						}
					}
				}
			}
		}
		findpathRect = (int[,])RoomRealRect.Clone();
		for (int k = 0; k < height; k++)
		{
			for (int num3 = width - 1; num3 >= 0; num3--)
			{
				if (findpathRect[num3, k] > 0)
				{
					int key2 = RoomRealRect[num3, k] % 10000 - 1;
					if (TileMap2Goods.TryGetValue(key2, out value))
					{
						if (value > 1000 && value < 2000)
						{
							findpathRect[num3, k] = value;
						}
						else
						{
							findpathRect[num3, k] = 0;
						}
					}
				}
			}
		}
		mCallRect = (int[,])findpathRect.Clone();
	}

	public Vector2Int GetRoomXY(float x, float z)
	{
		return GetRoomXY(new Vector3(x, 0f, z));
	}

	public Vector2Int GetRoomXY(Vector3 pos)
	{
		Vector2Int result = default(Vector2Int);
		float num = pos.x - CombineOffset.x;
		int num2 = (int)(num * 10f) % 10;
		if (num2 < 5)
		{
			result.x = (int)num;
		}
		else
		{
			result.x = (int)num + 1;
		}
		float num3 = CombineOffset.y - pos.z / 1.23f;
		int num4 = (int)(num3 * 10f) % 10;
		if (num4 < 5)
		{
			result.y = (int)num3;
		}
		else
		{
			result.y = (int)num3 + 1;
		}
		result.x = MathDxx.Clamp(result.x, 0, width - 1);
		result.y = MathDxx.Clamp(result.y, 0, height - 1);
		return result;
	}

	public Vector2Int GetRoomXYInside(Vector3 pos)
	{
		Vector2Int roomXY = GetRoomXY(pos);
		roomXY.x = MathDxx.Clamp(roomXY.x, 1, width - 1);
		roomXY.y = MathDxx.Clamp(roomXY.y, 1, height - 1);
		return roomXY;
	}

	public Vector3 GetWorldPosition(int x, int y)
	{
		Vector3 worldPositionUnscale = GetWorldPositionUnscale(x, y);
		return new Vector3(worldPositionUnscale.x, 0f, worldPositionUnscale.z * 1.23f);
	}

	public Vector3 GetWorldPosition(Vector2Int v2)
	{
		return GetWorldPosition(v2.x, v2.y);
	}

	public Vector3 GetWorldPositionUnscale(Vector2Int v)
	{
		return GetWorldPositionUnscale(v.x, v.y);
	}

	public Vector3 GetWorldPositionUnscale(int x, int y)
	{
		return new Vector3((float)x + CombineOffset.x, 0f, CombineOffset.y - (float)y);
	}

	public bool IsXMiddle(int x)
	{
		return x == (width - 1) / 2;
	}

	public bool IsYMiddle(int y)
	{
		return y == (height - 1) / 2;
	}

	public bool IsEmpty(Vector2Int v)
	{
		return IsEmpty(v.x, v.y);
	}

	public bool IsEmpty(int x, int y)
	{
		if (x >= 0 && x < width && y >= 0 && y < height)
		{
			return TmxGoodsList[x, y].IsEmpty();
		}
		return false;
	}

	public bool IsEmpty(bool flystone, bool flywater, int x, int y)
	{
		if (x < 0 || x >= width)
		{
			return false;
		}
		if (y < 0 || y >= height)
		{
			return false;
		}
		if (!flystone && TmxGoodsList[x, y].ParentType == TMXGoodsParentType.Obstacle_GroundUp)
		{
			return false;
		}
		if (!flywater && TmxGoodsList[x, y].ParentType == TMXGoodsParentType.Obstacle_GroundDown)
		{
			return false;
		}
		return true;
	}

	private bool GetNearestEmpty(Vector2Int vInt, int count, ref Vector2Int v)
	{
		v = new Vector2Int(vInt.x, vInt.y);
		if (count == 0 && IsEmpty(v))
		{
			return true;
		}
		List<Vector2Int> list = new List<Vector2Int>();
		int i = vInt.x - count;
		if (i < 0)
		{
			i = 0;
		}
		else if (i >= width)
		{
			return false;
		}
		int num = vInt.x + count;
		if (num > width)
		{
			num = width;
		}
		int j = vInt.y - count;
		if (j < 0)
		{
			j = 0;
		}
		else if (j >= height)
		{
			return false;
		}
		int num2 = vInt.y + count;
		if (num2 > height)
		{
			num2 = height;
		}
		for (; i <= num; i++)
		{
			for (; j <= num2; j++)
			{
				if ((MathDxx.Abs(i - vInt.x) >= count || MathDxx.Abs(j - vInt.y) >= count) && IsEmpty(i, j))
				{
					list.Add(new Vector2Int(i, j));
				}
			}
		}
		if (list.Count > 0)
		{
			int num3 = int.MaxValue;
			int index = 0;
			for (i = 0; i < list.Count; i++)
			{
				int num4 = MathDxx.Abs(list[i].x - vInt.x) + MathDxx.Abs(list[i].y - vInt.y);
				if (num4 < num3)
				{
					num3 = num4;
					index = i;
				}
			}
			v = list[index];
			return true;
		}
		return false;
	}

	private Vector2Int GetNearestEmpty(Vector2Int vInt)
	{
		Vector2Int v = default(Vector2Int);
		for (int i = 0; i < 21 && !GetNearestEmpty(vInt, i, ref v); i++)
		{
		}
		return v;
	}

	public List<Vector2Int> GetNearEmptyList(Vector3 pos, ref Vector2Int GoldsCenter, int radius)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		Vector2Int roomXY = GetRoomXY(pos);
		GoldsCenter = GetNearestEmpty(roomXY);
		for (int i = GoldsCenter.x - radius; i <= GoldsCenter.x + radius; i++)
		{
			for (int j = GoldsCenter.y - radius; j <= GoldsCenter.y + radius; j++)
			{
				if (i >= 0 && i < width && j >= 0 && j < height && IsEmpty(i, j))
				{
					list.Add(new Vector2Int(i, j));
				}
			}
		}
		if (list.Count == 0)
		{
			return GetNearEmptyList(pos, ref GoldsCenter, radius + 1);
		}
		return list;
	}

	public int[,] GetRects()
	{
		return tiledata;
	}

	public int[,] GetFindPathRect()
	{
		return findpathRect;
	}

	public List<Vector2Int> GetRoundEmpty(Vector3 pos, int size)
	{
		GetRoundEmpty_v = GetRoomXY(pos);
		GetRoundEmpty_list.Clear();
		int i = GetRoundEmpty_v.x - size;
		for (int num = GetRoundEmpty_v.x + size + 1; i < num; i++)
		{
			if (i < 0 || i >= width)
			{
				continue;
			}
			int j = GetRoundEmpty_v.y - size;
			for (int num2 = GetRoundEmpty_v.y + size + 1; j < num2; j++)
			{
				if (j >= 0 && j < height && findpathRect[i, j] == 0)
				{
					GetRoundEmpty_list.Add(new Vector2Int(i, j));
				}
			}
		}
		return GetRoundEmpty_list;
	}

	public List<Vector2Int> GetRoundFly(Vector3 pos, int size)
	{
		GetRoundEmpty_v = GetRoomXY(pos);
		GetRoundEmpty_list.Clear();
		int i = GetRoundEmpty_v.x - size;
		for (int num = GetRoundEmpty_v.x + size + 1; i < num; i++)
		{
			if (i < 0 || i >= width)
			{
				continue;
			}
			int j = GetRoundEmpty_v.y - size;
			for (int num2 = GetRoundEmpty_v.y + size + 1; j < num2; j++)
			{
				if (j >= 0 && j < height && findpathRect[i, j] != 1001 && findpathRect[i, j] != 1007 && findpathRect[i, j] != 1009 && findpathRect[i, j] != 1008)
				{
					GetRoundEmpty_list.Add(new Vector2Int(i, j));
				}
			}
		}
		return GetRoundEmpty_list;
	}

	public List<Vector2Int> GetRoundSideEmpty(int[,] rects, Vector3 pos, int size)
	{
		Vector2Int roomXY = GetRoomXY(pos);
		sides_resultlist.Clear();
		int num = Mathf.Clamp(roomXY.x - size, 0, width);
		int num2 = Mathf.Clamp(roomXY.x + size + 1, 0, width);
		int num3 = Mathf.Clamp(roomXY.y - size, 0, height);
		int num4 = Mathf.Clamp(roomXY.y + size + 1, 0, height);
		int num5 = roomXY.x - size;
		int num6 = roomXY.x + size;
		int num7 = roomXY.y - size;
		int num8 = roomXY.y + size;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if ((i == num5 || i == num6 || j == num7 || j == num8) && rects[i, j] == 0)
				{
					sides_resultlist.Add(new Vector2Int(i, j));
				}
			}
		}
		return sides_resultlist;
	}

	private List<Vector2Int> GetHorizontalEmpty(EntityBase entity)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		Vector2Int roomXY = GetRoomXY(entity.position);
		for (int i = 0; i < width; i++)
		{
			if (findpathRect[i, roomXY.y] == 0)
			{
				list.Add(new Vector2Int(i, roomXY.y));
			}
		}
		return list;
	}

	private List<Vector2Int> GetVerticalEmpty(EntityBase entity)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		Vector2Int roomXY = GetRoomXY(entity.position);
		for (int i = 0; i < width; i++)
		{
			if (findpathRect[roomXY.x, i] == 0)
			{
				list.Add(new Vector2Int(roomXY.x, i));
			}
		}
		return list;
	}

	public List<Vector3> GetRoundNotSame(Vector3 selfwordpos, int range, int count)
	{
		List<Vector3> list = new List<Vector3>();
		Vector2Int roomXY = GetRoomXY(selfwordpos);
		Vector3 worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(roomXY);
		list.Add(worldPosition);
		for (int i = 0; i < count; i++)
		{
			RandomItem(selfwordpos, range, out float endx, out float endz);
			while (GetRoundNotSame_CheckSame(list, endx, endz))
			{
				RandomItem(selfwordpos, range, out endx, out endz);
			}
			list.Add(new Vector3(endx, 0f, endz));
		}
		list.Remove(worldPosition);
		return list;
	}

	private bool GetRoundNotSame_CheckSame(List<Vector3> list, float endx, float endz)
	{
		int i = 0;
		for (int num = list.Count; i < num; i++)
		{
			Vector3 vector = list[i];
			if (vector.x == endx)
			{
				Vector3 vector2 = list[i];
				if (vector2.z == endz)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void RandomItem(Vector3 pos, int range, out float endx, out float endz)
	{
		List<Vector2Int> roundEmpty = GetRoundEmpty(pos, range);
		int num = 0;
		while (roundEmpty.Count == 0 && num < 10)
		{
			range++;
			num++;
			roundEmpty = GetRoundEmpty(pos, range);
		}
		if (roundEmpty.Count == 0)
		{
			endx = pos.x;
			endz = pos.z;
			return;
		}
		int index = GameLogic.Random(0, roundEmpty.Count);
		Vector3 worldPosition = GetWorldPosition(roundEmpty[index]);
		endx = worldPosition.x;
		endz = worldPosition.z;
	}

	public void RandomItem(EntityBase entity, int range, out float endx, out float endz)
	{
		if (!entity)
		{
			Vector3 vector = RandomPosition();
			endx = vector.x;
			endz = vector.z;
		}
		else
		{
			RandomItem(entity.position, range, out endx, out endz);
		}
	}

	public void RandomFly(EntityBase entity, int range, out float endx, out float endz)
	{
		if (!entity)
		{
			Vector3 vector = RandomPosition();
			endx = vector.x;
			endz = vector.z;
			return;
		}
		List<Vector2Int> roundFly = GetRoundFly(entity.position, range);
		int num = 0;
		while (roundFly.Count == 0 && num < 10)
		{
			range++;
			num++;
			roundFly = GetRoundFly(entity.position, range);
		}
		if (roundFly.Count == 0)
		{
			Vector3 position = entity.position;
			endx = position.x;
			Vector3 position2 = entity.position;
			endz = position2.z;
		}
		else
		{
			int index = GameLogic.Random(0, roundFly.Count);
			Vector3 worldPosition = GetWorldPosition(roundFly[index]);
			endx = worldPosition.x;
			endz = worldPosition.z;
		}
	}

	public void RandomItemSide(EntityBase entity, int range, out float endx, out float endz)
	{
		List<Vector2Int> roundSideEmpty = GetRoundSideEmpty(findpathRect, entity.position, range);
		if (roundSideEmpty.Count == 0)
		{
			Vector3 position = entity.position;
			endx = position.x;
			Vector3 position2 = entity.position;
			endz = position2.z;
		}
		else
		{
			int index = GameLogic.Random(0, roundSideEmpty.Count);
			Vector3 worldPosition = GetWorldPosition(roundSideEmpty[index]);
			endx = worldPosition.x;
			endz = worldPosition.z;
		}
	}

	public void RandomItemSides(EntityBase entity, int rangemin, int rangemax, out float endx, out float endz)
	{
		for (int i = rangemin; i <= rangemax; i++)
		{
			sides_listtemp = GetRoundSideEmpty(findpathRect, entity.position, i);
			int j = 0;
			for (int num = sides_listtemp.Count; j < num; j++)
			{
				sides_list.Add(sides_listtemp[j]);
			}
		}
		int num2 = 0;
		while (sides_list.Count == 0 && num2 < 10)
		{
			num2++;
			rangemax++;
			sides_listtemp = GetRoundSideEmpty(findpathRect, entity.position, rangemax);
			int k = 0;
			for (int num3 = sides_listtemp.Count; k < num3; k++)
			{
				sides_list.Add(sides_listtemp[k]);
			}
		}
		if (sides_list.Count == 0)
		{
			Vector3 vector = RandomPosition();
			endx = vector.x;
			endz = vector.z;
		}
		else
		{
			int index = GameLogic.Random(0, sides_list.Count);
			Vector3 worldPosition = GetWorldPosition(sides_list[index]);
			endx = worldPosition.x;
			endz = worldPosition.z;
		}
	}

	public bool RandomItemLine(EntityBase entity, bool dir, int rangemin, int rangemax, out float endx, out float endz)
	{
		Vector2Int roomXY = GetRoomXY(entity.position);
		List<Vector2Int> list = (!dir) ? GetVerticalEmpty(entity) : GetHorizontalEmpty(entity);
		if (list.Count > 0)
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				int num2 = MathDxx.Abs(roomXY.x - list[num].x) + MathDxx.Abs(roomXY.y - list[num].y);
				if (num2 < rangemin || num2 > rangemax)
				{
					list.RemoveAt(num);
				}
			}
			int index = GameLogic.Random(0, list.Count);
			Vector3 worldPosition = GetWorldPosition(list[index]);
			endx = worldPosition.x;
			endz = worldPosition.z;
			return true;
		}
		endx = 0f;
		endz = 0f;
		return false;
	}

	public Vector2Int RandomItemSide(EntityBase entity)
	{
		List<Vector2Int> roundSideEmpty = GetRoundSideEmpty(findpathRect, entity.position, 1);
		if (roundSideEmpty.Count == 0)
		{
			return GetRoomXY(entity.position);
		}
		int index = GameLogic.Random(0, roundSideEmpty.Count);
		return roundSideEmpty[index];
	}

	public Vector2Int RandomRoomXY()
	{
		int x = GameLogic.Random(0, width);
		int y = GameLogic.Random(0, height);
		return new Vector2Int(x, y);
	}

	public Vector3 RandomPosition()
	{
		int x = GameLogic.Random(0, width);
		int y = GameLogic.Random(0, height);
		while (!IsEmpty(x, y))
		{
			x = GameLogic.Random(0, width);
			y = GameLogic.Random(0, height);
		}
		return GetWorldPosition(x, y);
	}

	public Vector3 RandomOutPosition()
	{
		int num = GameLogic.Random(0, 4);
		int x = 0;
		int y = 0;
		switch (num)
		{
		case 0:
			x = 0;
			y = GameLogic.Random(0, height);
			break;
		case 1:
			x = width - 1;
			y = GameLogic.Random(0, height);
			break;
		case 2:
			x = GameLogic.Random(0, width);
			y = 0;
			break;
		case 3:
			x = GameLogic.Random(0, width);
			y = height - 1;
			break;
		}
		return GetWorldPosition(x, y);
	}

	public List<Vector3> RandomOutPositions(int count)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < 7; i++)
		{
			Vector3 vector = RandomOutPosition();
			while (ishave(list, vector))
			{
				vector = RandomOutPosition();
			}
			list.Add(vector);
		}
		return list;
	}

	private bool ishave(List<Vector3> list, Vector3 pos)
	{
		int i = 0;
		for (int num = list.Count; i < num; i++)
		{
			Vector3 vector = list[i];
			if (vector.x == pos.x)
			{
				Vector3 vector2 = list[i];
				if (vector2.z == pos.z)
				{
					return true;
				}
			}
		}
		return false;
	}

	public Vector3 RandomPositionRange(EntityBase entity, int range)
	{
		if (!entity)
		{
			return Vector3.zero;
		}
		Vector2Int roomXY = GetRoomXY(entity.position);
		int value = GameLogic.Random(roomXY.x - range, roomXY.x + range);
		int value2 = GameLogic.Random(roomXY.y - range, roomXY.y + range);
		value = MathDxx.Clamp(value, 2, width - 2);
		value2 = MathDxx.Clamp(value2, 2, height - 2);
		return GetWorldPosition(value, value2);
	}

	public Vector3 RandomPosition(int area)
	{
		switch (area)
		{
		case 0:
		{
			int x4 = GameLogic.Random(0, width / 2);
			int y4 = GameLogic.Random(0, height / 2);
			return GetWorldPosition(x4, y4);
		}
		case 1:
		{
			int x3 = GameLogic.Random(width / 2, width);
			int y3 = GameLogic.Random(0, height / 2);
			return GetWorldPosition(x3, y3);
		}
		case 2:
		{
			int x2 = GameLogic.Random(0, width / 2);
			int y2 = GameLogic.Random(height / 2, height);
			return GetWorldPosition(x2, y2);
		}
		case 3:
		{
			int x = GameLogic.Random(width / 2, width);
			int y = GameLogic.Random(height / 2, height);
			return GetWorldPosition(x, y);
		}
		default:
			return RandomPosition();
		}
	}

	public bool IsValid(Vector2Int v)
	{
		return IsValid(v.x, v.y);
	}

	public bool IsValid(int x, int y)
	{
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	public bool ExcuteRelativeDirection(Vector2Int from, Vector2Int to, out Vector2Int dir, bool fly = false)
	{
		if (from.x == to.x)
		{
			int num = (from.y <= to.y) ? from.y : to.y;
			int num2 = (from.y >= to.y) ? from.y : to.y;
			if (IsValid(from) && IsValid(to) && !fly)
			{
				for (int i = num + 1; i < num2; i++)
				{
					if (findpathRect[from.x, i] != 0)
					{
						dir = default(Vector2Int);
						return false;
					}
				}
			}
			if (from.y < to.y)
			{
				dir = new Vector2Int(0, -1);
			}
			else
			{
				dir = new Vector2Int(0, 1);
			}
			return true;
		}
		if (from.y == to.y)
		{
			int num3 = (from.x <= to.x) ? from.x : to.x;
			int num4 = (from.x >= to.x) ? from.x : to.x;
			if (IsValid(from) && IsValid(to) && !fly)
			{
				for (int j = num3 + 1; j < num4; j++)
				{
					if (findpathRect[j, from.y] != 0)
					{
						dir = default(Vector2Int);
						return false;
					}
				}
			}
			if (from.x < to.x)
			{
				dir = new Vector2Int(1, 0);
			}
			else
			{
				dir = new Vector2Int(-1, 0);
			}
			return true;
		}
		dir = default(Vector2Int);
		return false;
	}

	private void DealWater()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (IsWater(i, j))
				{
					CreateWater(i, j);
				}
			}
		}
	}

	private void CreateWater(int x, int y)
	{
		GameObject original = ResourceManager.Load<GameObject>(Utils.GetString("Game/Goods/1006"));
		GameObject gameObject = UnityEngine.Object.Instantiate(original);
		mGoodsList.Add(gameObject);
		gameObject.transform.parent = mRoomCtrl.GoodsParent;
		float x2 = (float)x + CombineOffset.x;
		float z = (float)(-y) + CombineOffset.y;
		gameObject.transform.localPosition = new Vector3(x2, 0f, z);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		Transform transform = gameObject.transform.Find("child/good");
		if ((bool)transform)
		{
			SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
			if ((bool)component)
			{
				int num = water_checkround(x, y);
				component.sprite = SpriteManager.GetMap(Utils.FormatString("water{0}", num));
			}
		}
	}

	private int water_checkround(int x, int y)
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				waterchecks[i, j] = IsWater(i + x - 1, j + y - 1);
			}
		}
		return GetWaterID(waterchecks);
	}

	private int GetWaterID(bool[,] checks)
	{
		string text = string.Empty;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				text = ((!checks[j, i]) ? (text + "0") : (text + "1"));
			}
		}
		Goods_water beanById = LocalModelManager.Instance.Goods_water.GetBeanById(text);
		if (beanById != null)
		{
			int num = GameLogic.Random(0, beanById.WaterID.Length);
			return beanById.WaterID[num];
		}
		return checkwaterid(text);
	}

	private int checkwaterid(string s)
	{
		IList<Goods_water> allBeans = LocalModelManager.Instance.Goods_water.GetAllBeans();
		bool flag = false;
		int i = 0;
		for (int num = allBeans.Count; i < num; i++)
		{
			string checkID = allBeans[i].CheckID;
			flag = true;
			int j = 0;
			for (int length = checkID.Length; j < length; j++)
			{
				if (checkID[j] != '_' && checkID[j] != s[j])
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				int num2 = GameLogic.Random(0, allBeans[i].WaterID.Length);
				return allBeans[i].WaterID[num2];
			}
		}
		SdkManager.Bugly_Report("MapCreator_Water.checkwaterid", Utils.FormatString("{0} is check error.", s));
		return 1501;
	}

	private bool IsWater(int x, int y)
	{
		if (IsValid(x, y) && GetGoodID(x, y) == 1006)
		{
			return true;
		}
		return false;
	}

	private void water_test()
	{
		bool[,] checks = new bool[3, 3];
		water_test(checks, 0);
	}

	private bool water_test_iswater(bool[,] checks, int x, int y)
	{
		if (x >= 0 && x < 3 && y >= 0 && y < 3)
		{
			return checks[x, y];
		}
		return false;
	}

	private void water_test(bool[,] checks, int index)
	{
		int num = index % 3;
		int num2 = index / 3;
		checks[num, num2] = true;
		if (index == 8)
		{
			if (checks[1, 1])
			{
				GetWaterID(checks);
			}
		}
		else
		{
			water_test(checks, index + 1);
		}
		checks[num, num2] = false;
		if (index == 8)
		{
			if (checks[1, 1])
			{
				GetWaterID(checks);
			}
		}
		else
		{
			water_test(checks, index + 1);
		}
	}

	private void waveroom_init()
	{
		if (mRoomType != RoomGenerateBase.RoomType.eBoss && mRoomType != RoomGenerateBase.RoomType.eNormal)
		{
			SdkManager.Bugly_Report("MapCreator_WaveRoom", Utils.FormatString("RoomType:{0} in wave type  is not a valid type.", mRoomType));
			return;
		}
		GameLogic.Self.Event_PositionBy += waveroom_playermove;
		waveroom_maxwave.Clear();
		waveroom_time.Clear();
		waveroom_maxwave.Add(RoomGenerateBase.RoomType.eBoss, LocalModelManager.Instance.Stage_Level_stagechapter.waveroom_get_bosswave() - 1);
		waveroom_maxwave.Add(RoomGenerateBase.RoomType.eNormal, LocalModelManager.Instance.Stage_Level_stagechapter.waveroom_get_monsterwave() - 1);
		waveroom_time.Add(RoomGenerateBase.RoomType.eBoss, LocalModelManager.Instance.Stage_Level_stagechapter.waveroom_get_bosswave_time());
		waveroom_time.Add(RoomGenerateBase.RoomType.eNormal, LocalModelManager.Instance.Stage_Level_stagechapter.waveroom_get_monsterwave_time());
		waveroom_currentwave = -1;
		waveroom_startwave = false;
		waveroom_battlecache_init();
	}

	private void waveroom_deinit()
	{
		if ((bool)GameLogic.Self)
		{
			GameLogic.Self.Event_PositionBy -= waveroom_playermove;
		}
		waveroom_currentwave_createend = false;
		waveroom_killseq();
	}

	public void waveroom_killseq()
	{
		waveroom_pool.Clear();
	}

	private int get_max_wave()
	{
		if (waveroom_maxwave.TryGetValue(mRoomType, out int value))
		{
			return value;
		}
		return -1;
	}

	private int get_time()
	{
		if (waveroom_time.TryGetValue(mRoomType, out int value))
		{
			return value;
		}
		return -1;
	}

	private void waveroom_playermove(Vector3 moveby)
	{
		if (!GameLogic.Self || waveroom_startwave)
		{
			return;
		}
		Vector3 position = GameLogic.Self.position;
		if (position.z >= 0f)
		{
			waveroom_startwave = true;
			if (waveroom_currentwave < get_max_wave())
			{
				waveroom_createnext_wave();
			}
		}
	}

	private void waveroom_create_good()
	{
        Debug.Log("@LOG waveroom_create_good");
        waveroom_nodelist.Clear();
		XmlDocument xmlDocument = new XmlDocument();
		string tmxString = GetTmxString(MapID);
		if (tmxString.Equals(string.Empty))
		{
			SdkManager.Bugly_Report("MapCreator_Init", Utils.FormatString("readTileMap [{0}] mode:{1} stage:{2} is not found!!!", MapID, GameLogic.Hold.BattleData.GetMode().ToString(), GameLogic.Hold.BattleData.Level_CurrentStage));
			MapID = "firstroom";
			tmxString = GetTmxString(MapID);
		}
		xmlDocument.LoadXml(tmxString);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("map/tileset");
		XmlAttributeCollection attributes = xmlDocument.SelectSingleNode("map").Attributes;
		width = int.Parse(attributes["width"].Value);
		height = int.Parse(attributes["height"].Value);
		tiledata = new int[width, height];
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("map/layer");
		bool flag = false;
		if (waveroom_nodelist.Count == 0)
		{
			int num = 0;
			for (int i = 0; i < xmlNodeList.Count; i++)
			{
				XmlNode xmlNode2 = xmlNodeList[i];
				string value = xmlNode2.Attributes["name"].Value;
				if (value == "layer1")
				{
					flag = true;
					waveroom_createmap(xmlNode2);
				}
				else
				{
					waveroom_nodelist.Add(xmlNode2);
					num++;
				}
			}
		}
		if (!flag)
		{
			waveroom_createmap(xmlNodeList[0]);
		}
	}

	private void waveroom_createmap(XmlNode node)
	{
		string innerText = node.SelectSingleNode("data").InnerText;
		string[] array = innerText.Split('\n');
		for (int i = 1; i < array.Length - 1; i++)
		{
			string[] array2 = array[i].Split(',');
			for (int j = 0; j < width; j++)
			{
				tiledata[j, i - 1] = int.Parse(array2[j]);
			}
		}
		initRoomRealRectGoods();
		CreateAllGoods();
	}

	private void waveroom_createnext_wave()
	{
		if (waveroom_nodelist.Count == 0)
		{
			waveroom_currentwave_createend = true;
			waveroom_currentwave = get_max_wave();
			return;
		}
		XmlNode xmlNode = waveroom_random_node();
		string innerText = xmlNode.SelectSingleNode("data").InnerText;
		string[] array = innerText.Split('\n');
		for (int i = 1; i < array.Length - 1; i++)
		{
			string[] array2 = array[i].Split(',');
			for (int j = 0; j < width; j++)
			{
				tiledata[j, i - 1] = int.Parse(array2[j]);
			}
		}
		waveroom_currentwave++;
		send_ui(get_time());
		waveroom_currentwave_createend = false;
		Sequence s = waveroom_pool.Get();
		s.AppendCallback(delegate
		{
			List<Vector2Int> createPositionList = GetCreatePositionList();
			int k = 0;
			for (int num = createPositionList.Count; k < num; k++)
			{
				Vector3 worldPosition = GetWorldPosition(createPositionList[k]);
				GameLogic.PlayEffect(3100023, worldPosition);
			}
		});
		s.AppendInterval(0.9f).AppendCallback(delegate
		{
			CreateAllGoods();
			waveroom_currentwave_createend = true;
		});
		if (waveroom_currentwave < get_max_wave())
		{
			Sequence s2 = waveroom_pool.Get();
			s2.AppendInterval(get_time()).AppendCallback(delegate
			{
				waveroom_createnext_wave();
			});
		}
	}

	public void waveroom_currentwave_clear()
	{
		if (waveroom_currentwave < get_max_wave() && waveroom_currentwave_createend)
		{
			waveroom_killseq();
			waveroom_createnext_wave();
		}
	}

	public bool waveroom_is_clear()
	{
		if (!waveroom_currentwave_createend)
		{
			return false;
		}
		return waveroom_currentwave >= get_max_wave();
	}

	public void waveroom_battlecache_init()
	{
		if (LocalSave.Instance.BattleIn_GetIn())
		{
			waveroom_currentwave = int.MaxValue;
			waveroom_currentwave_createend = true;
		}
	}

	private XmlNode waveroom_random_node()
	{
		int index = GameLogic.Random(0, waveroom_nodelist.Count);
		XmlNode result = waveroom_nodelist[index];
		waveroom_nodelist.RemoveAt(index);
		return result;
	}

	private void send_ui(int time)
	{
		mWaveData.maxwave = get_max_wave() + 1;
		mWaveData.currentwave = waveroom_currentwave + 1;
		mWaveData.showui = (mWaveData.maxwave > 1 && mWaveData.currentwave < mWaveData.maxwave);
		mWaveData.lasttime = time;
		Facade.Instance.SendNotification("BattleUI_level_wave_update", mWaveData);
	}
}
