




using UnityEngine;

public class RedNodeCtrl : MonoBehaviour
{
	private Transform child;

	private RedNodeOneCtrl _redctrl;

	private RedNodeOneCtrl mRedCtrl
	{
		get
		{
			if (_redctrl == null && child != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/ACommon/RedNodeOne"));
				gameObject.SetParentNormal(child);
				_redctrl = gameObject.GetComponent<RedNodeOneCtrl>();
			}
			return _redctrl;
		}
	}

	public int Value
	{
		set
		{
			if (value > 0)
			{
				child_show(show: true);
				mRedCtrl.Value = value;
			}
			else
			{
				child_show(show: false);
			}
		}
	}

	private void Awake()
	{
		child = base.transform.Find("child");
	}

	private void child_show(bool show)
	{
		if ((bool)child)
		{
			child.gameObject.SetActive(show);
		}
	}

	public void DestroyChild()
	{
		if (_redctrl != null)
		{
			UnityEngine.Object.DestroyImmediate(_redctrl.gameObject);
		}
	}

	private void SetText(string value)
	{
		mRedCtrl.SetText(value);
	}

	public void SetType(RedNodeType type)
	{
		mRedCtrl.SetType(type);
	}
}
