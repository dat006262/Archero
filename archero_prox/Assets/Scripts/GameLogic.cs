




using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class GameLogic
{
	public enum EGameState
	{
		Main,
		Gaming,
		Pause,
		Over
	}

	private class AnimationData
	{
		public AnimationCurve[] curves;

		public AnimationData(int id1, int id2, int id3)
		{
			curves = new AnimationCurve[3];
			curves[0] = LocalModelManager.Instance.Curve_curve.GetCurve(id1);
			curves[1] = LocalModelManager.Instance.Curve_curve.GetCurve(id2);
			curves[2] = LocalModelManager.Instance.Curve_curve.GetCurve(id3);
		}
	}

	private static int bPause = 0;

	private static EGameState GameState = EGameState.Main;

	public const float RoomScaleZ = 1.23f;

	public static int DesignWidth = 720;

	public static int DesignHeight = 1280;

	public static int ScreenWidth = Screen.width;

	public static int ScreenHeight = Screen.height;

	public static float ScreenRatio = (float)ScreenHeight / (float)ScreenWidth;

	public static int Width = Screen.width;

	public static int Height = Screen.height;

	public static float WidthScale = (float)Width / (float)DesignWidth;

	public static float HeightScale = (float)Height / (float)DesignHeight;

	public static Vector2 ScreenSize;

	public static float WidthScaleAll = (!(WidthScale < HeightScale)) ? 1f : (WidthScale / HeightScale);

	private static float _WidthReal = 0f;

	private static Vector3 GetCanHit_mePos;

	private static Vector3 GetCanHit_dir;

	private static RaycastHit[] GetCanHit_rayhits;

	private static int GetCanHit_RayLength;

	private static Quaternion GetCanHit_ChildRotate;

	private static float GetCanHit_Angle;

	private static List<Vector2Int> RandomItem_list;

	private static HoldManager _Hold = null;

	private static ReleaseManager _Release = null;

	private static bool mInGame = false;

	private static SelfAttributeData _SelfAttribute = null;

	private static int BulletID = 0;

	private static Dictionary<HitType, AnimationData> mAnimationList = new Dictionary<HitType, AnimationData>();

	public const string QualityString = "SettingQuality";

	public static Dictionary<int, int> mQualitys = new Dictionary<int, int>
	{
		{
			1,
			480
		},
		{
			2,
			720
		},
		{
			3,
			1080
		}
	};

	private static int mBeforeWidth = -1;

	public static bool Paused => bPause > 0;

	public static float WidthReal
	{
		get
		{
			if (_WidthReal == 0f)
			{
				float num = (float)Width / (float)Height * (float)DesignHeight;
				num = (_WidthReal = MathDxx.Clamp(num, num, 720f));
			}
			return _WidthReal;
		}
		set
		{
			_WidthReal = value;
		}
	}

	public static HoldManager Hold => _Hold;

	public static ReleaseManager Release => _Release;

	public static bool InGame => mInGame;

	public static EntityHero Self => Release.Entity.Self;

	public static SelfAttributeData SelfAttribute
	{
		get
		{
			if (_SelfAttribute == null)
			{
				_SelfAttribute = new SelfAttributeData();
			}
			return _SelfAttribute;
		}
	}

	public static SelfAttributeData SelfAttributeShow
	{
		get
		{
			SelfAttributeData selfAttributeData = new SelfAttributeData();
			selfAttributeData.Init();
			return selfAttributeData;
		}
	}

	public static GameMode AdventureMode
	{
		get
		{
			return (GameMode)PlayerPrefs.GetInt("GameLogic.AdventureMode", 1000);
		}
		set
		{
			PlayerPrefs.SetInt("GameLogic.AdventureMode", (int)value);
		}
	}

	public static int Main_Stage
	{
		get
		{
			return PlayerPrefs.GetInt("GameLogic.Main_Stage", 0);
		}
		set
		{
			PlayerPrefs.SetInt("GameLogic.Main_Stage", value);
		}
	}

	public static int QualityID
	{
		get
		{
			if (!PlayerPrefsEncrypt.HasKey("SettingQuality"))
			{
				if (PlatformHelper.GetFlagShip())
				{
					QualityID = 3;
				}
				else
				{
					QualityID = 2;
				}
			}
			return PlayerPrefsEncrypt.GetInt("SettingQuality", 2);
		}
		set
		{
			PlayerPrefsEncrypt.SetInt("SettingQuality", value);
		}
	}

	public static void SetPause(bool pause)
	{
		bPause += (pause ? 1 : (-1));
		Time.timeScale = ((bPause <= 0) ? 1f : 0f);
	}

	public static void SetGameState(EGameState state)
	{
		GameState = state;
		switch (state)
		{
		case EGameState.Gaming:
			Release.Game.StartGame();
			break;
		case EGameState.Over:
			Release.Game.EndGame();
			break;
		}
	}

	public static void PlayBattle_Main()
	{
		int modeLevelKey = GameConfig.GetModeLevelKey();
        Debug.Log("@LOG GameLogic.PlayBattle_Main modeLevelKey:" + modeLevelKey);
        LocalSave.Instance.Modify_Key(-modeLevelKey);
		Hold.Sound.PlayUI(1000003);
		Hold.BattleData.SetMode(GameMode.eLevel, BattleSource.eWorld);
		WindowUI.ShowLoading(delegate
		{
			WindowUI.ShowWindow(WindowID.WindowID_Battle);
		}, null, null, BattleLoadProxy.LoadingType.eFirstBattle);
		CLifeTransPacket cLifeTransPacket = new CLifeTransPacket();
		cLifeTransPacket.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
		cLifeTransPacket.m_nType = 1;
		cLifeTransPacket.m_nMaterial = (ushort)modeLevelKey;
		NetManager.SendInternal(cLifeTransPacket, SendType.eCache, delegate(NetResponse response)
		{
            if (!response.IsSuccess)
			{
            }
		});
	}

	public static void ResetRectTransform(Transform t)
	{
		ResetRectTransform(t as RectTransform);
	}

	public static void ResetRectTransform(RectTransform tran)
	{
		tran.offsetMin = Vector2.zero;
		tran.offsetMax = Vector2.zero;
		tran.sizeDelta = Vector2.zero;
		tran.localPosition = Vector3.zero;
		tran.localScale = Vector3.one;
	}

	public static bool GetCanHit(EntityBase me, EntityBase other)
	{
		if (!me.Child)
		{
			return false;
		}
		if (!other)
		{
			return false;
		}
		if (!other.GetMeshShow())
		{
			return false;
		}
		Vector3 position = other.position;
		if (position.y < -1f)
		{
			return false;
		}
		GetCanHit_ChildRotate = me.Child.transform.localRotation;
		Vector3 position2 = other.position;
		float x = position2.x;
		Vector3 position3 = me.position;
		float x2 = x - position3.x;
		Vector3 position4 = other.position;
		float z = position4.z;
		Vector3 position5 = me.position;
		GetCanHit_Angle = Utils.getAngle(x2, z - position5.z);
		me.Child.transform.localRotation = Quaternion.Euler(0f, GetCanHit_Angle, 0f);
		GetCanHit_dir = other.position - me.m_Body.LeftBullet.transform.position;
		me.m_Body.LeftBullet.transform.rotation = Quaternion.Euler(0f, Utils.getAngle(GetCanHit_dir), 0f);
		GetCanHit_dir.y = 0f;
		GetCanHit_mePos = me.m_Body.LeftBullet.transform.position;
		GetCanHit_mePos.y = 0f;
		GetCanHit_rayhits = Physics.RaycastAll(GetCanHit_mePos, GetCanHit_dir, GetCanHit_dir.magnitude, (1 << LayerManager.Bullet2Map) | (1 << LayerManager.MapOutWall));
		me.Child.transform.localRotation = GetCanHit_ChildRotate;
		me.m_Body.LeftBullet.transform.localRotation = Quaternion.identity;
		int i = 0;
		for (int num = GetCanHit_rayhits.Length; i < num; i++)
		{
			if (GetCanHit_rayhits[i].collider.gameObject.layer == LayerManager.Bullet2Map || GetCanHit_rayhits[i].collider.gameObject.layer == LayerManager.MapOutWall)
			{
				other.SetCanHit(value: false);
				return false;
			}
		}
		other.SetCanHit(value: true);
		return true;
	}

	public static bool CheckLine(EntityBase self, EntityBase other)
	{
		Vector3 vector = other.position - self.position;
		Vector3 normalized = vector.normalized;
		float num = vector.magnitude - 0.5f;
		RaycastHit[] array = Physics.SphereCastAll(self.position + normalized * 0.01f, self.GetCollidersSize(), normalized, num - 0.01f);
		int i = 0;
		for (int num2 = array.Length; i < num2; i++)
		{
			RaycastHit raycastHit = array[i];
			if (raycastHit.collider.gameObject.layer == LayerManager.Stone || raycastHit.collider.gameObject.layer == LayerManager.Waters)
			{
				return true;
			}
		}
		return false;
	}

	public static void RandomItem(EntityBase entity, int range, out float endx, out float endz)
	{
		RandomItem_list = Release.MapCreatorCtrl.GetRoundEmpty(entity.position, range);
		if (RandomItem_list.Count == 0)
		{
			Vector3 position = entity.position;
			endx = position.x;
			Vector3 position2 = entity.position;
			endz = position2.z;
		}
		else
		{
			int index = Random(0, RandomItem_list.Count);
			Vector3 worldPosition = Release.MapCreatorCtrl.GetWorldPosition(RandomItem_list[index]);
			endx = worldPosition.x;
			endz = worldPosition.z;
		}
	}

	public static void SetHold(HoldManager hold)
	{
		_Hold = hold;
	}

	public static void SetRelease(ReleaseManager release)
	{
		_Release = release;
	}

	public static GameObject EffectGet(string key)
	{
		if ((bool)Release)
		{
			return Release.Effect.Get(key);
		}
		return null;
	}

	public static void EffectCache(GameObject o)
	{
		if ((bool)Release)
		{
			if (Release.MapEffect.check_is_map_effect(o))
			{
				Release.MapEffect.Cache(o);
			}
			else
			{
				Release.Effect.Cache(o);
			}
		}
		else if ((bool)o)
		{
			UnityEngine.Object.Destroy(o);
		}
	}

	public static GameObject EntityGet(string key)
	{
		if ((bool)Release)
		{
			return Release.EntityCache.Get(key);
		}
		return null;
	}

	public static void EntityCache(GameObject o, int maxcount)
	{
		if ((bool)Release)
		{
			Release.EntityCache.Cache(o, maxcount);
		}
	}

	public static GameObject BulletGet(int bulletID)
	{
		if ((bool)Release)
		{
			return Release.Bullet.Get(bulletID);
		}
		return null;
	}

	public static void BulletCache(int bulletID, GameObject o)
	{
		if ((bool)Release)
		{
			Release.Bullet.Cache(bulletID, o);
		}
	}

	public static GameObject HoldGet(string key)
	{
		return Hold.Pool.Get(key);
	}

	public static void HoldCache(GameObject o)
	{
		Hold.Pool.Cache(o);
	}

	public static void PlayEffect(int fxId, Vector3 position)
	{
		Fx_fx beanById = LocalModelManager.Instance.Fx_fx.GetBeanById(fxId);
		Transform transform = Release.MapEffect.Get(beanById.Path).transform;
		transform.position = position;
	}

	public static void SetInGame(bool gaming)
	{
		mInGame = gaming;
		GameNode.m_Camera.gameObject.SetActive(mInGame);
		GameNode.m_Light.SetActive(mInGame);
	}

	public static EntityBase FindTarget(EntityBase self)
	{
		return Release.Entity.GetNearTarget(self);
	}

	public static int GetBulletID()
	{
		int bulletID = BulletID;
		BulletID++;
		return bulletID;
	}

	public static bool IsSameTeam(EntityBase me, EntityBase other)
	{
		return GetTeam(me) == GetTeam(other);
	}

	private static int GetTeam(EntityBase entity)
	{
		if (entity == null)
		{
			return 0;
		}
		if (entity.Type == EntityType.Hero)
		{
			return 1;
		}
		if (entity.Type == EntityType.Baby)
		{
			if ((entity as EntityBabyBase).GetParent().Type == EntityType.Hero)
			{
				return 1;
			}
		}
		else if (entity.Type == EntityType.PartBody && (entity as EntityPartBodyBase).GetParent().Type == EntityType.Hero)
		{
			return 1;
		}
		return 2;
	}

	public static int Random(int min, int max)
	{
		return UnityEngine.Random.Range(min, max);
	}

	public static float Random(float min, float max)
	{
		return UnityEngine.Random.Range(min, max);
	}

	public static long Random(long min, long max)
	{
		return UnityEngine.Random.Range((int)min, (int)max);
	}

	public static List<BattleDropData> GetExpList(int exp)
	{
		List<BattleDropData> list = new List<BattleDropData>();
		int num = exp / 10;
		exp %= 10;
		int num2 = exp;
		for (int i = 0; i < num; i++)
		{
			list.Add(new BattleDropData(FoodType.eExp, FoodOneType.eExp02, 0));
		}
		for (int j = 0; j < num2; j++)
		{
			list.Add(new BattleDropData(FoodType.eExp, FoodOneType.eExp01, 0));
		}
		return list;
	}

	public static long GetMaxHP(int entityid)
	{
		EntityAttributeBase entityAttributeBase = new EntityAttributeBase(entityid);
		string[] monsterTmxAttributes = Hold.BattleData.mModeData.GetMonsterTmxAttributes();
		if (monsterTmxAttributes != null && monsterTmxAttributes.Length > 0)
		{
			int i = 0;
			for (int num = monsterTmxAttributes.Length; i < num; i++)
			{
				Goods_goods.GoodData goodData = Goods_goods.GetGoodData(monsterTmxAttributes[i]);
				entityAttributeBase.Excute(goodData);
			}
		}
		return entityAttributeBase.HPValue.Value;
	}

	public static AnimationCurve GetHPChangerAnimation(HitType type, int curve)
	{
		if (mAnimationList.TryGetValue(type, out AnimationData value))
		{
			return value.curves[curve];
		}
		switch (type)
		{
		case HitType.Crit:
			value = new AnimationData(100016, 100014, 100015);
			break;
		case HitType.HeadShot:
			value = new AnimationData(100017, 100014, 100015);
			break;
		default:
			value = new AnimationData(100013, 100014, 100015);
			break;
		}
		mAnimationList.Add(type, value);
		return value.curves[curve];
	}

	public static EElementType GetElement(string value)
	{
		if (value != null)
		{
			if (value == "Att_Fire")
			{
				return EElementType.eFire;
			}
			if (value == "Att_Poison")
			{
				return EElementType.ePoison;
			}
			if (value == "Att_Thunder")
			{
				return EElementType.eThunder;
			}
			if (value == "Att_Ice")
			{
				return EElementType.eIce;
			}
		}
		return EElementType.eNone;
	}

	public static void PlayEffect(string path, Transform parent)
	{
		GameObject gameObject = EffectGet(path);
		gameObject.transform.SetParent(parent);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
	}

	public static void SendHit_Bullet(EntityBase target, EntityBase source, long beforehit, HitType hittype, HitBulletStruct bulletdata)
	{
		SendHit_Bullet(target, source, beforehit, hittype, bulletdata, target.m_Data.HittedEffectID);
	}

	public static void SendHit_Bullet(EntityBase target, EntityBase source, long beforehit, HitType hittype, HitBulletStruct bulletdata, int soundid)
	{
		if (!(target == null))
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetBulletHitStruct(source, beforehit, hittype, bulletdata, soundid));
		}
	}

	public static void SendHit_Trap(EntityBase target, long beforehit)
	{
		if (!(target == null))
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetTrapHitStruct(target, beforehit));
		}
	}

	public static void SendHit_Trap(EntityBase target, long beforehit, int soundid)
	{
		if (!(target == null))
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetTrapHitStruct(target, beforehit, soundid));
		}
	}

	public static void SendHit_Body(EntityBase target, EntityBase source, long beforehit)
	{
		SendHit_Body(target, source, beforehit, 4100001);
	}

	public static void SendHit_Body(EntityBase target, EntityBase source, long beforehit, int soundid)
	{
		if (!(target == null) && beforehit < 0)
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetBodyHitStruct(source, beforehit, soundid));
		}
	}

	public static void SendHit_Buff(EntityBase target, EntityBase source, long beforehit, EElementType element, int buffid)
	{
		if (!(target == null))
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetBuffHitStruct(source, beforehit, element, buffid));
		}
	}

	public static void SendHit_Rebound(EntityBase target, EntityBase source, HitStruct hs)
	{
		if (!(target == null))
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetReboundHitStruct(source, hs));
		}
	}

	public static void SendHit_Skill(EntityBase target, long beforehit)
	{
		if (!(target == null))
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetSkillHitStruct(target, beforehit));
		}
	}

	public static void SendHit_Skill(EntityBase target, long beforehit, int soundid)
	{
		if (!(target == null))
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetSkillHitStruct(beforehit, soundid));
		}
	}

	public static void Send_Recover(EntityBase target, long value)
	{
		if (!(target == null))
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetRecoverStruct(value));
		}
	}

	private static HitStruct GetBulletHitStruct(EntityBase source, long beforehit, HitType hittype, HitBulletStruct bulletdata)
	{
		int soundid = 0;
		if (null != source && source.m_Data != null)
		{
			soundid = source.m_Data.HittedEffectID;
		}
		return GetBulletHitStruct(source, beforehit, hittype, bulletdata, soundid);
	}

	private static HitStruct GetBulletHitStruct(EntityBase source, long beforehit, HitType hittype, HitBulletStruct bulletdata, int soundid)
	{
		return GetHitStruct(source, beforehit, hittype, bulletdata, HitSourceType.eBullet, EElementType.eNone, 0, soundid);
	}

	private static HitStruct GetTrapHitStruct(EntityBase entity, long beforehit)
	{
		int soundid = 0;
		if (entity != null && entity.m_Data != null)
		{
			soundid = entity.m_Data.HittedEffectID;
		}
		return GetTrapHitStruct(beforehit, soundid);
	}

	private static HitStruct GetTrapHitStruct(EntityBase entity, long beforehit, int soundid)
	{
		return GetTrapHitStruct(beforehit, soundid);
	}

	private static HitStruct GetSkillHitStruct(long beforehit, int soundid)
	{
		return GetHitStruct(null, beforehit, HitType.Normal, null, HitSourceType.eSkill, EElementType.eNone, 0, soundid);
	}

	private static HitStruct GetSkillHitStruct(EntityBase entity, long beforehit)
	{
		int soundid = 0;
		if (entity != null && entity.m_Data != null)
		{
			soundid = entity.m_Data.HittedEffectID;
		}
		return GetSkillHitStruct(beforehit, soundid);
	}

	private static HitStruct GetTrapHitStruct(long beforehit, int soundid)
	{
		return GetHitStruct(null, beforehit, HitType.Normal, null, HitSourceType.eTrap, EElementType.eNone, 0, soundid);
	}

	private static HitStruct GetBodyHitStruct(EntityBase source, long beforehit)
	{
		int soundid = 4100001;
		return GetBodyHitStruct(source, beforehit, soundid);
	}

	private static HitStruct GetBodyHitStruct(EntityBase source, long beforehit, int soundid)
	{
		return GetHitStruct(source, beforehit, HitType.Normal, null, HitSourceType.eBody, EElementType.eNone, 0, soundid);
	}

	private static HitStruct GetBuffHitStruct(EntityBase source, long beforehit, EElementType element, int buffid)
	{
		int soundid = 0;
		return GetHitStruct(source, beforehit, HitType.Normal, null, HitSourceType.eBuff, element, buffid, soundid);
	}

	private static HitStruct GetReboundHitStruct(EntityBase source, HitStruct hs)
	{
		int soundid = 0;
		return GetHitStruct(source, hs.before_hit, HitType.Rebound, null, hs.sourcetype, EElementType.eNone, 0, soundid);
	}

	private static HitStruct GetRecoverStruct(long value)
	{
		return GetHitStruct(null, value, HitType.Add, null, HitSourceType.eRecover, EElementType.eNone, 0, 0);
	}

	private static HitStruct GetHitStruct(EntityBase source, long beforehit, HitType hittype, HitBulletStruct bulletdata, HitSourceType sourcetype, EElementType element, int buffid, int soundid)
	{
		HitStruct result = default(HitStruct);
		result.source = source;
		result.before_hit = beforehit;
		result.type = ((beforehit > 0) ? HitType.Add : hittype);
		result.bulletdata = bulletdata;
		result.sourcetype = sourcetype;
		result.element = element;
		result.buffid = buffid;
		result.soundid = soundid;
		return result;
	}

	public static void SendBuff(EntityBase target, EntityBase source, int buffid, params float[] args)
	{
		SendBuffInternal(target, source, buffid, args);
	}

	public static void SendBuffInternal(EntityBase target, EntityBase source, int buffid, params float[] args)
	{
		if (!(target == null))
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Add_Buff, new BattleStruct.BuffStruct
			{
				entity = source,
				buffId = buffid,
				args = args
			});
		}
	}

	public static void SendBuff(EntityBase target, int buffid, params float[] args)
	{
		if (!(target == null))
		{
			target.ExcuteCommend(EBattleAction.EBattle_Action_Add_Buff, new BattleStruct.BuffStruct
			{
				buffId = buffid,
				args = args
			});
		}
	}

	public static void UpdateResolution()
	{
		SetResolution(mQualitys[QualityID]);
	}

	public static void ResetMaxResolution()
	{
		SetResolution(1080);
	}

	private static void SetResolution(int width, int height)
	{
		if (mBeforeWidth != width)
		{
			mBeforeWidth = width;
			Debugger.Log("SetResolution w " + width + " h " + height);
			Screen.SetResolution(width, height, fullscreen: true);
		}
	}

	private static bool SetResolution(int res)
	{
		int screenHeight = ScreenHeight;
		int screenWidth = ScreenWidth;
		screenHeight = (int)((float)screenHeight * (float)res / (float)screenWidth);
		if (ScreenWidth >= res && ScreenHeight >= screenHeight)
		{
			SetResolution(res, screenHeight);
			return true;
		}
		SetResolution(ScreenWidth, ScreenHeight);
		return false;
	}

	public static void ShowHPMaxChange(long change)
	{
		CreateHPChanger(null, Self, new HitStruct
		{
			type = HitType.HPMaxChange,
			real_hit = change
		});
	}

	public static void CreateHPChanger(EntityBase from, EntityBase to, HitStruct hs)
	{
		GameObject gameObject = EffectGet("Game/UI/HPChanger");
		gameObject.SetParentNormal(GameNode.m_HP);
		gameObject.GetComponent<HPChanger>().Init(to, hs);
		if ((bool)from && from.IsSelf && hs.type == HitType.Crit && (hs.sourcetype == HitSourceType.eBullet || hs.sourcetype == HitSourceType.eBody))
		{
			GameNode.CameraShake(CameraShakeType.Crit);
		}
	}

	public static void ShowPowerUpdate(int before, int after)
	{
		GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/MainUI/PowerUpdate"));
		gameObject.SetParentNormal(GameNode.m_FrontEvent);
		RectTransform rectTransform = gameObject.transform as RectTransform;
		rectTransform.anchoredPosition = new Vector2(0f, (float)Height * -0.1f);
		gameObject.GetComponent<PowerUpdateCtrl>().Init(Random(100, 200), Random(100, 200));
	}
}
