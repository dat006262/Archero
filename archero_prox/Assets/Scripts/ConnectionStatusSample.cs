




#if ENABLE_BEST_HTTP
using BestHTTP.Examples;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
#endif
using System;
using UnityEngine;

internal sealed class ConnectionStatusSample : MonoBehaviour
{
	private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/signalr");

#if ENABLE_BEST_HTTP
	private Connection signalRConnection;

	private GUIMessageList messages = new GUIMessageList();
#endif

	private void Start()
	{
#if ENABLE_BEST_HTTP
		signalRConnection = new Connection(URI, "StatusHub");
		signalRConnection.OnNonHubMessage += signalRConnection_OnNonHubMessage;
		signalRConnection.OnError += signalRConnection_OnError;
		signalRConnection.OnStateChanged += signalRConnection_OnStateChanged;
		signalRConnection["StatusHub"].OnMethodCall += statusHub_OnMethodCall;
		signalRConnection.Open();
#endif
	}

	private void OnDestroy()
	{
#if ENABLE_BEST_HTTP
		signalRConnection.Close();
#endif
	}

	private void OnGUI()
	{
#if ENABLE_BEST_HTTP
		GUIHelper.DrawArea(GUIHelper.ClientArea, drawHeader: true, delegate
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("START") && signalRConnection.State != ConnectionStates.Connected)
			{
				signalRConnection.Open();
			}
			if (GUILayout.Button("STOP") && signalRConnection.State == ConnectionStates.Connected)
			{
				signalRConnection.Close();
				messages.Clear();
			}
			if (GUILayout.Button("PING") && signalRConnection.State == ConnectionStates.Connected)
			{
				signalRConnection["StatusHub"].Call("Ping");
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(20f);
			GUILayout.Label("Connection Status Messages");
			GUILayout.BeginHorizontal();
			GUILayout.Space(20f);
			messages.Draw(Screen.width - 20, 0f);
			GUILayout.EndHorizontal();
		});
#endif
	}

#if ENABLE_BEST_HTTP
	private void signalRConnection_OnNonHubMessage(Connection manager, object data)
	{
		messages.Add("[Server Message] " + data.ToString());
	}

	private void signalRConnection_OnStateChanged(Connection manager, ConnectionStates oldState, ConnectionStates newState)
	{
		messages.Add($"[State Change] {oldState} => {newState}");
	}

	private void signalRConnection_OnError(Connection manager, string error)
	{
		messages.Add("[Error] " + error);
	}

	private void statusHub_OnMethodCall(Hub hub, string method, params object[] args)
	{
		string arg = (args.Length <= 0) ? string.Empty : (args[0] as string);
		string arg2 = (args.Length <= 1) ? string.Empty : args[1].ToString();
		switch (method)
		{
		case "joined":
			messages.Add($"[{hub.Name}] {arg} joined at {arg2}");
			break;
		case "rejoined":
			messages.Add($"[{hub.Name}] {arg} reconnected at {arg2}");
			break;
		case "leave":
			messages.Add($"[{hub.Name}] {arg} leaved at {arg2}");
			break;
		default:
			messages.Add($"[{hub.Name}] {method}");
			break;
		}
	}
#endif
}
