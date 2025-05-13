




using System.Collections.Generic;
using UnityEngine;

public class TipsManager : CInstance<TipsManager>
{
	private struct TipsData
	{
		public string value1;

		public string value2;
	}

	private int tipsCount;

	private Queue<TipsData> mCacheList = new Queue<TipsData>();

	private TipsCtrl Get()
	{
		GameObject gameObject = GameLogic.EffectGet("Game/UI/TipsOne");
		gameObject.transform.SetParent(GameNode.m_Tips);
		RectTransform rectTransform = gameObject.transform as RectTransform;
		rectTransform.localScale = Vector3.one;
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		return gameObject.GetComponent<TipsCtrl>();
	}

	public void Cache(GameObject o)
	{
		GameLogic.EffectCache(o);
	}

	public void CanShowNext()
	{
		if (tipsCount > 0)
		{
			tipsCount--;
			if (mCacheList.Count > 0)
			{
				TipsData tipsData = mCacheList.Dequeue();
				ShowMust(tipsData.value1, tipsData.value2);
			}
		}
	}

	public void Show(string value1, string value2 = "")
	{
		if (tipsCount == 0)
		{
			ShowMust(value1, value2);
		}
		else
		{
			mCacheList.Enqueue(new TipsData
			{
				value1 = value1,
				value2 = value2
			});
		}
		tipsCount++;
	}

	private void ShowMust(string value1, string value2)
	{
		TipsCtrl tipsCtrl = Get();
		tipsCtrl.Init(value1, value2);
	}

	public void ShowSkill(int skillId)
	{
		string skillName = GameLogic.Hold.Language.GetSkillName(skillId);
		string skillContent = GameLogic.Hold.Language.GetSkillContent(skillId);
		Show(skillName, skillContent);
	}

	public void Clear()
	{
		GameNode.m_Tips.DestroyChildren();
		tipsCount = 0;
		mCacheList.Clear();
	}
}
