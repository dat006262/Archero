




using System.Collections.Generic;
using UnityEngine;

public class UniWebViewNativeListener : MonoBehaviour
{
	private static Dictionary<string, UniWebViewNativeListener> listeners = new Dictionary<string, UniWebViewNativeListener>();

	[HideInInspector]
	public UniWebView webView;

	public string Name => base.gameObject.name;

	public static void AddListener(UniWebViewNativeListener target)
	{
		listeners.Add(target.Name, target);
	}

	public static void RemoveListener(string name)
	{
		listeners.Remove(name);
	}

	public static UniWebViewNativeListener GetListener(string name)
	{
		UniWebViewNativeListener value = null;
		if (listeners.TryGetValue(name, out value))
		{
			return value;
		}
		return null;
	}

	public void PageStarted(string url)
	{
		UniWebViewLogger.Instance.Info("Page Started Event. Url: " + url);
		webView.InternalOnPageStarted(url);
	}

	public void PageFinished(string result)
	{
		UniWebViewLogger.Instance.Info("Page Finished Event. Url: " + result);
		UniWebViewNativeResultPayload payload = JsonUtility.FromJson<UniWebViewNativeResultPayload>(result);
		webView.InternalOnPageFinished(payload);
	}

	public void PageErrorReceived(string result)
	{
		UniWebViewLogger.Instance.Info("Page Error Received Event. Result: " + result);
		UniWebViewNativeResultPayload payload = JsonUtility.FromJson<UniWebViewNativeResultPayload>(result);
		webView.InternalOnPageErrorReceived(payload);
	}

	public void ShowTransitionFinished(string identifer)
	{
		UniWebViewLogger.Instance.Info("Show Transition Finished Event. Identifier: " + identifer);
		webView.InternalOnShowTransitionFinished(identifer);
	}

	public void HideTransitionFinished(string identifer)
	{
		UniWebViewLogger.Instance.Info("Hide Transition Finished Event. Identifier: " + identifer);
		webView.InternalOnHideTransitionFinished(identifer);
	}

	public void AnimateToFinished(string identifer)
	{
		UniWebViewLogger.Instance.Info("Animate To Finished Event. Identifier: " + identifer);
		webView.InternalOnAnimateToFinished(identifer);
	}

	public void AddJavaScriptFinished(string result)
	{
		UniWebViewLogger.Instance.Info("Add JavaScript Finished Event. Result: " + result);
		UniWebViewNativeResultPayload payload = JsonUtility.FromJson<UniWebViewNativeResultPayload>(result);
		webView.InternalOnAddJavaScriptFinished(payload);
	}

	public void EvalJavaScriptFinished(string result)
	{
		UniWebViewLogger.Instance.Info("Eval JavaScript Finished Event. Result: " + result);
		UniWebViewNativeResultPayload payload = JsonUtility.FromJson<UniWebViewNativeResultPayload>(result);
		webView.InternalOnEvalJavaScriptFinished(payload);
	}

	public void MessageReceived(string result)
	{
		UniWebViewLogger.Instance.Info("Message Received Event. Result: " + result);
		webView.InternalOnMessageReceived(result);
	}

	public void WebViewKeyDown(string keyCode)
	{
		UniWebViewLogger.Instance.Info("Web View Key Down: " + keyCode);
		if (int.TryParse(keyCode, out int result))
		{
			webView.InternalOnWebViewKeyDown(result);
		}
		else
		{
			UniWebViewLogger.Instance.Critical("Failed in converting key code: " + keyCode);
		}
	}

	public void WebViewDone(string param)
	{
		UniWebViewLogger.Instance.Info("Web View Done Event.");
		webView.InternalOnShouldClose();
	}

	public void WebContentProcessDidTerminate(string param)
	{
		UniWebViewLogger.Instance.Info("Web Content Process Terminate Event.");
		webView.InternalWebContentProcessDidTerminate();
	}
}
