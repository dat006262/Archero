




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;

public class EventChest1UICtrl : MediatorCtrlBase
{
	public ButtonCtrl Button_Start;

	public EventChest1TurnCtrl mTurnCtrl;

	private TurnTableType resultType;

	private string[] args;

	protected override void OnInit()
	{
		mTurnCtrl.TurnEnd = delegate(TurnTableType type)
		{
			resultType = type;
			WindowUI.CloseWindow(WindowID.WindowID_EventChect1);
		};
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("EventChest1Proxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("EventChest1UICtrl", "OnOpen proxy is null.");
		}
		if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("EventChest1UICtrl", "OnOpen proxy.Data is null.");
		}
		if (!(proxy.Data is string[]))
		{
			SdkManager.Bugly_Report("EventChest1UICtrl", "OnOpen proxy.Data is not string[].");
		}
		args = (proxy.Data as string[]);
		if (args.Length != 2)
		{
			SdkManager.Bugly_Report("EventChest1UICtrl", "OnOpen proxy.Data.Length != 2.");
		}
		GameLogic.SetPause(pause: true);
		Button_Start.onClick = delegate
		{
			Button_Start.gameObject.SetActive(value: false);
			mTurnCtrl.Init();
		};
		Button_Start.gameObject.SetActive(value: true);
		InitUI();
	}

	private void InitUI()
	{
		int result = 0;
		int result2 = 0;
		if (int.TryParse(args[0], out result) && int.TryParse(args[1], out result2))
		{
			Drop_DropModel.DropData[] array = new Drop_DropModel.DropData[2];
			List<Drop_DropModel.DropData> dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(result);
			if (dropList.Count > 0)
			{
				array[0] = dropList[0];
			}
			List<Drop_DropModel.DropData> dropList2 = LocalModelManager.Instance.Drop_Drop.GetDropList(result2);
			if (dropList2.Count > 0)
			{
				array[1] = dropList2[0];
			}
			mTurnCtrl.InitGood(array);
		}
		else
		{
			SdkManager.Bugly_Report("EventChest1UICtrl", "OnOpen proxy.Data is not all int.");
		}
	}

	protected override void OnClose()
	{
		GameLogic.SetPause(pause: false);
		GameLogic.Release.Mode.RoomGenerate.EventClose(new RoomGenerateBase.EventCloseTransfer
		{
			windowid = WindowID.WindowID_EventChect1,
			data = resultType
		});
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	public override void OnLanguageChange()
	{
	}
}
