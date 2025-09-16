




using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class EventChest1TurnCtrl : MonoBehaviour
{
	private const int TableCount = 6;

	public Action<TurnTableType> TurnEnd;

	public Transform child;

	public Transform arrow;

	public List<EventChest1OneCtrl> mList;

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

	private TurnTableData resultData;

	private List<TurnTableData> list = new List<TurnTableData>();

	private int playCount;

	private ActionBasic action = new ActionBasic();

	private Drop_DropModel.DropData[] equips;

	public void Init()
	{
		bStart = true;
		bDelayTurnEnd = false;
		starttime = Time.unscaledTime;
		speed = -20f;
		speedtime = UnityEngine.Random.Range(0.9f, 1.1f);
		child.localRotation = Quaternion.Euler(0f, 0f, 0f);
		offset = 5f;
		rotateangle = 0f;
	}

	public void DeInit()
	{
		action.DeInit();
	}

	public void InitGood(Drop_DropModel.DropData[] equips)
	{
		this.equips = equips;
		if ((bool)GameLogic.Self)
		{
			playCount = 0;
			list.Clear();
			action.Init(IgnoreTimeScale: true);
			for (int i = 0; i < 3; i++)
			{
				TurnTableData turnTableData = new TurnTableData();
				turnTableData.type = TurnTableType.Boss;
				list.Add(turnTableData);
			}
			for (int j = 0; j < 1; j++)
			{
				TurnTableData turnTableData2 = new TurnTableData();
				turnTableData2.type = TurnTableType.BigEquip;
				turnTableData2.value = equips[1];
				list.Add(turnTableData2);
			}
			for (int k = 0; k < 1; k++)
			{
				TurnTableData turnTableData3 = new TurnTableData();
				turnTableData3.type = TurnTableType.SmallEquip;
				turnTableData3.value = equips[0];
				list.Add(turnTableData3);
			}
			for (int l = 0; l < 1; l++)
			{
				TurnTableData turnTableData4 = new TurnTableData();
				turnTableData4.type = TurnTableType.Hitted;
				list.Add(turnTableData4);
			}
			list.RandomSort();
			int m = 0;
			for (int count = list.Count; m < count; m++)
			{
				mList[m].Init(list[m]);
			}
		}
	}

	private void CheckResult()
	{
		EventChest1OneCtrl eventChest1OneCtrl = mList[0];
		int num = 0;
		for (int i = 1; i < 6; i++)
		{
			EventChest1OneCtrl eventChest1OneCtrl2 = mList[i];
			Vector3 eulerAngles = eventChest1OneCtrl2.transform.eulerAngles;
			float minAngle = GetMinAngle(eulerAngles.z);
			Vector3 eulerAngles2 = eventChest1OneCtrl.transform.eulerAngles;
			if (minAngle < GetMinAngle(eulerAngles2.z))
			{
				num = i;
				eventChest1OneCtrl = eventChest1OneCtrl2;
			}
		}
		resultData = eventChest1OneCtrl.mData;
		Debugger.Log(Utils.FormatString("result : {0} {1} {2}", num, resultData.type, (resultData.value != null) ? resultData.value.ToString() : string.Empty));
		string empty = string.Empty;
		string empty2 = string.Empty;
		switch (resultData.type)
		{
		case TurnTableType.Boss:
			GameLogic.Release.Mode.RoomGenerate.SendEvent("Event_TurnTable_Boss");
			break;
		case TurnTableType.Hitted:
			if ((bool)GameLogic.Self)
			{
				float num2 = GameLogic.Self.m_EntityData.MaxHP;
				int num3 = (int)((0f - num2) * 0.3f);
				GameLogic.Self.ChangeHP(null, num3);
			}
			break;
		}
		if (empty != string.Empty)
		{
			CInstance<TipsManager>.Instance.Show(empty, empty2);
		}
		playCount++;
		resultData.type = TurnTableType.Get;
		mList[num].Init(resultData);
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
			TurnEnd(resultData.type);
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
				speed *= 0.97f;
				offset *= 0.97f;
			}
			else if (num < 3f)
			{
				speed *= 0.98f;
				offset *= 0.98f;
			}
			else
			{
				speed *= 0.99f;
				offset *= 0.99f;
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
