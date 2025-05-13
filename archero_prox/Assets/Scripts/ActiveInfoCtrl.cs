




using TableTool;
using UnityEngine;

public class ActiveInfoCtrl : MonoBehaviour
{
	public Transform diffparent;

	public GameObject copyDiff;

	private LocalUnityObjctPool mObjPool;

	private void Awake()
	{
		mObjPool = LocalUnityObjctPool.Create(base.gameObject);
		mObjPool.CreateCache<ActiveDiffCtrl>(copyDiff);
	}

	public void Init(Stage_Level_activityModel.ActivityTypeData one)
	{
		mObjPool.Collect<ActiveDiffCtrl>();
		int count = one.mIds.Count;
		for (int i = 0; i < count; i++)
		{
			ActiveDiffCtrl activeDiffCtrl = mObjPool.DeQueue<ActiveDiffCtrl>();
			activeDiffCtrl.Init(i, one.GetData(i));
			RectTransform rectTransform = activeDiffCtrl.transform as RectTransform;
			rectTransform.SetParentNormal(diffparent);
			rectTransform.anchoredPosition = new Vector2(-240f + 170f * (float)i, 0f);
		}
	}
}
