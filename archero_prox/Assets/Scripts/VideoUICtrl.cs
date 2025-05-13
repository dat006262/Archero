




using DG.Tweening;
using PureMVC.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class VideoUICtrl : MediatorCtrlBase
{
	public Image Image_Boss;

	public Image Image_Hero;

	public Text Text_1;

	public Text Text_2;

	private bool bStartLogin;

	private bool bShowNet;

	private int mLoginSate;

	private Sequence seq;

	private Sequence seq_login;

	protected override void OnInit()
	{
		mLoginSate = 0;
		LocalSave.Instance.DoLogin_Start(OnLoginCallback);
		bStartLogin = false;
		Text_1.color = new Color(1f, 1f, 1f, 0f);
		Text_2.color = new Color(1f, 1f, 1f, 0f);
		Image_Boss.color = new Color(1f, 1f, 1f, 0f);
		Image_Hero.color = new Color(1f, 1f, 1f, 0f);
		Image_Hero.rectTransform.anchoredPosition = new Vector2(0f, -300f);
		KillSequence();
		seq = DOTween.Sequence();
		seq.AppendCallback(delegate
		{
			DOTween.Sequence().Append(Image_Boss.DOFade(1f, 1f)).AppendCallback(delegate
			{
				GameLogic.Hold.Sound.PlayUI(1100001);
			});
			DOTween.Sequence().AppendInterval(0.5f).Append(Text_1.DOFade(1f, 1f));
		});
		seq.AppendInterval(2f);
		seq.AppendCallback(delegate
		{
			Text_1.color = new Color(1f, 1f, 1f, 0f);
		});
		seq.Append(Image_Hero.DOFade(1f, 1f));
		seq.Join(Image_Hero.rectTransform.DOAnchorPosY(0f, 1f));
		seq.Append(Text_2.DOFade(1f, 1f));
		seq.AppendInterval(1f);
		seq.AppendCallback(delegate
		{
			Text_2.color = new Color(1f, 1f, 1f, 0f);
		});
		seq.AppendCallback(delegate
		{
			bStartLogin = true;
			WindowUI.CloseWindow(WindowID.WindowID_VideoPlay);
			if (LocalSave.Instance.SaveExtra.guidebattleProcess == 0)
			{
				GameLogic.PlayBattle_Main();
			}
			else
			{
				WindowUI.ShowWindow(WindowID.WindowID_Main);
				WindowUI.ShowMask(value: true);
				WindowUI.ShowMask(value: false);
			}
		});
	}

	private void ShowRetry()
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("first_login_title");
		string languageByTID2 = GameLogic.Hold.Language.GetLanguageByTID("first_login_content");
		string languageByTID3 = GameLogic.Hold.Language.GetLanguageByTID("common_retry");
		WindowUI.ShowPopWindowOneUI(languageByTID, languageByTID2, languageByTID3, closebuttonshow: false, delegate
		{
			DOTween.Sequence().AppendInterval(1f).AppendCallback(delegate
			{
				mLoginSate = 0;
				LocalSave.Instance.DoLogin_Start(OnLoginCallback_Retry);
			});
		});
	}

	private void OnLoginCallback_Retry()
	{
		if (LocalSave.Instance.GetServerUserID() == 0)
		{
			ShowRetry();
		}
	}

	private void OnLoginCallback()
	{
		if (LocalSave.Instance.GetServerUserID() == 0)
		{
			mLoginSate = 2;
		}
		else
		{
			mLoginSate = 1;
		}
	}

	private void ShowNetDoing(bool value)
	{
		if (value && LocalSave.Instance.GetServerUserID() == 0 && !bShowNet)
		{
			bShowNet = true;
			WindowUI.ShowNetDoing(value: true);
		}
		if (!value && bShowNet)
		{
			WindowUI.ShowNetDoing(value: false);
		}
	}

	private void Update()
	{
		if (bStartLogin && LocalSave.Instance.GetServerUserID() != 0)
		{
			WindowUI.CloseWindow(WindowID.WindowID_VideoPlay);
			WindowUI.ShowWindow(WindowID.WindowID_Login);
		}
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	protected override void OnClose()
	{
		KillSequence();
		ShowNetDoing(value: false);
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
		Text_1.text = GameLogic.Hold.Language.GetLanguageByTID("cg_text_1");
		Text_2.text = GameLogic.Hold.Language.GetLanguageByTID("cg_text_2");
	}
}
