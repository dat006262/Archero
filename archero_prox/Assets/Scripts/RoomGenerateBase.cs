




using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerateBase
{
	public enum RoomType
	{
		eInvalid,
		eNormal,
		eEvent,
		eBoss
	}

	public class Room
	{
		public int RoomID
		{
			get;
			private set;
		}

		public int ResourcesID
		{
			get;
			private set;
		}

		public int RoomHeight
		{
			get;
			private set;
		}

		public string TMXID
		{
			get;
			private set;
		}

		public bool IsBossRoom => RoomType == RoomType.eBoss;

		public RoomType RoomType
		{
			get;
			private set;
		}

		public float playery
		{
			get;
			private set;
		}

		public void SetRoomID(int id)
		{
			RoomType = RoomType.eNormal;
			RoomID = id;
			ResourcesID = 1;
			RoomHeight = 21;
		}

		public void SetTmx(string tmxid)
		{
			TMXID = tmxid;
			ExcuteTmxName();
			InitResource();
			InitPlayerY();
		}

		private void ExcuteTmxName()
		{
			RoomType = GameLogic.Release.MapCreatorCtrl.CheckTmxID(TMXID);
		}

		private void InitResource()
		{
			ResourcesID = GameLogic.Release.MapCreatorCtrl.GetRoomResourceID(TMXID);
			RoomHeight = GameLogic.Release.MapCreatorCtrl.GetRoomHeight("InitResource", TMXID);
		}

		private void InitPlayerY()
		{
			playery = (float)(-RoomHeight) / 2f - 1f;
		}
	}

	public class EventCloseTransfer
	{
		public WindowID windowid;

		public object data;
	}

	public class WaveData
	{
		public Action OnCreateWave;

		private float firstWaveTime;

		private int maxWave;

		private float waveTime;

		private int currentWave;

		private float wavealivetime;

		private Sequence s_wave;

		public bool IsEnd => currentWave >= maxWave && Updater.AliveTime - wavealivetime > 1f;

		public WaveData(float firstWaveTime, string[] args)
		{
			this.firstWaveTime = firstWaveTime;
			if (args.Length == 2)
			{
				maxWave = int.Parse(args[0]);
				waveTime = float.Parse(args[1]);
			}
			else
			{
				maxWave = 1;
				waveTime = 0f;
			}
		}

		public void Start()
		{
			currentWave = 0;
			UpdateActiveData();
		}

		public void Stop()
		{
			s_wave.Kill();
		}

		private void UpdateActiveData()
		{
			if (currentWave < maxWave)
			{
				s_wave = DOTween.Sequence();
				if (currentWave == 0)
				{
					s_wave.AppendInterval(firstWaveTime);
				}
				else
				{
					s_wave.AppendInterval(waveTime);
				}
				s_wave.AppendCallback(delegate
				{
					wavealivetime = Updater.AliveTime;
					currentWave++;
					if (OnCreateWave != null)
					{
						OnCreateWave();
					}
					UpdateActiveData();
				});
			}
		}
	}

	public const string FirstRoomTmx = "firstroom";

	public const string EmptyRoomTmx = "emptyroom";

	protected Action mGuidEndAction;

	protected List<string> mUsedMaps = new List<string>();

	protected Dictionary<string, GameObject> mapList = new Dictionary<string, GameObject>();

	protected Dictionary<int, Room> roomList = new Dictionary<int, Room>();

	private int _currentRoomID;

	private Sequence s_absorb;

	protected int maxRoomID;

	private int opendoorIndex;

	protected RoomControlBase roomCtrl;

	protected GameObject currentMap;

	protected float mCurrentRoomTime;

	private SequencePool mSequencePool = new SequencePool();

	private const int DemonInitRatio = 100;

	private int mDemonRatio = 100;

	protected int currentRoomID
	{
		get
		{
			return _currentRoomID;
		}
		set
		{
			_currentRoomID = value;
			GameLogic.Hold.BattleData.BossHPClear();
		}
	}

	public void Init()
	{
		mCurrentRoomTime = Updater.AliveTime;
		LocalSave.Instance.BattleIn_Init();
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	public void DeInit()
	{
		CacheMap(currentMap);
		mSequencePool.Clear();
		if (s_absorb != null)
		{
			s_absorb.Kill();
		}
		OnDeInit();
	}

	protected virtual void OnDeInit()
	{
	}

	public void Clear()
	{
	}

	public float GetCurrentRoomTime()
	{
		return mCurrentRoomTime;
	}

	public int GetCurrentRoomID()
	{
		return currentRoomID;
	}

	public virtual bool IsLastRoom()
	{
		return true;
	}

	public void PlayerMove()
	{
		if (Updater.AliveTime - mCurrentRoomTime < 1f)
		{
			mCurrentRoomTime -= 1f;
		}
	}

	public virtual void PlayerDead()
	{
	}

	public void StartGame()
	{
		roomList.Clear();
		opendoorIndex = -1;
		if (LocalSave.Instance.BattleIn_GetIn())
		{
			currentRoomID = LocalSave.Instance.BattleIn_GetRoomID();
			Room room = new Room();
			room.SetRoomID(currentRoomID);
			room.SetTmx(LocalSave.Instance.BattleIn_GetTmxID());
			roomList.Add(currentRoomID, room);
		}
		else
		{
			currentRoomID = 0;
			Room room2 = new Room();
			room2.SetRoomID(currentRoomID);
			room2.SetTmx(OnGetFirstRoomTMX());
			roomList.Add(currentRoomID, room2);
			UpdateBattleIn();
		}
		maxRoomID = GameLogic.Hold.BattleData.mModeData.GetMaxLayer();
		if (!LocalSave.Instance.BattleIn_GetIn())
		{
			CInstance<PlayerPrefsMgr>.Instance.gametime.set_value(0);
			CInstance<PlayerPrefsMgr>.Instance.gametime.flush();
		}
		OnStartGame();
		GotoNextDoor();
		OnStartGameEnd();
	}

	protected virtual void OnStartGame()
	{
	}

	protected virtual string OnGetFirstRoomTMX()
	{
		return "firstroom";
	}

	private bool CanRandomTmx(string tmxid)
	{
		return !mUsedMaps.Contains(tmxid);
	}

	private void AddUsedTmx(string tmxid)
	{
		mUsedMaps.Add(tmxid);
	}

	protected string RandomTmx(string[] tmxids)
	{
		return RandomTmx(tmxids, null);
	}

	protected string RandomTmx(string[] tmxids, XRandom random)
	{
		List<string> list = new List<string>();
		int i = 0;
		for (int num = tmxids.Length; i < num; i++)
		{
			string text = tmxids[i];
			if (CanRandomTmx(text))
			{
				list.Add(text);
			}
		}
		int index = random?.nextInt(0, list.Count) ?? GameLogic.Random(0, list.Count);
		string text2 = list[index];
		AddUsedTmx(text2);
		if (!GameLogic.Release.MapCreatorCtrl.HaveTmx(text2))
		{
			SdkManager.Bugly_Report("RoomGenerateBase", Utils.FormatString("stage:{0} RandomTmx[{1}] is dont have!!!", GameLogic.Hold.BattleData.Level_CurrentStage, text2));
			return RandomTmx(tmxids);
		}
		return text2;
	}

	private void GotoNextDoor()
	{
		RandomNextRoom();
		Room room = roomList[currentRoomID];
		GameObject gameObject = currentMap = CreateMapObject(room.ResourcesID);
		roomCtrl = gameObject.GetComponent<RoomControlBase>();
		gameObject.name += Utils.FormatString("_{0}_{1}_{2}", room.RoomID, room.ResourcesID, room.TMXID);
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1.23f);
		Room value = null;
		roomList.TryGetValue(currentRoomID + 1, out value);
		GameLogic.Release.Mode.SetGoodsParent(roomCtrl.GetGoodsDropParent());
		GameLogic.Release.MapCreatorCtrl.CreateMap(new MapCreator.Transfer
		{
			roomctrl = roomCtrl,
			roomid = currentRoomID,
			resourcesid = room.ResourcesID,
			tmxid = room.TMXID
		});
		roomCtrl.Init(new RoomControlBase.Mode_LevelData
		{
			room = room,
			nextroom = value
		});
		if (gotonextdoor_canopen())
		{
			OpenDoorDelay(0f);
		}
		else
		{
			GameLogic.Self.SetAbsorb(enable: false);
		}
		if (_currentRoomID > 0)
		{
			GameConfig.MapGood.Init();
			if (roomList[currentRoomID].IsBossRoom && (bool)GameLogic.Self && GameLogic.Self.OnInBossRoom != null)
			{
				GameLogic.Self.OnInBossRoom();
			}
			if (LocalSave.Instance.BattleIn_GetIn())
			{
				GameLogic.Self.SetPosition(new Vector3(0f, 0f, 0f - roomList[currentRoomID].playery));
			}
			else
			{
				GameLogic.Self.SetPosition(new Vector3(0f, 0f, roomList[currentRoomID].playery));
			}
			if (GameLogic.Release.Mode.OnGotoNextRoom != null)
			{
				GameLogic.Release.Mode.OnGotoNextRoom(roomList[currentRoomID]);
			}
			GameLogic.Self.OnGotoNextRoom();
			GameLogic.Release.Bullet.CacheAll();
			GameLogic.Release.MapEffect.Clear();
			CameraControlM.Instance.ResetCameraPosition();
			GameLogic.Self.SetAbsorbRangeMax(value: false);
			GameLogic.Hold.PreLoad(currentRoomID);
			ResourceManager.UnloadUnused();
		}
	}

	protected virtual void OnStartGameEnd()
	{
	}

	protected virtual bool gotonextdoor_canopen()
	{
		if (GameLogic.Release.Entity.GetActiveEntityCount() == 0)
		{
			return true;
		}
		return false;
	}

	public void EnterDoor()
	{
		mDemonRatio = 100;
		OnEnterDoorBefore();
		GameNode.DestroyMonsterNode();
		if ((bool)roomCtrl)
		{
			roomCtrl.Clear();
		}
		CacheMap(currentMap);
		GameLogic.Release.Entity.MonstersClear();
		currentRoomID++;
		mCurrentRoomTime = Updater.AliveTime;
		if (currentRoomID <= maxRoomID)
		{
			GotoNextDoor();
		}
		OnEnterDoorAfter();
	}

	private void RandomNextRoom()
	{
		int currentRoomID = this.currentRoomID;
		if (currentRoomID <= maxRoomID && !roomList.ContainsKey(currentRoomID))
		{
			Room room = new Room();
			room.SetRoomID(currentRoomID);
			if (currentRoomID == 0)
			{
				room.SetTmx("emptyroom");
			}
			else
			{
				room.SetTmx(OnGetTmxID(currentRoomID));
			}
			roomList.Add(currentRoomID, room);
		}
		int num = this.currentRoomID + 1;
		if (maxRoomID == 0)
		{
			SdkManager.Bugly_Report(GetType().ToString(), "maxRoomID is 0");
		}
		else if (num <= maxRoomID && !roomList.ContainsKey(num))
		{
			Room room2 = new Room();
			room2.SetRoomID(num);
			room2.SetTmx(OnGetTmxID(num));
			roomList.Add(num, room2);
		}
	}

	protected virtual void OnEnterDoorBefore()
	{
	}

	protected virtual void OnEnterDoorAfter()
	{
	}

	protected virtual string OnGetTmxID(int roomid)
	{
		SdkManager.Bugly_Report("RoomGenerateBase.OnGetTmxID", Utils.FormatString("{0} {1} is not found.", GetType().ToString(), roomid));
		return string.Empty;
	}

	public void CheckOpenDoor()
	{
		if (CanOpenDoor())
		{
			OpenDoor();
			if ((bool)GameLogic.Self)
			{
				GameLogic.Self.SetAbsorb(enable: true);
			}
		}
	}

	public virtual bool CanOpenDoor()
	{
		return GameLogic.Release.Entity.GetActiveEntityCount() == 0;
	}

	public void OpenDoor()
	{
		OpenDoorDelay(1.5f);
	}

	public bool IsDoorOpen()
	{
		return roomCtrl.IsDoorOpen();
	}

	private void OpenDoorDelay(float delay)
	{
		if (opendoorIndex != currentRoomID)
		{
			GameLogic.Self.SetAbsorbRangeMax(value: true);
			opendoorIndex = currentRoomID;
			if ((bool)GameLogic.Self && roomList[currentRoomID].IsBossRoom)
			{
				GameLogic.SendBuff(GameLogic.Self, 1100);
			}
			UpdateBattleIn();
			if (delay <= 0f)
			{
				OpenDoorDelayCallBack();
				return;
			}
			Sequence seq = DOTween.Sequence().AppendInterval(delay).AppendCallback(delegate
			{
				GameNode.CameraShake(CameraShakeType.Crit);
				OpenDoorDelayCallBack();
			});
			mSequencePool.Add(seq);
		}
	}

	private void OpenDoorDelayCallBack()
	{
		GameLogic.Hold.Sound.PlayUI(5000008);
		roomCtrl.OpenDoor(value: true);
		ShowBossDeadEvent();
		OnOpenDoor();
	}

	private void ShowBossDeadEvent()
	{
		if (roomList[currentRoomID].IsBossRoom && !IsLastRoom())
		{
			if ((bool)GameLogic.Self)
			{
				long value = GameLogic.Self.m_EntityData.attribute.KillBossShield.Value;
				GameLogic.Self.m_EntityData.UpdateShieldValueChange(value);
				float value2 = GameLogic.Self.m_EntityData.attribute.KillBossShieldPercent.Value;
				long change = MathDxx.CeilToInt((float)GameLogic.Self.m_EntityData.attribute.GetHPBase() * value2);
				GameLogic.Self.m_EntityData.UpdateShieldValueChange(change);
			}
			int num = GameLogic.Random(0, 100);
			int goodsID = 9001;
			if (num < mDemonRatio || GameLogic.Self.m_EntityData.GetOnlyDemon())
			{
				goodsID = 9008;
			}
			GameLogic.Release.MapCreatorCtrl.CreateOneGoods(goodsID, 5, 3);
		}
	}

	protected virtual void OnOpenDoor()
	{
	}

	private void UpdateBattleIn()
	{
		LocalSave.Instance.BattleIn_UpdateRoomID(currentRoomID);
		LocalSave.Instance.BattleIn_UpdateTmxID(roomList[currentRoomID].TMXID);
		LocalSave.Instance.BattleIn_UpdateResourcesID(roomList[currentRoomID].ResourcesID);
		LocalSave.Instance.BattleIn_SetHaveBattle(value: true);
		float gold = GameLogic.Hold.BattleData.GetGold();
		LocalSave.Instance.BattleIn_UpdateGold(gold);
		LocalSave.Instance.BattleIn_UpdateExp(GameLogic.Self.m_EntityData.GetCurrentExp());
		LocalSave.Instance.BattleIn_UpdateLevel(GameLogic.Self.m_EntityData.GetLevel());
	}

	protected GameObject CreateMapObject(int resourcesid)
	{
		string text = Utils.FormatString("Game/Map/Map{0}{1:D2}", SpriteManager.GetStylePrefix(), resourcesid);
		if (mapList.TryGetValue(text, out GameObject value))
		{
			value.gameObject.SetActive(value: true);
			value.transform.SetParent(GameNode.m_Room.transform);
			value.name = text;
			return value;
		}
		UnityEngine.Object @object = ResourceManager.Load<GameObject>(text);
		if (!@object)
		{
			SdkManager.Bugly_Report("RoomGenerateBase", Utils.FormatString("CreateMapObject ResourceManager.Load[{0}] is invalid!!!", text));
		}
		value = (UnityEngine.Object.Instantiate(@object) as GameObject);
		if (!value)
		{
			SdkManager.Bugly_Report("RoomGenerateBase", Utils.FormatString("CreateMapObject GameObject {0}] is invalid!!!", text));
		}
		value.transform.SetParent(GameNode.m_Room.transform);
		value.name = text;
		mapList.Add(text, value);
		return value;
	}

	protected static void CacheMap(GameObject o)
	{
		if (!(o == null))
		{
			o.GetComponent<RoomControlBase>().Clear();
			o.transform.SetParent(GameNode.MapCacheNode.transform);
			o.SetActive(value: false);
		}
	}

	public static void PreloadMap(int id)
	{
	}

	public void AddGuildToMap(GameObject o)
	{
		o.transform.SetParent(currentMap.transform);
		o.transform.localPosition = Vector3.zero;
		o.transform.localScale = Vector3.one;
		o.transform.localRotation = Quaternion.identity;
	}

	public void SetGuideEndAction(Action callback)
	{
		mGuidEndAction = callback;
	}

	public void EventClose(EventCloseTransfer data)
	{
		OnEventClose(data);
	}

	protected virtual void OnEventClose(EventCloseTransfer data)
	{
	}

	public void MonsterDead(EntityBase entity)
	{
		OnMonsterDead(entity);
	}

	protected virtual void OnMonsterDead(EntityBase entity)
	{
	}

	public void PlayerHitted(long changehp)
	{
		if (changehp < 0)
		{
			mDemonRatio -= GameConfig.GetDemonPerHit();
			int demonMin = GameConfig.GetDemonMin();
			if (mDemonRatio < demonMin)
			{
				mDemonRatio = demonMin;
			}
		}
		OnPlayerHitted(changehp);
	}

	protected virtual void OnPlayerHitted(long changehp)
	{
	}

	public bool IsBattleLoad()
	{
		return OnIsBattleLoad();
	}

	protected bool OnIsBattleLoad()
	{
		GameLogic.Hold.BattleData.SetLayer(currentRoomID);
		if (currentRoomID >= maxRoomID)
		{
			OnEnd();
			return false;
		}
		return true;
	}

	protected virtual void OnEnd()
	{
		LocalSave.Instance.BattleIn_DeInit();
		WindowUI.ShowWindow(WindowID.WindowID_GameOver);
	}

	protected virtual void OnReceiveEvent(string eventName, object data)
	{
	}

	public void SendEvent(string eventName, object data = null)
	{
		OnReceiveEvent(eventName, data);
	}

	public object GetEvent(string eventName, object data = null)
	{
		return OnGetEvent(eventName, data);
	}

	protected virtual object OnGetEvent(string eventName, object data = null)
	{
		return null;
	}
}
