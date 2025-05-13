




using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeOneCtrl : MonoBehaviour
{
	public Text Text_ID;

	public GameObject dots;

	private int mIndex;

	private Stage_Level_activity mData;

	public void Init(int index, Stage_Level_activity data, int allcount)
	{
		mIndex = index;
		mData = data;
		Text_ID.text = (mData.ID - 2100).ToString();
		bool flag = index < allcount - 1;
		if (flag != dots.activeSelf)
		{
			dots.SetActive(flag);
		}
	}
}
