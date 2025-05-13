




#if ENABLE_BEST_HTTP
using BestHTTP.Examples;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
#endif
using System.Collections.Generic;
using UnityEngine;

internal class BaseHub
#if ENABLE_BEST_HTTP
    : Hub
#endif
{
	private string Title;

#if ENABLE_BEST_HTTP
	private GUIMessageList messages = new GUIMessageList();
#endif

	public BaseHub(string name, string title)
#if ENABLE_BEST_HTTP
		: base(name)
#endif
	{
		Title = title;
#if ENABLE_BEST_HTTP
		On("joined", Joined);
		On("rejoined", Rejoined);
		On("left", Left);
		On("invoked", Invoked);
#endif
	}

#if ENABLE_BEST_HTTP
	private void Joined(Hub hub, MethodCallMessage methodCall)
	{
		Dictionary<string, object> dictionary = methodCall.Arguments[2] as Dictionary<string, object>;
		messages.Add(string.Format("{0} joined at {1}\n\tIsAuthenticated: {2} IsAdmin: {3} UserName: {4}", methodCall.Arguments[0], methodCall.Arguments[1], dictionary["IsAuthenticated"], dictionary["IsAdmin"], dictionary["UserName"]));
	}

	private void Rejoined(Hub hub, MethodCallMessage methodCall)
	{
		messages.Add($"{methodCall.Arguments[0]} reconnected at {methodCall.Arguments[1]}");
	}

	private void Left(Hub hub, MethodCallMessage methodCall)
	{
		messages.Add($"{methodCall.Arguments[0]} left at {methodCall.Arguments[1]}");
	}

	private void Invoked(Hub hub, MethodCallMessage methodCall)
	{
		messages.Add($"{methodCall.Arguments[0]} invoked hub method at {methodCall.Arguments[1]}");
	}
#endif

	public void InvokedFromClient()
	{
#if ENABLE_BEST_HTTP
		Call("invokedFromClient", OnInvoked, OnInvokeFailed);
#endif
	}

#if ENABLE_BEST_HTTP
	private void OnInvoked(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		UnityEngine.Debug.Log(hub.Name + " invokedFromClient success!");
	}

	private void OnInvokeFailed(Hub hub, ClientMessage originalMessage, FailureMessage result)
	{
		UnityEngine.Debug.LogWarning(hub.Name + " " + result.ErrorMessage);
	}
#endif

	public void Draw()
	{
		GUILayout.Label(Title);
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
#if ENABLE_BEST_HTTP
		messages.Draw(Screen.width - 20, 100f);
#endif
		GUILayout.EndHorizontal();
	}
}
