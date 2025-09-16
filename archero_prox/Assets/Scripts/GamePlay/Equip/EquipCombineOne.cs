




using DG.Tweening;
using System;
using UnityEngine;

public class EquipCombineOne : MonoBehaviour
{
	public DOTweenAnimation child_ani;

	public ButtonCtrl mButton;

	public GameObject equiparent;

	public GameObject mLock;

	public GameObject mChoose_First;

	public GameObject mChoose_Second;

	public Action<EquipCombineOne> OnButtonClick;

	private EquipOneCtrl mEquip;

	public EquipCombineChooseOne mChoose
	{
		get;
		private set;
	}

	public int mIndex
	{
		get;
		private set;
	}

	public LocalSave.EquipOne mData
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
		mData = one;
		if (mEquip == null)
		{
			mEquip = CInstance<UIResourceCreator>.Instance.GetEquip(equiparent.transform);
			mEquip.ShowAniEnable(value: false);
		}
		SetButtonEnable(value: true);
		mEquip.Init(one);
		mEquip.UpdateWear();
		if (one.CanCombine)
		{
			mEquip.SetRedNodeType(RedNodeType.eWarning);
		}
		mEquip.SetButtonEnable(value: false);
		SetChoose(null);
		SetLock(value: false);
	}

	public void SetLock(bool value)
	{
		if (!(mEquip == null))
		{
			mLock.SetActive(value);
		}
	}

	public void PlayAni(bool value)
	{
        if (child_ani)
        {
            if (value)
            {
                child_ani.DOPlay();
                return;
            }
            child_ani.DOPause();
            child_ani.transform.localPosition = Vector3.zero;
            child_ani.transform.localScale = Vector3.one;
        }
	}

	public void SetChoose(EquipCombineChooseOne one)
	{
		mChoose = one;
		if (mChoose == null)
		{
			mChoose_First.SetActive(value: false);
			mChoose_Second.SetActive(value: false);
		}
		else
		{
			mChoose_First.SetActive(one.mIndex == 0);
			mChoose_Second.SetActive(one.mIndex > 0);
			PlayAni(value: false);
		}
	}

	public void SetButtonEnable(bool value)
	{
		mButton.enabled = value;
	}
}
