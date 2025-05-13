




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using TableTool;

public class BoxOpenOneUICtrl : MediatorCtrlBase
{
	public ButtonCtrl Button_Close;

	public BoxOpenOneCurrencyCtrl mCurrencyCtrl;

	public BoxOpenOneEquipCtrl mEquipCtrl;

	private int state;

	private Drop_DropModel.DropData mTransfer;

	private Sequence seq;

	private Sequence seq_close;

	protected override void OnInit()
	{
		Button_Close.onClick = OnClickButton;
	}

	private void CloseUI()
	{
		WindowUI.CloseWindow(WindowID.WindowID_BoxOpenOne);
	}

	protected override void OnOpen()
	{
		state = 0;
		IProxy proxy = Facade.Instance.RetrieveProxy("BoxOpenOneProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("BoxOpenOneCtrl", Utils.FormatString("BoxOpenOneProxy is null."));
			CloseUI();
		}
		else if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("BoxOpenOneCtrl", Utils.FormatString("BoxOpenOneProxy.Data is null."));
			CloseUI();
		}
		else if (!(proxy.Data is Drop_DropModel.DropData))
		{
			SdkManager.Bugly_Report("BoxOpenOneCtrl", Utils.FormatString("BoxOpenOneProxy is not Drop_DropModel.DropData."));
			CloseUI();
		}
		else
		{
			mTransfer = (proxy.Data as Drop_DropModel.DropData);
			InitUI();
		}
	}

	private void InitUI()
	{
		mCurrencyCtrl.gameObject.SetActive(value: false);
		mEquipCtrl.gameObject.SetActive(value: false);
		seq = DOTween.Sequence();
		switch (mTransfer.type)
		{
		case PropType.eCurrency:
			mCurrencyCtrl.gameObject.SetActive(value: true);
			seq.Append(mCurrencyCtrl.Init(mTransfer));
			break;
		case PropType.eEquip:
		{
			mEquipCtrl.gameObject.SetActive(value: true);
			LocalSave.EquipOne equipOne = new LocalSave.EquipOne();
			equipOne.EquipID = mTransfer.id;
			equipOne.Level = 1;
			equipOne.Count = 1;
			seq.Append(mEquipCtrl.Init(equipOne, mTransfer.count));
			break;
		}
		default:
			SdkManager.Bugly_Report("BoxOpenOneUICtrl", Utils.FormatString("InitUI {0}", mTransfer.ToString()));
			break;
		}
		seq.AppendCallback(delegate
		{
		});
		seq.AppendCallback(delegate
		{
			DelayClose();
		});
	}

	private void DelayClose()
	{
		seq_close = DOTween.Sequence().AppendInterval(1f).AppendCallback(delegate
		{
			CloseUI();
		});
	}

	private void OnClickButton()
	{
		switch (state)
		{
		case 0:
			state = 1;
			if (seq != null)
			{
				seq.Complete(withCallbacks: true);
				seq = null;
			}
			break;
		case 1:
			CloseUI();
			break;
		}
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
		if (seq_close != null)
		{
			seq_close.Kill();
			seq_close = null;
		}
	}

	protected override void OnClose()
	{
		KillSequence();
		if (mTransfer.OnClose != null)
		{
			mTransfer.OnClose();
		}
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
