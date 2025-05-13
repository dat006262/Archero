




#if ENABLE_BEST_HTTP
using BestHTTP.Examples;
using BestHTTP.SocketIO;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class SocketIOChatSample : MonoBehaviour
{
	private enum ChatStates
	{
		Login,
		Chat
	}

	private readonly TimeSpan TYPING_TIMER_LENGTH = TimeSpan.FromMilliseconds(700.0);

#if ENABLE_BEST_HTTP
	private SocketManager Manager;
#endif

	private ChatStates State;

	private string userName = string.Empty;

	private string message = string.Empty;

	private string chatLog = string.Empty;

	private Vector2 scrollPos;

	private bool typing;

	private DateTime lastTypingTime = DateTime.MinValue;

	private List<string> typingUsers = new List<string>();

	private void Start()
	{
#if ENABLE_BEST_HTTP
		State = ChatStates.Login;
		SocketOptions socketOptions = new SocketOptions();
		socketOptions.AutoConnect = false;
		Manager = new SocketManager(new Uri("http://localhost:3000/socket.io/"), socketOptions);
		Manager.Socket.On("login", OnLogin);
		Manager.Socket.On("new message", OnNewMessage);
		Manager.Socket.On("user joined", OnUserJoined);
		Manager.Socket.On("user left", OnUserLeft);
		Manager.Socket.On("typing", OnTyping);
		Manager.Socket.On("stop typing", OnStopTyping);
		Manager.Socket.On(SocketIOEventTypes.Error, delegate(Socket socket, Packet packet, object[] args)
		{
			UnityEngine.Debug.LogError($"Error: {args[0].ToString()}");
		});
		Manager.GetSocket("/nsp").On(SocketIOEventTypes.Connect, delegate(Socket socket, Packet packet, object[] arg)
		{
			UnityEngine.Debug.LogWarning("Connected to /nsp");
			socket.Emit("testmsg", "Message from /nsp 'on connect'");
		});
		Manager.GetSocket("/nsp").On("nsp_message", delegate(Socket socket, Packet packet, object[] arg)
		{
			UnityEngine.Debug.LogWarning("nsp_message: " + arg[0]);
		});
		Manager.Open();
#endif
    }

    private void OnDestroy()
	{
#if ENABLE_BEST_HTTP
		Manager.Close();
#endif
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
#if ENABLE_BEST_HTTP
			SampleSelector.SelectedSample.DestroyUnityObject();
#endif
		}
		if (typing)
		{
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan t = utcNow - lastTypingTime;
			if (t >= TYPING_TIMER_LENGTH)
			{
#if ENABLE_BEST_HTTP
				Manager.Socket.Emit("stop typing");
#endif
				typing = false;
			}
		}
	}

	private void OnGUI()
	{
		switch (State)
		{
		case ChatStates.Login:
			DrawLoginScreen();
			break;
		case ChatStates.Chat:
			DrawChatScreen();
			break;
		}
	}

	private void DrawLoginScreen()
	{
#if ENABLE_BEST_HTTP
		GUIHelper.DrawArea(GUIHelper.ClientArea, drawHeader: true, delegate
		{
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			GUIHelper.DrawCenteredText("What's your nickname?");
			userName = GUILayout.TextField(userName);
			if (GUILayout.Button("Join"))
			{
				SetUserName();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		});
#endif
	}

	private void DrawChatScreen()
	{
#if ENABLE_BEST_HTTP
		GUIHelper.DrawArea(GUIHelper.ClientArea, drawHeader: true, delegate
		{
			GUILayout.BeginVertical();
			scrollPos = GUILayout.BeginScrollView(scrollPos);
			GUILayout.Label(chatLog, GUILayout.ExpandWidth(expand: true), GUILayout.ExpandHeight(expand: true));
			GUILayout.EndScrollView();
			string text = string.Empty;
			if (typingUsers.Count > 0)
			{
				text += $"{typingUsers[0]}";
				for (int i = 1; i < typingUsers.Count; i++)
				{
					text += $", {typingUsers[i]}";
				}
				text = ((typingUsers.Count != 1) ? (text + " are typing!") : (text + " is typing!"));
			}
			GUILayout.Label(text);
			GUILayout.Label("Type here:");
			GUILayout.BeginHorizontal();
			message = GUILayout.TextField(message);
			if (GUILayout.Button("Send", GUILayout.MaxWidth(100f)))
			{
				SendMessage();
			}
			GUILayout.EndHorizontal();
			if (GUI.changed)
			{
				UpdateTyping();
			}
			GUILayout.EndVertical();
		});
#endif
	}

	private void SetUserName()
	{
		if (!string.IsNullOrEmpty(userName))
		{
			State = ChatStates.Chat;
#if ENABLE_BEST_HTTP
			Manager.Socket.Emit("add user", userName);
#endif
		}
	}

	private void SendMessage()
	{
		if (!string.IsNullOrEmpty(message))
		{
#if ENABLE_BEST_HTTP
			Manager.Socket.Emit("new message", message);
#endif
			chatLog += $"{userName}: {message}\n";
			message = string.Empty;
		}
	}

	private void UpdateTyping()
	{
		if (!typing)
		{
			typing = true;
#if ENABLE_BEST_HTTP
			Manager.Socket.Emit("typing");
#endif
		}
		lastTypingTime = DateTime.UtcNow;
	}

	private void addParticipantsMessage(Dictionary<string, object> data)
	{
		int num = Convert.ToInt32(data["numUsers"]);
		if (num == 1)
		{
			chatLog += "there's 1 participant\n";
			return;
		}
		string text = chatLog;
		chatLog = text + "there are " + num + " participants\n";
	}

	private void addChatMessage(Dictionary<string, object> data)
	{
		string arg = data["username"] as string;
		string arg2 = data["message"] as string;
		chatLog += $"{arg}: {arg2}\n";
	}

	private void AddChatTyping(Dictionary<string, object> data)
	{
		string item = data["username"] as string;
		typingUsers.Add(item);
	}

	private void RemoveChatTyping(Dictionary<string, object> data)
	{
		string username = data["username"] as string;
		int num = typingUsers.FindIndex((string name) => name.Equals(username));
		if (num != -1)
		{
			typingUsers.RemoveAt(num);
		}
	}

#if ENABLE_BEST_HTTP
	private void OnLogin(Socket socket, Packet packet, params object[] args)
	{
		chatLog = "Welcome to Socket.IO Chat — \n";
		addParticipantsMessage(args[0] as Dictionary<string, object>);
	}

	private void OnNewMessage(Socket socket, Packet packet, params object[] args)
	{
		addChatMessage(args[0] as Dictionary<string, object>);
	}

	private void OnUserJoined(Socket socket, Packet packet, params object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string arg = dictionary["username"] as string;
		chatLog += $"{arg} joined\n";
		addParticipantsMessage(dictionary);
	}

	private void OnUserLeft(Socket socket, Packet packet, params object[] args)
	{
		Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
		string arg = dictionary["username"] as string;
		chatLog += $"{arg} left\n";
		addParticipantsMessage(dictionary);
	}

	private void OnTyping(Socket socket, Packet packet, params object[] args)
	{
		AddChatTyping(args[0] as Dictionary<string, object>);
	}

	private void OnStopTyping(Socket socket, Packet packet, params object[] args)
	{
		RemoveChatTyping(args[0] as Dictionary<string, object>);
	}
#endif
}
