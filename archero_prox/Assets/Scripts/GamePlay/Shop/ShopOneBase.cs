




using UnityEngine;

public abstract class ShopOneBase : MonoBehaviour
{
	private RectTransform _rectt;

	public RectTransform mRectTransform
	{
		get
		{
			if (_rectt == null)
			{
				_rectt = (base.transform as RectTransform);
			}
			return _rectt;
		}
	}

	private void Awake()
	{
		OnAwake();
	}

	protected virtual void OnAwake()
	{
	}

	public void Init()
	{
		OnInit();
	}

	protected abstract void OnInit();

	public void Deinit()
	{
		OnDeinit();
	}

	protected abstract void OnDeinit();

	public abstract void OnLanguageChange();

	public abstract void UpdateNet();
}
