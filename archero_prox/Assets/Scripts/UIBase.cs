




using PureMVC.Interfaces;
using UnityEngine;

public abstract class UIBase
{
	protected GameObject mView;

	protected Transform mParent;

	public UIBase(Transform parent)
	{
		mParent = parent;
	}

	public void InitBefore()
	{
		OnInitBefore();
	}

	protected virtual void OnInitBefore()
	{
	}

	public void Init()
	{
		OnInit();
	}

	protected abstract void OnInit();

	public void DeInit()
	{
		OnDeInit();
	}

	protected abstract void OnDeInit();

	public void Open()
	{
		OnOpen();
	}

	protected abstract void OnOpen();

	public void Close()
	{
		OnClose();
	}

	protected abstract void OnClose();

	public void HandleNotification(INotification notification)
	{
		OnHandleNotification(notification);
	}

	protected abstract void OnHandleNotification(INotification notification);

	public abstract void OnLanguageChange();

	public virtual object OnGetEvent(string eventName)
	{
		return null;
	}
}
