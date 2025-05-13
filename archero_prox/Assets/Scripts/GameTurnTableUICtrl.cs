




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class GameTurnTableUICtrl : MediatorCtrlBase
{
	public GameObject gameturnparent;

	public ButtonCtrl Button_Start;

	public GameTurnTableCtrl mTurnCtrl;

	public Text Text_Title;

	public Text Text_Start;

	public AdTurnTableCtrl mAdTurnCtrl;

	private TurnTableType resultType;

	private SequencePool mSeqPool = new SequencePool();

	private float adx;

	private bool show_currency;

	protected override void OnInit()
	{
		Vector3 localPosition = mAdTurnCtrl.transform.localPosition;
		adx = localPosition.x;
		mTurnCtrl.TurnEnd = delegate(TurnTableData data)
		{
			resultType = data.type;
			if (LocalSave.Instance.BattleAd_CanShow())
			{
				SdkManager.send_event_ad(ADSource.eTurntable, "SHOW", 0, 0, string.Empty, string.Empty);
				mSeqPool.Get().SetUpdate(isIndependentUpdate: true).AppendInterval(0.5f)
					.Append(gameturnparent.transform.DOLocalMoveX(0f - adx, 0.5f).SetEase(Ease.OutBack))
					.AppendInterval(0.5f)
					.AppendCallback(delegate
					{
						WindowUI.ShowCurrency(WindowID.WindowID_Currency);
						show_currency = true;
						mAdTurnCtrl.show_close(value: true);
					});
			}
			else
			{
				WindowUI.CloseWindow(WindowID.WindowID_GameTurnTable);
			}
		};
		RectTransform rectTransform = base.transform as RectTransform;
		RectTransform rectTransform2 = gameturnparent.transform as RectTransform;
		rectTransform2.sizeDelta = rectTransform.sizeDelta;
		RectTransform rectTransform3 = mAdTurnCtrl.transform as RectTransform;
		rectTransform3.sizeDelta = rectTransform.sizeDelta;
		mAdTurnCtrl.Init();
		mAdTurnCtrl.onClickClose = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_GameTurnTable);
		};
	}

	protected override void OnOpen()
	{
		show_currency = false;
		gameturnparent.transform.localPosition = Vector3.zero;
		GameLogic.Hold.Sound.PlayUI(1000004);
		GameLogic.SetPause(pause: true);
		Button_Start.onClick = delegate
		{
			Button_Start.gameObject.SetActive(value: false);
			mTurnCtrl.Init();
		};
		Button_Start.gameObject.SetActive(value: true);
		InitUI();
		mAdTurnCtrl.Open();
	}

	private void InitUI()
	{
		List<TurnTableData> list = new List<TurnTableData>();
		TurnTableData turnTableData = new TurnTableData();
		turnTableData.type = TurnTableType.ExpBig;
		int exp = LocalModelManager.Instance.Exp_exp.GetBeanById(GameLogic.Self.m_EntityData.GetLevel()).Exp;
		int num = (int)GameLogic.Random((float)exp * 0.4f, (float)exp * 0.6f);
		turnTableData.value = num;
		list.Add(turnTableData);
		for (int i = 0; i < 2; i++)
		{
			TurnTableData turnTableData2 = new TurnTableData();
			turnTableData2.type = TurnTableType.ExpSmall;
			int exp2 = LocalModelManager.Instance.Exp_exp.GetBeanById(GameLogic.Self.m_EntityData.GetLevel()).Exp;
			int num2 = (int)GameLogic.Random((float)exp2 * 0.1f, (float)exp2 * 0.3f);
			turnTableData2.value = num2;
			list.Add(turnTableData2);
		}
		TurnTableData turnTableData3 = new TurnTableData();
		turnTableData3.type = TurnTableType.HPAdd;
		list.Add(turnTableData3);
		turnTableData3.value = 1100001;
		TurnTableData turnTableData4 = new TurnTableData();
		turnTableData4.type = TurnTableType.PlayerSkill;
		int randomSkill = GameLogic.Self.GetRandomSkill();
		for (int j = 0; j < 100; j++)
		{
			if (!GetRerandom(randomSkill))
			{
				break;
			}
			randomSkill = GameLogic.Self.GetRandomSkill();
		}
		turnTableData4.value = randomSkill;
		list.Add(turnTableData4);
		TurnTableData turnTableData5 = new TurnTableData();
		turnTableData5.type = TurnTableType.EventSkill;
		turnTableData5.value = GameLogic.Release.Form.GetRandomID("GameTurntable");
		list.Add(turnTableData5);
		mTurnCtrl.InitGood(list);
	}

	private bool GetRerandom(int skillid)
	{
		if (LocalModelManager.Instance.Skill_slotin.IsWeaponSkillID(skillid))
		{
			return true;
		}
		Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillid);
		if (beanById == null)
		{
			return true;
		}
		if (ContainsArrow(beanById.Attributes))
		{
			return true;
		}
		return false;
	}

	private bool ContainsArrow(string[] attrs)
	{
		int i = 0;
		for (int num = attrs.Length; i < num; i++)
		{
			Goods_goods.GoodData goodData = Goods_goods.GetGoodData(attrs[i]);
			switch (goodData.goodType)
			{
			case "BulletForward":
			case "BulletBackward":
			case "BulletContinue":
			case "BulletForSide":
			case "BulletSide":
				return true;
			}
		}
		return false;
	}

	protected override void OnClose()
	{
		if (show_currency)
		{
			show_currency = false;
			WindowUI.CloseCurrency();
		}
		mAdTurnCtrl.Deinit();
		mSeqPool.Clear();
		GameLogic.SetPause(pause: false);
		GameLogic.Release.Mode.RoomGenerate.EventClose(new RoomGenerateBase.EventCloseTransfer
		{
			windowid = WindowID.WindowID_GameTurnTable,
			data = resultType
		});
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_Title");
		Text_Start.text = GameLogic.Hold.Language.GetLanguageByTID("开始");
		mAdTurnCtrl.OnLanguageChange();
	}
}
