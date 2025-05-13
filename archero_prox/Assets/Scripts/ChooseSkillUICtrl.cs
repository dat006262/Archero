




using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSkillUICtrl : MediatorCtrlBase
{
	public GameObject cantclickObj;

	public List<Text> skillnameList;

	public Text Text_Level;

	public Text Text_Content;

	public GameObject levelparent;

	public Animator Ani_bg;

	public Animator Ani_skill;

	public Animator Ani_level;

	public Animator Ani_content;

	public List<ButtonCtrl> skillbutton;

	public List<ChooseSkillButtonCtrl> chooseskillbutton;

	public List<ChooseSkillOneCtrl> chooseones;

	public List<ChooseSkillColumnCtrl> columns;

	private int level;

	private ChooseSkillProxy.Transfer mTransfer;

	protected override void OnInit()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				ButtonCtrl buttonCtrl = skillbutton[i];
				ChooseSkillButtonCtrl @object = chooseskillbutton[i];
				buttonCtrl.onClick = @object.OnClick;
			}
		}
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("ChooseSkillProxy");
		if (proxy != null)
		{
			mTransfer = (proxy.Data as ChooseSkillProxy.Transfer);
			GameLogic.SetPause(pause: true);
			InitUI();
		}
	}

	private void InitUI()
	{
		if (!GameLogic.Self)
		{
			return;
		}
		Ani_bg.enabled = true;
		Ani_skill.enabled = true;
		Ani_level.enabled = true;
		Ani_content.enabled = true;
		level = GameLogic.Self.GetLearnSkillCount() + 2;
		GameLogic.Release.Game.JoyEnable(enable: false);
		List<int> list = null;
		if (LocalSave.Instance.BattleIn_GetIn())
		{
			list = LocalSave.Instance.BattleIn_GetLevelUpSkills();
		}
		else
		{
			list = GetSkill9();
			LocalSave.Instance.BattleIn_UpdateLevelUpSkills((int)mTransfer.type, list);
		}
		int num = 10;
		int num2 = 0;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				chooseones[num2].Init(list[i * 3 + j], skillnameList[i]);
				num2++;
			}
			columns[i].Init(num, skillnameList[i]);
			num += 5;
		}
		for (int k = 0; k < 3; k++)
		{
			skillnameList[k].text = string.Empty;
		}
		cantclickObj.SetActive(value: true);
	}

	private List<int> GetSkill9()
	{
		switch (mTransfer.type)
		{
		case ChooseSkillProxy.ChooseSkillType.eLevel:
			return GameLogic.Self.GetSkill9();
		case ChooseSkillProxy.ChooseSkillType.eFirst:
			return GameLogic.Self.GetFirstSkill9();
		default:
			return null;
		}
	}

	private void OnSkillActionEnd()
	{
		cantclickObj.SetActive(value: false);
	}

	protected void AniDisable()
	{
		Ani_bg.enabled = false;
		Ani_skill.enabled = false;
		Ani_level.enabled = false;
		Ani_content.enabled = false;
	}

	protected override void OnClose()
	{
		if (mTransfer == null)
		{
			return;
		}
		GameLogic.SetPause(pause: false);
		AniDisable();
		GameLogic.Release.Game.JoyEnable(enable: true);
		Facade.Instance.SendNotification("BATTLE_CHOOSESKILL_TO_BATTLE_CLOSE");
		if (mTransfer.type == ChooseSkillProxy.ChooseSkillType.eLevel)
		{
			if ((bool)GameLogic.Self && GameLogic.Self.OnLevelUp != null)
			{
				GameLogic.Self.OnLevelUp(level);
			}
			if (GameLogic.Release.Mode != null && GameLogic.Release.Mode.RoomGenerate != null)
			{
				GameLogic.Release.Mode.RoomGenerate.EventClose(new RoomGenerateBase.EventCloseTransfer
				{
					windowid = WindowID.WindowID_ChooseSkill
				});
			}
		}
		mTransfer = null;
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
		if (name == null)
		{
			return;
		}
		if (!(name == "BATTLE_CHOOSESKILL_ACTION_END"))
		{
			if (name == "BATTLE_CHOOSESKILL_SKILL_CHOOSE")
			{
				LocalSave.Instance.BattleIn_UpdateLevelUpSkills(0, null);
				int skillId = (int)body;
				GameLogic.Self.AddSkill(skillId);
				CInstance<TipsManager>.Instance.ShowSkill(skillId);
				WindowUI.CloseWindow(WindowID.WindowID_ChooseSkill);
			}
		}
		else
		{
			OnSkillActionEnd();
		}
	}

	public override void OnLanguageChange()
	{
		if (mTransfer == null)
		{
			return;
		}
		switch (mTransfer.type)
		{
		case ChooseSkillProxy.ChooseSkillType.eLevel:
			if (GameLogic.Self.m_EntityData.IsMaxLevel())
			{
				Text_Level.text = GameLogic.Hold.Language.GetLanguageByTID("LevelUpSkill_MaxLevel");
			}
			else
			{
				Text_Level.text = GameLogic.Hold.Language.GetLanguageByTID("LevelUpSkill_升到了", level.ToString());
			}
			Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("LevelUpSkill_选择");
			break;
		case ChooseSkillProxy.ChooseSkillType.eFirst:
			Text_Level.text = GameLogic.Hold.Language.GetLanguageByTID("LevelUpSkill_初始");
			Text_Content.text = string.Empty;
			break;
		}
	}
}
