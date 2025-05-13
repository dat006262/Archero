




using Dxx.Util;
using TableTool;
using UnityEngine;

public class HoldManager : MonoBehaviour
{
	private SoundManager _Sound;

	private LanguageManager _Language;

	private DropManager _Drop;

	private HoldPoolManager _Pool;

	private GuideManager _Guide;

	private int mPreLoadCount = -1;

	private BattleModuleData _BattleData;

	private Vector3 sound_pos = Vector3.one * 1000f;

	public SoundManager Sound => _Sound;

	public LanguageManager Language
	{
		get
		{
			if (_Language == null)
			{
				_Language = new LanguageManager();
			}
			return _Language;
		}
	}

	public DropManager Drop
	{
		get
		{
			if (_Drop == null)
			{
				_Drop = new DropManager();
			}
			return _Drop;
		}
	}

	public HoldPoolManager Pool
	{
		get
		{
			if (_Pool == null)
			{
				_Pool = new HoldPoolManager();
			}
			return _Pool;
		}
	}

	public GuideManager Guide
	{
		get
		{
			if (_Guide == null)
			{
				_Guide = new GuideManager();
			}
			return _Guide;
		}
	}

	public BattleModuleData BattleData
	{
		get
		{
			if (_BattleData == null)
			{
				_BattleData = new BattleModuleData();
			}
			return _BattleData;
		}
	}

	private void Awake()
	{
		GameLogic.SetHold(this);
		RoomGenerateBase.PreloadMap(1);
	}

	public void SetSound(SoundManager sound)
	{
		_Sound = sound;
	}

	public void BattleDataReset()
	{
		Drop.Reset();
		BattleData.Reset();
		mPreLoadCount = -1;
		PreLoad(0);
	}

	public void PreLoad(int id)
	{
		Preload_load beanById = LocalModelManager.Instance.Preload_load.GetBeanById(id);
		if (beanById != null && mPreLoadCount < id)
		{
			mPreLoadCount = id;
			PreLoadPlayerBullets(beanById.PlayerBulletsPath);
			PreLoadBullets(beanById.BulletsPath);
			PreLoadEffects(beanById.EffectsPath);
			PreLoadMapEffects(beanById.MapEffectsPath);
			PreLoadGoods(beanById.GoodsPath);
			PreLoadSounds(beanById.SoundPath);
		}
	}

	public void PreLoadPlayerBullets(string[] s)
	{
		int i = 0;
		for (int num = s.Length; i < num; i++)
		{
			string[] array = s[i].Split(',');
			int bulletID = int.Parse(array[0]);
			int count = int.Parse(array[1]);
			PreLoadPlayerBullet(bulletID, count);
		}
	}

	private void PreLoadBullets(string[] s)
	{
		int i = 0;
		for (int num = s.Length; i < num; i++)
		{
			string[] array = s[i].Split(',');
			int bulletID = int.Parse(array[0]);
			int count = int.Parse(array[1]);
			PreLoadBullet(bulletID, count);
		}
	}

	private void PreLoadEffects(string[] s)
	{
		int i = 0;
		for (int num = s.Length; i < num; i++)
		{
			string[] array = s[i].Split(',');
			string path = array[0];
			int count = int.Parse(array[1]);
			PreLoadEffect(path, count);
		}
	}

	private void PreLoadGoods(string[] s)
	{
		int i = 0;
		for (int num = s.Length; i < num; i++)
		{
			string[] array = s[i].Split(',');
			int goodid = int.Parse(array[0]);
			int count = int.Parse(array[1]);
			PreLoadGoods(goodid, count);
		}
	}

	private void PreLoadSounds(int[] ids)
	{
		int i = 0;
		for (int num = ids.Length; i < num; i++)
		{
			PreloadSound(ids[i]);
		}
	}

	public void PreloadSound(int soundid)
	{
		GameLogic.Hold.Sound.PlayHitted(soundid, sound_pos, 0f);
	}

	public void PreLoadPlayerBullet(int BulletID, int count)
	{
		GameObject[] array = new GameObject[count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = GameLogic.Release.PlayerBullet.Get(BulletID);
		}
		for (int j = 0; j < array.Length; j++)
		{
			GameLogic.Release.PlayerBullet.Cache(BulletID, array[j]);
		}
	}

	private void PreLoadBullet(int BulletID, int count)
	{
		GameObject[] array = new GameObject[count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = GameLogic.BulletGet(BulletID);
		}
		for (int j = 0; j < array.Length; j++)
		{
			GameLogic.BulletCache(BulletID, array[j]);
		}
	}

	private void PreLoadBullet(int BulletID)
	{
		GameObject[] array = new GameObject[6];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = GameLogic.BulletGet(BulletID);
		}
		for (int j = 0; j < array.Length; j++)
		{
			GameLogic.BulletCache(BulletID, array[j]);
		}
	}

	private void PreLoadGoods(int goodid, int count)
	{
		GameObject[] array = new GameObject[count];
		string @string = Utils.GetString("Game/Food/", goodid);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = GameLogic.EffectGet(@string);
		}
		for (int j = 0; j < array.Length; j++)
		{
			GameLogic.EffectCache(array[j]);
		}
	}

	private void PreLoadEffect(string path, int count)
	{
		GameObject[] array = new GameObject[count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = GameLogic.EffectGet(path);
		}
	}

	private void PreLoadMapEffects(string[] s)
	{
		int i = 0;
		for (int num = s.Length; i < num; i++)
		{
			string[] array = s[i].Split(',');
			string path = array[0];
			int count = int.Parse(array[1]);
			PreLoadMapEffect(path, count);
		}
	}

	private void PreLoadMapEffect(string path, int count)
	{
		GameObject[] array = new GameObject[count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = GameLogic.Release.MapEffect.Get(path);
		}
	}
}
