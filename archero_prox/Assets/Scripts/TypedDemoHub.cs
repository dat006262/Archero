




#if ENABLE_BEST_HTTP
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
#endif
using UnityEngine;

internal class TypedDemoHub
#if ENABLE_BEST_HTTP
    : Hub
#endif
{
	private string typedEchoResult = string.Empty;

	private string typedEchoClientResult = string.Empty;

	public TypedDemoHub()
#if ENABLE_BEST_HTTP
		: base("typeddemohub")
#endif
	{
#if ENABLE_BEST_HTTP
		On("Echo", Echo);
#endif
	}

#if ENABLE_BEST_HTTP
	private void Echo(Hub hub, MethodCallMessage methodCall)
	{
		typedEchoClientResult = $"{methodCall.Arguments[0]} #{methodCall.Arguments[1]} triggered!";
	}
#endif

	public void Echo(string msg)
	{
#if ENABLE_BEST_HTTP
		Call("echo", OnEcho_Done, msg);
#endif
	}

#if ENABLE_BEST_HTTP
	private void OnEcho_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		typedEchoResult = "TypedDemoHub.Echo(string message) invoked!";
	}
#endif

	public void Draw()
	{
		GUILayout.Label("Typed callback");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.BeginVertical();
		GUILayout.Label(typedEchoResult);
		GUILayout.Label(typedEchoClientResult);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
	}
}
