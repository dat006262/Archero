




using System.Reflection;
using UnityEngine;

public class UniWebViewAndroidStaticListener : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnJavaMessage(string message)
	{
		string[] array = message.Split("@"[0]);
		if (array.Length < 3)
		{
			UnityEngine.Debug.Log("Not enough parts for receiving a message.");
			return;
		}
		UniWebViewNativeListener listener = UniWebViewNativeListener.GetListener(array[0]);
		if (listener == null)
		{
			UnityEngine.Debug.Log("Unable to find listener");
			return;
		}
		MethodInfo method = typeof(UniWebViewNativeListener).GetMethod(array[1]);
		if (method == null)
		{
			UnityEngine.Debug.Log("Cannot find correct method to invoke: " + array[1]);
		}
		int num = array.Length - 2;
		string[] array2 = new string[num];
		for (int i = 0; i < num; i++)
		{
			array2[i] = array[i + 2];
		}
		method.Invoke(listener, new object[1]
		{
			string.Join("@", array2)
		});
	}
}
