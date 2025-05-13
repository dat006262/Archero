




using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipCombineParent : MonoBehaviour
{
	public GameObject child;

	public RectTransform arrow;

	public List<GameObject> mCombineBG;

	public GameObject copychoose;

	public Action<EquipCombineChooseOne> OnCombineDown;

	private int mCount;

	private int width = 120;

	private LocalUnityObjctPool mPool;

	private List<EquipCombineChooseOne> mChooses = new List<EquipCombineChooseOne>();

	public List<int> mChooseIndexs = new List<int>();

	private List<float> mPos3 = new List<float>
	{
		-150f,
		30f,
		150f
	};

	private List<float> mPos2 = new List<float>
	{
		-100f,
		80f
	};

	private int equipid;

	private void Awake()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<EquipCombineChooseOne>(copychoose);
	}

	public void init_data(int count, EquipCombineOne equip)
	{
		equipid = equip.mData.EquipID;
		mCount = count;
		mChooseIndexs.Clear();
		mChooseIndexs.Add(equip.mIndex);
		for (int i = 0; i < count - 1; i++)
		{
			mChooseIndexs.Add(-1);
		}
	}

	public void Init(int count, EquipCombineOne equip)
	{
		mCount = count;
		mChooses.Clear();
		mPool.Collect<EquipCombineChooseOne>();
		List<float> list = mPos3;
		for (int i = 0; i < count; i++)
		{
			EquipCombineChooseOne equipCombineChooseOne = mPool.DeQueue<EquipCombineChooseOne>();
			equipCombineChooseOne.gameObject.SetParentNormal(mCombineBG[i]);
			RectTransform rectTransform = equipCombineChooseOne.transform as RectTransform;
			rectTransform.SetAsFirstSibling();
			equipCombineChooseOne.Clear();
			if (i == 0)
			{
				equipCombineChooseOne.Init(i, equip.mData);
				equipCombineChooseOne.Set_Choose_Equip(equip);
			}
			else
			{
				equipCombineChooseOne.Init(i, equip.mData);
				equipCombineChooseOne.ShowMask(show: true);
			}
			equipCombineChooseOne.OnButtonClick = OnCombineDown;
			mChooses.Add(equipCombineChooseOne);
		}
	}

	public bool can_choose(EquipCombineOne one)
	{
		if (equipid == one.mData.EquipID)
		{
			return true;
		}
		return false;
	}

	public void Show(bool value)
	{
		if (!value)
		{
			equipid = 0;
			mPool.Collect<EquipCombineChooseOne>();
			int i = 0;
			for (int count = mChooseIndexs.Count; i < count; i++)
			{
				modify_chooseindex(i, -1);
			}
		}
	}

	public EquipCombineChooseOne ChooseOne(EquipCombineOne one)
	{
		int i = 0;
		for (int count = mChooseIndexs.Count; i < count; i++)
		{
			if (mChooseIndexs[i] < 0)
			{
				modify_chooseindex(i, one.mIndex);
				mChooses[i].Set_Choose_Equip(one);
				return mChooses[i];
			}
		}
		return null;
	}

	public void down_one(int index)
	{
		int i = 0;
		for (int count = mChooseIndexs.Count; i < count; i++)
		{
			if (mChooseIndexs[i] == index)
			{
				modify_chooseindex(i, -1);
				if (mChooses[index].mEquipChoose != null)
				{
					mChooses[index].mEquipChoose.PlayAni(value: true);
				}
				mChooses[index].Clear();
			}
		}
	}

	private void modify_chooseindex(int index, int value)
	{
		mChooseIndexs[index] = value;
		debug_indexs();
	}

	public int FindEmpty()
	{
		int i = 0;
		for (int count = mChooseIndexs.Count; i < count; i++)
		{
			if (mChooseIndexs[i] < 0)
			{
				return i;
			}
		}
		return -1;
	}

	private void debug_indexs()
	{
	}

	public int get_choose_index(EquipCombineOne one)
	{
		int i = 0;
		for (int count = mChooseIndexs.Count; i < count; i++)
		{
			if (mChooseIndexs[i] == one.mIndex)
			{
				return i;
			}
		}
		return -1;
	}

	public EquipCombineChooseOne GetChoose(int index)
	{
		if (index >= 0 && index < mChooses.Count)
		{
			return mChooses[index];
		}
		return null;
	}

	public int GetIndex(int index)
	{
		debug_indexs();
		if (index >= 0 && index < mChooseIndexs.Count)
		{
			return mChooseIndexs[index];
		}
		return -1;
	}

	public float GetScale(int index)
	{
		Vector3 localScale = mCombineBG[index].transform.localScale;
		return localScale.x;
	}

	public Vector3 GetPosition(int index)
	{
		return mCombineBG[index].transform.position;
	}

	public bool Is_Full()
	{
		return FindEmpty() < 0;
	}
}
