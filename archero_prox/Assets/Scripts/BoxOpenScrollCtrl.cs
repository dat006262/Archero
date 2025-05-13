




using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class BoxOpenScrollCtrl : MonoBehaviour
{
	public GameObject scrollparent;

	public GameObject itemone;

	public Action OnScrollEnd;

	private int count = 25;

	private float perwidth;

	private float endposx;

	private List<BoxOpenOneCtrl> mList = new List<BoxOpenOneCtrl>();

	private RectTransform listone;

	private LocalUnityObjctPool mPool;

	private int state;

	private float maxrotatetime = 2f;

	private float maxlength;

	private float maxspeed = 1500f;

	private float framedis;

	private float framealldis;

	private List<int> mEquips;

	private int currentindex;

	private int gocount;

	private int resultequipid;

	private int resultindex;

	private int resultcount;

	private void Awake()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<BoxOpenOneCtrl>(itemone);
		Vector2 sizeDelta = (itemone.transform as RectTransform).sizeDelta;
		perwidth = sizeDelta.x;
	}

	public void Init()
	{
		mEquips = GetEquips();
		mEquips.RandomSort();
		framealldis = 0f;
		mList.Clear();
		mPool.Collect<BoxOpenOneCtrl>();
		for (int i = 0; i < count; i++)
		{
			BoxOpenOneCtrl boxOpenOneCtrl = mPool.DeQueue<BoxOpenOneCtrl>();
			boxOpenOneCtrl.Init(mEquips[i]);
			RectTransform rectTransform = boxOpenOneCtrl.transform as RectTransform;
			rectTransform.name = i.ToString();
			rectTransform.SetParentNormal(scrollparent);
			rectTransform.anchoredPosition = new Vector2(perwidth * (float)i, 0f);
			mList.Add(boxOpenOneCtrl);
		}
		endposx = perwidth * (float)count;
		DOTween.Sequence().AppendInterval(0f).AppendCallback(delegate
		{
			StartScroll();
		});
	}

	private void StartScroll()
	{
		gocount = 0;
		mEquips.RandomSort();
		currentindex = 0;
		framealldis = 0f;
		state = 1;
		for (int i = 0; i < count; i++)
		{
			mList[i].mRectTransform.anchoredPosition = new Vector2(perwidth * (float)i, 0f);
			mList[i].Init(mEquips[currentindex]);
			GotoNextEquip();
		}
	}

	public void ReceiveData(Drop_DropModel.DropData transfer)
	{
		resultequipid = transfer.id;
		resultcount = transfer.count;
		resultindex = GetResultIndex();
		int num;
		if (resultindex > currentindex && resultindex < currentindex + 20)
		{
			num = currentindex + 20;
			num %= mEquips.Count;
			mEquips.RemoveAt(resultindex);
			mEquips.Insert(num, resultequipid);
		}
		else
		{
			num = resultindex;
		}
		currentindex = num - 15;
		if (currentindex < 0)
		{
			currentindex += mEquips.Count;
		}
		resultindex = GetResultIndex();
		maxlength = (float)(gocount + 23) * perwidth;
		state = 2;
	}

	private int GetResultIndex()
	{
		int i = 0;
		for (int num = mEquips.Count; i < num; i++)
		{
			if (mEquips[i] == resultequipid)
			{
				return i;
			}
		}
		SdkManager.Bugly_Report("BoxOpenScrollCtrl", Utils.FormatString("GetResultIndex[{0}] is error.", resultequipid));
		return -1;
	}

	private void GotoNextEquip()
	{
		currentindex++;
		currentindex %= mEquips.Count;
	}

	private void Update()
	{
		if (state == 0)
		{
			return;
		}
		switch (state)
		{
		case 1:
			framedis = Updater.delta * maxspeed;
			framealldis += framedis;
			for (int num2 = mList.Count - 1; num2 >= 0; num2--)
			{
				listone = mList[num2].mRectTransform;
				RectTransform rectTransform3 = listone;
				Vector2 anchoredPosition4 = listone.anchoredPosition;
				rectTransform3.anchoredPosition = new Vector2(anchoredPosition4.x - framedis, 0f);
				Vector2 anchoredPosition5 = listone.anchoredPosition;
				if (anchoredPosition5.x <= (0f - perwidth) * 2f)
				{
					RectTransform rectTransform4 = listone;
					Vector2 anchoredPosition6 = listone.anchoredPosition;
					rectTransform4.anchoredPosition = new Vector2(anchoredPosition6.x + endposx, 0f);
					mList[num2].Init(mEquips[currentindex]);
					gocount++;
					GotoNextEquip();
				}
			}
			break;
		case 2:
			framedis = Updater.delta * maxspeed;
			if (framealldis + framedis > maxlength)
			{
				framedis = maxlength - framealldis;
				state = 3;
			}
			framealldis += framedis;
			for (int num = mList.Count - 1; num >= 0; num--)
			{
				listone = mList[num].mRectTransform;
				RectTransform rectTransform = listone;
				Vector2 anchoredPosition = listone.anchoredPosition;
				rectTransform.anchoredPosition = new Vector2(anchoredPosition.x - framedis, 0f);
				Vector2 anchoredPosition2 = listone.anchoredPosition;
				if (anchoredPosition2.x <= (0f - perwidth) * 2f)
				{
					RectTransform rectTransform2 = listone;
					Vector2 anchoredPosition3 = listone.anchoredPosition;
					rectTransform2.anchoredPosition = new Vector2(anchoredPosition3.x + endposx, 0f);
					mList[num].Init(mEquips[currentindex]);
					GotoNextEquip();
				}
			}
			if (state == 3)
			{
				DealSlow();
			}
			break;
		}
	}

	private void DealSlow()
	{
		for (int i = 0; i < count; i++)
		{
			int index = i;
			RectTransform mRectTransform = mList[index].mRectTransform;
			RectTransform target = mRectTransform;
			Vector3 localPosition = mRectTransform.localPosition;
			target.DOLocalMoveX(localPosition.x - perwidth * 15f, 4f).SetEase(Ease.OutCubic).OnComplete(delegate
			{
				if (index == count - 1 && OnScrollEnd != null)
				{
					OnScrollEnd();
				}
			});
		}
	}

	private List<int> GetEquips()
	{
		List<int> list = new List<int>();
		IList<Equip_equip> allBeans = LocalModelManager.Instance.Equip_equip.GetAllBeans();
		int i = 0;
		for (int num = allBeans.Count; i < num; i++)
		{
			list.Add(allBeans[i].Id);
		}
		return list;
	}
}
