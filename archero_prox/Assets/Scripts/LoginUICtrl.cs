




using DG.Tweening;
using PureMVC.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class LoginUICtrl : MediatorCtrlBase
{
	public GameObject loginobj;

	public Image Image_BG;

	public Image Image_Splash1;

	public CanvasGroup mCanvasGroup;

	private Sequence seq_loading;

	protected override void OnInitBefore()
	{
		bInitSize = false;
	}

	protected override void OnInit()
	{
	}

	protected override void OnOpen()
	{
		OnLogin();
	}

	private void OnLogin()
	{
		KillSequence();
		DOTween.Sequence().AppendInterval(0.2f).AppendCallback(delegate
		{
			WindowUI.ShowWindow(WindowID.WindowID_Main);
		})
			.AppendInterval(0.3f)
			.Append(mCanvasGroup.DOFade(0f, 0.5f).SetEase(Ease.Linear))
			.AppendCallback(delegate
			{
				ApplicationEvent.Instance.check_app_start();
				WindowUI.CloseWindow(WindowID.WindowID_Login);
			});
	}

	private void KillSequence()
	{
		if (seq_loading != null)
		{
			seq_loading.Kill();
			seq_loading = null;
		}
	}

	protected override void OnClose()
	{
		KillSequence();
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
