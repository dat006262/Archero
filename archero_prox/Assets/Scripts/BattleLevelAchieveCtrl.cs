




using Dxx.Util;
using UnityEngine;

public class BattleLevelAchieveCtrl : MonoBehaviour
{
	public GameObject child;

	private BattleConditionUIBase mCondition;

	private bool bShow = true;

	public void Show(bool value)
	{
		if ((bool)child)
		{
			child.SetActive(value);
		}
		bShow = value;
		if (bShow)
		{
			if (mCondition != null)
			{
				UnityEngine.Object.Destroy(mCondition.gameObject);
			}
			LocalSave.AchieveDataOne achieveDataOne = LocalSave.Instance.Achieve_Get(GameLogic.Hold.BattleData.ActiveID);
			if (achieveDataOne == null)
			{
				SdkManager.Bugly_Report("BattleLevelAchieveCtrl", Utils.FormatString("Achieveid[{0}]  dont in achievelist!!!", GameLogic.Hold.BattleData.ActiveID));
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/BattleUI/condition/condition{0}", achieveDataOne.mData.CondType)));
			gameObject.SetParentNormal(child);
			mCondition = gameObject.GetComponent<BattleConditionUIBase>();
			mCondition.Init(achieveDataOne);
		}
	}

	private void Update()
	{
		if (bShow && (bool)mCondition)
		{
			mCondition.Refresh();
		}
	}
}
