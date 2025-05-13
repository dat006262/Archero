




#if ENABLE_BEST_HTTP
using BestHTTP.Cookies;
using BestHTTP.Examples;
using BestHTTP.JSON;
using BestHTTP.SignalR;
using BestHTTP.SignalR.JsonEncoders;
#endif
using System;
using UnityEngine;

public sealed class ConnectionAPISample : MonoBehaviour
{
	private enum MessageTypes
	{
		Send,
		Broadcast,
		Join,
		PrivateMessage,
		AddToGroup,
		RemoveFromGroup,
		SendToGroup,
		BroadcastExceptMe
	}

	private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/raw-connection/");

#if ENABLE_BEST_HTTP
	private Connection signalRConnection;
#endif

	private string ToEveryBodyText = string.Empty;

	private string ToMeText = string.Empty;

	private string PrivateMessageText = string.Empty;

	private string PrivateMessageUserOrGroupName = string.Empty;

#if ENABLE_BEST_HTTP
	private GUIMessageList messages = new GUIMessageList();
#endif

	private void Start()
	{
		if (PlayerPrefs.HasKey("userName"))
		{
#if ENABLE_BEST_HTTP
			CookieJar.Set(URI, new Cookie("user", PlayerPrefs.GetString("userName")));
#endif
		}
#if ENABLE_BEST_HTTP
		signalRConnection = new Connection(URI);
		signalRConnection.JsonEncoder = new LitJsonEncoder();
		signalRConnection.OnStateChanged += signalRConnection_OnStateChanged;
		signalRConnection.OnNonHubMessage += signalRConnection_OnGeneralMessage;
		signalRConnection.Open();
#endif
	}

	private void OnGUI()
	{
#if ENABLE_BEST_HTTP
		GUIHelper.DrawArea(GUIHelper.ClientArea, drawHeader: true, delegate
		{
			GUILayout.BeginVertical();
			GUILayout.Label("To Everybody");
			GUILayout.BeginHorizontal();
			ToEveryBodyText = GUILayout.TextField(ToEveryBodyText, GUILayout.MinWidth(100f));
			if (GUILayout.Button("Broadcast"))
			{
				Broadcast(ToEveryBodyText);
			}
			if (GUILayout.Button("Broadcast (All Except Me)"))
			{
				BroadcastExceptMe(ToEveryBodyText);
			}
			if (GUILayout.Button("Enter Name"))
			{
				EnterName(ToEveryBodyText);
			}
			if (GUILayout.Button("Join Group"))
			{
				JoinGroup(ToEveryBodyText);
			}
			if (GUILayout.Button("Leave Group"))
			{
				LeaveGroup(ToEveryBodyText);
			}
			GUILayout.EndHorizontal();
			GUILayout.Label("To Me");
			GUILayout.BeginHorizontal();
			ToMeText = GUILayout.TextField(ToMeText, GUILayout.MinWidth(100f));
			if (GUILayout.Button("Send to me"))
			{
				SendToMe(ToMeText);
			}
			GUILayout.EndHorizontal();
			GUILayout.Label("Private Message");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Message:");
			PrivateMessageText = GUILayout.TextField(PrivateMessageText, GUILayout.MinWidth(100f));
			GUILayout.Label("User or Group name:");
			PrivateMessageUserOrGroupName = GUILayout.TextField(PrivateMessageUserOrGroupName, GUILayout.MinWidth(100f));
			if (GUILayout.Button("Send to user"))
			{
				SendToUser(PrivateMessageUserOrGroupName, PrivateMessageText);
			}
			if (GUILayout.Button("Send to group"))
			{
				SendToGroup(PrivateMessageUserOrGroupName, PrivateMessageText);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(20f);
			if (signalRConnection.State == ConnectionStates.Closed)
			{
				if (GUILayout.Button("Start Connection"))
				{
					signalRConnection.Open();
				}
			}
			else if (GUILayout.Button("Stop Connection"))
			{
				signalRConnection.Close();
			}
			GUILayout.Space(20f);
			GUILayout.Label("Messages");
			GUILayout.BeginHorizontal();
			GUILayout.Space(20f);
			messages.Draw(Screen.width - 20, 0f);
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		});
#endif
	}

	private void OnDestroy()
	{
#if ENABLE_BEST_HTTP
		signalRConnection.Close();
#endif
	}

#if ENABLE_BEST_HTTP
	private void signalRConnection_OnGeneralMessage(Connection manager, object data)
	{
		string str = Json.Encode(data);
		messages.Add("[Server Message] " + str);
	}
#endif

#if ENABLE_BEST_HTTP
	private void signalRConnection_OnStateChanged(Connection manager, ConnectionStates oldState, ConnectionStates newState)
	{
		messages.Add($"[State Change] {oldState.ToString()} => {newState.ToString()}");
	}
#endif

	private void Broadcast(string text)
	{
#if ENABLE_BEST_HTTP
		signalRConnection.Send(new
		{
			Type = MessageTypes.Broadcast,
			Value = text
		});
#endif
	}

	private void BroadcastExceptMe(string text)
	{
#if ENABLE_BEST_HTTP
		signalRConnection.Send(new
		{
			Type = MessageTypes.BroadcastExceptMe,
			Value = text
		});
#endif
	}

	private void EnterName(string name)
	{
#if ENABLE_BEST_HTTP
		signalRConnection.Send(new
		{
			Type = MessageTypes.Join,
			Value = name
		});
#endif
	}

	private void JoinGroup(string groupName)
	{
#if ENABLE_BEST_HTTP
		signalRConnection.Send(new
		{
			Type = MessageTypes.AddToGroup,
			Value = groupName
		});
#endif
	}

	private void LeaveGroup(string groupName)
	{
#if ENABLE_BEST_HTTP
		signalRConnection.Send(new
		{
			Type = MessageTypes.RemoveFromGroup,
			Value = groupName
		});
#endif
	}

	private void SendToMe(string text)
	{
#if ENABLE_BEST_HTTP
		signalRConnection.Send(new
		{
			Type = MessageTypes.Send,
			Value = text
		});
#endif
	}

	private void SendToUser(string userOrGroupName, string text)
	{
#if ENABLE_BEST_HTTP
		signalRConnection.Send(new
		{
			Type = MessageTypes.PrivateMessage,
			Value = $"{userOrGroupName}|{text}"
		});
#endif
	}

	private void SendToGroup(string userOrGroupName, string text)
	{
#if ENABLE_BEST_HTTP
		signalRConnection.Send(new
		{
			Type = MessageTypes.SendToGroup,
			Value = $"{userOrGroupName}|{text}"
		});
#endif
	}
}
