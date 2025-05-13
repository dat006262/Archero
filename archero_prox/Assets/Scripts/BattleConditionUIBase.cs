




using UnityEngine;

public class BattleConditionUIBase : MonoBehaviour
{
	protected LocalSave.AchieveDataOne mData;

	public void Init(LocalSave.AchieveDataOne data)
	{
		mData = data;
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	public void Refresh()
	{
		OnRefresh();
	}

	protected virtual void OnRefresh()
	{
	}
}
