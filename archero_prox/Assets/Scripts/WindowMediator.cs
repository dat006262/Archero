




using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowMediator : Mediator
{
	public enum LayerType
	{
		eRoot,
		eInGame,
		eFront,
		eFrontEvent,
		eFront2,
		eFront3,
		eFrontMask,
		eFrontNet
	}

	public class WindowCacheData
	{
		public string name;

		public GameObject obj;

		public float lasttime;
	}

	public static Dictionary<string, WindowCacheData> mCacheUIPanel = new Dictionary<string, WindowCacheData>();

	public static Dictionary<string, FrontEventCtrl> mFrontEventList = new Dictionary<string, FrontEventCtrl>();

	public static Dictionary<string, Sequence> mSequences = new Dictionary<string, Sequence>();

	protected string UIPath;

	private GameObject _popupparent;

	private FrontEventCtrl mFrontEventCtrl;

	protected GameObject _MonoView;

	public sealed override IEnumerable<string> ListNotificationInterests
	{
		get
		{
			List<string> list = new List<string>();
			list.Add("PUB_LANGUAGE_UPDATE");
			list.Add("PUB_UI_UPDATE_CURRENCY");
			list.Add("PUB_NETCONNECT_UPDATE");
			list.Add("PUB_UI_UPDATE_PING");
			List<string> onListNotificationInterests = OnListNotificationInterests;
			int i = 0;
			for (int count = onListNotificationInterests.Count; i < count; i++)
			{
				list.Add(onListNotificationInterests[i]);
			}
			return list;
		}
	}

	public virtual List<string> OnListNotificationInterests => new List<string>();

	public WindowMediator(string path)
	{
		m_mediatorName = GetType().ToString();
		UIPath = path;
	}

	public static void RemoveCache(string name)
	{
		if (mCacheUIPanel.TryGetValue(name, out WindowCacheData value))
		{
			mFrontEventList.TryGetValue(name, out FrontEventCtrl value2);
			if (value2 != null)
			{
				UnityEngine.Object.Destroy(value2.gameObject);
				mFrontEventList.Remove(name);
			}
			else if (value.obj != null)
			{
				UnityEngine.Object.Destroy(value.obj);
				value.obj = null;
			}
			mCacheUIPanel.Remove(name);
		}
		MediatorBase.Remove(name);
	}

	public static Transform GetParent(string name)
	{
		switch (UIResourceDefine.GetWindowLayerType(name))
		{
		case LayerType.eRoot:
			return GameNode.m_UIMain;
		case LayerType.eInGame:
			return GameNode.m_InGame;
		case LayerType.eFront:
			return GameNode.m_Front;
		case LayerType.eFrontEvent:
			return GameNode.m_FrontEvent;
		case LayerType.eFront2:
			return GameNode.m_Front2;
		case LayerType.eFront3:
			return GameNode.m_Front3;
		case LayerType.eFrontMask:
			return GameNode.m_FrontMask;
		case LayerType.eFrontNet:
			return GameNode.m_FrontNet;
		default:
			return null;
		}
	}

	private Sequence GetSeq()
	{
		Sequence value = null;
		if (mSequences.TryGetValue(m_mediatorName, out value))
		{
			if (value != null)
			{
				value.Kill();
				value = null;
			}
			value = DOTween.Sequence();
			mSequences[m_mediatorName] = value;
		}
		else
		{
			value = DOTween.Sequence();
			mSequences.Add(m_mediatorName, value);
		}
		return value;
	}

	private void ClearSeq()
	{
		Sequence value = null;
		if (mSequences.TryGetValue(m_mediatorName, out value) && value != null)
		{
			value.Kill();
			value = null;
		}
	}

	public override void OnRegister()
	{
		ClearSeq();
		OnRegisterBefore();
		GameObject gameObject = null;
		WindowCacheData value;
		bool flag = mCacheUIPanel.TryGetValue(m_mediatorName, out value);
		if (value != null)
		{
			gameObject = value.obj;
		}
		if (!gameObject)
		{
			_MonoView = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/" + UIPath));
			if (UIResourceDefine.GetWindowPop(m_mediatorName))
			{
				_popupparent = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/ACommon/PopUpParent"));
				_popupparent.name = m_mediatorName;
				RectTransform rectTransform = _popupparent.transform as RectTransform;
				rectTransform.SetParent(GetParent(m_mediatorName));
				rectTransform.localPosition = Vector3.zero;
				rectTransform.offsetMin = Vector2.zero;
				rectTransform.offsetMax = Vector2.zero;
				mFrontEventCtrl = _popupparent.GetComponent<FrontEventCtrl>();
				mFrontEventList.Add(m_mediatorName, mFrontEventCtrl);
				mFrontEventCtrl.Play(value: true);
				_MonoView.SetParentNormal(_popupparent.transform);
			}
			else
			{
				_MonoView.SetParentNormal(GetParent(m_mediatorName));
			}
			base.ViewComponent = _MonoView;
			if (!flag)
			{
				mCacheUIPanel.Add(m_mediatorName, new WindowCacheData
				{
					name = m_mediatorName,
					obj = _MonoView,
					lasttime = Time.realtimeSinceStartup
				});
			}
			else
			{
				mCacheUIPanel[m_mediatorName].obj = _MonoView;
				mCacheUIPanel[m_mediatorName].lasttime = Time.realtimeSinceStartup;
			}
			OnRegisterOnce();
		}
		else
		{
			mCacheUIPanel[m_mediatorName].lasttime = Time.realtimeSinceStartup;
			_MonoView = gameObject;
			_MonoView.SetActive(value: true);
			_MonoView.transform.SetAsLastSibling();
			mFrontEventList.TryGetValue(m_mediatorName, out mFrontEventCtrl);
			if (mFrontEventCtrl != null)
			{
				mFrontEventCtrl.gameObject.SetActive(value: true);
				mFrontEventCtrl.transform.SetAsLastSibling();
				mFrontEventCtrl.Play(value: true);
			}
		}
		OnRegisterEvery();
		OnLanguageChange();
	}

	public sealed override void OnRemove()
	{
		if (UIResourceDefine.GetWindowPop(m_mediatorName) && mFrontEventCtrl != null)
		{
			mFrontEventCtrl.Play(value: false);
			GetSeq().AppendInterval(0.15f).AppendCallback(delegate
			{
				if (mFrontEventCtrl != null)
				{
					mFrontEventCtrl.gameObject.SetActive(value: false);
				}
				WindowUI.PopClose();
			}).SetUpdate(isIndependentUpdate: true);
		}
		else
		{
			if ((bool)_MonoView)
			{
				_MonoView.SetActive(value: false);
			}
			WindowUI.PopClose();
		}
		OnRemoveAfter();
	}

	protected virtual void OnRemoveAfter()
	{
	}

	protected virtual void OnRegisterBefore()
	{
	}

	protected virtual void OnRegisterOnce()
	{
	}

	protected virtual void OnRegisterEvery()
	{
	}

	public sealed override void HandleNotification(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
		if (name != null && name == "PUB_LANGUAGE_UPDATE")
		{
			OnLanguageChange();
		}
		OnHandleNotification(notification);
	}

	public virtual void OnHandleNotification(INotification notification)
	{
	}

	public virtual void OnShowWindow()
	{
	}

	protected abstract void OnLanguageChange();

	public void WindowShowUI(bool show)
	{
		if (!_MonoView)
		{
			return;
		}
		_MonoView.SetActive(show);
		if (mFrontEventCtrl != null)
		{
			if (show)
			{
				mFrontEventCtrl.gameObject.SetActive(value: true);
				mFrontEventCtrl.Play(value: true);
			}
			else
			{
				mFrontEventCtrl.Play(value: false);
				DOTween.Sequence().AppendInterval(0.15f).AppendCallback(delegate
				{
					mFrontEventCtrl.gameObject.SetActive(value: false);
				})
					.SetUpdate(isIndependentUpdate: true);
			}
		}
	}
}
