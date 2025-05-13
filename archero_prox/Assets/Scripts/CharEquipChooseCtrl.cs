




using System;
using UnityEngine;

public class CharEquipChooseCtrl : MonoBehaviour
{
	[NonSerialized]
	public LocalSave.EquipOne equipdata;

	private int wearindex;

	private bool bShow;

	private string uniqueid;

	private int mIndex;

	public void Init(LocalSave.EquipOne equip)
	{
		if (string.IsNullOrEmpty(uniqueid))
		{
			bShow = false;
		}
		else if (!uniqueid.Equals(equip.UniqueID) || wearindex != equip.WearIndex)
		{
			bShow = false;
		}
		uniqueid = equip.UniqueID;
		wearindex = equip.WearIndex;
		equipdata = equip;
	}

	public void Show(bool show)
	{
	}

	public void Miss()
	{
	}

	public void ChangeShow()
	{
		Show(!bShow);
	}

	public bool GetShow()
	{
		return bShow;
	}

	public void SetIndex(int index)
	{
		mIndex = index;
	}

	public int GetIndex()
	{
		return mIndex;
	}

	public void UpdateNet()
	{
	}

	public void OnLanguageChange()
	{
	}
}
