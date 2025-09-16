




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenUICtrl : MediatorCtrlBase
{
	private enum EState
	{
		eDoing,
		eDone
	}

	public BoxOpenBoxCtrl mBoxCtrl;

	public BoxOpenGetCtrl mGetCtrl;

	public ButtonCtrl mButtonRetry;

	public Text Text_Retry;

	public GoldTextCtrl mGoldCtrl;

	public ButtonCtrl mButtonClose;

	public TapToCloseCtrl mTapCloseCtrl;

	public ButtonCtrl Button_Shadow;

	public GameObject copyitems;

	private BoxOpenProxy.Transfer mTransfer;

	private List<Drop_DropModel.DropData> mEquipTransfer = new List<Drop_DropModel.DropData>();

	private int currentIndex;

	private EState mState;

	protected override void OnInit()
	{
		mWindowID = WindowID.WindowID_BoxOpen;
		copyitems.SetActive(value: false);
		mBoxCtrl.mScrollCtrl.OnScrollEnd = OnScrollEnd;
		Button_Shadow.onClick = OnClickShadow;
		mButtonClose.onClick = delegate
		{
			WindowUI.CloseWindow(mWindowID);
		};
		mButtonRetry.onClick = delegate
		{
			if (mTransfer != null && mTransfer.retry_callback != null)
			{
				if (LocalSave.Instance.GetDiamond() < mTransfer.GetCurrentDiamond())
				{
					WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond);
				}
				else
				{
					mTransfer.retry_callback();
				}
			}
		};
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("BoxOpenProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("BoxOpenUICtrl", Utils.FormatString("proxy is null"));
			return;
		}
		if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("BoxOpenUICtrl", Utils.FormatString("proxy.Data is null"));
			return;
		}
		if (!(proxy.Data is BoxOpenProxy.Transfer))
		{
			SdkManager.Bugly_Report("BoxOpenUICtrl", Utils.FormatString("proxy is not a BoxOpenProxy.Transfer."));
			return;
		}
		mTransfer = (proxy.Data as BoxOpenProxy.Transfer);
		if (mTransfer.list == null)
		{
			SdkManager.Bugly_Report("BoxOpenUICtrl", Utils.FormatString("proxy.list is null."));
			return;
		}
		ExcuteEquips();
		InitUI();
	}

	private void update_retry_button()
	{
		mGoldCtrl.SetCurrencyType(CurrencyType.Diamond);
		mGoldCtrl.SetValue(mTransfer.GetCurrentDiamond());
	}

	private void ExcuteEquips()
	{
		mEquipTransfer.Clear();
		int i = 0;
		for (int count = mTransfer.list.Count; i < count; i++)
		{
			if (mTransfer.list[i].type == PropType.eEquip)
			{
				Drop_DropModel.DropData dropData = new Drop_DropModel.DropData();
				dropData.type = mTransfer.list[i].type;
				int haveCount = GetHaveCount(mTransfer.list[i].id);
				dropData.id = mTransfer.list[i].id;
				dropData.count = mTransfer.list[i].count;
				mEquipTransfer.Add(dropData);
				SdkManager.send_event_equipment("GET", dropData.id, dropData.count, 1, mTransfer.source, 0);
			}
			else
			{
				mEquipTransfer.Add(mTransfer.list[i]);
			}
		}
	}

	private int GetHaveCount(int id)
	{
		int num = 0;
		int i = 0;
		for (int count = mEquipTransfer.Count; i < count; i++)
		{
			if (mEquipTransfer[i].id == id)
			{
				num += mEquipTransfer[i].count;
			}
		}
		return num;
	}

	private void show_close(bool value)
	{
		mButtonRetry.transform.parent.gameObject.SetActive(value);
		mButtonClose.transform.parent.gameObject.SetActive(value);
		if (value)
		{
			update_retry_button();
		}
	}

	private void InitUI()
	{
		show_close(value: false);
		mTapCloseCtrl.Show(value: false);
		mState = EState.eDoing;
		mGetCtrl.Show(value: false);
		mBoxCtrl.mBoxCtrl.ShowOpenEffect(value: false);
		mBoxCtrl.gameObject.SetActive(value: true);
		mBoxCtrl.PlayScrollShow(value: false);
		mBoxCtrl.mBoxCtrl.ShowBoxOpeningEffect(value: false);
		mBoxCtrl.mBoxCtrl.ShowBoxOneEffect(value: false);
		currentIndex = 0;
		int i = 0;
		for (int count = mEquipTransfer.Count; i < count; i++)
		{
			mEquipTransfer[i].OnClose = OnOneUIClose;
		}
		GameLogic.Hold.Sound.PlayUI(1000009);
		mBoxCtrl.mBoxCtrl.Play("BoxOpenShow");
		Sequence s = DOTween.Sequence();
		s.AppendInterval(0.9f);
		s.AppendCallback(delegate
		{
			mBoxCtrl.mBoxCtrl.Play("BoxOpenOpen");
			DOTween.Sequence().AppendInterval(0.75f).AppendCallback(delegate
			{
				GameLogic.Hold.Sound.PlayUI(1000012);
				mBoxCtrl.mBoxCtrl.ShowBoxOneEffect(value: true);
				PlayCurrent();
			})
				.AppendInterval(0.2f)
				.AppendCallback(delegate
				{
					mBoxCtrl.mBoxCtrl.ShowBoxOpeningEffect(value: true);
				});
		});
	}

	private void OnOneUIClose()
	{
		currentIndex++;
		PlayCurrent();
	}

	private void PlayCurrent()
	{
		if (currentIndex < mEquipTransfer.Count)
		{
			DOTween.Sequence().AppendCallback(delegate
			{
				GameLogic.Hold.Sound.PlayUI(1000011);
				Facade.Instance.RegisterProxy(new BoxOpenOneProxy(mEquipTransfer[currentIndex]));
				WindowUI.ShowWindow(WindowID.WindowID_BoxOpenOne);
			});
		}
		else
		{
			PlayGet();
		}
	}

	private void OnScrollEnd()
	{
		mBoxCtrl.mBoxCtrl.Play("BoxOpenOpen");
		DOTween.Sequence().AppendInterval(0.15f).AppendCallback(delegate
		{
			mBoxCtrl.mBoxCtrl.ShowOpenEffect(value: true);
		})
			.AppendInterval(0.2f)
			.AppendCallback(delegate
			{
				mBoxCtrl.PlayScrollShow(value: false);
				Facade.Instance.RegisterProxy(new BoxOpenOneProxy(mEquipTransfer[currentIndex]));
				WindowUI.ShowWindow(WindowID.WindowID_BoxOpenOne);
			});
	}

	private void PlayGet()
	{
		mBoxCtrl.gameObject.SetActive(value: false);
		mGetCtrl.Show(value: true);
		DOTween.Sequence().Append(mGetCtrl.Init(mEquipTransfer)).AppendCallback(delegate
		{
			ChangeState(EState.eDone);
		});
	}

	private void OnClickShadow()
	{
		EState eState = mState;
		if (eState == EState.eDone)
		{
		}
	}

	private void ChangeState(EState state)
	{
		mState = state;
		EState eState = mState;
		if (eState == EState.eDone)
		{
			show_close(value: true);
		}
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
	}
}
