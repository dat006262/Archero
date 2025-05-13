




using PureMVC.Interfaces;
using System.Collections.Generic;

public class MediatorBase : WindowMediator, IMediator, INotifier
{
	public static Dictionary<string, MediatorCtrlBase> mCtrlList = new Dictionary<string, MediatorCtrlBase>();

	private MediatorCtrlBase _ctrl;

	private MediatorCtrlBase ctrl
	{
		get
		{
			if (!mCtrlList.TryGetValue(m_mediatorName, out _ctrl) && (bool)_MonoView)
			{
				_ctrl = _MonoView.transform.Find("offset").GetComponent<MediatorCtrlBase>();
				_ctrl.Init();
				mCtrlList.Add(m_mediatorName, _ctrl);
			}
			return _ctrl;
		}
	}

	public MediatorBase(string path)
		: base(path)
	{
	}

	public static void Remove(string name)
	{
		if (mCtrlList.ContainsKey(name))
		{
			mCtrlList.Remove(name);
		}
	}

	protected override void OnRegisterOnce()
	{
	}

	protected override void OnRegisterEvery()
	{
		ctrl.Open();
	}

	protected override void OnRemoveAfter()
	{
		if ((bool)ctrl)
		{
			ctrl.Close();
		}
	}

	public override void OnHandleNotification(INotification notification)
	{
		ctrl.OnHandleNotification(notification);
		string name = notification.Name;
		object body = notification.Body;
		if (name == null)
		{
		}
	}

	protected override void OnLanguageChange()
	{
		ctrl.OnLanguageChange();
	}

	public override object GetEvent(string eventName)
	{
		return ctrl.OnGetEvent(eventName);
	}
}
