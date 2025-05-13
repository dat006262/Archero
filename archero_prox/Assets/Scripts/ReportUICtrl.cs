




using Dxx.Util;
using Newtonsoft.Json;
using PureMVC.Interfaces;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReportUICtrl : MediatorCtrlBase
{
	[Serializable]
	public class PlayerInfo
	{
		public ulong serveruserid;

		public string serveruseridsub;

		public string uuid;

		public int platform;

		public int sdklogintype;

		public string sdkloginid;

		public int source;
	}

	public Text Text_Title;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	public RectTransform viewparent;

	public UniWebView mView;

	private float width;

	private float height;

	private string url = "http://feedback.habby.fun/";

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_Report);
		};
		Button_Shadow.onClick = Button_Close.onClick;
		mView.Init(GameNode.m_Front.GetComponent<Canvas>());
		mView.OnMessageReceived += OnMessageReceived;
		mView.OnPageStarted += OnPageStarted;
		mView.OnPageFinished += OnPageFinished;
		mView.OnKeyCodeReceived += OnKeyCodeReceived;
		mView.OnPageErrorReceived += OnPageErrorReceived;
		mView.OnShouldClose += OnShouldClose;
		mView.SetBackButtonEnabled(enabled: false);
		mView.SetHorizontalScrollBarEnabled(enabled: false);
		mView.SetVerticalScrollBarEnabled(enabled: true);
		mView.SetBouncesEnabled(enabled: false);
		UniWebView.SetJavaScriptEnabled(enabled: true);
		width = mView.Frame.width;
		height = mView.Frame.height;
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		StartCoroutine("start_load");
	}

	private string get_player_info()
	{
		PlayerInfo playerInfo = new PlayerInfo();
		playerInfo.platform = PlatformHelper.GetPlatformID();
		playerInfo.sdklogintype = SdkManager.GetLoginType();
		playerInfo.serveruserid = LocalSave.Instance.GetServerUserID();
		playerInfo.serveruseridsub = LocalSave.Instance.GetServerUserIDSub();
		playerInfo.sdkloginid = LocalSave.Instance.GetUserID();
		playerInfo.uuid = PlatformHelper.GetUUID();
		playerInfo.source = 0;
		return JsonConvert.SerializeObject(playerInfo);
	}

	private IEnumerator start_load()
	{
		mView.Show();
		yield return null;
		mView.Load(url);
	}

	private void OnPageErrorReceived(UniWebView webView, int errorCode, string errorMessage)
	{
		Debugger.Log("WebView", "OnPageErrorReceived ：" + $"errorCode:{errorCode},errorMessage{errorMessage}");
	}

	private void OnKeyCodeReceived(UniWebView webView, int keyCode)
	{
		Debugger.Log("WebView", "OnKeyCodeReceived keycode:" + keyCode);
	}

	private void OnPageFinished(UniWebView webView, int statusCode, string url)
	{
		Debugger.Log("WebView", "OnPageFinished statusCode:" + $"statusCode:{statusCode},url{url}");
		mView.EvaluateJavaScript(Utils.FormatString("get_player_info('{0}');", get_player_info()), delegate(UniWebViewNativeResultPayload response)
		{
			if (response != null)
			{
				Debugger.Log("WebView", "EvaluateJavaScript response  identifier    : " + response.identifier);
				Debugger.Log("WebView", "EvaluateJavaScript response  resultCode    : " + response.resultCode);
				Debugger.Log("WebView", "EvaluateJavaScript response  data          : " + response.data);
			}
			else
			{
				Debugger.Log("WebView", "response is null!");
			}
		});
	}

	private void OnPageStarted(UniWebView webView, string url)
	{
		Debugger.Log("WebView", "OnPageStarted " + url);
	}

	private void OnMessageReceived(UniWebView webView, UniWebViewMessage message)
	{
		Debugger.Log("WebView", "OnMessageReceived :" + message.RawMessage);
		if (message.Path.Equals("game"))
		{
			string text = message.Args["score"];
			string text2 = message.Args["name"];
			UnityEngine.Debug.Log("Your final score is: " + text + "name :" + text2);
			if (mView.isActiveAndEnabled)
			{
				string jsString = $"openParamOne({int.Parse(text) * 2 + int.Parse(text2)});";
				mView.EvaluateJavaScript(jsString, delegate(UniWebViewNativeResultPayload payload)
				{
					if (payload.resultCode.Equals("0"))
					{
						UnityEngine.Debug.Log("Game Started!=========>");
					}
					else
					{
						UnityEngine.Debug.Log("Something goes wrong: " + payload.data);
					}
				});
			}
		}
	}

	private bool OnShouldClose(UniWebView webView)
	{
		Debugger.Log("WebView", "OnShouldClose");
		mView.CleanCache();
		return true;
	}

	protected override void OnClose()
	{
		mView.CleanCache();
		StopCoroutine("start_load");
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	public override void OnLanguageChange()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("setting_report");
	}
}
