




using System.Collections.Generic;

public class UIResourceDefine
{
	public class WindowData	
	{
		public string ClassName;//Name Script

		public WindowMediator.LayerType LayerType; // Layer trên Hierarchy

		public bool isPop;//Co Phai Popup khong 

		public int State; // 0->Chỉ mở Main| 1->Chỉ mở inGame| 3 ->Ad Inside và thanh tài nguyên | 2-> Còn lai
	}

	public static Dictionary<WindowID, WindowData> windowClass = new Dictionary<WindowID, WindowData>
	{
		{
			WindowID.WindowID_SettingDebug,
			new WindowData
			{
				ClassName = "SettingDebugMediator",
				LayerType = WindowMediator.LayerType.eFrontEvent,
				isPop = true
			}
		},
		{
			WindowID.WindowID_DatTestPopup,
			new WindowData
			{
				ClassName = "DatTestPopupUIMediator",
				LayerType = WindowMediator.LayerType.eFront,
				isPop = true,
				State = 0
			}
		}
	};

	public static string UIPrefabPath = "UIPanel/";

	public static WindowMediator.LayerType GetWindowLayerType(string classname)
	{
		Dictionary<WindowID, WindowData>.Enumerator enumerator = windowClass.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.ClassName == classname)
			{
				return enumerator.Current.Value.LayerType;
			}
		}
		return WindowMediator.LayerType.eRoot;
	}

	public static bool GetWindowPop(string classname)
	{
		Dictionary<WindowID, WindowData>.Enumerator enumerator = windowClass.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value.ClassName == classname)
			{
				return enumerator.Current.Value.isPop;
			}
		}
		return false;
	}
}
