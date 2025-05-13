




#if ENABLE_BEST_HTTP
using BestHTTP.WebSocket;
#endif
using Newtonsoft.Json;
using System;
using System.Text;

public class MatchDefenceTimeSocketCtrl : Singleton<MatchDefenceTimeSocketCtrl>
{
	public enum ConnectState
	{
		eConnecting,
		eConnected,
		eClose
	}

	private const string url = "wss://api-archer.habby.mobi:4443/matching";

#if ENABLE_BEST_HTTP
	private WebSocket webSocket;
#endif

	private ConnectState mState = ConnectState.eClose;

	private string otheruserid = string.Empty;

	private string myname = string.Empty;

	public ConnectState State => mState;

	public bool IsConnected => mState == ConnectState.eConnected;

	public void init()
	{
#if ENABLE_BEST_HTTP
		if (webSocket == null)
		{
			webSocket = new WebSocket(new Uri("wss://api-archer.habby.mobi:4443/matching"));
			WebSocket obj = webSocket;
			obj.OnOpen = (OnWebSocketOpenDelegate)Delegate.Combine(obj.OnOpen, new OnWebSocketOpenDelegate(OnOpen));
			WebSocket obj2 = webSocket;
			obj2.OnMessage = (OnWebSocketMessageDelegate)Delegate.Combine(obj2.OnMessage, new OnWebSocketMessageDelegate(OnMessageReceived));
			WebSocket obj3 = webSocket;
			obj3.OnBinary = (OnWebSocketBinaryDelegate)Delegate.Combine(obj3.OnBinary, new OnWebSocketBinaryDelegate(OnBinaryReceived));
			WebSocket obj4 = webSocket;
			obj4.OnError = (OnWebSocketErrorDelegate)Delegate.Combine(obj4.OnError, new OnWebSocketErrorDelegate(OnError));
			WebSocket obj5 = webSocket;
			obj5.OnClosed = (OnWebSocketClosedDelegate)Delegate.Combine(obj5.OnClosed, new OnWebSocketClosedDelegate(OnClosed));
		}
#endif
	}

	public void Deinit()
	{
#if ENABLE_BEST_HTTP
		if (webSocket != null)
		{
			webSocket.Close();
		}
#endif
	}

	public void Connect()
	{
#if ENABLE_BEST_HTTP
		if (webSocket == null)
		{
			mState = ConnectState.eConnecting;
			init();
			otheruserid = string.Empty;
			webSocket.Open();
		}
#endif
	}

	public void Close()
	{
#if ENABLE_BEST_HTTP
		if (webSocket != null)
		{
			Send(MatchMessageType.eUserLeave);
			webSocket.OnOpen = null;
			webSocket.OnMessage = null;
			webSocket.OnError = null;
			webSocket.OnClosed = null;
			webSocket.Close();
			webSocket = null;
			mState = ConnectState.eClose;
		}
#endif
	}

	private byte[] getBytes(string message)
	{
		return Encoding.Default.GetBytes(message);
	}

	public void Send(MatchMessageType msgtype, int arg = 0)
	{
#if ENABLE_BEST_HTTP
		if (webSocket != null && webSocket.IsOpen)
		{
			MatchMessage matchMessage = new MatchMessage();
			myname = LocalSave.Instance.GetUserName();
			matchMessage.userid = LocalSave.Instance.GetServerUserID().ToString();
			if (myname == string.Empty)
			{
				myname = matchMessage.userid;
			}
			matchMessage.nickname = myname;
			matchMessage.msgtype = (short)msgtype;
			matchMessage.argint = arg;
			Send(JsonConvert.SerializeObject(matchMessage));
		}
#endif
	}

	public void Send(string str)
	{
#if ENABLE_BEST_HTTP
		webSocket.Send(str);
#endif
	}

#if ENABLE_BEST_HTTP
	private void OnOpen(WebSocket ws)
	{
		Debugger.Log("connected");
		mState = ConnectState.eConnected;
		Send(MatchMessageType.eUserComeIn);
	}

	private void OnMessageReceived(WebSocket ws, string message)
	{
		try
		{
			MatchMessage matchMessage = JsonConvert.DeserializeObject<MatchMessage>(message);
			if (string.IsNullOrEmpty(matchMessage.userid))
			{
				goto IL_0053;
			}
			if (string.IsNullOrEmpty(otheruserid))
			{
				otheruserid = matchMessage.userid;
				goto IL_0053;
			}
			if (!(otheruserid != matchMessage.userid))
			{
				goto IL_0053;
			}
			goto end_IL_0000;
			IL_0053:
			switch (matchMessage.msgtype)
			{
			case 10:
			{
				long num = matchMessage.argint;
				GameLogic.Hold.BattleData.Challenge_UpdateMode(13101, BattleSource.eMatch);
				GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_set_random_seed", num);
				WindowUI.ShowWindow(WindowID.WindowID_Battle);
				GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_me_updatename", myname);
				GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_other_updatename", matchMessage.nickname);
				break;
			}
			case 22:
				GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_other_dead");
				break;
			case 21:
				GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_other_learn_skill", matchMessage.argint);
				break;
			case 23:
				GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_other_reborn");
				break;
			case 24:
				GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_other_updatescore", matchMessage.argint);
				break;
			}
			end_IL_0000:;
		}
		catch
		{
		}
	}

	private void OnBinaryReceived(WebSocket ws, byte[] bytes)
	{
		Debugger.Log(Encoding.ASCII.GetString(bytes));
	}

	private void OnClosed(WebSocket ws, ushort code, string message)
	{
		Debugger.Log("onclose " + message);
	}
#endif

	private new void OnDestroy()
	{
#if ENABLE_BEST_HTTP
		if (webSocket != null && webSocket.IsOpen)
		{
			webSocket.Close();
			Deinit();
		}
#endif
	}

#if ENABLE_BEST_HTTP
	private void OnError(WebSocket ws, Exception ex)
	{
		string str = string.Empty;
		if (ws.InternalRequest.Response != null)
		{
			str = $"Status Code from Server: {ws.InternalRequest.Response.StatusCode} and Message: {ws.InternalRequest.Response.Message}";
		}
		Debugger.Log("OnError " + str);
	}
#endif
}
