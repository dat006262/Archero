




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class MainUILevelItem : MonoBehaviour
{
	public ButtonCtrl Button_Click;

	public GameObject stageparent;

	public Action OnButtonClick;

	private int stageId = 1;

	private GameObject stageitem;

	private Stage_Level_stagechapter beanData;

	private long mCount;

	public int StageID => StageID;

	private void Awake()
	{
		Button_Click.onClick = delegate
		{
			if (OnButtonClick != null)
			{
				OnButtonClick();
			}
		};
	}

	public void Init(int stageId)
	{
		this.stageId = stageId;
		beanData = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(stageId);
		InitStage();
	}

	private void InitStage()
	{
		stageparent.transform.DestroyChildren();
		if ((bool)stageitem)
		{
			UnityEngine.Object.Destroy(stageitem);
		}
		try
		{
			string text = Utils.FormatString("UIPanel/MainUI/Stage/Stage{0:D2}", stageId);
			GameObject gameObject = ResourceManager.Load<GameObject>(text);
			if (gameObject != null)
			{
				stageitem = UnityEngine.Object.Instantiate(gameObject);
				stageitem.SetParentNormal(stageparent);
			}
			else
			{
				SdkManager.Bugly_Report("MainUILevelitem", "error1", Utils.FormatString("path:[{0}] ResourceManager.Load is null", text));
			}
		}
		catch
		{
			SdkManager.Bugly_Report("MainUILevelitem", "error2", Utils.FormatString("Create StageItem erro stageId : {0}", stageId));
		}
	}
}
