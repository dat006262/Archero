




using UnityEngine;

public class UniWebViewLogger
{
	public enum Level
	{
		Verbose = 0,
		Debug = 10,
		Info = 20,
		Critical = 80,
		Off = 99
	}

	private static UniWebViewLogger instance;

	private Level level;

	public Level LogLevel
	{
		get
		{
			return level;
		}
		set
		{
			Log(Level.Off, "Setting UniWebView logger level to: " + value);
			level = value;
			UniWebViewInterface.SetLogLevel((int)value);
		}
	}

	public static UniWebViewLogger Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new UniWebViewLogger(Level.Critical);
			}
			return instance;
		}
	}

	private UniWebViewLogger(Level level)
	{
		this.level = level;
	}

	public void Verbose(string message)
	{
		Log(Level.Verbose, message);
	}

	public void Debug(string message)
	{
		Log(Level.Debug, message);
	}

	public void Info(string message)
	{
		Log(Level.Info, message);
	}

	public void Critical(string message)
	{
		Log(Level.Critical, message);
	}

	private void Log(Level level, string message)
	{
		if (level >= LogLevel)
		{
			string message2 = "<UniWebView> " + message;
			if (level == Level.Critical)
			{
				UnityEngine.Debug.LogError(message2);
			}
			else
			{
				UnityEngine.Debug.Log(message2);
			}
		}
	}
}
