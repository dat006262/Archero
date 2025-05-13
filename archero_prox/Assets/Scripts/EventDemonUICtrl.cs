




using Dxx.Util;
using PureMVC.Interfaces;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EventDemonUICtrl : MediatorCtrlBase
{
	public Text texttitle;

	public Text texttitle2;

	public ButtonCtrl buttonok;

	public Text textok;

	public ButtonCtrl buttoncancel;

	public Text textcancel;

	public Text text_content1;

	public Text text_content2;

	public Image image_1;

	public Image image_2;

	private int mLoseid;

	private int mGetid;

	private int mFormid;

	protected override void OnInit()
	{
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		GameLogic.Hold.Sound.PlayUI(1000004);
		buttonok.onClick = OnClickOK;
		buttoncancel.onClick = OnClickCanccel;
		GameLogic.SetPause(pause: true);
		int num = GameLogic.Random(0, 100);
		bool flag = num < 40;
		Room_eventdemontext2lose room_eventdemontext2lose = null;
		int key = mFormid = GameLogic.Release.Form.GetRandomID("DemonSkill");
		Room_eventdemontext2skill beanById = LocalModelManager.Instance.Room_eventdemontext2skill.GetBeanById(key);
		int num2 = GameLogic.Random(0, beanById.Loses.Length);
		int num3 = beanById.Loses[num2];
		room_eventdemontext2lose = LocalModelManager.Instance.Room_eventdemontext2lose.GetBeanById(beanById.Loses[num2]);
		mGetid = beanById.GetID;
		text_content2.text = Utils.FormatString("{0} : {1}", GameLogic.Hold.Language.GetLanguageByTID("获得技能"), GameLogic.Hold.Language.GetSkillName(mGetid));
		int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(mGetid).SkillIcon;
		image_2.sprite = SpriteManager.GetSkillIcon(skillIcon);
		image_2.GetComponent<RectTransform>().sizeDelta = new Vector2(120f, 120f);
		mLoseid = room_eventdemontext2lose.LoseID;
		Goods_food beanById2 = LocalModelManager.Instance.Goods_food.GetBeanById(mLoseid);
		Goods_goods.GoodData goodData = Goods_goods.GetGoodData(beanById2.Values[0]);
		string text = MathDxx.Abs((long)((float)(GameLogic.Self.m_EntityData.attribute.HPValue.ValueLong * goodData.value) / 10000f)).ToString();
		text_content1.text = Utils.FormatString("{0} {1} {2}", GameLogic.Hold.Language.GetLanguageByTID("失去"), text, GameLogic.Hold.Language.GetLanguageByTID(room_eventdemontext2lose.Content1));
	}

	private void OnClickOK()
	{
		GameLogic.Self.AddSkill(mGetid);
		GameLogic.Release.Form.RemoveID("DemonSkill", mFormid);
		CInstance<TipsManager>.Instance.ShowSkill(mGetid);
		GameLogic.Self.GetGoods(mLoseid);
		WindowUI.CloseWindow(WindowID.WindowID_EventDemon);
	}

	private void OnClickCanccel()
	{
		WindowUI.CloseWindow(WindowID.WindowID_EventDemon);
	}

	protected override void OnClose()
	{
		GameLogic.SetPause(pause: false);
		GameLogic.Release.Mode.RoomGenerate.EventClose(new RoomGenerateBase.EventCloseTransfer
		{
			windowid = WindowID.WindowID_EventDemon
		});
		if ((bool)GameLogic.Self && GameLogic.Self.OnMissDemon != null)
		{
			GameLogic.Self.OnMissDemon();
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
		texttitle.text = GameLogic.Hold.Language.GetLanguageByTID("恶魔房标题");
		texttitle2.text = GameLogic.Hold.Language.GetLanguageByTID("恶魔房标题2");
		textok.text = GameLogic.Hold.Language.GetLanguageByTID("签订");
		textcancel.text = GameLogic.Hold.Language.GetLanguageByTID("拒绝");
	}
}
