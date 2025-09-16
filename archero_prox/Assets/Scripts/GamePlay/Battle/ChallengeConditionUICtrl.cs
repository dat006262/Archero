




using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeConditionUICtrl : MonoBehaviour
{
	public GameObject child;

	public GameObject copyitem;

	public Text Text_ConditionContent;

	private LocalUnityObjctPool mPool;

	private List<Text> mList = new List<Text>();

	private void Awake()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<ChallengeConditionOneCtrl>(copyitem);
	}

	public void Init()
	{
		Text_ConditionContent.text = GameLogic.Hold.Language.GetLanguageByTID("Challenge_Condition");
		mPool.Collect<ChallengeConditionOneCtrl>();
		List<string> list = GameLogic.Hold.BattleData.Challenge_GetConditions();
		bool flag = list.Count > 0;
		child.SetActive(flag);
		if (flag)
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				ChallengeConditionOneCtrl challengeConditionOneCtrl = mPool.DeQueue<ChallengeConditionOneCtrl>();
				challengeConditionOneCtrl.transform.SetParentNormal(child);
				challengeConditionOneCtrl.transform.localPosition = new Vector3(0f, i * -40, 0f);
				challengeConditionOneCtrl.Init(list[i]);
			}
		}
	}
}
