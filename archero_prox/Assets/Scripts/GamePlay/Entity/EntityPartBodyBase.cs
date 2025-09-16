




using System;

public class EntityPartBodyBase : EntityCallBase
{
	protected ActionBasic action = new ActionBasic();

	private ConditionTime mCondition;

	public Action<int> OnRemoveEvent;

	public bool bGotoRoomRemove;

	protected override string ModelPath => "Game/PartBody/PartBody";

	protected override void OnInit()
	{
		base.OnInit();
		m_MoveCtrl = new MoveControl();
		m_AttackCtrl = new AttackControl();
		m_MoveCtrl.Init(this);
		m_AttackCtrl.Init(this);
		GameLogic.Release.Entity.SetPartBody(this);
		m_EntityData.Modify_Invincible(value: true);
		SetCollider(enable: false);
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
	}

	protected override void StartInit()
	{
		base.StartInit();
		ShowHP(show: false);
		PlayEffect(3100008, m_Body.EffectMask.transform.position);
	}

	protected override void OnDeInitLogic()
	{
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		if (OnRemoveEvent != null)
		{
			OnRemoveEvent(m_Data.CharID);
			OnRemoveEvent = null;
		}
		if (!bGotoRoomRemove)
		{
			GameLogic.PlayEffect(3100008, base.position);
		}
		base.OnDeInitLogic();
	}

	public void SetAliveTime(float time)
	{
		mCondition = new ConditionTime
		{
			time = time
		};
	}

	protected override void UpdateProcess(float delta)
	{
		base.UpdateProcess(delta);
		if (GetIsDead())
		{
			return;
		}
		m_AttackCtrl.UpdateProgress();
		if (mCondition != null && mCondition.IsEnd())
		{
			mCondition = null;
			if (OnRemoveEvent != null)
			{
				OnRemoveEvent(m_Data.CharID);
				OnRemoveEvent = null;
			}
			GameLogic.Release.Entity.RemovePartBody(this);
		}
	}

	protected override void UpdateFixed()
	{
		if (!GetIsDead())
		{
			m_MoveCtrl.UpdateProgress();
		}
	}

	private void OnGotoNextRoom(RoomGenerateBase.Room room)
	{
		OnGotoNextRooms(room);
	}

	protected virtual void OnGotoNextRooms(RoomGenerateBase.Room room)
	{
		GameLogic.Release.Entity.RemovePartBody(this, gotonextroom: true);
	}
}
