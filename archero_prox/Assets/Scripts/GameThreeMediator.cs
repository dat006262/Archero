




using PureMVC.Interfaces;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameThreeMediator : WindowMediator, IMediator, INotifier
{
	private static GameThreeUICtrl ctrl;

	private static Text textcontent;

	private static Text textok;

	private static Text textok_shadow;

	private int count = 3;

	public override List<string> OnListNotificationInterests => new List<string>();

	public GameThreeMediator()
		: base("GameThreeUIPanel")
	{
	}

	protected override void OnRegisterOnce()
	{
		textcontent = _MonoView.transform.Find("Title/Text_Content").GetComponent<Text>();
		ctrl = _MonoView.GetComponent<GameThreeUICtrl>();
		ctrl.SetCount(count);
		ctrl.SetEndCallback(delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_GameThree);
		});
		ctrl.DoAllActions();
	}

	protected override void OnRegisterEvery()
	{
		GameLogic.SetPause(pause: true);
	}

	protected override void OnRemoveAfter()
	{
		GameLogic.SetPause(pause: false);
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
		textcontent.text = GameLogic.Hold.Language.GetLanguageByTID("猜一猜");
	}
}
