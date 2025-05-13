




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class RewardShowUICtrl : MediatorCtrlBase
{
	private enum EState
	{
		eDoing,
		eDone
	}

	public BoxOpenGetCtrl mGetCtrl;

	public ButtonCtrl mButtonClose;

	public TapToCloseCtrl mTapCloseCtrl;

	public ButtonCtrl Button_Shadow;

	public GameObject copyitems;

	private RewardShowProxy.Transfer mTransfer;

	private List<Drop_DropModel.DropData> mEquipTransfer = new List<Drop_DropModel.DropData>();

	private int currentIndex;

	private EState mState;

	protected override void OnInit()
	{
		copyitems.SetActive(value: false);
		Button_Shadow.onClick = OnClickShadow;
		mButtonClose.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_RewardShow);
		};
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("RewardShowProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("RewardShowUICtrl", Utils.FormatString("proxy is null"));
			return;
		}
		if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("RewardShowUICtrl", Utils.FormatString("proxy.Data is null"));
			return;
		}
		if (!(proxy.Data is RewardShowProxy.Transfer))
		{
			SdkManager.Bugly_Report("RewardShowUICtrl", Utils.FormatString("proxy is not a RewardShowProxy.Transfer."));
			return;
		}
		mTransfer = (proxy.Data as RewardShowProxy.Transfer);
		if (mTransfer.list == null)
		{
			SdkManager.Bugly_Report("RewardShowUICtrl", Utils.FormatString("proxy.list is null."));
			return;
		}
		ExcuteEquips();
		InitUI();
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
		mButtonClose.transform.parent.gameObject.SetActive(value);
	}

	private void InitUI()
	{
		show_close(value: false);
		mTapCloseCtrl.Show(value: false);
		mState = EState.eDoing;
		mGetCtrl.Show(value: false);
		currentIndex = 0;
		int i = 0;
		for (int count = mEquipTransfer.Count; i < count; i++)
		{
			mEquipTransfer[i].OnClose = OnOneUIClose;
		}
		GameLogic.Hold.Sound.PlayUI(1000009);
		DOTween.Sequence().AppendInterval(0.2f).AppendCallback(delegate
		{
			GameLogic.Hold.Sound.PlayUI(1000012);
			PlayCurrent();
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
		DOTween.Sequence().AppendInterval(0.15f).AppendCallback(delegate
		{
		})
			.AppendInterval(0.2f)
			.AppendCallback(delegate
			{
				Facade.Instance.RegisterProxy(new BoxOpenOneProxy(mEquipTransfer[currentIndex]));
				WindowUI.ShowWindow(WindowID.WindowID_BoxOpenOne);
			});
	}

	private void PlayGet()
	{
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
