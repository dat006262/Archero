




using Dxx.Util;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class StageListOneCtrl : MonoBehaviour
{
	public GameObject commonparent;

	public GameObject commingparent;

	public Text Text_CommingSoon;

	public Text Text_Content;

	public Text Text_Stage;

	public Text Text_Info;

	public Text Text_Level;

	public GameObject stageparent;

	public StageListSkillsCtrl mSkillsCtrl;

	private GameObject stageitem;

	private int stageId;

	private bool bCommingSoon;

	public void Init(int stage, bool unlock)
	{
		mSkillsCtrl.gameObject.SetActive(value: false);
		int maxChapter_Hero = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter_Hero();
		Text_Content.text = string.Empty;
		bCommingSoon = (stage > maxChapter_Hero);
		commonparent.SetActive(!bCommingSoon);
		commingparent.SetActive(bCommingSoon);
		if (!bCommingSoon)
		{
			stageId = stage;
			Text_Stage.text = GameLogic.Hold.Language.GetLanguageByTID("ChapterIndex_x", stage);
			Text_Info.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("ChapterInfo_{0}", stage));
			int currentMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(stage);
			string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("stagelist_stagelength");
			Text_Level.text = Utils.FormatString("{0} : {1}", languageByTID, currentMaxLevel);
			List<int> skillsByStage = LocalModelManager.Instance.Skill_slotin.GetSkillsByStage(stage);
			InitStage();
		}
		else
		{
			Text_CommingSoon.color = LocalModelManager.Instance.Stage_Level_stagechapter.GetChapterColor(stage);
			if (maxChapter_Hero == 6)
			{
				Text_CommingSoon.text = GameLogic.Hold.Language.GetLanguageByTID("stagelist_hero");
			}
			else
			{
				Text_CommingSoon.text = GameLogic.Hold.Language.GetLanguageByTID("Main_CommingSoon");
			}
		}
	}

	private void InitStage()
	{
		if ((bool)stageitem)
		{
			UnityEngine.Object.Destroy(stageitem);
		}
		stageitem = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/MainUI/Stage/Stage{0:D2}", stageId)));
		stageitem.SetParentNormal(stageparent);
		Image[] componentsInChildren = stageitem.GetComponentsInChildren<Image>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].material = null;
		}
	}
}
