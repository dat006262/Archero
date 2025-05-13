




using Dxx.Util;
using System.Collections;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public class SoundData
	{
		public GameObject obj;

		public AudioSource audio;

		public bool Valid => obj != null && audio != null;
	}

	public enum BackgroundMusicType
	{
		eMain,
		eBattle
	}

	private class BGMusicData
	{
		public string path;

		public float volume;
	}

	private Dictionary<string, AudioClip> _soundDictionary;

	private Dictionary<string, Queue<SoundData>> _soundObjDic;

	private AudioSource audioSourceEffect;

	private const string SoundPath = "Sound/";

	private AnimationCurve animationCurve;

	private bool bSound;

	private int walk_walk;

	private string walk_path;

	private float walk_Time;

	private bool walk_Start;

	private bool bMusic;

	private static Dictionary<BackgroundMusicType, BGMusicData> mBGList = new Dictionary<BackgroundMusicType, BGMusicData>
	{
		{
			BackgroundMusicType.eMain,
			new BGMusicData
			{
				path = "uibg",
				volume = 1f
			}
		},
		{
			BackgroundMusicType.eBattle,
			new BGMusicData
			{
				path = "battlebg",
				volume = 0.5f
			}
		}
	};

	private void InitSound()
	{
		bSound = GetSoundLocal();
	}

	public bool GetSound()
	{
		return bSound;
	}

	private bool GetSoundLocal()
	{
		return PlayerPrefsEncrypt.GetBool("Sound", defaultValue: true);
	}

	public void SetSound(bool sound)
	{
		PlayerPrefsEncrypt.SetBool("Sound", sound);
		bSound = sound;
	}

	public bool ChangeSound()
	{
		SetSound(!bSound);
		return bSound;
	}

	public void DeInit()
	{
		_soundDictionary.Clear();
		_soundObjDic.Clear();
		GameNode.SoundNode.DestroyChildren();
	}

	private void Awake()
	{
		GameLogic.Hold.SetSound(this);
		InitSound();
		InitMusic();
		_soundDictionary = new Dictionary<string, AudioClip>();
		_soundObjDic = new Dictionary<string, Queue<SoundData>>();
		audioSourceEffect = GetComponent<AudioSource>();
		animationCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, -0.5f), new Keyframe(1.7f, 0.5f, -0.2f, -0.15f), new Keyframe(10f, 0f, 0f, 0f));
	}

	private IEnumerator Start()
	{
		yield return new WaitForSecondsRealtime(0.5f);
		PlayAudioSource(walk_path, 0f);
	}

	public void PreloadSound(string path)
	{
	}

	private GameObject PlayAtPoint(string audioEffectName, Vector3 pos, float volume = 1f)
	{
		if (!bSound)
		{
			return null;
		}
		audioEffectName = "Sound/" + audioEffectName;
		if (_soundDictionary.ContainsKey(audioEffectName))
		{
			return PlayAudio(audioEffectName, pos, volume);
		}
		AudioClip audioClip = ResourceManager.Load<AudioClip>(audioEffectName);
		if (audioClip != null)
		{
			_soundDictionary.Add(audioEffectName, audioClip);
			return PlayAudio(audioEffectName, pos, volume);
		}
		return null;
	}

	public GameObject PlayEntityDead(int id, Vector3 pos)
	{
		Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
		return PlayAtPoint(Utils.GetString(beanById.Path, id), pos, beanById.Volumn);
	}

	public GameObject PlayBodyHit(int id, Vector3 pos)
	{
		Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
		return PlayAtPoint(Utils.GetString(beanById.Path, id), pos, beanById.Volumn);
	}

	public GameObject PlayBattleSpecial(int id, Vector3 pos)
	{
		Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
		return PlayAtPoint(beanById.Path, pos, beanById.Volumn);
	}

	public GameObject PlayGetGoods(int id, Vector3 pos)
	{
		Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
		return PlayAtPoint(Utils.GetString(beanById.Path, id), pos, beanById.Volumn);
	}

	public GameObject PlayBulletHitWall(int id, Vector3 pos)
	{
		Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
		return PlayAtPoint(Utils.GetString(beanById.Path, id), pos, beanById.Volumn);
	}

	public GameObject PlayBulletDead(int id, Vector3 pos)
	{
		Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
		return PlayAtPoint(Utils.GetString(beanById.Path, id), pos, beanById.Volumn);
	}

	public void PlayUI(SoundUIType type)
	{
		PlayUI((int)type);
	}

	public void PlayUI(int id)
	{
		Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
		PlayAtPoint(beanById.Path, Vector3.zero, beanById.Volumn);
	}

	public GameObject PlayBulletCreate(int id, Vector3 pos)
	{
		Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
		return PlayAtPoint(Utils.GetString(beanById.Path, id), pos, beanById.Volumn);
	}

	public GameObject PlayHitted(int id, Vector3 pos, float volumn = -1f)
	{
		Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
		if (volumn < 0f)
		{
			volumn = beanById.Volumn;
		}
		return PlayAtPoint(beanById.Path, pos, volumn);
	}

	public GameObject PlayMonsterSkill(int id, Vector3 pos)
	{
		Sound_sound beanById = LocalModelManager.Instance.Sound_sound.GetBeanById(id);
		return PlayAtPoint(beanById.Path, pos, beanById.Volumn);
	}

	public void PlayAudioSource(string audioEffectName, float volumn = 1f)
	{
		if (!bSound)
		{
			return;
		}
		audioEffectName = Utils.GetString("Sound/", audioEffectName);
		if (_soundDictionary.ContainsKey(audioEffectName))
		{
			audioSourceEffect.clip = _soundDictionary[audioEffectName];
			audioSourceEffect.volume = volumn;
			audioSourceEffect.Play();
			return;
		}
		AudioClip audioClip = ResourceManager.Load<AudioClip>(audioEffectName);
		if (audioClip != null)
		{
			_soundDictionary.Add(audioEffectName, audioClip);
			audioSourceEffect.clip = _soundDictionary[audioEffectName];
			audioSourceEffect.volume = volumn;
			audioSourceEffect.Play();
		}
	}

	private GameObject PlayAudio(string sound, Vector3 pos, float volume)
	{
		Queue<SoundData> value = null;
		SoundData soundData = null;
		if (_soundObjDic.TryGetValue(sound, out value))
		{
			while (value.Count > 0)
			{
				soundData = value.Dequeue();
				if (soundData.Valid)
				{
					break;
				}
			}
		}
		if (soundData == null)
		{
			if (value == null)
			{
				value = new Queue<SoundData>();
				_soundObjDic.Add(sound, value);
			}
			soundData = new SoundData();
			soundData.obj = GameLogic.HoldGet("Sound/AudioSource");
			soundData.obj.name = sound;
			AutoDespawnSound component = soundData.obj.GetComponent<AutoDespawnSound>();
			component.callback = DespawnSoundCallback;
			component.sounddata = soundData;
			component.SetDespawnTime(_soundDictionary[sound].length);
			soundData.audio = soundData.obj.GetComponent<AudioSource>();
		}
		soundData.obj.SetActive(value: true);
		soundData.audio.transform.position = pos;
		soundData.audio.clip = _soundDictionary[sound];
		soundData.audio.volume = volume;
		soundData.audio.Play();
		return soundData.obj;
	}

	private void DespawnSoundCallback(string sound, SoundData data)
	{
		if (data != null)
		{
			Queue<SoundData> value = null;
			if (!_soundObjDic.TryGetValue(sound, out value))
			{
				value = new Queue<SoundData>();
				_soundObjDic.Add(sound, value);
			}
			if (data.obj != null)
			{
				value.Enqueue(data);
				data.obj.SetActive(value: false);
			}
		}
	}

	public GameObject PlayAudioAttack(string path, Vector3 pos, float volumn = 1f)
	{
		if (!bSound)
		{
			return null;
		}
		path = Utils.GetString("attack/", path);
		return PlayAtPoint(path, pos, volumn);
	}

	public void PlayWalk()
	{
		walk_Start = true;
	}

	public void StopWalk()
	{
		walk_Start = false;
	}

	private void WalkUpdate()
	{
		if (walk_Start && Updater.AliveTime - walk_Time > 0.3f)
		{
			walk_walk = GameLogic.Random(1, 8);
			walk_path = Utils.GetString("walk/footstep0", walk_walk);
			PlayAudioSource(walk_path, 0.5f);
			walk_Time = Updater.AliveTime;
		}
	}

	private void Update()
	{
		if (bSound)
		{
			WalkUpdate();
		}
	}

	private void InitMusic()
	{
		bMusic = GetMusicLocal();
		UpdateMusicVolume();
	}

	public bool GetMusic()
	{
		return bMusic;
	}

	private bool GetMusicLocal()
	{
		return PlayerPrefsEncrypt.GetBool("Music", defaultValue: true);
	}

	public bool ChangeMusic()
	{
		SetMusic(!bMusic);
		return bMusic;
	}

	private void UpdateMusicVolume()
	{
		GameNode.BackgroundMusic.volume = ((!bMusic) ? 0f : 1f);
	}

	public void SetMusic(bool music)
	{
		PlayerPrefsEncrypt.SetBool("Music", music);
		bMusic = music;
		UpdateMusicVolume();
	}

	public void PlayBackgroundMusic(BackgroundMusicType type)
	{
		string path = mBGList[type].path;
		if (_soundDictionary.TryGetValue(path, out AudioClip value))
		{
			GameNode.BackgroundMusic.clip = value;
			GameNode.BackgroundMusic.Play();
			return;
		}
		value = ResourceManager.Load<AudioClip>(Utils.FormatString("Sound/BG/{0}", path));
		if (value != null)
		{
			_soundDictionary.Add(path, value);
			GameNode.BackgroundMusic.clip = value;
			GameNode.BackgroundMusic.volume = ((!bMusic) ? 0f : mBGList[type].volume);
			GameNode.BackgroundMusic.Play();
		}
		else
		{
			GameNode.BackgroundMusic.Stop();
		}
	}

	public void PauseBackgroundMusic()
	{
		GameNode.BackgroundMusic.Pause();
	}

	public void ResumeBackgroundMusic()
	{
		GameNode.BackgroundMusic.UnPause();
	}

	public void StopBackgroundMusic()
	{
		GameNode.BackgroundMusic.Stop();
	}

	public void SetBackgroundVolume(float volume)
	{
		GameNode.BackgroundMusic.volume = volume;
	}
}
