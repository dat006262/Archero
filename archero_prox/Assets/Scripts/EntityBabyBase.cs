




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class EntityBabyBase : EntityCallBase
{
	protected ActionBasic action = new ActionBasic();

	protected sealed override string ModelPath => "Game/Baby/Baby";

	public Character_Baby m_BabyData
	{
		get;
		private set;
	}

	protected override void OnInit()
	{
		base.OnInit();
		m_BabyData = LocalModelManager.Instance.Character_Baby.GetBeanById(ClassID.ToString());
		m_MoveCtrl = new MoveControl();
		InitAttackControl();
		m_MoveCtrl.Init(this);
		m_AttackCtrl.Init(this);
		GameLogic.Release.Entity.SetBaby(this);
		UpdateAttributes();
		bool babyResistBullet = m_Parent.m_EntityData.GetBabyResistBullet();
		SetCollider(babyResistBullet);
		ShowHP(show: false);
		RemoveColliders();
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
	}

	protected virtual void InitAttackControl()
	{
		m_AttackCtrl = new AttackControl();
	}

	protected override void OnDeInitLogic()
	{
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		base.OnDeInitLogic();
	}

	public override bool SetHitted(HittedData data)
	{
		return false;
	}

	protected override void UpdateProcess(float delta)
	{
		base.UpdateProcess(delta);
		if (!GetIsDead())
		{
			m_AttackCtrl.UpdateProgress();
		}
	}

	protected override void UpdateFixed()
	{
		if (!GetIsDead())
		{
			m_MoveCtrl.UpdateProgress();
		}
	}

	public void UpdateAttributes()
	{
		RemoveAlreadyAddAttributes();
		int i = 0;
		for (int count = m_Parent.m_EntityData.mBabyAttributes.Count; i < count; i++)
		{
			string text = m_Parent.m_EntityData.mBabyAttributes[i];
			m_EntityData.mSelfAttributes.Add(text);
			m_EntityData.ExcuteAttributes(text);
		}
	}

	private void RemoveAlreadyAddAttributes()
	{
		int i = 0;
		for (int count = m_EntityData.mSelfAttributes.Count; i < count; i++)
		{
			string str = m_EntityData.mSelfAttributes[i].Replace("+", "-");
			m_EntityData.ExcuteAttributes(str);
		}
		m_EntityData.mSelfAttributes.Clear();
	}

	public void UpdateSkillIds()
	{
		int i = 0;
		for (int count = m_Parent.m_EntityData.mBabySkillIds.Count; i < count; i++)
		{
			int skillId = m_Parent.m_EntityData.mBabySkillIds[i];
			AddSkill(skillId);
		}
	}

	protected virtual void OnGotoNextRoom(RoomGenerateBase.Room room)
	{
		float angle = GameLogic.Random(-80f, 80f);
		float x = MathDxx.Sin(angle);
		float z = MathDxx.Cos(angle);
		base.transform.position = m_Parent.position + new Vector3(x, 0f, z) * GameLogic.Random(0f, 1f);
	}
}
