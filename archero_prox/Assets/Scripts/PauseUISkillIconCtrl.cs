




using Dxx.Util;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class PauseUISkillIconCtrl : MonoBehaviour
{
	private Image image;

	private Text Text_SkillID;

	private void Awake()
	{
		image = base.transform.Find("child/Image").GetComponent<Image>();
		Text_SkillID = base.transform.Find("child/Text").GetComponent<Text>();
		Text_SkillID.gameObject.SetActive(value: false);
	}

	public void Init(int SkillID)
	{
		int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(SkillID).SkillIcon;
		if (skillIcon == 0)
		{
			SdkManager.Bugly_Report("PauseUISkillIconCtrl", Utils.FormatString("Init iconid == 0   skillid:{0}", SkillID));
		}
		image.sprite = SpriteManager.GetSkillIcon(skillIcon);
	}
}
