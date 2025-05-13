




using DG.Tweening;
using PureMVC.Interfaces;
using UnityEngine.UI;

public class GameOverMatchDefenceTimeCtrl : GameOverModeCtrlBase
{
	public TapToCloseCtrl mCloseCtrl;

	public Text Text_Result;

	protected override void OnInit()
	{
		mCloseCtrl.OnClose = OnClickClose;
	}

	protected override void OnOpen()
	{
		UpdateUI();
	}

	protected override void OnClose()
	{
	}

	private void UpdateUI()
	{
		mCloseCtrl.Show(value: false);
		if (GameLogic.Hold.BattleData.Win)
		{
			Text_Result.text = "You win!";
		}
		else
		{
			Text_Result.text = "You loss.";
		}
		DOTween.Sequence().AppendInterval(1f).AppendCallback(delegate
		{
			mCloseCtrl.Show(value: true);
		})
			.SetUpdate(isIndependentUpdate: true);
	}

	private void OnClickClose()
	{
		WindowUI.ShowLoading(delegate
		{
			WindowUI.ShowWindow(WindowID.WindowID_Main);
		});
	}

	public override object OnGetEvent(string eventName)
	{
		return base.OnGetEvent(eventName);
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	public override void OnLanguageChange()
	{
	}
}
