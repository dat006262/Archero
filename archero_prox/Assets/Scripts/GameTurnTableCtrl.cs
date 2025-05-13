




using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class GameTurnTableCtrl : MonoBehaviour
{
	private const int TableCount = 6;

	public Action<TurnTableData> TurnEnd;

	public Transform child;

	public Transform arrow;

	public List<GameTurnTableOneCtrl> mList;

	private const float Speed = -20f;

	private float speed;

	private float speedtime;

	private float starttime;

	private bool bStart;

	private bool bDelayTurnEnd;

	private float turnendstarttime;

	private float turnendupdatetime = 0.4f;

	private float offset = 5f;

	private float rotateangle;

	private SequencePool mSeqPool = new SequencePool();

	private TurnTableData resultData;

	private TurnTableData resultGet = new TurnTableData
	{
		type = TurnTableType.Get
	};

	private List<TurnTableData> list = new List<TurnTableData>();

	private int playCount;

	private ActionBasic action = new ActionBasic();

	public void Init()
	{
		bStart = true;
		bDelayTurnEnd = false;
		starttime = Time.unscaledTime;
		speed = -20f;
		speedtime = UnityEngine.Random.Range(0.6f, 0.9f);
		child.localRotation = Quaternion.Euler(0f, 0f, 0f);
		offset = 5f;
		rotateangle = 0f;
	}

	public void DeInit()
	{
		action.DeInit();
		mSeqPool.Clear();
	}

	public void InitGood(List<TurnTableData> list)
	{
		if ((bool)GameLogic.Self)
		{
			playCount = 0;
			this.list = list;
			action.Init(IgnoreTimeScale: true);
			list.RandomSort();
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				mList[i].Init(list[i]);
			}
		}
	}

	private void CheckResult()
	{
		GameTurnTableOneCtrl gameTurnTableOneCtrl = mList[0];
		int index = 0;
		for (int i = 1; i < 6; i++)
		{
			GameTurnTableOneCtrl gameTurnTableOneCtrl2 = mList[i];
			Vector3 eulerAngles = gameTurnTableOneCtrl2.transform.eulerAngles;
			float minAngle = GetMinAngle(eulerAngles.z);
			Vector3 eulerAngles2 = gameTurnTableOneCtrl.transform.eulerAngles;
			if (minAngle < GetMinAngle(eulerAngles2.z))
			{
				index = i;
				gameTurnTableOneCtrl = gameTurnTableOneCtrl2;
			}
		}
		resultData = gameTurnTableOneCtrl.mData;
		string text = string.Empty;
		string value = string.Empty;
		switch (resultData.type)
		{
		case TurnTableType.ExpBig:
		case TurnTableType.ExpSmall:
			GameLogic.Release.Mode.CreateGoods(GameLogic.Self.position, GameLogic.GetExpList((int)resultData.value), 2);
			break;
		case TurnTableType.HPAdd:
		{
			int skillId = (int)resultData.value;
			GameLogic.Self.AddSkillAttribute(skillId);
			text = GameLogic.Hold.Language.GetSkillName(skillId);
			value = GameLogic.Hold.Language.GetSkillContent(skillId);
			break;
		}
		case TurnTableType.Empty:
			text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_EmptyTitle");
			value = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_EmptyContent");
			break;
		case TurnTableType.Gold:
		{
			long num3 = (long)resultData.value;
			text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_GoldTitle");
			value = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_GoldContent", num3);
			if (num3 <= 0)
			{
				break;
			}
			IMediator mediator = Facade.Instance.RetrieveMediator("BattleModuleMediator");
			if (mediator != null)
			{
				object @event = mediator.GetEvent("Event_GetGoldPosition");
				if (@event != null && @event is Vector3)
				{
					Vector3 endpos = (Vector3)@event;
					CurrencyFlyCtrl.PlayGet(CurrencyType.BattleGold, num3, new Vector3((float)GameLogic.Width / 2f, (float)GameLogic.Height / 2f, 0f), endpos, delegate(long onecount)
					{
						GameLogic.Hold.BattleData.AddGold(onecount);
					}, null, mask: false);
				}
				else
				{
					GameLogic.Hold.BattleData.AddGold(num3);
				}
			}
			break;
		}
		case TurnTableType.Diamond:
		{
			long num2 = (long)resultData.value;
			text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_DiamondTitle");
			value = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_DiamondContent", num2);
			CReqItemPacket itemPacket = NetManager.GetItemPacket(null);
			itemPacket.m_nPacketType = 1;
			itemPacket.m_nDiamondAmount = (uint)num2;
			LocalSave.Instance.Modify_Diamond(num2, updateui: false);
			NetManager.SendInternal(itemPacket, SendType.eUDP, delegate(NetResponse response)
			{
				if (!response.IsSuccess)
				{
				}
			});
			CurrencyFlyCtrl.PlayGet(CurrencyType.Diamond, num2, new Vector3((float)GameLogic.Width / 2f, (float)GameLogic.Height / 2f, 0f), delegate
			{
			}, delegate
			{
			}, mask: false);
			break;
		}
		case TurnTableType.PlayerSkill:
		{
			int num = (int)resultData.value;
			GameLogic.Self.LearnExtraSkill(num);
			text = GameLogic.Hold.Language.GetSkillName(num);
			value = GameLogic.Hold.Language.GetSkillContent(num);
			break;
		}
		case TurnTableType.EventSkill:
		{
			Room_eventgameturn beanById = LocalModelManager.Instance.Room_eventgameturn.GetBeanById((int)resultData.value);
			int getID = beanById.GetID;
			GameLogic.Self.AddSkill(getID);
			text = GameLogic.Hold.Language.GetSkillName(getID);
			value = GameLogic.Hold.Language.GetSkillContent(getID);
			break;
		}
		case TurnTableType.Reward_Gold2:
		case TurnTableType.Reward_Gold3:
		case TurnTableType.Reward_Item2:
		case TurnTableType.Reward_Item3:
		case TurnTableType.Reward_All2:
		case TurnTableType.Reward_All3:
			GameLogic.Hold.BattleData.SetRewardType(resultData.type);
			break;
		}
		if (text != string.Empty)
		{
			CInstance<TipsManager>.Instance.Show(text, value);
		}
		playCount++;
		mList[index].Init(resultGet);
		if (playCount <= GameLogic.Self.m_EntityData.TurnTableCount)
		{
			bDelayTurnEnd = false;
			action.AddActionIgnoreWaitDelegate(0.4f, delegate
			{
				Init();
			});
		}
	}

	private float GetMinAngle(float angle)
	{
		float num = Mathf.Abs(angle);
		float num2 = Mathf.Abs(angle + 360f);
		if (num > num2)
		{
			num = num2;
		}
		num2 = Mathf.Abs(angle - 360f);
		if (num > num2)
		{
			num = num2;
		}
		return num;
	}

	private void Update()
	{
		if (bStart)
		{
			RotateAction();
		}
		if (bDelayTurnEnd && Time.unscaledTime - turnendstarttime > turnendupdatetime)
		{
			TurnEnd(resultData);
			bDelayTurnEnd = false;
		}
	}

	private void RotateAction()
	{
		bool flag = false;
		if (Time.unscaledTime - starttime < speedtime)
		{
			Transform transform = child;
			Vector3 localEulerAngles = child.localEulerAngles;
			transform.localRotation = Quaternion.Euler(0f, 0f, localEulerAngles.z + speed);
			flag = true;
		}
		else
		{
			flag = true;
			float num = Mathf.Abs(speed);
			if (num < 0.5f)
			{
				speed *= 0.961f;
				offset *= 0.961f;
			}
			else if (num < 3f)
			{
				speed *= 0.971f;
				offset *= 0.971f;
			}
			else
			{
				speed *= 0.981f;
				offset *= 0.981f;
			}
			Transform transform2 = child;
			Vector3 localEulerAngles2 = child.localEulerAngles;
			transform2.localRotation = Quaternion.Euler(0f, 0f, localEulerAngles2.z + speed);
			if (Mathf.Abs(speed) < 0.2f)
			{
				bStart = false;
				flag = false;
				bDelayTurnEnd = true;
				turnendstarttime = Time.unscaledTime;
				CheckResult();
			}
		}
		rotateangle -= speed;
		if (rotateangle >= 60f)
		{
			rotateangle -= 60f;
			GameLogic.Hold.Sound.PlayUI(1000005);
		}
		if (flag)
		{
			child.localPosition = GetRandomOffset();
			arrow.localPosition = GetRandomOffset() * 0.5f;
		}
	}

	private Vector3 GetRandomOffset()
	{
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
			return new Vector3(0f - offset, 0f - offset, 0f);
		case 1:
			return new Vector3(0f - offset, offset, 0f);
		case 2:
			return new Vector3(offset, 0f - offset, 0f);
		case 3:
			return new Vector3(offset, offset, 0f);
		default:
			return Vector3.zero;
		}
	}
}
