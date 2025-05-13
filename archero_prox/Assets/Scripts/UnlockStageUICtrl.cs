




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.UI;

public class UnlockStageUICtrl : MediatorCtrlBase
{
	public ButtonCtrl Button_Close;

	public Text Text_Title;

	public CanvasGroup titlecanvas;

	public CanvasGroup levelcanvas;

	public CanvasGroup infocanvas;

	public CanvasGroup skillcanvas;

	public Text Text_Close;

	public Text Text_Info;

	public UnlockStageLevelCtrl mLevelCtrl;

	public UnlockStageSkillCtrl mSkillCtrl;

	private UnlockStageProxy.Transfer mTransfer;

	private Tweener t_close;

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_UnlockStage);
		};
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("UnlockStageProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("UnlockStageUICtrl", "OnOpen proxy is null.");
			Button_Close.onClick();
		}
		if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("UnlockStageUICtrl", "OnOpen proxy.Data is null.");
			Button_Close.onClick();
		}
		if (!(proxy.Data is UnlockStageProxy.Transfer))
		{
			SdkManager.Bugly_Report("UnlockStageUICtrl", "OnOpen proxy.Data is not UnlockStageProxy.Transfer.");
			Button_Close.onClick();
		}
		mTransfer = (proxy.Data as UnlockStageProxy.Transfer);
		InitUI();
	}

	private void InitUI()
	{
		mLevelCtrl.Init(mTransfer.StageID);
		Text_Info.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("ChapterInfo_{0}", mTransfer.StageID));
		Button_Close.gameObject.SetActive(value: false);
		Text_Close.gameObject.SetActive(value: false);
		titlecanvas.alpha = 0f;
		levelcanvas.alpha = 0f;
		infocanvas.alpha = 0f;
		skillcanvas.alpha = 0f;
		(titlecanvas.transform as RectTransform).anchoredPosition = new Vector2(0f, -700f);
		mLevelCtrl.transform.localPosition = Vector3.zero;
		infocanvas.transform.localPosition = Vector3.zero;
		mSkillCtrl.transform.localPosition = Vector3.zero;
		float time = 0.3f;
		float num = 0f;
		Sequence sequence = DOTween.Sequence();
		sequence.AppendCallback(delegate
		{
			Sequence s4 = DOTween.Sequence();
			s4.Append(titlecanvas.DOFade(1f, time));
			RectTransform target = titlecanvas.transform as RectTransform;
			s4.Join(target.DOAnchorPosY(-600f, time));
		});
		sequence.AppendInterval(time + num);
		sequence.AppendCallback(delegate
		{
			Sequence s3 = DOTween.Sequence();
			s3.Append(levelcanvas.DOFade(1f, time));
			s3.Join(levelcanvas.transform.DOLocalMoveY(100f, time));
		});
		sequence.AppendInterval(time + num);
		sequence.AppendCallback(delegate
		{
			Sequence s2 = DOTween.Sequence();
			s2.Append(infocanvas.DOFade(1f, time));
			s2.Join(infocanvas.transform.DOLocalMoveY(100f, time));
		});
		sequence.AppendInterval(time + num);
		if (mSkillCtrl.GetUnlockSkillCount(mTransfer.StageID) > 0)
		{
			sequence.AppendCallback(delegate
			{
				Sequence s = DOTween.Sequence();
				s.Append(skillcanvas.DOFade(1f, time));
				s.Join(skillcanvas.transform.DOLocalMoveY(100f, time));
			});
			sequence.AppendInterval(time + num);
			mSkillCtrl.Init(sequence, mTransfer.StageID);
			sequence.AppendInterval(0.5f);
		}
		sequence.AppendCallback(delegate
		{
			Text_Close.gameObject.SetActive(value: true);
			Button_Close.gameObject.SetActive(value: true);
		});
	}

	protected override void OnClose()
	{
		mSkillCtrl.DeInit();
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("UnlockNewStage");
		Text_Close.text = GameLogic.Hold.Language.GetLanguageByTID("TapToClose");
	}
}
