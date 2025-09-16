




using Dxx.Util;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeInfoCtrl : MonoBehaviour
{
	public Text Text_Title;

	public Text Text_SuccessContent;

	public Text Text_Success;

	public Text Text_RewardContent;

	public Text Text_RewardCount;

	public Image Image_RewardIcon;

	public Text Text_ChallengeButton;

	public ButtonCtrl Button_Challenge;

	public ChallengeConditionUICtrl mConditionUICtrl;

	private int m_ID;

	private void Awake()
	{
		Button_Challenge.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_Challenge);
			GameLogic.Hold.BattleData.Challenge_UpdateMode(m_ID);
			GameLogic.Hold.Sound.PlayUI(1000003);
			WindowUI.ShowWindow(WindowID.WindowID_Battle);
		};
	}

	public void Init(int id)
	{
		m_ID = id;
		GameLogic.Hold.BattleData.Challenge_Init(m_ID);
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("Challenge_Title", (m_ID - 2100).ToString());
		Text_Success.text = GameLogic.Hold.BattleData.Challenge_GetSuccessString();
		mConditionUICtrl.Init();
		int[] reward = GameLogic.Hold.BattleData.ActiveData.Reward;
		if (reward.Length < 3)
		{
			SdkManager.Bugly_Report("ChallengeUICtrl", Utils.FormatString("ActiveData[{0}] reward.length < 3", GameLogic.Hold.BattleData.ActiveData.ID));
		}
		switch (reward[0])
		{
		case 1:
		{
			FoodOneType foodOneType = (FoodOneType)reward[1];
			Text_RewardCount.text = Utils.FormatString("{0}", reward[2]);
			break;
		}
		case 3:
		{
			int equipIcon = LocalModelManager.Instance.Equip_equip.GetBeanById(reward[1]).EquipIcon;
			Text_RewardCount.text = Utils.FormatString("{0}个", reward[2]);
			Image_RewardIcon.sprite = SpriteManager.GetEquip(equipIcon);
			break;
		}
		case 5:
		{
			int groupID = LocalModelManager.Instance.Skill_slotout.GetBeanById(reward[1]).GroupID;
			Text_RewardCount.text = Utils.FormatString("{0}个", reward[2]);
			Image_RewardIcon.sprite = SpriteManager.GetCard(groupID);
			break;
		}
		}
		Text_SuccessContent.text = GameLogic.Hold.Language.GetLanguageByTID("Challenge_Success");
		Text_RewardContent.text = GameLogic.Hold.Language.GetLanguageByTID("Challenge_Reward");
		Text_ChallengeButton.text = GameLogic.Hold.Language.GetLanguageByTID("Challenge_Button");
	}
}
