




using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPGetClient : MonoBehaviour
{
	public class HTTPGetData
	{
		public string header;

		public string body;
	}

	private static GameObject _HTTPGetObject;

	private UnityWebRequest _uwr;

	private float starttime;

	private static GameObject HTTPGetObject
	{
		get
		{
			if (_HTTPGetObject == null)
			{
				_HTTPGetObject = new GameObject("HTTPGetClient");
			}
			return _HTTPGetObject;
		}
	}

	private bool isTimeOut => Time.realtimeSinceStartup - starttime >= (float)_uwr.timeout;

	public static void SendGet(string uri, Action<string> callback)
	{
		HTTPGetClient hTTPGetClient = HTTPGetObject.AddComponent<HTTPGetClient>();
		hTTPGetClient.StartSendGet(uri, callback);
	}

	public static void SendPost(string uri, WWWForm form, Action<string> callback)
	{
		HTTPGetClient hTTPGetClient = HTTPGetObject.AddComponent<HTTPGetClient>();
		hTTPGetClient.StartSendPost(uri, form, callback);
	}

	public void StartSendGet(string uri, Action<string> callback)
	{
		StartCoroutine(SendGetInternal(uri, callback));
	}

	public IEnumerator SendGetInternal(string uri, Action<string> callback)
	{
		_uwr = UnityWebRequest.Get(uri);
		try
		{
			_uwr.timeout = 10;
			_uwr.method = "GET";
			starttime = Time.realtimeSinceStartup;
			_uwr.SendWebRequest();
			while (!_uwr.isDone && !isTimeOut)
			{
				yield return null;
			}
			if (isTimeOut)
			{
				StartCoroutine(SendGetInternal(uri, callback));
			}
			else
			{
				Debugger.Log("length = " + _uwr.downloadHandler.text.Length + " text = " + _uwr.downloadHandler.text);
				string result = _uwr.downloadHandler.text;
				callback(result);
			}
		}
		finally
		{
            //@TODO CANNOT DECODE base._003C_003E__Finally0();
            //base._003C_003E__Finally0();
		}
	}

	public void StartSendPost(string uri, WWWForm form, Action<string> callback)
	{
		StartCoroutine(SendPostInternal(uri, form, callback));
	}

	public IEnumerator SendPostInternal(string uri, WWWForm form, Action<string> callback)
	{
		_uwr = UnityWebRequest.Post(uri, form);
		try
		{
			_uwr.timeout = 10;
			_uwr.method = "POST";
			starttime = Time.realtimeSinceStartup;
			_uwr.SendWebRequest();
			while (!_uwr.isDone && !isTimeOut)
			{
				yield return null;
			}
			if (isTimeOut)
			{
				StartCoroutine(SendPostInternal(uri, form, callback));
			}
			else
			{
				Debugger.Log("length = " + _uwr.downloadHandler.text.Length + " text = " + _uwr.downloadHandler.text);
				string result = _uwr.downloadHandler.text;
				callback(result);
			}
		}
		finally
		{
            //@TODO CANNOT DECODE base._003C_003E__Finally0();
            //base._003C_003E__Finally0();
        }
    }
}
