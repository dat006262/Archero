




using Dxx.UI;
using System;
using UnityEngine;

public class InfinityBase<T> : MonoBehaviour where T : Component
{
	public InfinityScrollGroup infinity;

	public GameObject copyItem;

	public int initDisplayCount = 40;

	public Action<int, T> updatecallback;

	public void Init(int itemcount)
	{
		infinity.RegUpdateCallback<T>(UpdateChildCallbak);
		infinity.Init(initDisplayCount, itemcount, copyItem);
	}

	public void SetItemCount(int itemcount)
	{
		infinity.SetItemCount(itemcount);
	}

	public void Refresh()
	{
		infinity.RefreshAll();
	}

	private void UpdateChildCallbak(int index, T data)
	{
		if (updatecallback != null)
		{
			updatecallback(index, data);
		}
	}
}
