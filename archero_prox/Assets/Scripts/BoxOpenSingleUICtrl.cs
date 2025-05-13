




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenSingleUICtrl : MediatorCtrlBase
{
	public BoxOpenBoxAniCtrl mBoxCtrl;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	public GameObject effect_light;

	public GameObject titleparent;

	public CanvasGroup equipparent;

	public CanvasGroup nameparent;

	public Text Text_Quality;

	public Text Text_Name;

	public Text Text_Info;

	public Image Image_BG;

	public Image Image_Icon;

	public BoxOpenSingleRetryCtrl mRetryCtrl;

	private LocalSave.EquipOne equipdata;

	private SequencePool mSeqPool = new SequencePool();

	private BoxOpenSingleProxy.Transfer mTransfer;

	protected override void OnInit()
	{
		mWindowID = WindowID.WindowID_BoxOpen;
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_BoxOpenSingle);
		};
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("BoxOpenSingleProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("BoxOpenSingleUICtrl", Utils.FormatString("proxy is null"));
			return;
		}
		if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("BoxOpenSingleUICtrl", Utils.FormatString("proxy.Data is null"));
			return;
		}
		if (!(proxy.Data is BoxOpenSingleProxy.Transfer))
		{
			SdkManager.Bugly_Report("BoxOpenSingleUICtrl", Utils.FormatString("proxy is not a BoxOpenSingleProxy.Transfer."));
			return;
		}
		mTransfer = (proxy.Data as BoxOpenSingleProxy.Transfer);
		if (mTransfer.data == null)
		{
			SdkManager.Bugly_Report("BoxOpenUICtrl", Utils.FormatString("Transfer.data is null."));
			return;
		}
		mRetryCtrl.onRetry = delegate
		{
			if (mTransfer != null && mTransfer.retry_callback != null)
			{
				if (LocalSave.Instance.GetDiamondBoxFreeCount(mTransfer.boxtype) == 0 && LocalSave.Instance.GetDiamond() < mTransfer.GetCurrentDiamond())
				{
					WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond);
				}
				else
				{
					mTransfer.retry_callback();
				}
			}
		};
		InitUI();
	}

	private void show_close(bool value)
	{
		Button_Close.transform.parent.gameObject.SetActive(value);
	}

	private void InitUI()
	{
		mBoxCtrl.Init();
		SdkManager.send_event_equipment("GET", mTransfer.data.id, mTransfer.data.count, 1, mTransfer.source, 0);
		mRetryCtrl.transform.localScale = Vector3.zero;
		equipdata = new LocalSave.EquipOne();
		equipdata.EquipID = mTransfer.data.id;
		effect_light.SetActive(value: false);
		Text_Name.text = equipdata.NameOnlyString;
		Text_Quality.text = equipdata.QualityString;
		Text_Name.color = equipdata.qualityColor;
		Text_Quality.color = equipdata.qualityColor;
		Text_Info.text = equipdata.InfoString;
		Text_Info.color = new Color(1f, 1f, 1f, 0f);
		titleparent.SetActive(value: false);
		equipparent.alpha = 0f;
		nameparent.alpha = 0f;
		nameparent.transform.localScale = Vector3.one * 1.5f;
		equipparent.transform.localScale = Vector3.one;
		show_close(value: false);
		mBoxCtrl.ShowOpenEffect(value: false);
		mBoxCtrl.transform.localScale = Vector3.one;
		mBoxCtrl.ShowBoxOpeningEffect(value: false);
		mBoxCtrl.ShowBoxOneEffect(value: false);
		GameLogic.Hold.Sound.PlayUI(1000009);
		mBoxCtrl.Play(BoxOpenBoxAniCtrl.BoxState.BoxOpenShow, mTransfer.boxtype);
		Sequence s = mSeqPool.Get();
		s.AppendInterval(0.9f);
		s.AppendCallback(delegate
		{
			mBoxCtrl.Play(BoxOpenBoxAniCtrl.BoxState.BoxOpenOpen, mTransfer.boxtype);
			mSeqPool.Get().AppendInterval(0.75f).AppendCallback(delegate
			{
				GameLogic.Hold.Sound.PlayUI(1000012);
				mBoxCtrl.ShowBoxOneEffect(value: true);
			})
				.AppendInterval(0.1f)
				.AppendCallback(delegate
				{
					mBoxCtrl.ShowBoxOpeningEffect(value: true);
					titleparent.SetActive(value: true);
					Image_Icon.sprite = equipdata.Icon;
					Image_BG.color = new Color(0f, 0f, 0f, 1f);
					Image_Icon.color = new Color(0f, 0f, 0f, 1f);
					effect_light.SetActive(value: true);
					Sequence s2 = mSeqPool.Get();
					s2.Append(equipparent.DOFade(1f, 0.6f));
					s2.Join(equipparent.transform.DOScale(1.1f, 0.6f));
					s2.AppendCallback(delegate
					{
					});
					s2.Append(Image_BG.DOColor(Color.white, 0.6f));
					s2.Join(Image_Icon.DOColor(Color.white, 0.6f));
					s2.Join(equipparent.transform.DOScale(1f, 0.6f));
					s2.AppendCallback(delegate
					{
						nameparent.alpha = 1f;
					});
					s2.Append(nameparent.transform.DOScale(1f, 0.1f).SetEase(Ease.Linear));
					s2.Append(Text_Info.DOFade(1f, 0.3f).SetEase(Ease.Linear));
					s2.AppendCallback(delegate
					{
						Sequence s3 = mSeqPool.Get();
						s3.Append(mBoxCtrl.transform.DOScale(0f, 0.25f).SetEase(Ease.Linear));
						s3.Append(mRetryCtrl.transform.DOScale(1f, 0.25f).SetEase(Ease.Linear));
						mRetryCtrl.Init(mTransfer.boxtype, mTransfer.GetCurrentDiamond(), mTransfer.GetStartDiamond());
						show_close(value: true);
					});
				});
		});
	}

	protected override void OnClose()
	{
		mSeqPool.Clear();
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
		mRetryCtrl.OnLanguageChange();
	}
}
