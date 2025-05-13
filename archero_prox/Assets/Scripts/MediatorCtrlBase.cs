




using PureMVC.Interfaces;
using UnityEngine;

public abstract class MediatorCtrlBase : MonoBehaviour
{
	protected bool bInitSize = true;

	protected WindowID mWindowID;

	protected virtual void OnInitBefore()
	{
	}

	public void Init()
	{
		OnInitBefore();
		if (bInitSize)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (GameLogic.ScreenRatio > 1.77777779f)
			{
				rectTransform.sizeDelta = new Vector2(GameLogic.Width, GameLogic.Height);
			}
			else
			{
				rectTransform.sizeDelta = new Vector2((float)GameLogic.ScreenWidth * (float)GameLogic.DesignHeight / (float)GameLogic.ScreenHeight, GameLogic.DesignHeight);
			}
		}
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	public void SetArgs(object o)
	{
		OnSetArgs(o);
	}

	protected virtual void OnSetArgs(object o)
	{
	}

	public void Open()
	{
		OnOpen();
	}

	protected virtual void OnOpen()
	{
	}

	public void Close()
	{
		OnClose();
	}

	protected virtual void OnClose()
	{
	}

	public virtual void OnHandleNotification(INotification notification)
	{
	}

	public abstract void OnLanguageChange();

	public virtual object OnGetEvent(string eventName)
	{
		return null;
	}
}
