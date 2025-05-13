




using System;
using System.Collections.Generic;
using UnityEngine;
using static MoPub;

public class AdsRequestHelper : MonoBehaviour
{
	protected class RequestLoop
	{
		private enum Status
		{
			UNINITIALIZED,
			REQUESTING,
			FAILED,
			LOADED,
			SHOWING
		}

		public delegate void DoRequest();

		public delegate bool CheckLoaded();

		public delegate void ReportError(string error);

		private float lastRequestTime;

		private float lastLoadedTime;

		private const int MIN_REQUEST_INTERVAL = 10;

		private const int MAX_REQUEST_INTERVAL = 600;

		private const int MAX_LOADED_INTERVAL = 300;

		private const int MAX_SHOW_INTERVAL = 300;

		private Status status;

		private DoRequest doRequest;

		private CheckLoaded checkLoaded;

		private ReportError reportError;

		public RequestLoop(DoRequest doRequest, CheckLoaded checkLoaded, ReportError reportError)
		{
			this.doRequest = doRequest;
			this.checkLoaded = checkLoaded;
			this.reportError = reportError;
		}

		public void Init()
		{
			DebugLog("RequestLoop.Init()");
			Request();
		}

		private void Request()
		{
			DebugLog("RequestLoop.Request()");
			doRequest();
			lastRequestTime = Time.realtimeSinceStartup;
			status = Status.REQUESTING;
		}

		public void checkRequest()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			switch (status)
			{
			case Status.REQUESTING:
				if (lastRequestTime + 600f <= realtimeSinceStartup)
				{
					DebugLog("RequestLoop.REQUESTING");
					reportError("ADS Requesting Timeout");
					Request();
				}
				break;
			case Status.FAILED:
				if (lastRequestTime + 10f <= realtimeSinceStartup)
				{
					DebugLog("RequestLoop.FAILED");
					Request();
				}
				break;
			case Status.LOADED:
				if (lastLoadedTime + 300f <= realtimeSinceStartup && !checkLoaded())
				{
					DebugLog("RequestLoop.LOADED");
					reportError("ADS Expired");
					Request();
				}
				break;
			case Status.SHOWING:
				if (lastLoadedTime + 300f <= realtimeSinceStartup)
				{
					DebugLog("RequestLoop.SHOWING");
					reportError("ADS Showing too long");
					Request();
				}
				break;
			}
		}

		public void onLoad()
		{
			DebugLog("RequestLoop.onLoad()");
			lastLoadedTime = Time.realtimeSinceStartup;
			status = Status.LOADED;
		}

		public void onFail()
		{
			DebugLog("RequestLoop.onFail()");
			status = Status.FAILED;
		}

		public void onOpen()
		{
			DebugLog("RequestLoop.onOpen()");
			lastLoadedTime = Time.realtimeSinceStartup;
			status = Status.SHOWING;
		}

		public void onClose()
		{
			DebugLog("RequestLoop.onClose()");
			Request();
		}

		public bool isLoaded()
		{
			return status == Status.LOADED;
		}
	}

	protected class MsgQueue
	{
		private readonly Queue<Action> _executionQueue = new Queue<Action>();

		public void Update()
		{
			Action action = null;
			lock (_executionQueue)
			{
				if (_executionQueue.Count > 0)
				{
					action = _executionQueue.Dequeue();
					DebugLog("MsgQueue items count : " + _executionQueue.Count.ToString());
				}
			}
			action?.Invoke();
		}

		public void Run(Action action)
		{
			lock (_executionQueue)
			{
				_executionQueue.Enqueue(action);
			}
		}
	}

	public interface AdsDriver
	{
		bool isLoaded();

		bool isBusy();

		bool isPlaying();

		bool Show();
	}

	protected abstract class BaseDriver : AdsDriver
	{
		protected string adUnitId;

		protected AdsCallback callback;

		public abstract void Init(AdsCallback callback);

		public abstract bool isLoaded();

		public abstract bool isBusy();

		public abstract bool isPlaying();

		public abstract bool Show();

		public abstract void doRequest();

		public abstract string getName();

		public virtual void updateConfig(string config)
		{
		}

		public void LogFunc(string log)
		{
			DebugLog(getName() + "." + log);
		}
	}

	protected class MopubInterstitialDriver : BaseDriver
	{
		private bool playing;

		private bool busy;

		private bool loaded;

		public MopubInterstitialDriver(string adUnitId)
		{
			base.adUnitId = adUnitId;
		}

		public override string getName()
		{
			return "MopubInterstitialDriver";
		}

		public override void Init(AdsCallback callback)
		{
			LogFunc("Init()");
			MoPubManager.OnInterstitialLoadedEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnInterstitialLoadedEvent(" + adUnit + ")");
				loaded = true;
				callback.onLoad(this, "Mopub");
			};
			MoPubManager.OnInterstitialFailedEvent += delegate(string adUnit, string msg)
			{
				DebugLog("MoPubManager.OnInterstitialFailedEvent(" + adUnit + ", " + msg + ")");
				loaded = false;
				callback.onFail(this, msg);
			};
			MoPubManager.OnInterstitialShownEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnInterstitialShownEvent(" + adUnit + ")");
				playing = true;
				callback.onOpen(this, "Mopub");
			};
			MoPubManager.OnInterstitialDismissedEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnInterstitialDismissedEvent(" + adUnit + ")");
				playing = false;
				busy = false;
				loaded = false;
				callback.onClose(this, "Mopub");
			};
			MoPubManager.OnInterstitialClickedEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnInterstitialClickedEvent(" + adUnit + ")");
				callback.onClick(this, "Mopub");
			};
			MoPubManager.OnInterstitialExpiredEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnInterstitialExpiredEvent(" + adUnit + ")");
				loaded = false;
				callback.onFail(this, "Mopub ADS Expired");
			};
			base.callback = callback;
		}

		public override void doRequest()
		{
			LogFunc("doRequest()");
			DebugLog("MoPub.RequestInterstitialAd(" + adUnitId + ")");
            MoPub.RequestInterstitialAd(adUnitId, string.Empty, string.Empty);
			callback.onRequest(this, "Mopub");
		}

		public override bool isLoaded()
		{
			return loaded;
		}

		public override bool isBusy()
		{
			return busy;
		}

		public override bool isPlaying()
		{
			return playing;
		}

		public override bool Show()
		{
			LogFunc("Show()");
			if (isLoaded())
			{
				busy = true;
				DebugLog("MoPub.ShowInterstitialAd(" + adUnitId + ")");
                MoPub.ShowInterstitialAd(adUnitId);
				return true;
			}
			return false;
		}
	}

	protected class MopubRewardedDriver : BaseDriver
	{
		private bool playing;

		private bool busy;

		private bool loaded;

		public MopubRewardedDriver(string adUnitId)
		{
			base.adUnitId = adUnitId;
		}

		public override string getName()
		{
			return "MopubRewardedDriver";
		}

		public override void Init(AdsCallback callback)
		{
			LogFunc("Init()");
			MoPubManager.OnRewardedVideoLoadedEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnRewardedVideoLoadedEvent(" + adUnit + ")");
				loaded = true;
				callback.onLoad(this, "Mopub");
			};
			MoPubManager.OnRewardedVideoFailedEvent += delegate(string adUnit, string msg)
			{
				DebugLog("MoPubManager.OnRewardedVideoFailedEvent(" + adUnit + ", " + msg + ")");
				loaded = false;
				callback.onFail(this, msg);
			};
			MoPubManager.OnRewardedVideoShownEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnRewardedVideoShownEvent(" + adUnit + ")");
				playing = true;
				callback.onOpen(this, "Mopub");
			};
			MoPubManager.OnRewardedVideoFailedToPlayEvent += delegate(string adUnit, string msg)
			{
				DebugLog("MoPubManager.OnRewardedVideoFailedToPlayEvent(" + adUnit + ", " + msg + ")");
				playing = false;
				busy = false;
				loaded = false;
				callback.onClose(this, "Mopub");
				callback.onFail(this, msg);
			};
			MoPubManager.OnRewardedVideoClosedEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnRewardedVideoClosedEvent(" + adUnit + ")");
				playing = false;
				busy = false;
				loaded = false;
				callback.onClose(this, "Mopub");
			};
			MoPubManager.OnRewardedVideoClickedEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnRewardedVideoClickedEvent(" + adUnit + ")");
				callback.onClick(this, "Mopub");
			};
			MoPubManager.OnRewardedVideoLeavingApplicationEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnRewardedVideoLeavingApplicationEvent(" + adUnit + ")");
				callback.onClick(this, "Mopub");
			};
			MoPubManager.OnRewardedVideoExpiredEvent += delegate(string adUnit)
			{
				DebugLog("MoPubManager.OnRewardedVideoExpiredEvent(" + adUnit + ")");
				loaded = false;
				callback.onFail(this, "Mopub ADS Expired");
			};
			MoPubManager.OnRewardedVideoReceivedRewardEvent += delegate(string adUnit, string reward, float amount)
			{
				DebugLog("MoPubManager.OnRewardedVideoReceivedRewardEvent(" + adUnit + ")");
				callback.onReward(this, "Mopub");
			};
			base.callback = callback;
		}

		public override void doRequest()
		{
			LogFunc("doRequest()");
			DebugLog("MoPub.RequestRewardedVideo(" + adUnitId + ")");
            MoPub.RequestRewardedVideo(adUnitId);
			callback.onRequest(this, "Mopub");
		}

		public override bool isLoaded()
		{
			if (loaded != MoPub.HasRewardedVideo(adUnitId))
			{
				LogFunc("loaded = " + loaded + ", MoPub.HasRewardedVideo(" + adUnitId + ") = " + !loaded);
			}
			return MoPub.HasRewardedVideo(adUnitId);
		}

		public override bool Show()
		{
			LogFunc("Show()");
			if (isLoaded())
			{
				busy = true;
				DebugLog("MoPub.ShowRewardedVideo(" + adUnitId + ")");
                MoPub.ShowRewardedVideo(adUnitId);
				return true;
			}
			return false;
		}

		public override bool isBusy()
		{
			return busy;
		}

		public override bool isPlaying()
		{
			return playing;
		}
	}

	public interface AdsCallback
	{
		void onRequest(AdsDriver sender, string networkName);

		void onLoad(AdsDriver sender, string networkName);

		void onFail(AdsDriver sender, string msg);

		void onOpen(AdsDriver sender, string networkName);

		void onClose(AdsDriver sender, string networkName);

		void onClick(AdsDriver sender, string networkName);

		void onReward(AdsDriver sender, string networkName);
	}

	protected class WrappedDriver : BaseDriver, AdsCallback
	{
		private BaseDriver driver;

		private RequestLoop loop;

		private MsgQueue queue;

		public WrappedDriver(BaseDriver driver)
		{
			this.driver = driver;
		}

		public override string getName()
		{
			return driver.getName() + "+WrappedDriver";
		}

		public override void Init(AdsCallback callback)
		{
			LogFunc("Init()");
			base.callback = callback;
			queue = inst.queue;
			BaseDriver baseDriver = driver;
			RequestLoop.DoRequest doRequest = baseDriver.doRequest;
			BaseDriver baseDriver2 = driver;
			loop = new RequestLoop(doRequest, baseDriver2.isLoaded, reportError);
			driver.Init(this);
			loop.Init();
			inst.loops.Add(loop);
		}

		protected void reportError(string error)
		{
			onFail(this, error);
		}

		public override void doRequest()
		{
			LogFunc("doRequest()");
		}

		public override bool isLoaded()
		{
			return driver.isLoaded();
		}

		public override bool isBusy()
		{
			return driver.isBusy();
		}

		public override bool isPlaying()
		{
			return driver.isPlaying();
		}

		public override bool Show()
		{
			LogFunc("Show()");
			if (isLoaded())
			{
				loop.onOpen();
			}
			return driver.Show();
		}

		public void onRequest(AdsDriver sender, string networkName)
		{
			queue.Run(delegate
			{
				LogFunc("onRequest()");
				callback.onRequest(this, networkName);
			});
		}

		public void onLoad(AdsDriver sender, string networkName)
		{
			queue.Run(delegate
			{
				LogFunc("onLoad()");
				callback.onLoad(this, networkName);
				loop.onLoad();
			});
		}

		public void onFail(AdsDriver sender, string msg)
		{
			queue.Run(delegate
			{
				LogFunc("onFail()");
				callback.onFail(this, msg);
				loop.onFail();
			});
		}

		public void onOpen(AdsDriver sender, string networkName)
		{
			queue.Run(delegate
			{
				LogFunc("onOpen()");
				callback.onOpen(this, networkName);
			});
		}

		public void onClose(AdsDriver sender, string networkName)
		{
			queue.Run(delegate
			{
				LogFunc("onClose()");
				callback.onClose(this, networkName);
				loop.onClose();
			});
		}

		public void onClick(AdsDriver sender, string networkName)
		{
			queue.Run(delegate
			{
				LogFunc("onClick()");
				callback.onClick(this, networkName);
			});
		}

		public void onReward(AdsDriver sender, string networkName)
		{
			queue.Run(delegate
			{
				LogFunc("onReward()");
				callback.onReward(this, networkName);
			});
		}
	}

	protected class CombinedDriver : BaseDriver, AdsCallback
	{
		private enum Strategy
		{
			PRIORITIZED,
			RANDOM
		}

		private BaseDriver[] drivers;

		private int[] rates;

		private Dictionary<char, BaseDriver> driverList;

		private bool loaded;

		private Strategy strategy;

		public CombinedDriver(Dictionary<char, BaseDriver> driverList, string defaultConfig)
		{
			this.driverList = driverList;
			updateConfig(defaultConfig);
		}

		public override string getName()
		{
			return "CombinedDriver";
		}

		public override void updateConfig(string config)
		{
			LogFunc("UpdateConfig(" + config + ")");
			if (config == null || driverList == null)
			{
				return;
			}
			string[] array = config.Split(',');
			Strategy strategy = Strategy.PRIORITIZED;
			List<BaseDriver> list = new List<BaseDriver>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null || array[i].Length < 1)
				{
					continue;
				}
				char key = array[i][0];
				if (!driverList.ContainsKey(key))
				{
					continue;
				}
				int result;
				if (array[i].Length > 1)
				{
					if (!int.TryParse(array[i].Substring(1), out result) || result <= 0)
					{
						continue;
					}
					strategy = Strategy.RANDOM;
				}
				else
				{
					result = 0;
				}
				list.Add(driverList[key]);
				list2.Add(result);
			}
			rates = list2.ToArray();
			drivers = list.ToArray();
			this.strategy = strategy;
			LogFunc("UpdateConfig(" + config + ") SUCCESS, strategy = " + this.strategy.ToString() + ", drivers = " + drivers.Length);
			for (int j = 0; j < drivers.Length; j++)
			{
				LogFunc("drivers[" + j + "] = " + drivers[j].getName() + ", rates[" + j + "] = " + rates[j]);
			}
		}

		public override void Init(AdsCallback callback)
		{
			LogFunc("Init()");
			base.callback = callback;
			if (driverList != null)
			{
				foreach (BaseDriver value in driverList.Values)
				{
					value.Init(this);
				}
			}
			loaded = false;
		}

		public override void doRequest()
		{
			if (driverList != null)
			{
				LogFunc("doRequest() This shouldn't be called.");
				foreach (BaseDriver value in driverList.Values)
				{
					value.doRequest();
				}
			}
			loaded = false;
		}

		public override bool isLoaded()
		{
			if (drivers == null)
			{
				return false;
			}
			for (int i = 0; i < drivers.Length; i++)
			{
				if ((strategy != Strategy.RANDOM || rates[i] != 0) && drivers[i].isLoaded())
				{
					return true;
				}
			}
			return false;
		}

		public override bool isBusy()
		{
			if (drivers == null)
			{
				return false;
			}
			for (int i = 0; i < drivers.Length; i++)
			{
				if (drivers[i].isBusy())
				{
					return true;
				}
			}
			return false;
		}

		public override bool isPlaying()
		{
			if (drivers == null)
			{
				return false;
			}
			for (int i = 0; i < drivers.Length; i++)
			{
				if (drivers[i].isPlaying())
				{
					return true;
				}
			}
			return false;
		}

		public override bool Show()
		{
			if (drivers == null)
			{
				return false;
			}
			LogFunc("Show()");
			int num = 0;
			List<int> list = new List<int>();
			for (int i = 0; i < drivers.Length; i++)
			{
				LogFunc("drivers[" + i + "] = " + drivers[i].getName() + ", isLoaded = " + drivers[i].isLoaded());
				if (drivers[i].isLoaded())
				{
					if (strategy == Strategy.PRIORITIZED)
					{
						drivers[i].Show();
						return true;
					}
					num += rates[i];
					list.Add(i);
				}
			}
			if (strategy == Strategy.RANDOM)
			{
				if (num <= 0)
				{
					return false;
				}
				float num2 = UnityEngine.Random.Range(0f, num);
				foreach (int item in list)
				{
					num2 -= (float)rates[item];
					if (num2 <= 0f)
					{
						drivers[item].Show();
						return true;
					}
				}
			}
			return false;
		}

		public void onRequest(AdsDriver sender, string networkName)
		{
			LogFunc("onRequest()");
			int num = 0;
			while (true)
			{
				if (num < drivers.Length)
				{
					if (drivers[num] == sender)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			callback.onRequest(this, networkName);
		}

		public void onLoad(AdsDriver sender, string networkName)
		{
			LogFunc("onLoad()");
			int num = 0;
			while (true)
			{
				if (num < drivers.Length)
				{
					if (drivers[num] == sender)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			loaded = true;
			callback.onLoad(this, networkName);
		}

		public void onFail(AdsDriver sender, string msg)
		{
			LogFunc("onFail()");
			int num = 0;
			while (true)
			{
				if (num < drivers.Length)
				{
					if (drivers[num] == sender)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			loaded = false;
			callback.onFail(this, msg);
		}

		public void onOpen(AdsDriver sender, string networkName)
		{
			LogFunc("onOpen()");
			callback.onOpen(this, networkName);
		}

		public void onClose(AdsDriver sender, string networkName)
		{
			LogFunc("onClose()");
			callback.onClose(this, networkName);
		}

		public void onClick(AdsDriver sender, string networkName)
		{
			LogFunc("onClick()");
			callback.onClick(this, networkName);
		}

		public void onReward(AdsDriver sender, string networkName)
		{
			LogFunc("onReward()");
			callback.onReward(this, networkName);
		}
	}

	public interface CallbackManager
	{
		void AddCallback(AdsCallback callback);

		void RemoveCallback(AdsCallback callback);
	}

	protected class CallbackRouter : CallbackManager, AdsCallback
	{
		private List<AdsCallback> callbacks = new List<AdsCallback>();

		private AdsCallback exclusiveCallback;

		public void AddCallback(AdsCallback callback)
		{
			DebugLog("CallbackRouter.AddCallback()");
			callbacks.Add(callback);
		}

		public void RemoveCallback(AdsCallback callback)
		{
			DebugLog("CallbackRouter.RemoveCallback()");
			callbacks.Remove(callback);
		}

		public void SetExclusiveCallback(AdsCallback callback)
		{
			exclusiveCallback = callback;
		}

		public void onRequest(AdsDriver sender, string networkName)
		{
			DebugLog("CallbackRouter.onRequest(" + callbacks.Count + ")");
			foreach (AdsCallback callback in callbacks)
			{
				callback.onRequest(sender, networkName);
			}
		}

		public void onLoad(AdsDriver sender, string networkName)
		{
			DebugLog("CallbackRouter.onLoad(" + callbacks.Count + ")");
			foreach (AdsCallback callback in callbacks)
			{
				callback.onLoad(sender, networkName);
			}
		}

		public void onFail(AdsDriver sender, string msg)
		{
			DebugLog("CallbackRouter.onFail(" + callbacks.Count + ")");
			foreach (AdsCallback callback in callbacks)
			{
				callback.onFail(sender, msg);
			}
		}

		public void onOpen(AdsDriver sender, string networkName)
		{
			if (exclusiveCallback != null)
			{
				if (callbacks.Contains(exclusiveCallback))
				{
					DebugLog("CallbackRouter.onOpen()");
					exclusiveCallback.onOpen(sender, networkName);
				}
				else
				{
					DebugLog("CallbackRouter.onOpen(null)");
				}
			}
			else
			{
				DebugLog("CallbackRouter.onOpen(" + callbacks.Count + ")");
				foreach (AdsCallback callback in callbacks)
				{
					callback.onOpen(sender, networkName);
				}
			}
		}

		public void onClose(AdsDriver sender, string networkName)
		{
			if (exclusiveCallback != null)
			{
				if (callbacks.Contains(exclusiveCallback))
				{
					DebugLog("CallbackRouter.onClose()");
					exclusiveCallback.onClose(sender, networkName);
				}
				else
				{
					DebugLog("CallbackRouter.onClose(null)");
				}
			}
			else
			{
				DebugLog("CallbackRouter.onClose(" + callbacks.Count + ")");
				foreach (AdsCallback callback in callbacks)
				{
					callback.onClose(sender, networkName);
				}
			}
		}

		public void onClick(AdsDriver sender, string networkName)
		{
			if (exclusiveCallback != null)
			{
				if (callbacks.Contains(exclusiveCallback))
				{
					DebugLog("CallbackRouter.onClick()");
					exclusiveCallback.onClick(sender, networkName);
				}
				else
				{
					DebugLog("CallbackRouter.onClick(null)");
				}
			}
			else
			{
				DebugLog("CallbackRouter.onClick(" + callbacks.Count + ")");
				foreach (AdsCallback callback in callbacks)
				{
					callback.onClick(sender, networkName);
				}
			}
		}

		public void onReward(AdsDriver sender, string networkName)
		{
			if (exclusiveCallback != null)
			{
				if (callbacks.Contains(exclusiveCallback))
				{
					DebugLog("CallbackRouter.onReward()");
					exclusiveCallback.onReward(sender, networkName);
				}
				else
				{
					DebugLog("CallbackRouter.onReward(null)");
				}
			}
			else
			{
				DebugLog("CallbackRouter.onReward(" + callbacks.Count + ")");
				foreach (AdsCallback callback in callbacks)
				{
					callback.onReward(sender, networkName);
				}
			}
		}
	}

	public interface AdsAdapter : AdsDriver, CallbackManager
	{
		bool Show(AdsCallback enabledCallback);

		void UpdateConfig(string config);
	}

	protected class WrappedAdapter : AdsAdapter, AdsDriver, CallbackManager
	{
		private CallbackRouter callbacks = new CallbackRouter();

		private BaseDriver driver;

		public WrappedAdapter(BaseDriver driver)
		{
			this.driver = driver;
			driver.Init(callbacks);
		}

		public bool isLoaded()
		{
			try
			{
				return driver.isLoaded();
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				return false;
			}
		}

		public bool isBusy()
		{
			try
			{
				return driver.isBusy();
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				return false;
			}
		}

		public bool isPlaying()
		{
			try
			{
				return driver.isPlaying();
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				return false;
			}
		}

		public bool Show()
		{
			try
			{
				DebugLog("WrappedAdapter.Show()");
				callbacks.SetExclusiveCallback(null);
				return driver.Show();
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				return false;
			}
		}

		public bool Show(AdsCallback enabledCallback)
		{
			try
			{
				DebugLog("WrappedAdapter.Show(callback)");
				callbacks.SetExclusiveCallback(enabledCallback);
				return driver.Show();
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
				return false;
			}
		}

		public void UpdateConfig(string config)
		{
			try
			{
				driver.updateConfig(config);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}

		public void AddCallback(AdsCallback callback)
		{
			DebugLog("WrappedAdapter.AddCallback()");
			callbacks.AddCallback(callback);
		}

		public void RemoveCallback(AdsCallback callback)
		{
			DebugLog("WrappedAdapter.RemoveCallback()");
			callbacks.RemoveCallback(callback);
		}
	}

	protected class DummyAdapter : AdsAdapter, AdsDriver, CallbackManager
	{
		private List<AdsCallback> callbacks = new List<AdsCallback>();

		private AdsAdapter adapter;

		private string config;

		public bool isLoaded()
		{
			if (adapter != null)
			{
				return adapter.isLoaded();
			}
			return false;
		}

		public bool isBusy()
		{
			if (adapter != null)
			{
				return adapter.isBusy();
			}
			return false;
		}

		public bool isPlaying()
		{
			if (adapter != null)
			{
				return adapter.isPlaying();
			}
			return false;
		}

		public bool Show()
		{
			DebugLog("DummyAdapter.Show()");
			if (adapter != null)
			{
				return adapter.Show();
			}
			return false;
		}

		public bool Show(AdsCallback enabledCallback)
		{
			DebugLog("DummyAdapter.Show(callback)");
			if (adapter != null)
			{
				return adapter.Show(enabledCallback);
			}
			return false;
		}

		public void UpdateConfig(string config)
		{
			if (adapter != null)
			{
				adapter.UpdateConfig(config);
			}
			else
			{
				this.config = config;
			}
		}

		public void AddCallback(AdsCallback callback)
		{
			DebugLog("DummyAdapter.AddCallback()");
			try
			{
				if (adapter != null)
				{
					adapter.AddCallback(callback);
				}
				else
				{
					callbacks.Add(callback);
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}

		public void RemoveCallback(AdsCallback callback)
		{
			DebugLog("DummyAdapter.RemoveCallback()");
			if (adapter != null)
			{
				adapter.RemoveCallback(callback);
			}
			else
			{
				callbacks.Remove(callback);
			}
		}

		public void SetAdapter(AdsAdapter adapter)
		{
			DebugLog("DummyAdapter.SetAdapter()");
			foreach (AdsCallback callback in callbacks)
			{
				adapter.AddCallback(callback);
			}
			callbacks.Clear();
			if (config != null)
			{
				adapter.UpdateConfig(config);
				config = null;
			}
			this.adapter = adapter;
		}
	}

	public struct AdsConfiguration
	{
		public string admobApp;

		public string admobInterstitial;

		public string admobRewarded;

		public string mopubApp;

		public string mopubInterstitial;

		public string mopubRewarded;
	}

	protected static AdsRequestHelper inst;

	private AdsConfiguration config;

	protected MsgQueue queue = new MsgQueue();

	protected List<RequestLoop> loops = new List<RequestLoop>();

	private AdsAdapter interstitialAdapter;

	private AdsAdapter rewardedAdapter;

	private static DummyAdapter interstitialAdapterDummy;

	private static DummyAdapter rewardedAdapterDummy;

	public static void DebugLog(string msg)
	{
	}

	public static bool isInitialized()
	{
		return inst != null;
	}

	public static void Init()
	{
		string empty = string.Empty;
		empty = "171f58958cd34cc79d891c222471ec79";
		if (!string.IsNullOrEmpty(empty))
		{
			AdsConfiguration adsConfiguration = default(AdsConfiguration);
			adsConfiguration.mopubRewarded = empty;
			Init(adsConfiguration);
		}
	}

	public static void Init(AdsConfiguration config, Action<string> onComplete = null)
	{
#if UNITY_EDITOR
		return;
#endif
		if (inst == null)
		{
			DebugLog("AdsRequestHelper.Init()");
			try
			{
				DebugLog("MoPub.InitializeSdk()");
				if (onComplete != null)
				{
					MoPubManager.OnSdkInitializedEvent += onComplete;
				}
				SdkConfiguration sdkConfiguration = default(SdkConfiguration);
				sdkConfiguration.AdUnitId = config.mopubRewarded;
				sdkConfiguration.LogLevel = LogLevel.Debug;
				sdkConfiguration.MediatedNetworks = new MoPubBase.MediatedNetwork[5]
				{
					new SupportedNetwork.AdMob(),
					new SupportedNetwork.AppLovin(),
					new SupportedNetwork.Facebook(),
					new SupportedNetwork.IronSource(),
					new SupportedNetwork.Unity()
				};
                MoPub.InitializeSdk(sdkConfiguration);
				DebugLog("MoPub.LoadRewardedVideoPluginsForAdUnits()");
                MoPub.LoadRewardedVideoPluginsForAdUnits(new string[1]
				{
					config.mopubRewarded
				});
				GameObject gameObject = new GameObject("AdsHelperObject", typeof(AdsRequestHelper));
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				inst = gameObject.GetComponent<AdsRequestHelper>();
				inst.config = config;
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
	}

	protected AdsAdapter getInterstitialAdapterInternal()
	{
		if (interstitialAdapter == null)
		{
			DebugLog("AdsRequestHelper.getInterstitialAdapter()");
			Dictionary<char, BaseDriver> dictionary = new Dictionary<char, BaseDriver>();
			List<string> list = new List<string>();
			dictionary['M'] = new WrappedDriver(new MopubInterstitialDriver(config.mopubInterstitial));
			list.Add("M");
			interstitialAdapter = new WrappedAdapter(new CombinedDriver(dictionary, string.Join(",", list)));
		}
		return interstitialAdapter;
	}

	public static AdsAdapter getInterstitialAdapter()
	{
		if (inst == null)
		{
			if (interstitialAdapterDummy == null)
			{
				interstitialAdapterDummy = new DummyAdapter();
			}
			return interstitialAdapterDummy;
		}
		try
		{
			if (interstitialAdapterDummy != null)
			{
				interstitialAdapterDummy.SetAdapter(inst.getInterstitialAdapterInternal());
			}
			return inst.getInterstitialAdapterInternal();
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			if (interstitialAdapterDummy == null)
			{
				interstitialAdapterDummy = new DummyAdapter();
			}
			return interstitialAdapterDummy;
		}
	}

	protected AdsAdapter getRewardedAdapterInternal()
	{
		if (rewardedAdapter == null)
		{
			DebugLog("AdsRequestHelper.getRewardedAdapter()");
			Dictionary<char, BaseDriver> dictionary = new Dictionary<char, BaseDriver>();
			List<string> list = new List<string>();
			dictionary['M'] = new WrappedDriver(new MopubRewardedDriver(config.mopubRewarded));
			list.Add("M");
			rewardedAdapter = new WrappedAdapter(new CombinedDriver(dictionary, string.Join(",", list)));
		}
		return rewardedAdapter;
	}

	public static AdsAdapter getRewardedAdapter()
	{
		if (inst == null)
		{
			if (rewardedAdapterDummy == null)
			{
				rewardedAdapterDummy = new DummyAdapter();
			}
			return rewardedAdapterDummy;
		}
		try
		{
			if (rewardedAdapterDummy != null)
			{
				rewardedAdapterDummy.SetAdapter(inst.getRewardedAdapterInternal());
			}
			return inst.getRewardedAdapterInternal();
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			if (rewardedAdapterDummy == null)
			{
				rewardedAdapterDummy = new DummyAdapter();
			}
			return rewardedAdapterDummy;
		}
	}

	private void Update()
	{
		try
		{
			queue.Update();
			foreach (RequestLoop loop in loops)
			{
				loop.checkRequest();
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}
}
