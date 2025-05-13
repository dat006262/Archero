




using Dxx.Util;
using UnityEngine;

public class CurrencyExpCtrl : MonoBehaviour
{
	private ProgressTextCtrl mProgressTextCtrl;

	private void Awake()
	{
		Transform transform = base.transform.Find("fg/ProgressTextBar");
		if ((bool)transform)
		{
			mProgressTextCtrl = transform.GetComponent<ProgressTextCtrl>();
		}
	}

	public void SetProgress(int current, int max)
	{
		if (max <= 0)
		{
			SdkManager.Bugly_Report("CurrencyExpCtrl.cs", Utils.FormatString("SetProgress({0}, {1}) is invalid!", current, max));
		}
		else if ((bool)mProgressTextCtrl)
		{
			mProgressTextCtrl.current = current;
			mProgressTextCtrl.max = max;
		}
	}
}
