




using Dxx.Util;
using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EventAngelUICtrl : MediatorCtrlBase
{
	private struct GetData
	{
		public int eventID;

		public int getid;

		public int formid;
	}

	public Text texttitle;

	public Text texttitle2;

	public ButtonCtrl buttonok1;

	public ButtonCtrl buttonok2;

	public List<Text> text_content;

	public List<Image> image;

	private const int ChooseCount = 2;

	private GetData mData = default(GetData);

	private int mRecoverHPId = 1100001;

	protected override void OnInit()
	{
		buttonok1.onClick = OnClickOK1;
		buttonok2.onClick = OnClickOK2;
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		GameLogic.Hold.Sound.PlayUI(1000004);
		GameLogic.SetPause(pause: true);
		InitSkill();
		image[1].sprite = SpriteManager.GetSkillIcon(mRecoverHPId);
		text_content[1].text = GameLogic.Hold.Language.GetSkillName(mRecoverHPId);
	}

	private void InitSkill()
	{
		int randomID = GameLogic.Release.Form.GetRandomID("AngelSkill");
		Room_eventangelskill beanById = LocalModelManager.Instance.Room_eventangelskill.GetBeanById(randomID);
		text_content[0].text = GameLogic.Hold.Language.GetSkillName(beanById.GetID);
		int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(beanById.GetID).SkillIcon;
		Sprite skillIcon2 = SpriteManager.GetSkillIcon(skillIcon);
		if (skillIcon2 != null)
		{
			image[0].sprite = skillIcon2;
		}
		mData.getid = beanById.GetID;
		mData.eventID = beanById.EventID;
		mData.formid = randomID;
	}

	private void OnClickOK1()
	{
		GameLogic.Self.AddSkill(mData.getid);
		GameLogic.Release.Form.RemoveID("AngelSkill", mData.formid);
		CInstance<TipsManager>.Instance.ShowSkill(mData.getid);
		WindowUI.CloseWindow(WindowID.WindowID_EventAngel);
	}

	private void OnClickOK2()
	{
		int num = 40;
		if (GameLogic.Random(0f, 1f) < GameLogic.Self.m_EntityData.attribute.AngelR2Rate.Value)
		{
			num *= 2;
		}
		GameLogic.Self.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + {1}", "HPRecoverFixed%", num));
		CInstance<TipsManager>.Instance.ShowSkill(mRecoverHPId);
		WindowUI.CloseWindow(WindowID.WindowID_EventAngel);
	}

	protected override void OnClose()
	{
		GameLogic.SetPause(pause: false);
		GameLogic.Release.Mode.RoomGenerate.EventClose(new RoomGenerateBase.EventCloseTransfer
		{
			windowid = WindowID.WindowID_EventAngel
		});
		if ((bool)GameLogic.Self && GameLogic.Self.OnMissAngel != null)
		{
			GameLogic.Self.OnMissAngel();
		}
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	public override void OnLanguageChange()
	{
		texttitle.text = GameLogic.Hold.Language.GetLanguageByTID("天使房标题");
		texttitle2.text = GameLogic.Hold.Language.GetLanguageByTID("天使房标题2");
	}
}
