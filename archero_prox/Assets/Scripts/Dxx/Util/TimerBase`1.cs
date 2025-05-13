




using Dxx.Collections;
using System;
using UnityEngine;

namespace Dxx.Util
{
	public abstract class TimerBase<T> : MonoBehaviour where T : TimerBase<T>
	{
		private struct TimerData
		{
			public long id;

			public string key;

			public float interval;

			public Action<float> onTimer1;

			public Action onTimer2;

			public Action onCanceled;

			public int times;
		}

		private static T s_instance;

		private long mIdGen;

		private KeyedPriorityQueue<long, TimerData, double> mQueue = new KeyedPriorityQueue<long, TimerData, double>();

		protected virtual bool InGameUpdate => true;

		public event Action onTimerChanged;

		public static void Register(string key, float delay, float interval, Action onTimer)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, delay, interval, null, onTimer, null, 1);
			}
		}

		public static void Register(string key, float delay, float interval, Action<float> onTimer)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, delay, interval, onTimer, null, null, 1);
			}
		}

		public static void Register(string key, float delay, float interval, Action onTimer, Action onCanceled)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, delay, interval, null, onTimer, onCanceled, 1);
			}
		}

		public static void Register(string key, float delay, float interval, Action<float> onTimer, Action onCanceled)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, delay, interval, onTimer, null, onCanceled, 1);
			}
		}

		public static void Register(string key, float delay, float interval, Action onTimer, int times)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, delay, interval, null, onTimer, null, times);
			}
		}

		public static void Register(string key, float delay, float interval, Action<float> onTimer, int times)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, delay, interval, onTimer, null, null, times);
			}
		}

		public static void Register(string key, float delay, float interval, Action onTimer, Action onCanceled, int times)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, delay, interval, null, onTimer, onCanceled, times);
			}
		}

		public static void Register(string key, float delay, float interval, Action<float> onTimer, Action onCanceled, int times)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, delay, interval, onTimer, null, onCanceled, times);
			}
		}

		public static void Register(string key, float timeout, Action onTimer)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, timeout, timeout, null, onTimer, null, 1);
			}
		}

		public static void Register(string key, float timeout, Action<float> onTimer)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, timeout, timeout, onTimer, null, null, 1);
			}
		}

		public static void Register(string key, float timeout, Action onTimer, Action onCanceled)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, timeout, timeout, null, onTimer, onCanceled, 1);
			}
		}

		public static void Register(string key, float timeout, Action<float> onTimer, Action onCanceled)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, timeout, timeout, onTimer, null, onCanceled, 1);
			}
		}

		public static void Register(string key, float timeout, Action onTimer, int times)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, timeout, timeout, null, onTimer, null, times);
			}
		}

		public static void Register(string key, float timeout, Action<float> onTimer, int times)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, timeout, timeout, onTimer, null, null, times);
			}
		}

		public static void Register(string key, float timeout, Action onTimer, Action onCanceled, int times)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, timeout, timeout, null, onTimer, onCanceled, times);
			}
		}

		public static void Register(string key, float timeout, Action<float> onTimer, Action onCanceled, int times)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.RegisterInternal(key, timeout, timeout, onTimer, null, onCanceled, times);
			}
		}

		public static ulong Register(float delay, float interval, Action onTimer)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, delay, interval, null, onTimer, null, 1);
		}

		public static ulong Register(float delay, float interval, Action<float> onTimer)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, delay, interval, onTimer, null, null, 1);
		}

		public static ulong Register(float delay, float interval, Action onTimer, Action onCanceled)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, delay, interval, null, onTimer, onCanceled, 1);
		}

		public static ulong Register(float delay, float interval, Action<float> onTimer, Action onCanceled)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, delay, interval, onTimer, null, onCanceled, 1);
		}

		public static ulong Register(float delay, float interval, Action onTimer, int times)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, delay, interval, null, onTimer, null, times);
		}

		public static ulong Register(float delay, float interval, Action<float> onTimer, int times)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, delay, interval, onTimer, null, null, times);
		}

		public static ulong Register(float delay, float interval, Action onTimer, Action onCanceled, int times)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, delay, interval, null, onTimer, onCanceled, times);
		}

		public static ulong Register(float delay, float interval, Action<float> onTimer, Action onCanceled, int times)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, delay, interval, onTimer, null, onCanceled, times);
		}

		public static ulong Register(float timeout, Action onTimer)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, timeout, timeout, null, onTimer, null, 1);
		}

		public static ulong Register(float timeout, Action<float> onTimer)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, timeout, timeout, onTimer, null, null, 1);
		}

		public static ulong Register(float timeout, Action onTimer, Action onCanceled)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, timeout, timeout, null, onTimer, onCanceled, 1);
		}

		public static ulong Register(float timeout, Action<float> onTimer, Action onCanceled)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, timeout, timeout, onTimer, null, onCanceled, 1);
		}

		public static ulong Register(float timeout, Action onTimer, int times)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, timeout, timeout, null, onTimer, null, times);
		}

		public static ulong Register(float timeout, Action<float> onTimer, int times)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, timeout, timeout, onTimer, null, null, times);
		}

		public static ulong Register(float timeout, Action onTimer, Action onCanceled, int times)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, timeout, timeout, null, onTimer, onCanceled, times);
		}

		public static ulong Register(float timeout, Action<float> onTimer, Action onCanceled, int times)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return 0uL;
			}
			return instance.RegisterInternal(null, timeout, timeout, onTimer, null, onCanceled, times);
		}

		public static bool Unregister(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException(key);
			}
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return false;
			}
			return instance.UnregisterInternal(key);
		}

		public static void Unregister()
		{
			T instance = GetInstance();
			if (!((UnityEngine.Object)instance == (UnityEngine.Object)null))
			{
				instance.OnRemove();
			}
		}

		public static bool Unregister(ulong id)
		{
			T instance = GetInstance();
			if ((UnityEngine.Object)instance == (UnityEngine.Object)null)
			{
				return false;
			}
			return instance.UnregisterInternal((long)id);
		}

		private static T GetInstance()
		{
			if ((UnityEngine.Object)s_instance == (UnityEngine.Object)null && Application.isPlaying)
			{
				GameObject gameObject = new GameObject(typeof(T).Name);
				s_instance = gameObject.AddComponent<T>();
				gameObject.transform.parent = ((!s_instance.InGameUpdate) ? null : GameNode.m_Battle.transform);
			}
			return s_instance;
		}

		protected abstract void OnChanged();

		protected abstract void OnUpdate();

		protected int GetTimers()
		{
			return mQueue.Count;
		}

		private ulong RegisterInternal(string key, float delay, float interval, Action<float> onTimer1, Action onTimer2, Action onCanceled, int times)
		{
			long num = 0L;
			if (key == null)
			{
				num = ++mIdGen;
			}
			else
			{
				num += key.GetHashCode();
				num -= int.MaxValue;
				if (mQueue.TryGetItem(num, out TimerData value))
				{
					mQueue.RemoveFromQueue(num);
					if (value.onCanceled != null)
					{
						try
						{
							value.onCanceled();
						}
						catch (Exception exception)
						{
							UnityEngine.Debug.LogException(exception);
						}
					}
				}
			}
			TimerData value2 = default(TimerData);
			value2.id = num;
			value2.key = key;
			value2.interval = interval;
			value2.onTimer1 = onTimer1;
			value2.onTimer2 = onTimer2;
			value2.onCanceled = onCanceled;
			value2.times = times;
			mQueue.Enqueue(num, value2, GetCurrentTime() + delay);
			if (!base.enabled)
			{
				base.enabled = true;
			}
			OnChanged();
			if (this.onTimerChanged != null)
			{
				this.onTimerChanged();
			}
			return (ulong)((num > 0) ? num : 0);
		}

		private bool UnregisterInternal(string key)
		{
			long num = key.GetHashCode();
			num -= int.MaxValue;
			return UnregisterInternal(num);
		}

		private bool UnregisterInternal(long id)
		{
			bool flag = mQueue.RemoveFromQueue(id);
			if (flag && mQueue.Count <= 0 && base.enabled)
			{
				base.enabled = false;
			}
			if (flag)
			{
				OnChanged();
				if (this.onTimerChanged != null)
				{
					this.onTimerChanged();
				}
			}
			return flag;
		}

		public float GetCurrentTime()
		{
			return (!InGameUpdate) ? Time.time : Updater.AliveTime;
		}

		protected void Update()
		{
			if (InGameUpdate && !GameLogic.InGame)
			{
				return;
			}
			OnUpdate();
			double num = GetCurrentTime();
			while (mQueue.Count > 0)
			{
				mQueue.Peek(out long key, out double priority);
				if (priority > num)
				{
					break;
				}
				TimerData value = mQueue.Dequeue(out key, out priority);
				if (value.times != 1)
				{
					if (value.times > 0)
					{
						value.times--;
					}
					mQueue.Enqueue(key, value, priority + (double)value.interval);
				}
				if (value.onTimer1 != null)
				{
					try
					{
						value.onTimer1((float)(num - priority));
					}
					catch (Exception exception)
					{
						UnityEngine.Debug.LogException(exception);
					}
				}
				if (value.onTimer2 != null)
				{
					try
					{
						value.onTimer2();
					}
					catch (Exception exception2)
					{
						UnityEngine.Debug.LogException(exception2);
					}
				}
				if (mQueue.Count <= 0 && base.enabled)
				{
					base.enabled = false;
				}
				OnChanged();
				if (this.onTimerChanged != null)
				{
					this.onTimerChanged();
				}
			}
		}

		public void OnRemove()
		{
			UnityEngine.Object.Destroy(s_instance.gameObject);
			s_instance = (T)null;
		}
	}
}
