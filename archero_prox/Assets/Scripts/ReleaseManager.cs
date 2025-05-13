




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class ReleaseManager : MonoBehaviour
{
	private GameManager _Game;

	private EntityManager _Entity;

	private BulletManager _Bullet;

	private BulletManager _PlayerBullet;

	private EffectManager _Effect;

	private MapEffectManager _MapEffect;

	private EntityCacheManager _EntityCache;

	private GameFormManager _Form;

	private MapCreator _MapCreator;

	private FindPath _Path;

	private GoodsCreateManager _GoodsCreate;

	private ReleaseModeManager _Mode;

	public GameManager Game
	{
		get
		{
			if (_Game == null)
			{
				_Game = new GameManager();
			}
			return _Game;
		}
	}

	public EntityManager Entity
	{
		get
		{
			if (_Entity == null)
			{
				_Entity = new GameObject("EntityManager").AddComponent<EntityManager>();
			}
			return _Entity;
		}
	}

	public BulletManager Bullet
	{
		get
		{
			if (_Bullet == null)
			{
				_Bullet = new BulletManager(0);
				_Bullet.parent = GameNode.m_BulletParent;
			}
			return _Bullet;
		}
	}

	public BulletManager PlayerBullet
	{
		get
		{
			if (_PlayerBullet == null)
			{
				_PlayerBullet = new BulletManager(1);
				_PlayerBullet.parent = GameNode.m_PlayerBullet;
			}
			return _PlayerBullet;
		}
	}

	public EffectManager Effect
	{
		get
		{
			if (_Effect == null)
			{
				_Effect = new EffectManager();
			}
			return _Effect;
		}
	}

	public MapEffectManager MapEffect
	{
		get
		{
			if (_MapEffect == null)
			{
				_MapEffect = new MapEffectManager();
			}
			return _MapEffect;
		}
	}

	public EntityCacheManager EntityCache
	{
		get
		{
			if (_EntityCache == null)
			{
				_EntityCache = new EntityCacheManager();
			}
			return _EntityCache;
		}
	}

	public GameFormManager Form
	{
		get
		{
			if (_Form == null)
			{
				_Form = new GameFormManager();
			}
			return _Form;
		}
	}

	public MapCreator MapCreatorCtrl
	{
		get
		{
			if (_MapCreator == null)
			{
				_MapCreator = new MapCreator();
			}
			return _MapCreator;
		}
	}

	public FindPath Path
	{
		get
		{
			if (_Path == null)
			{
				_Path = new FindPath();
			}
			return _Path;
		}
	}

	public GoodsCreateManager GoodsCreate
	{
		get
		{
			if (_GoodsCreate == null)
			{
				_GoodsCreate = new GoodsCreateManager();
			}
			return _GoodsCreate;
		}
	}

	public ReleaseModeManager Mode
	{
		get
		{
			if (_Mode == null)
			{
				_Mode = new ReleaseModeManager();
			}
			return _Mode;
		}
	}

	private void Awake()
	{
		GameLogic.SetRelease(this);
	}

	public void Release()
	{
		Time.timeScale = 1f;
		Bullet.Release();
		PlayerBullet.Release();
		Effect.Release();
		Form.Release();
		GoodsCreate.Release();
		if (_Path != null)
		{
			_Path.DeInit();
		}
		Game.Release();
		EntityCache.Release();
		Entity.DeInit();
		Mode.DeInit();
		MapEffect.Release();
		LocalModelManager.Instance.Drop_Drop.ClearGoldDrop();
		CInstance<TipsManager>.Instance.Clear();
		Updater.GetUpdater().OnRelease();
		Updater.UpdaterDeinit();
		Goods1151.DoorData.DeInit();
		UnityEngine.Object.DestroyImmediate(GameNode.m_Battle);
		GameNode.MapCacheNode.DestroyChildren();
		GameLogic.Hold.Sound.DeInit();
		if (_MapCreator != null)
		{
			_MapCreator.Deinit();
		}
		if (_Entity != null)
		{
			UnityEngine.Object.DestroyImmediate(_Entity.gameObject);
			_Entity = null;
		}
		TimerBase<Timer>.Unregister();
		TimeClock.Clear();
		GameNode.m_HP.DestroyChildren();
		_Game = null;
		_Bullet = null;
		_PlayerBullet = null;
		_Effect = null;
		_MapEffect = null;
		_EntityCache = null;
		_MapCreator = null;
		_Path = null;
		_GoodsCreate = null;
		_Mode = null;
		_Form = null;
		Goods1151.DoorData = null;
		GC.Collect();
	}
}
