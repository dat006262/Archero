




using System;
using UnityEngine;

namespace Dxx.Util
{
	public class Updater : MonoBehaviour
	{
		private static bool bCreate;

		private static float _AliveTime;

		private static float _deltatime;

		private static float _unscaleAliveTime;

		private static float _unscaledeltatime;

		public int count;

		private static Updater updater;

		public static float AliveTime => Time.time;

		public static float delta => Time.deltaTime;

		public static float deltaIgnoreTime => Time.unscaledDeltaTime;

		public static float unscaleAliveTime => Time.unscaledTime;

		public event Action<float> onUpdate;

		public event Action<float> onUpdateIgnoreTime;

		public event Action onLateUpdate;

		public event Action onFixedUpdate;

		public event Action<float> onUpdateUI;

		public event Action<float> onUpdateUIIgnoreTime;

		private void Update()
		{
			_deltatime = Time.deltaTime;
			_AliveTime += _deltatime;
			if (!GameLogic.Paused)
			{
				_unscaledeltatime = Time.unscaledDeltaTime;
				_unscaleAliveTime += _unscaledeltatime;
			}
			if (this.onUpdateIgnoreTime != null)
			{
				this.onUpdateIgnoreTime(deltaIgnoreTime);
			}
			if (this.onUpdate != null && !GameLogic.Paused)
			{
				this.onUpdate(delta);
			}
			if (this.onUpdateUI != null && !GameLogic.Paused)
			{
				this.onUpdateUI(delta);
			}
		}

		public void Init()
		{
		}

		public void OnRelease()
		{
			count = 0;
		}

		private void LateUpdate()
		{
			if (this.onLateUpdate != null && !GameLogic.Paused)
			{
				this.onLateUpdate();
			}
		}

		private void FixedUpdate()
		{
			if (this.onFixedUpdate != null && !GameLogic.Paused)
			{
				this.onFixedUpdate();
			}
		}

		public static Updater Get(GameObject go)
		{
			return go.GetComponent<Updater>() ?? go.AddComponent<Updater>();
		}

		public static Updater GetUpdater()
		{
			if (updater == null)
			{
				GameObject gameObject = new GameObject("updater");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				updater = Get(gameObject);
				bCreate = true;
			}
			return updater;
		}

		public static void UpdaterDeinit()
		{
			if (updater != null)
			{
				UnityEngine.Object.Destroy(updater.gameObject);
				updater = null;
				bCreate = false;
			}
		}

		public static void AddUpdate(string name, Action<float> func, bool IgnoreTimeScale = false)
		{
			Updater updater = GetUpdater();
			if (!IgnoreTimeScale)
			{
				updater.onUpdate += func;
			}
			else
			{
				updater.onUpdateIgnoreTime += func;
			}
		}

		public static void RemoveUpdate(string name, Action<float> func)
		{
			if (bCreate)
			{
				Updater updater = GetUpdater();
				updater.onUpdate -= func;
				updater.onUpdateIgnoreTime -= func;
			}
		}

		public static void AddLateUpdate(Action func)
		{
			GetUpdater().onLateUpdate += func;
		}

		public static void RemoveLateUpdate(Action func)
		{
			if (bCreate)
			{
				GetUpdater().onLateUpdate -= func;
			}
		}

		public static void AddFixedUpdate(Action func)
		{
			GetUpdater().onFixedUpdate += func;
		}

		public static void RemoveFixedUpdate(Action func)
		{
			if (bCreate)
			{
				GetUpdater().onFixedUpdate -= func;
			}
		}

		public static void AddUpdateUI(Action<float> func, bool IgnoreTimeScale = false)
		{
			if (!IgnoreTimeScale)
			{
				GetUpdater().onUpdateUI += func;
			}
			else
			{
				GetUpdater().onUpdateUIIgnoreTime += func;
			}
		}

		public static void RemoveUpdateUI(Action<float> func)
		{
			if (bCreate)
			{
				GetUpdater().onUpdateUI -= func;
				GetUpdater().onUpdateUIIgnoreTime -= func;
			}
		}
	}
}
