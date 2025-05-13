




using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class AchieveItemCtrl : MonoBehaviour
{
	public AchieveInfinity mInfinity;

	private List<int> mList = new List<int>();

	private int mStageID;

	public void Init(int stage)
	{
		mStageID = stage;
		mList = LocalModelManager.Instance.Achieve_Achieve.GetStageList(stage, haveglobal: true);
		mList.Sort(delegate(int a, int b)
		{
			bool flag = LocalSave.Instance.Achieve_IsFinish(a);
			bool flag2 = LocalSave.Instance.Achieve_IsFinish(b);
			bool flag3 = LocalSave.Instance.Achieve_Isgot(a);
			bool flag4 = LocalSave.Instance.Achieve_Isgot(b);
			if (flag3 && flag4)
			{
				return (a >= b) ? 1 : (-1);
			}
			return (!flag3 && !flag4) ? ((flag == flag2) ? ((a >= b) ? 1 : (-1)) : ((!flag) ? 1 : (-1))) : (flag3 ? 1 : (-1));
		});
		mInfinity.Init(mList.Count);
		mInfinity.updatecallback = UpdateChildCallBack;
		InitUI();
		OnLanguageChange();
	}

	private void InitUI()
	{
		mInfinity.SetItemCount(mList.Count);
		mInfinity.Refresh();
	}

	private void UpdateChildCallBack(int index, AchieveOneCtrl one)
	{
		one.Init(index, mList[index]);
	}

	private void OnLanguageChange()
	{
	}
}
