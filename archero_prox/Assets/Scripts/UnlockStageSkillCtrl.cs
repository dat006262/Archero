




using DG.Tweening;
using Dxx.Util;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class UnlockStageSkillCtrl : MonoBehaviour
{
	public GameObject child;

	public GameObject copyitem;

	public Text Text_SkillContent;

	private const int LineCount = 5;

	private const float WidthOne = 145f;

	private const float HeightOne = 145f;

	private LocalUnityObjctPool mPool;

	private void Awake()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<UnlockStageSkillOneCtrl>(copyitem);
		copyitem.SetActive(value: false);
	}

	public void Init(Sequence seq, int stage)
	{
		mPool.Collect<UnlockStageSkillOneCtrl>();
		List<int> list = LocalModelManager.Instance.Skill_slotin.GetSkillsByStage(stage);
		int count = list.Count;
		count = MathDxx.Clamp(count, 0, 5);
		float startx = (float)(-(count - 1)) * 145f / 2f;
		float num = (float)list.Count * 0.1f;
		int i = 0;
		UnlockStageSkillOneCtrl ctrl;
		for (int count2 = list.Count; i < count2; i++)
		{
			int index = i;
			seq.AppendCallback(delegate
			{
				ctrl = mPool.DeQueue<UnlockStageSkillOneCtrl>();
				ctrl.transform.SetParentNormal(child);
				Vector3 a = new Vector3(startx + (float)(index % 5 * 145), -(index / 5) * 145, 0f);
				ctrl.transform.localPosition = a - new Vector3(0f, 50f, 0f);
				ctrl.Init(list[index]);
				Sequence s = DOTween.Sequence();
				ctrl.mCanvasGroup.alpha = 0f;
				s.Append(ctrl.mCanvasGroup.DOFade(1f, 0.2f));
				s.Join(ctrl.transform.DOLocalMoveY(a.y, 0.2f));
			});
			seq.AppendInterval(0.05f);
		}
		Text_SkillContent.text = GameLogic.Hold.Language.GetLanguageByTID("UnlockStage_Skill");
	}

	public void DeInit()
	{
		mPool.Collect<UnlockStageSkillOneCtrl>();
	}

	public int GetUnlockSkillCount(int stage)
	{
		return LocalModelManager.Instance.Skill_slotin.GetSkillsByStage(stage).Count;
	}
}
