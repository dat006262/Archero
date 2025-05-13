




using System;
using UnityEngine;

public class Unity2AndroidHelper : CInstance<Unity2AndroidHelper>
{
	private AndroidJavaClass jc;

	private AndroidJavaObject jo;

	public Unity2AndroidHelper()
	{
#if ENABLE_ANDROID_NATIVE
		jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif
	}

	public bool is_gp_avalible()
	{
#if ENABLE_ANDROID_NATIVE
		return jo.Call<bool>("is_gp_avalible", Array.Empty<object>());
#endif
        return false;
	}
}
