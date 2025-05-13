




using Dxx.Util;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ActiveOneCtrl : MonoBehaviour
{
	public Text Text_Name;

	public Image Image_Icon;

	public Text Text_Count;

	public Stage_Level_activityModel.ActivityTypeData activedata
	{
		get;
		private set;
	}

	public void Init(Stage_Level_activityModel.ActivityTypeData one)
	{
		activedata = one;
		Text_Name.text = activedata.GetData(0).Notes;
		int activeCount = LocalSave.Instance.GetActiveCount(activedata.index);
		Text_Count.text = Utils.FormatString("剩余次数：{0}", activeCount);
	}
}
