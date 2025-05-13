




using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingDebugMediator : WindowMediator, IMediator, INotifier
{
	private const string DoubleClickString = "DoubleClick";

	private const string AbsorbDelayString = "AbsorbDelay";

	private const string JoyScaleBGString = "JoyScaleBG";

	private const string JoyScaleTouchString = "JoyScaleTouch";

	private const string JoyRadiusString = "JoyRadius";

	public static Action OnValueChange;

	private static Transform ContentParent;

	public static float DoubleClick => float.Parse(GetValue("DoubleClick", "0.3"));

	public static int AbsorbDelay => int.Parse(GetValue("AbsorbDelay", "0"));

	public static int JoyScaleBG => int.Parse(GetValue("JoyScaleBG", "100"));

	public static int JoyScaleTouch => int.Parse(GetValue("JoyScaleTouch", "100"));

	public static int JoyRadius => int.Parse(GetValue("JoyRadius", "100"));

	public override List<string> OnListNotificationInterests => new List<string>();

	public SettingDebugMediator()
		: base("SettingDebugUIPanel")
	{
	}

	private static void SetValue(string name, string value)
	{
		PlayerPrefs.SetString(name, value);
	}

	private static string GetValue(string name, string defaultValue = "")
	{
		return PlayerPrefs.GetString(name, defaultValue);
	}

	protected override void OnRegisterOnce()
	{
		_MonoView.transform.Find("Button_Close").GetComponent<Button>().onClick.AddListener(delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_Setting);
		});
		ContentParent = _MonoView.transform.Find("Scroll View/Viewport/Content");
	}

	protected override void OnRegisterEvery()
	{
		SetSliderFloat("DoubleClick", 0.05f, 0.5f, 0.3f);
		SetSliderInt("AbsorbDelay", 0, 1000, 300);
		SetSliderInt("JoyScaleBG", 70, 130, 100);
		SetSliderInt("JoyScaleTouch", 40, 130, 100);
		SetSliderInt("JoyRadius", 50, 150, 100);
	}

	private void SetSliderInt(string name, int min, int max, int defaultValue)
	{
		Slider slider = ContentParent.Find(name + "/Slider").GetComponent<Slider>();
		Text text = ContentParent.Find(name + "/Slider/Handle Slide Area/Handle/Text").GetComponent<Text>();
		text.text = GetValue(name, defaultValue.ToString());
		int.TryParse(GetValue(name, defaultValue.ToString()), out int result);
		slider.value = (float)(result - min) / (float)(max - min);
		slider.onValueChanged.AddListener(delegate
		{
			SetValue(name, ((int)(slider.value * (float)(max - min) + (float)min)).ToString());
			text.text = GetValue(name, defaultValue.ToString());
			if (OnValueChange != null)
			{
				OnValueChange();
			}
		});
	}

	private void SetSliderFloat(string name, float min, float max, float defaultValue)
	{
		Slider slider = ContentParent.Find(name + "/Slider").GetComponent<Slider>();
		Text text = ContentParent.Find(name + "/Slider/Handle Slide Area/Handle/Text").GetComponent<Text>();
		text.text = GetValue(name, defaultValue.ToString());
		float.TryParse(GetValue(name, defaultValue.ToString()), out float result);
		slider.value = (result - min) / (max - min);
		slider.onValueChanged.AddListener(delegate
		{
			float f = slider.value * (max - min) + min;
			SetValue(value: Utils.GetFloat2(f).ToString(), name: name);
			text.text = GetValue(name, defaultValue.ToString());
			if (OnValueChange != null)
			{
				OnValueChange();
			}
		});
	}

	protected override void OnRemoveAfter()
	{
	}

	public override void OnHandleNotification(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
		if (name == null)
		{
		}
	}

	protected override void OnLanguageChange()
	{
	}
}
