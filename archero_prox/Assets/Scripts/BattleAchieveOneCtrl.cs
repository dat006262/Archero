




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BattleAchieveOneCtrl : MonoBehaviour
{
	public ButtonCtrl mButton;

	public Text Text_Content;

	public Image Image_BG;

	public Image Image_Finish;

	public Achieve_Achieve mData;

	public Action<int> OnButtonClick;

	private void Awake()
	{
		mButton.onClick = delegate
		{
			if (!LocalSave.Instance.Achieve_IsFinish(mData.ID) && OnButtonClick != null)
			{
				OnButtonClick(mData.ID);
			}
		};
	}

	public void Init(int achieveid)
	{
		mData = LocalModelManager.Instance.Achieve_Achieve.GetBeanById(achieveid);
		if (LocalSave.Instance.Achieve_IsFinish(achieveid))
		{
			Image_BG.sprite = SpriteManager.GetUICommon("ButtonSmall_Green");
			Image_Finish.gameObject.SetActive(value: true);
		}
		else
		{
			Image_BG.sprite = SpriteManager.GetUICommon("ButtonSmall_Yellow");
			Image_Finish.gameObject.SetActive(value: false);
		}
		LocalSave.AchieveDataOne achieveDataOne = LocalSave.Instance.Achieve_Get(achieveid);
		Text_Content.text = Utils.FormatString("挑战{0}:{1}", mData.Index, achieveDataOne.mCondition.GetConditionString());
	}
}
