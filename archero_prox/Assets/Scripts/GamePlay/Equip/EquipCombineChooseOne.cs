




using System;
using UnityEngine;

public class EquipCombineChooseOne : MonoBehaviour
{
	public ButtonCtrl mButton;

	public GameObject child;

	public GameObject mMask;

	public Action<EquipCombineChooseOne> OnButtonClick;

	private bool bMask;

	private EquipOneCtrl mEquip;

	public EquipCombineOne mEquipChoose
	{
		get;
		private set;
	}

	public LocalSave.EquipOne mEquipData
	{
		get;
		private set;
	}

	public int mIndex
	{
		get;
		private set;
	}

	private void Awake()
	{
		mButton.onClick = delegate
		{
			if (OnButtonClick != null)
			{
				OnButtonClick(this);
			}
		};
	}

	public void Init(int index, LocalSave.EquipOne one)
	{
		mIndex = index;
		mEquipData = one;
		if (mEquip == null)
		{
			mEquip = CInstance<UIResourceCreator>.Instance.GetEquip(child.transform);
			mEquip.ShowAniEnable(value: false);
		}
		bMask = true;
		ShowMask(show: false);
		mEquip.Init(mEquipData);
		mEquip.SetButtonEnable(value: false);
	}

	public void Set_Choose_Equip(EquipCombineOne one)
	{
		mEquipData = one.mData;
		mEquipChoose = one;
		mEquip.Init(one.mData);
		mEquip.SetButtonEnable(value: false);
		ShowMask(show: false);
	}

	public void Clear()
	{
		if (mEquipChoose != null)
		{
			mEquipChoose.SetChoose(null);
			mEquipChoose = null;
			ShowMask(show: true);
		}
	}

	public void ShowMask(bool show)
	{
		if (bMask != show)
		{
			bMask = show;
			mMask.SetActive(show);
			mEquip.ShowLevel(!show);
		}
	}
}
