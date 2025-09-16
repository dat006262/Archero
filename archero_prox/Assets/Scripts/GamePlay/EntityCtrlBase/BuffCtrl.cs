




using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;

public class BuffCtrl : EntityCtrlBase
{
	private Dictionary<int, BuffAloneBase> mBuffs = new Dictionary<int, BuffAloneBase>();

	private List<BuffAloneBase> mOverBuffs = new List<BuffAloneBase>();

	private List<int> removeList = new List<int>();

	private BuffAloneBase data1;

	private BuffAloneBase data2;

	private int mBuffsID;

	public override void OnStart(List<EBattleAction> actIds)
	{
		actIds.Add(EBattleAction.EBattle_Action_Add_Buff);
		actIds.Add(EBattleAction.EBattle_Action_Remove_Buff);
		SetUseUpdate();
	}

	public override void ExcuteCommend(EBattleAction id, object action)
	{
		switch (id)
		{
		case EBattleAction.EBattle_Action_Add_Buff:
			AddBuff((BattleStruct.BuffStruct)action);
			break;
		case EBattleAction.EBattle_Action_Remove_Buff:
			RemoveBuff((BattleStruct.BuffStruct)action);
			break;
		}
	}

	private void AddBuff(BattleStruct.BuffStruct data)
	{
		Buff_alone beanById = LocalModelManager.Instance.Buff_alone.GetBeanById(data.buffId);
		int buffID = beanById.BuffID;
		if (beanById.OverType == 0)
		{
			if (!mBuffs.ContainsKey(buffID))
			{
				mBuffs.Add(buffID, getBuff(data, beanById));
			}
			else
			{
				mBuffs[buffID].ResetBuffTime();
			}
		}
		else
		{
			mOverBuffs.Add(getBuff(data, beanById));
		}
	}

	private void RemoveBuff(BattleStruct.BuffStruct data)
	{
		Buff_alone beanById = LocalModelManager.Instance.Buff_alone.GetBeanById(data.buffId);
		RemoveBuff(beanById.BuffID);
	}

	private BuffAloneBase getBuff(BattleStruct.BuffStruct data, Buff_alone buff_alone)
	{
		int buffID = buff_alone.BuffID;
		BuffAloneBase buffAloneBase = null;
		Type type = Type.GetType(Utils.GetString("BuffAlone", buffID));
		buffAloneBase = ((!(type != null)) ? new BuffAloneBase() : (type.Assembly.CreateInstance(Utils.GetString("BuffAlone", buffID)) as BuffAloneBase));
		if (buffAloneBase != null)
		{
			buffAloneBase.Init(m_Entity, data, LocalModelManager.Instance.Buff_alone.GetBeanById(buffID));
		}
		else
		{
			SdkManager.Bugly_Report("BuffCtrl.cs", Utils.GetString("Buff ", buffID, " dont have"));
		}
		return buffAloneBase;
	}

	private void RemoveBuff(int buffId)
	{
		if (mBuffs.TryGetValue(buffId, out BuffAloneBase value))
		{
			value?.Remove();
			mBuffs.Remove(buffId);
		}
	}

	public override void OnUpdate(float delta)
	{
		removeList.Clear();
		List<int> list = new List<int>(mBuffs.Keys);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			mBuffsID = list[i];
			if (mBuffs.TryGetValue(mBuffsID, out data1) && data1 != null)
			{
				data1.OnUpdate(delta);
				if (data1.IsEnd())
				{
					removeList.Add(mBuffsID);
				}
			}
		}
		int j = 0;
		for (int count2 = removeList.Count; j < count2; j++)
		{
			RemoveBuff(removeList[j]);
		}
		for (int num = mOverBuffs.Count - 1; num >= 0; num--)
		{
			data2 = mOverBuffs[num];
			data2.OnUpdate(delta);
			if (mOverBuffs.Count == 0)
			{
				break;
			}
			if (data2.IsEnd())
			{
				data2.Remove();
				mOverBuffs.Remove(data2);
			}
		}
	}

	public override void OnRemove()
	{
		Dictionary<int, BuffAloneBase>.Enumerator enumerator = mBuffs.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.Remove();
		}
		mBuffs.Clear();
		int i = 0;
		for (int count = mOverBuffs.Count; i < count; i++)
		{
			mOverBuffs[i].Remove();
		}
		mOverBuffs.Clear();
	}
}
