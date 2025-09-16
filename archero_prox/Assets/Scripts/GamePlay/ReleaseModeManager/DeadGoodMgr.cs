




using System.Collections.Generic;
using UnityEngine;

public class DeadGoodMgr
{
	private List<DeadGoodCtrl> mList = new List<DeadGoodCtrl>();

	public void Init()
	{
	}

	public void DeInit()
	{
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			mList[i].DeInit();
		}
		mList.Clear();
	}

	public void StartDrop(Vector3 pos, List<BattleDropData> goodslist, int radius, Transform MapGoodsDrop)
	{
		DeadGoodCtrl ctrl = new DeadGoodCtrl();
		ctrl.Init();
		ctrl.StartDrop(pos, goodslist, radius, MapGoodsDrop, delegate
		{
			ctrl.DeInit();
			if (mList.Contains(ctrl))
			{
				mList.Remove(ctrl);
			}
		});
		mList.Add(ctrl);
	}
}
