




using System;
using System.Collections.Generic;
using UnityEngine;

public struct UniWebViewMessage
{
	public string RawMessage
	{
		get;
		private set;
	}

	public string Scheme
	{
		get;
		private set;
	}

	public string Path
	{
		get;
		private set;
	}

	public Dictionary<string, string> Args
	{
		get;
		private set;
	}

	public UniWebViewMessage(string rawMessage)
	{
		this = default(UniWebViewMessage);
		UniWebViewLogger.Instance.Debug("Try to parse raw message: " + rawMessage);
		RawMessage = WWW.UnEscapeURL(rawMessage);
		string[] array = rawMessage.Split(new string[1]
		{
			"://"
		}, StringSplitOptions.None);
		if (array.Length >= 2)
		{
			Scheme = array[0];
			UniWebViewLogger.Instance.Debug("Get scheme: " + Scheme);
			string text = string.Empty;
			for (int i = 1; i < array.Length; i++)
			{
				text += array[i];
			}
			UniWebViewLogger.Instance.Verbose("Build path and args string: " + text);
			string[] array2 = text.Split("?"[0]);
			Path = WWW.UnEscapeURL(array2[0].TrimEnd('/'));
			Args = new Dictionary<string, string>();
			if (array2.Length <= 1)
			{
				return;
			}
			string[] array3 = array2[1].Split("&"[0]);
			foreach (string text2 in array3)
			{
				string[] array4 = text2.Split("="[0]);
				if (array4.Length > 1)
				{
					string text3 = WWW.UnEscapeURL(array4[0]);
					if (Args.ContainsKey(text3))
					{
						string str = Args[text3];
						Args[text3] = str + "," + WWW.UnEscapeURL(array4[1]);
					}
					else
					{
						Args[text3] = WWW.UnEscapeURL(array4[1]);
					}
					UniWebViewLogger.Instance.Debug("Get arg, key: " + text3 + " value: " + Args[text3]);
				}
			}
		}
		else
		{
			UniWebViewLogger.Instance.Critical("Bad url scheme. Can not be parsed to UniWebViewMessage: " + rawMessage);
		}
	}
}
