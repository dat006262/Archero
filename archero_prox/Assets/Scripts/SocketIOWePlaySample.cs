




#if ENABLE_BEST_HTTP
using BestHTTP.Examples;
using BestHTTP.SocketIO;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class SocketIOWePlaySample : MonoBehaviour
{
	private enum States
	{
		Connecting,
		WaitForNick,
		Joined
	}

	private string[] controls = new string[8]
	{
		"left",
		"right",
		"a",
		"b",
		"up",
		"down",
		"select",
		"start"
	};

	private const float ratio = 1.5f;

	private int MaxMessages = 50;

	private States State;

#if ENABLE_BEST_HTTP
	private Socket Socket;
#endif

	private string Nick = string.Empty;

	private string messageToSend = string.Empty;

	private int connections;

	private List<string> messages = new List<string>();

	private Vector2 scrollPos;

	private Texture2D FrameTexture;

	private void Start()
	{
#if ENABLE_BEST_HTTP
		SocketOptions socketOptions = new SocketOptions();
		socketOptions.AutoConnect = false;
		SocketManager socketManager = new SocketManager(new Uri("http://io.weplay.io/socket.io/"), socketOptions);
		Socket = socketManager.Socket;
		Socket.On(SocketIOEventTypes.Connect, OnConnected);
		Socket.On("joined", OnJoined);
		Socket.On("connections", OnConnections);
		Socket.On("join", OnJoin);
		Socket.On("move", OnMove);
		Socket.On("message", OnMessage);
		Socket.On("reload", OnReload);
		Socket.On("frame", OnFrame, autoDecodePayload: false);
		Socket.On(SocketIOEventTypes.Error, OnError);
		socketManager.Open();
		State = States.Connecting;
#endif
	}

	private void OnDestroy()
	{
#if ENABLE_BEST_HTTP
		Socket.Manager.Close();
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
	}

    private void OnGUI()
    {
#if ENABLE_BEST_HTTP
        switch (State)
        {
            case States.Connecting:
                GUIHelper.DrawArea(GUIHelper.ClientArea, drawHeader: true, delegate
                {
                    GUILayout.BeginVertical();
                    GUILayout.FlexibleSpace();
                    GUIHelper.DrawCenteredText("Connecting to the server...");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();
                });
                break;
            case States.WaitForNick:
                GUIHelper.DrawArea(GUIHelper.ClientArea, drawHeader: true, delegate
                {
                    DrawLoginScreen();
                });
                break;
            case States.Joined:
                GUIHelper.DrawArea(GUIHelper.ClientArea, drawHeader: true, delegate
                {
                    if (FrameTexture != null)
                    {
                        GUILayout.Box(FrameTexture);
                    }
                    DrawControls();
                    DrawChat();
                });
                break;
        }
#endif
    }

	private void DrawLoginScreen()
	{
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
#if ENABLE_BEST_HTTP
		GUIHelper.DrawCenteredText("What's your nickname?");
#endif
		Nick = GUILayout.TextField(Nick);
		if (GUILayout.Button("Join"))
		{
			Join();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
	}

	private void DrawControls()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Controls:");
		for (int i = 0; i < controls.Length; i++)
		{
			if (GUILayout.Button(controls[i]))
			{
#if ENABLE_BEST_HTTP
				Socket.Emit("move", controls[i]);
#endif
			}
		}
		GUILayout.Label(" Connections: " + connections);
		GUILayout.EndHorizontal();
	}

	private void DrawChat(bool withInput = true)
	{
		GUILayout.BeginVertical();
		scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);
		for (int i = 0; i < messages.Count; i++)
		{
			GUILayout.Label(messages[i], GUILayout.MinWidth(Screen.width));
		}
		GUILayout.EndScrollView();
		if (withInput)
		{
			GUILayout.Label("Your message: ");
			GUILayout.BeginHorizontal();
			messageToSend = GUILayout.TextField(messageToSend);
			if (GUILayout.Button("Send", GUILayout.MaxWidth(100f)))
			{
				SendMessage();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	private void AddMessage(string msg)
	{
		messages.Insert(0, msg);
		if (messages.Count > MaxMessages)
		{
			messages.RemoveRange(MaxMessages, messages.Count - MaxMessages);
		}
	}

	private void SendMessage()
	{
		if (!string.IsNullOrEmpty(messageToSend))
		{
#if ENABLE_BEST_HTTP
			Socket.Emit("message", messageToSend);
#endif
			AddMessage($"{Nick}: {messageToSend}");
			messageToSend = string.Empty;
		}
	}

	private void Join()
	{
		PlayerPrefs.SetString("Nick", Nick);
#if ENABLE_BEST_HTTP
		Socket.Emit("join", Nick);
#endif
	}

	private void Reload()
	{
#if ENABLE_BEST_HTTP
		FrameTexture = null;
		if (Socket != null)
		{
			Socket.Manager.Close();
			Socket = null;
			Start();
		}
#endif
    }

#if ENABLE_BEST_HTTP
    private void OnConnected(Socket socket, Packet packet, params object[] args)
	{
		if (PlayerPrefs.HasKey("Nick"))
		{
			Nick = PlayerPrefs.GetString("Nick", "NickName");
			Join();
		}
		else
		{
			State = States.WaitForNick;
		}
		AddMessage("connected");
	}

	private void OnJoined(Socket socket, Packet packet, params object[] args)
	{
		State = States.Joined;
	}

	private void OnReload(Socket socket, Packet packet, params object[] args)
	{
		Reload();
	}

	private void OnMessage(Socket socket, Packet packet, params object[] args)
	{
		if (args.Length == 1)
		{
			AddMessage(args[0] as string);
		}
		else
		{
			AddMessage($"{args[1]}: {args[0]}");
		}
	}

	private void OnMove(Socket socket, Packet packet, params object[] args)
	{
		AddMessage($"{args[1]} pressed {args[0]}");
	}

	private void OnJoin(Socket socket, Packet packet, params object[] args)
	{
		string arg = (args.Length <= 1) ? string.Empty : $"({args[1]})";
		AddMessage($"{args[0]} joined {arg}");
	}

	private void OnConnections(Socket socket, Packet packet, params object[] args)
	{
		connections = Convert.ToInt32(args[0]);
	}

	private void OnFrame(Socket socket, Packet packet, params object[] args)
	{
		if (State == States.Joined)
		{
			if (FrameTexture == null)
			{
				FrameTexture = new Texture2D(0, 0, TextureFormat.RGBA32, mipChain: false);
				FrameTexture.filterMode = FilterMode.Point;
			}
			byte[] data = packet.Attachments[0];
			FrameTexture.LoadImage(data);
		}
	}

	private void OnError(Socket socket, Packet packet, params object[] args)
	{
		AddMessage($"--ERROR - {args[0].ToString()}");
	}
#endif
}
