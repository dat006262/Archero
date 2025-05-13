




using System;
using System.Collections.Generic;
using UnityEngine;

public class UniWebView : MonoBehaviour
{
	public delegate void PageStartedDelegate(UniWebView webView, string url);

	public delegate void PageFinishedDelegate(UniWebView webView, int statusCode, string url);

	public delegate void PageErrorReceivedDelegate(UniWebView webView, int errorCode, string errorMessage);

	public delegate void MessageReceivedDelegate(UniWebView webView, UniWebViewMessage message);

	public delegate bool ShouldCloseDelegate(UniWebView webView);

	public delegate void KeyCodeReceivedDelegate(UniWebView webView, int keyCode);

	public delegate void OrientationChangedDelegate(UniWebView webView, ScreenOrientation orientation);

	public delegate void OnWebContentProcessTerminatedDelegate(UniWebView webView);

	[Obsolete("OreintationChangedDelegate is a typo and deprecated. Use `OrientationChangedDelegate` instead.", true)]
	public delegate void OreintationChangedDelegate(UniWebView webView, ScreenOrientation orientation);

	private string id = Guid.NewGuid().ToString();

	private UniWebViewNativeListener listener;

	private bool isPortrait;

	[SerializeField]
	private string urlOnStart;

	[SerializeField]
	private bool showOnStart;

	private Dictionary<string, Action> actions = new Dictionary<string, Action>();

	private Dictionary<string, Action<UniWebViewNativeResultPayload>> payloadActions = new Dictionary<string, Action<UniWebViewNativeResultPayload>>();

	[SerializeField]
	private bool fullScreen;

	[SerializeField]
	private Rect frame;

	[SerializeField]
	private RectTransform referenceRectTransform;

	[SerializeField]
	private bool useToolbar;

	[SerializeField]
	private UniWebViewToolbarPosition toolbarPosition;

	private bool started;

	private Canvas mCanvas;

	private Color backgroundColor = Color.white;

	public Rect Frame
	{
		get
		{
			return frame;
		}
		set
		{
			frame = value;
			UpdateFrame();
		}
	}

	public RectTransform ReferenceRectTransform
	{
		get
		{
			return referenceRectTransform;
		}
		set
		{
			referenceRectTransform = value;
			UpdateFrame();
		}
	}

	public string Url => UniWebViewInterface.GetUrl(listener.Name);

	public bool CanGoBack => UniWebViewInterface.CanGoBack(listener.name);

	public bool CanGoForward => UniWebViewInterface.CanGoForward(listener.name);

	public Color BackgroundColor
	{
		get
		{
			return backgroundColor;
		}
		set
		{
			backgroundColor = value;
			UniWebViewInterface.SetBackgroundColor(listener.Name, value.r, value.g, value.b, value.a);
		}
	}

	public float Alpha
	{
		get
		{
			return UniWebViewInterface.GetWebViewAlpha(listener.Name);
		}
		set
		{
			UniWebViewInterface.SetWebViewAlpha(listener.Name, value);
		}
	}

	public event PageStartedDelegate OnPageStarted;

	public event PageFinishedDelegate OnPageFinished;

	public event PageErrorReceivedDelegate OnPageErrorReceived;

	public event MessageReceivedDelegate OnMessageReceived;

	public event ShouldCloseDelegate OnShouldClose;

	public event KeyCodeReceivedDelegate OnKeyCodeReceived;

	public event OrientationChangedDelegate OnOrientationChanged;

	public event OnWebContentProcessTerminatedDelegate OnWebContentProcessTerminated;

	[Obsolete("OnOreintationChanged is a typo and deprecated. Use `OnOrientationChanged` instead.", true)]
	public event OrientationChangedDelegate OnOreintationChanged;

	public void UpdateFrame()
	{
		Rect rect = NextFrameRect();
		UniWebViewInterface.SetFrame(listener.Name, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
	}

	private Rect NextFrameRect()
	{
		if (referenceRectTransform == null)
		{
			Debugger.Log("WebView", "referenceRectTransform == null");
			UniWebViewLogger.Instance.Info("Using Frame setting to determine web view frame.");
			return frame;
		}
		UniWebViewLogger.Instance.Info("Using reference RectTransform to determine web view frame.");
		Vector3[] array = new Vector3[4];
		referenceRectTransform.GetWorldCorners(array);
		Vector3 position = array[0];
		Vector3 position2 = array[1];
		Vector3 position3 = array[2];
		Vector3 position4 = array[3];
		Canvas canvas = mCanvas;
		switch (canvas.renderMode)
		{
		case RenderMode.ScreenSpaceCamera:
		case RenderMode.WorldSpace:
		{
			Camera worldCamera = canvas.worldCamera;
			if (worldCamera == null)
			{
				UniWebViewLogger.Instance.Critical("You need a render camera \r\n                        or event camera to use RectTransform to determine correct \r\n                        frame for UniWebView.");
				UniWebViewLogger.Instance.Info("No camera found. Fall back to ScreenSpaceOverlay mode.");
				break;
			}
			position = worldCamera.WorldToScreenPoint(position);
			position2 = worldCamera.WorldToScreenPoint(position2);
			position3 = worldCamera.WorldToScreenPoint(position3);
			position4 = worldCamera.WorldToScreenPoint(position4);
			if (Screen.width != GameLogic.ScreenWidth)
			{
				float num = (float)GameLogic.ScreenWidth / (float)Screen.width;
				float num2 = (float)GameLogic.ScreenHeight / (float)Screen.height;
				float x = position.x * num;
				float y = position2.y * num2;
				float x2 = position3.x / (float)Screen.width * (float)GameLogic.ScreenWidth;
				float y2 = position.y / (float)Screen.height * (float)GameLogic.ScreenHeight;
				position = new Vector3(x, y2, position.z);
				position2 = new Vector3(x, y, position2.z);
				position3 = new Vector3(x2, y, position3.z);
				position4 = new Vector3(x2, y2, position4.z);
			}
			break;
		}
		}
		float x3 = position2.x;
		float y3 = (float)GameLogic.ScreenHeight - position2.y;
		float width = position4.x - position2.x;
		float height = position2.y - position4.y;
		return new Rect(x3, y3, width, height);
	}

	private void _init()
	{
		GameObject gameObject = new GameObject(id);
		listener = gameObject.AddComponent<UniWebViewNativeListener>();
		gameObject.transform.parent = base.transform;
		listener.webView = this;
		UniWebViewNativeListener.AddListener(listener);
		Rect rect = (!fullScreen) ? NextFrameRect() : new Rect(0f, 0f, GameLogic.ScreenWidth, GameLogic.ScreenHeight);
		UniWebViewInterface.Init(listener.Name, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
		isPortrait = (GameLogic.ScreenHeight >= GameLogic.ScreenWidth);
	}

	public void Init(Canvas canvas)
	{
		mCanvas = canvas;
		_init();
	}

	private void Start()
	{
		if (showOnStart)
		{
			Show();
		}
		if (!string.IsNullOrEmpty(urlOnStart))
		{
			Load(urlOnStart);
		}
		started = true;
		if (referenceRectTransform != null)
		{
			UpdateFrame();
		}
	}

	private void Update()
	{
		bool flag = GameLogic.ScreenHeight >= GameLogic.ScreenWidth;
		if (isPortrait != flag)
		{
			isPortrait = flag;
			if (this.OnOrientationChanged != null)
			{
				this.OnOrientationChanged(this, isPortrait ? ScreenOrientation.Portrait : ScreenOrientation.LandscapeLeft);
			}
			if (referenceRectTransform != null)
			{
				UpdateFrame();
			}
		}
	}

	private void OnEnable()
	{
		if (started)
		{
			UniWebViewInterface.ShowWebViewDialog(listener.Name, show: true);
			Show();
		}
	}

	private void OnDisable()
	{
		if (started)
		{
			Hide();
			UniWebViewInterface.ShowWebViewDialog(listener.Name, show: false);
		}
	}

	public void Load(string url, bool skipEncoding = false)
	{
		UniWebViewInterface.Load(listener.Name, url, skipEncoding);
	}

	public void LoadHTMLString(string htmlString, string baseUrl, bool skipEncoding = false)
	{
		UniWebViewInterface.LoadHTMLString(listener.Name, htmlString, baseUrl, skipEncoding);
	}

	public void Reload()
	{
		UniWebViewInterface.Reload(listener.Name);
	}

	public void Stop()
	{
		UniWebViewInterface.Stop(listener.Name);
	}

	public void GoBack()
	{
		UniWebViewInterface.GoBack(listener.Name);
	}

	public void GoForward()
	{
		UniWebViewInterface.GoForward(listener.Name);
	}

	public void SetOpenLinksInExternalBrowser(bool flag)
	{
		UniWebViewInterface.SetOpenLinksInExternalBrowser(listener.Name, flag);
	}

	public bool Show(bool fade = false, UniWebViewTransitionEdge edge = UniWebViewTransitionEdge.None, float duration = 0.4f, Action completionHandler = null)
	{
		string text = Guid.NewGuid().ToString();
		bool flag = UniWebViewInterface.Show(listener.Name, fade, (int)edge, duration, text);
		if (flag && completionHandler != null)
		{
			if (fade || edge != 0)
			{
				actions.Add(text, completionHandler);
			}
			else
			{
				completionHandler();
			}
		}
		if (flag && useToolbar)
		{
			bool onTop = toolbarPosition == UniWebViewToolbarPosition.Top;
			SetShowToolbar(show: true, animated: false, onTop, fullScreen);
		}
		return flag;
	}

	public bool Hide(bool fade = false, UniWebViewTransitionEdge edge = UniWebViewTransitionEdge.None, float duration = 0.4f, Action completionHandler = null)
	{
		string text = Guid.NewGuid().ToString();
		bool flag = UniWebViewInterface.Hide(listener.Name, fade, (int)edge, duration, text);
		if (flag && completionHandler != null)
		{
			if (fade || edge != 0)
			{
				actions.Add(text, completionHandler);
			}
			else
			{
				completionHandler();
			}
		}
		if (flag && useToolbar)
		{
			bool onTop = toolbarPosition == UniWebViewToolbarPosition.Top;
			SetShowToolbar(show: false, animated: false, onTop, fullScreen);
		}
		return flag;
	}

	public bool AnimateTo(Rect frame, float duration, float delay = 0f, Action completionHandler = null)
	{
		string text = Guid.NewGuid().ToString();
		bool flag = UniWebViewInterface.AnimateTo(listener.Name, (int)frame.x, (int)frame.y, (int)frame.width, (int)frame.height, duration, delay, text);
		if (flag)
		{
			this.frame = frame;
			if (completionHandler != null)
			{
				actions.Add(text, completionHandler);
			}
		}
		return flag;
	}

	public void AddJavaScript(string jsString, Action<UniWebViewNativeResultPayload> completionHandler = null)
	{
		string text = Guid.NewGuid().ToString();
		UniWebViewInterface.AddJavaScript(listener.Name, jsString, text);
		if (completionHandler != null)
		{
			payloadActions.Add(text, completionHandler);
		}
	}

	public void EvaluateJavaScript(string jsString, Action<UniWebViewNativeResultPayload> completionHandler = null)
	{
		string text = Guid.NewGuid().ToString();
		UniWebViewInterface.EvaluateJavaScript(listener.Name, jsString, text);
		if (completionHandler != null)
		{
			payloadActions.Add(text, completionHandler);
		}
	}

	public void AddUrlScheme(string scheme)
	{
		if (scheme == null)
		{
			UniWebViewLogger.Instance.Critical("The scheme should not be null.");
		}
		else if (scheme.Contains("://"))
		{
			UniWebViewLogger.Instance.Critical("The scheme should not include invalid characters '://'");
		}
		else
		{
			UniWebViewInterface.AddUrlScheme(listener.Name, scheme);
		}
	}

	public void RemoveUrlScheme(string scheme)
	{
		if (scheme == null)
		{
			UniWebViewLogger.Instance.Critical("The scheme should not be null.");
		}
		else if (scheme.Contains("://"))
		{
			UniWebViewLogger.Instance.Critical("The scheme should not include invalid characters '://'");
		}
		else
		{
			UniWebViewInterface.RemoveUrlScheme(listener.Name, scheme);
		}
	}

	public void AddSslExceptionDomain(string domain)
	{
		if (domain == null)
		{
			UniWebViewLogger.Instance.Critical("The domain should not be null.");
		}
		else if (domain.Contains("://"))
		{
			UniWebViewLogger.Instance.Critical("The domain should not include invalid characters '://'");
		}
		else
		{
			UniWebViewInterface.AddSslExceptionDomain(listener.Name, domain);
		}
	}

	public void RemoveSslExceptionDomain(string domain)
	{
		if (domain == null)
		{
			UniWebViewLogger.Instance.Critical("The domain should not be null.");
		}
		else if (domain.Contains("://"))
		{
			UniWebViewLogger.Instance.Critical("The domain should not include invalid characters '://'");
		}
		else
		{
			UniWebViewInterface.RemoveSslExceptionDomain(listener.Name, domain);
		}
	}

	public void SetHeaderField(string key, string value)
	{
		if (key == null)
		{
			UniWebViewLogger.Instance.Critical("Header key should not be null.");
		}
		else
		{
			UniWebViewInterface.SetHeaderField(listener.Name, key, value);
		}
	}

	public void SetUserAgent(string agent)
	{
		UniWebViewInterface.SetUserAgent(listener.Name, agent);
	}

	public string GetUserAgent()
	{
		return UniWebViewInterface.GetUserAgent(listener.Name);
	}

	public static void SetAllowAutoPlay(bool flag)
	{
		UniWebViewInterface.SetAllowAutoPlay(flag);
	}

	public static void SetAllowInlinePlay(bool flag)
	{
	}

	public static void SetJavaScriptEnabled(bool enabled)
	{
		UniWebViewInterface.SetJavaScriptEnabled(enabled);
	}

	public static void SetAllowJavaScriptOpenWindow(bool flag)
	{
		UniWebViewInterface.SetAllowJavaScriptOpenWindow(flag);
	}

	public void CleanCache()
	{
		UniWebViewInterface.CleanCache(listener.Name);
	}

	public static void ClearCookies()
	{
		UniWebViewInterface.ClearCookies();
	}

	public static void SetCookie(string url, string cookie, bool skipEncoding = false)
	{
		UniWebViewInterface.SetCookie(url, cookie, skipEncoding);
	}

	public static string GetCookie(string url, string key, bool skipEncoding = false)
	{
		return UniWebViewInterface.GetCookie(url, key, skipEncoding);
	}

	public static void ClearHttpAuthUsernamePassword(string host, string realm)
	{
		UniWebViewInterface.ClearHttpAuthUsernamePassword(host, realm);
	}

	public void SetShowSpinnerWhileLoading(bool flag)
	{
		UniWebViewInterface.SetShowSpinnerWhileLoading(listener.Name, flag);
	}

	public void SetSpinnerText(string text)
	{
		UniWebViewInterface.SetSpinnerText(listener.Name, text);
	}

	public void SetHorizontalScrollBarEnabled(bool enabled)
	{
		UniWebViewInterface.SetHorizontalScrollBarEnabled(listener.Name, enabled);
	}

	public void SetVerticalScrollBarEnabled(bool enabled)
	{
		UniWebViewInterface.SetVerticalScrollBarEnabled(listener.Name, enabled);
	}

	public void SetBouncesEnabled(bool enabled)
	{
		UniWebViewInterface.SetBouncesEnabled(listener.Name, enabled);
	}

	public void SetZoomEnabled(bool enabled)
	{
		UniWebViewInterface.SetZoomEnabled(listener.Name, enabled);
	}

	public void AddPermissionTrustDomain(string domain)
	{
		UniWebViewInterface.AddPermissionTrustDomain(listener.Name, domain);
	}

	public void RemovePermissionTrustDomain(string domain)
	{
		UniWebViewInterface.RemovePermissionTrustDomain(listener.Name, domain);
	}

	public void SetBackButtonEnabled(bool enabled)
	{
		UniWebViewInterface.SetBackButtonEnabled(listener.Name, enabled);
	}

	public void SetUseWideViewPort(bool flag)
	{
		UniWebViewInterface.SetUseWideViewPort(listener.Name, flag);
	}

	public void SetLoadWithOverviewMode(bool flag)
	{
		UniWebViewInterface.SetLoadWithOverviewMode(listener.Name, flag);
	}

	public void SetImmersiveModeEnabled(bool enabled)
	{
		UniWebViewInterface.SetImmersiveModeEnabled(listener.Name, enabled);
	}

	public void SetShowToolbar(bool show, bool animated = false, bool onTop = true, bool adjustInset = false)
	{
	}

	public void SetToolbarDoneButtonText(string text)
	{
	}

	public static void SetWebContentsDebuggingEnabled(bool enabled)
	{
		UniWebViewInterface.SetWebContentsDebuggingEnabled(enabled);
	}

	public void SetWindowUserResizeEnabled(bool enabled)
	{
	}

	public void GetHTMLContent(Action<string> handler)
	{
		EvaluateJavaScript("document.documentElement.outerHTML", delegate(UniWebViewNativeResultPayload payload)
		{
			if (handler != null)
			{
				handler(payload.data);
			}
		});
	}

	public void SetAllowFileAccessFromFileURLs(bool flag)
	{
	}

	public void Print()
	{
		UniWebViewInterface.Print(listener.name);
	}

	private void OnDestroy()
	{
		UniWebViewNativeListener.RemoveListener(listener.Name);
		UniWebViewInterface.Destroy(listener.Name);
		UnityEngine.Object.Destroy(listener.gameObject);
	}

	private void OnApplicationPause(bool pause)
	{
		UniWebViewInterface.ShowWebViewDialog(listener.Name, !pause);
	}

	internal void InternalOnShowTransitionFinished(string identifier)
	{
		if (actions.TryGetValue(identifier, out Action value))
		{
			value();
			actions.Remove(identifier);
		}
	}

	internal void InternalOnHideTransitionFinished(string identifier)
	{
		if (actions.TryGetValue(identifier, out Action value))
		{
			value();
			actions.Remove(identifier);
		}
	}

	internal void InternalOnAnimateToFinished(string identifier)
	{
		if (actions.TryGetValue(identifier, out Action value))
		{
			value();
			actions.Remove(identifier);
		}
	}

	internal void InternalOnAddJavaScriptFinished(UniWebViewNativeResultPayload payload)
	{
		string identifier = payload.identifier;
		if (payloadActions.TryGetValue(identifier, out Action<UniWebViewNativeResultPayload> value))
		{
			value(payload);
			payloadActions.Remove(identifier);
		}
	}

	internal void InternalOnEvalJavaScriptFinished(UniWebViewNativeResultPayload payload)
	{
		string identifier = payload.identifier;
		if (payloadActions.TryGetValue(identifier, out Action<UniWebViewNativeResultPayload> value))
		{
			value(payload);
			payloadActions.Remove(identifier);
		}
	}

	internal void InternalOnPageFinished(UniWebViewNativeResultPayload payload)
	{
		if (this.OnPageFinished != null)
		{
			int result = -1;
			if (int.TryParse(payload.resultCode, out result))
			{
				this.OnPageFinished(this, result, payload.data);
			}
			else
			{
				UniWebViewLogger.Instance.Critical("Invalid status code received: " + payload.resultCode);
			}
		}
	}

	internal void InternalOnPageStarted(string url)
	{
		if (this.OnPageStarted != null)
		{
			this.OnPageStarted(this, url);
		}
	}

	internal void InternalOnPageErrorReceived(UniWebViewNativeResultPayload payload)
	{
		if (this.OnPageErrorReceived != null)
		{
			int result = -1;
			if (int.TryParse(payload.resultCode, out result))
			{
				this.OnPageErrorReceived(this, result, payload.data);
			}
			else
			{
				UniWebViewLogger.Instance.Critical("Invalid error code received: " + payload.resultCode);
			}
		}
	}

	internal void InternalOnMessageReceived(string result)
	{
		UniWebViewMessage message = new UniWebViewMessage(result);
		if (this.OnMessageReceived != null)
		{
			this.OnMessageReceived(this, message);
		}
	}

	internal void InternalOnWebViewKeyDown(int keyCode)
	{
		if (this.OnKeyCodeReceived != null)
		{
			this.OnKeyCodeReceived(this, keyCode);
		}
	}

	internal void InternalOnShouldClose()
	{
		if (this.OnShouldClose != null)
		{
			if (this.OnShouldClose(this))
			{
				UnityEngine.Object.Destroy(this);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	internal void InternalWebContentProcessDidTerminate()
	{
		if (this.OnWebContentProcessTerminated != null)
		{
			this.OnWebContentProcessTerminated(this);
		}
	}
}
