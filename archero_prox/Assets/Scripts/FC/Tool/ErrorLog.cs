




using System.Collections;
using UnityEngine;

public class ErrorLog : MonoBehaviour
{
	public AnimationCurve curve;

	private string message;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(5.5f);
		Application.logMessageReceived += MyLogCallback;
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= MyLogCallback;
	}

	private void MyLogCallback(string condition, string stackTrace, LogType type)
	{
		switch (type)
		{
		case LogType.Assert:
			message = "      receive an assert log,condition=" + condition + ",stackTrace=" + stackTrace;
			break;
		case LogType.Error:
			message = "      receive an Error log,condition=" + condition + ",stackTrace=" + stackTrace;
			break;
		case LogType.Exception:
			message = "      receive an Exception log,condition=" + condition + ",stackTrace=" + stackTrace;
			break;
		case LogType.Warning:
			if (condition.Contains("DOTWEEN"))
			{
				SdkManager.Bugly_Report("DOTWEEN", condition, stackTrace);
			}
			break;
		default:
			message = string.Empty;
			break;
		}
	}
}
