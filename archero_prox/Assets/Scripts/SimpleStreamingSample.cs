




#if ENABLE_BEST_HTTP
using BestHTTP.Examples;
using BestHTTP.SignalR;
#endif
using System;
using UnityEngine;

internal sealed class SimpleStreamingSample : MonoBehaviour
{
	private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/streaming-connection");

#if ENABLE_BEST_HTTP
	private Connection signalRConnection;

	private GUIMessageList messages = new GUIMessageList();
#endif

	private void Start()
	{
#if ENABLE_BEST_HTTP
		signalRConnection = new Connection(URI);
		signalRConnection.OnNonHubMessage += signalRConnection_OnNonHubMessage;
		signalRConnection.OnStateChanged += signalRConnection_OnStateChanged;
		signalRConnection.OnError += signalRConnection_OnError;
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
			GUILayout.Label("Messages");
			GUILayout.BeginHorizontal();
			GUILayout.Space(20f);
			messages.Draw(Screen.width - 20, 0f);
			GUILayout.EndHorizontal();
		});
#endif
	}

#if ENABLE_BEST_HTTP
	private void signalRConnection_OnNonHubMessage(Connection connection, object data)
	{
		messages.Add("[Server Message] " + data.ToString());
	}

	private void signalRConnection_OnStateChanged(Connection connection, ConnectionStates oldState, ConnectionStates newState)
	{
		messages.Add($"[State Change] {oldState} => {newState}");
	}

	private void signalRConnection_OnError(Connection connection, string error)
	{
		messages.Add("[Error] " + error);
	}
#endif
}
