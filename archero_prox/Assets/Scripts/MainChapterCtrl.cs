




using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainChapterCtrl : MonoBehaviour
{
	public GameObject copychapter;

	public ScrollRectBase mScrollRect;

	public ButtonCtrl Button_Left;

	public ButtonCtrl Button_Right;

	public GridLayoutGroup mLayoutGroup;

	public Action OnStageUpdate;

	private LocalUnityObjctPool mPool;

	private List<MainUILevelItem> mList = new List<MainUILevelItem>();

	private int currentstage;

	private void Awake()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<MainUILevelItem>(copychapter);
		copychapter.SetActive(value: false);
		Button_Left.onClick = delegate
		{
			if (currentstage > 1)
			{
				currentstage--;
				mScrollRect.SetPage(currentstage - 1, animate: true);
				update_button();
			}
		};
		Button_Right.onClick = delegate
		{
			if (currentstage < LocalSave.Instance.mStage.CurrentStage)
			{
				currentstage++;
				mScrollRect.SetPage(currentstage - 1, animate: true);
				update_button();
			}
		};
		mScrollRect.ValueChanged = delegate
		{
		};
		mScrollRect.EndDragItem = delegate(int stage)
		{
			currentstage = stage + 1;
			update_button();
		};
	}

	public void Init()
	{
		currentstage = GameLogic.Hold.BattleData.Level_CurrentStage;
		mPool.Collect<MainUILevelItem>();
		mList.Clear();
		mScrollRect.SetWhole(mLayoutGroup, LocalSave.Instance.mStage.CurrentStage);
		for (int i = 0; i < LocalSave.Instance.mStage.CurrentStage; i++)
		{
			MainUILevelItem mainUILevelItem = mPool.DeQueue<MainUILevelItem>();
			mainUILevelItem.OnButtonClick = OnClickItem;
			mainUILevelItem.gameObject.SetParentNormal(mScrollRect.content);
			RectTransform rectTransform = mainUILevelItem.transform as RectTransform;
			RectTransform rectTransform2 = rectTransform;
			float num = i;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			rectTransform2.anchoredPosition = new Vector2(0f, num * sizeDelta.x);
			mainUILevelItem.Init(i + 1);
			mList.Add(mainUILevelItem);
		}
		mScrollRect.SetPage(currentstage - 1, animate: false);
		update_button();
	}

	private void OnClickItem()
	{
		WindowUI.ShowWindow(WindowID.WindowID_StageList);
	}

	private void update_current()
	{
		if (currentstage - 1 >= 0 && currentstage - 1 < mList.Count)
		{
			mList[currentstage - 1].Init(currentstage);
			GameLogic.Hold.BattleData.Level_CurrentStage = currentstage;
			if (OnStageUpdate != null)
			{
				OnStageUpdate();
			}
		}
	}

	private void update_button()
	{
		update_current();
		Button_Left.gameObject.SetActive(value: true);
		Button_Right.gameObject.SetActive(value: true);
		if (currentstage >= LocalSave.Instance.mStage.CurrentStage)
		{
			Button_Right.gameObject.SetActive(value: false);
		}
		if (currentstage <= 1)
		{
			Button_Left.gameObject.SetActive(value: false);
		}
	}
}
