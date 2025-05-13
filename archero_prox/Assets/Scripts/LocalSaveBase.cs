




using System;

[Serializable]
public abstract class LocalSaveBase
{
	public void Refresh()
	{
		OnRefresh();
	}

	protected abstract void OnRefresh();
}
