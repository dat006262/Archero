




using System;
using UnityEngine;

public class AIBabyBase : AIBase
{
	public const float NearRange = 1f;

	protected EntityCallBase baby;

	protected int AttackID;

	protected EntityBase mParent;

	private float fardis = 5f;

	private string getactionname(string name)
	{
		return "AI2012 " + name;
	}

	protected override void OnInitOnce()
	{
		CInstance<BattleResourceCreator>.Instance.GetFootCircle(m_Entity.transform);
		IsDelayTime = false;
		AttackID = m_Entity.m_Data.WeaponID;
		m_Entity.ChangeWeapon(AttackID);
		base.OnInitOnce();
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGoToNextRoom));
	}

	protected override void OnInit()
	{
		baby = (m_Entity as EntityCallBase);
		mParent = baby.GetParent();
		AddAction(GetAILogic());
	}

	protected override void OnAIDeInit()
	{
		base.OnAIDeInit();
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGoToNextRoom));
	}

	protected virtual ActionBase GetAILogic()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		actionSequence.name = getactionname("seq1");
		ActionBase actionBase = new AIMove1010(m_Entity, baby.GetParent(), fardis);
		actionBase.name = getactionname("move1010");
		actionBase.ConditionBase = (() => GameLogic.Release.Mode.RoomGenerate.IsDoorOpen() ? true : false);
		actionSequence.AddAction(actionBase);
		actionSequence.AddAction(GetAttackOrMove());
		return actionSequence;
	}

	private ActionBase GetAttackOrMove()
	{
		ActionChooseIf actionChooseIf = new ActionChooseIf();
		actionChooseIf.name = getactionname("chooseif2");
		if (AttackID > 0 && GameLogic.Hold.BattleData.Challenge_AttackEnable())
		{
			OnAddAttack(actionChooseIf);
		}
		ActionBase babyMove = GetBabyMove();
		actionChooseIf.AddAction(babyMove);
		return actionChooseIf;
	}

	protected virtual void OnAddAttack(ActionChooseIf choose)
	{
		ActionSequence actionSequence = new ActionSequence();
		getactionname("attack seq");
		actionSequence.m_Entity = m_Entity;
		actionSequence.ConditionBase = FindTarget;
		ActionBase actionAttack = GetActionAttack(getactionname("attack1"), AttackID);
		actionAttack.ConditionBase = FindTarget;
		actionSequence.AddAction(actionAttack);
		actionSequence.AddAction(actionAttack);
		actionSequence.AddAction(actionAttack);
		AIMoveBabyMoveParent aIMoveBabyMoveParent = new AIMoveBabyMoveParent(m_Entity, mParent, 4);
		aIMoveBabyMoveParent.name = getactionname("moveparent");
		actionSequence.AddAction(GetActionWait(string.Empty, 100));
		actionSequence.AddAction(aIMoveBabyMoveParent);
		choose.AddAction(actionSequence);
	}

	protected bool GetFar()
	{
		if (!mParent)
		{
			return false;
		}
		if (!mParent.m_MoveCtrl.GetMoving())
		{
			return false;
		}
		return Vector3.Distance(m_Entity.position, mParent.position) > fardis;
	}

	protected ActionBase GetBabyMove()
	{
		ActionBase actionBase = new AIMoveBabyNormal(m_Entity, 1000, 2000, fardis);
		actionBase.name = getactionname("babynormal");
		return actionBase;
	}

	protected bool FindTarget()
	{
		EntityBase nearEntity = GameLogic.Release.Entity.GetNearEntity(m_Entity, 2.14748365E+09f, sameteam: false);
		m_Entity.m_HatredTarget = nearEntity;
		return nearEntity != null;
	}

	private void OnGoToNextRoom(RoomGenerateBase.Room room)
	{
	}
}
