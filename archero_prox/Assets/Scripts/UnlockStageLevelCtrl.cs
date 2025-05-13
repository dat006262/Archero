




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class UnlockStageLevelCtrl : MonoBehaviour
{
	public GameObject stageparent;

	private GameObject stageitem;

	public void Init(int stage)
	{
		if (stageitem != null)
		{
			UnityEngine.Object.Destroy(stageitem);
		}
		stageitem = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/MainUI/Stage/Stage{0:D2}", stage)));
		stageitem.SetParentNormal(stageparent);
		Image[] componentsInChildren = stageitem.GetComponentsInChildren<Image>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].material = null;
		}
	}
}
