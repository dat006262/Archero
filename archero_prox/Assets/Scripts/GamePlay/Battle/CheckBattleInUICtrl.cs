




using PureMVC.Interfaces;
using UnityEngine.UI;

public class CheckBattleInUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public Text Text_Content;

	public ButtonCtrl Button_Sure;

	public ButtonCtrl Button_Refuse;

	public Text Text_Sure;

	public Text Text_Refuse;

	protected override void OnInit()
	{
		Button_Sure.onClick = delegate
		{
			WindowUI.ShowLoading(delegate
			{
				LocalSave.Instance.BattleIn_Check();
			});
		};
		Button_Refuse.onClick = delegate
		{
			LocalSave.Instance.BattleIn_DeInit();
			WindowUI.CloseWindow(WindowID.WindowID_CheckBattleIn);
		};
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
	}

	protected override void OnClose()
	{
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("UICommon_Tip");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗");
		Text_Sure.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗确定");
		Text_Refuse.text = GameLogic.Hold.Language.GetLanguageByTID("恢复战斗取消");
	}
}
