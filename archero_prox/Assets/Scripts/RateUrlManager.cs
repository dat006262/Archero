




using UnityEngine;

public class RateUrlManager
{
	private static string getAppUrl()
	{
		//return "https://play.google.com/store/apps/details?id=com.game2019.archero";
        return string.Empty;
    }

    private static string getAdUrl()
	{
        //return "https://play.google.com/store/apps/details?id=com.mavis.slidey";
        return string.Empty;
	}

	public static void OpenAppUrl()
	{
		Application.OpenURL(getAppUrl());
	}

	public static void OpenAdUrl()
	{
		Application.OpenURL(getAdUrl());
	}
}
