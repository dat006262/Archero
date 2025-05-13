




using Dxx.Util;
using UnityEngine;

public class AIMove1006 : AIMoveBase
{
	protected float Move_NextX;

	protected float Move_NextY;

	private int min;

	private int max;

	public AIMove1006(EntityBase entity, int min, int max)
		: base(entity)
	{
		this.min = min;
		this.max = max;
	}

	protected override void OnInitBase()
	{
		SetHatred();
		if (CheckEnd())
		{
			End();
			return;
		}
		Move2Player();
		AIMoveStart();
		ConditionBase conditionRandomTime = AIMoveBase.GetConditionRandomTime(min, max);
		ConditionUpdate = conditionRandomTime.IsEnd;
	}

	protected virtual void SetHatred()
	{
	}

	protected override void OnUpdate()
	{
		if (CheckEnd())
		{
			End();
		}
		else
		{
			MoveNormal();
		}
	}

	private void Move2Player()
	{
		Vector3 position = m_Entity.m_HatredTarget.position;
		float x = position.x;
		Vector3 position2 = m_Entity.position;
		float x2 = x - position2.x;
		Vector3 position3 = m_Entity.m_HatredTarget.position;
		float z = position3.z;
		Vector3 position4 = m_Entity.position;
		Vector2 vector = new Vector2(x2, z - position4.z);
		Vector2 normalized = vector.normalized;
		Move_NextX = normalized.x;
		Move_NextY = normalized.y;
		if (normalized == Vector2.zero)
		{
			Vector2 normalized2 = new Vector2(GameLogic.Random(-1f, 1f), GameLogic.Random(-1f, 1f)).normalized;
			Move_NextX = normalized2.x;
			Move_NextY = normalized2.y;
		}
	}

	private void MoveNormal()
	{
		Move2Player();
		UpdateMoveData();
		AIMoving();
	}

	private void AIMoveStart()
	{
		UpdateMoveData();
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void UpdateMoveData()
	{
		m_MoveData.angle = Utils.getAngle(Move_NextX, Move_NextY);
		m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY);
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	private void AIMoving()
	{
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
	}

	private void AIMoveEnd()
	{
		End();
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}

	private bool CheckEnd()
	{
		return m_Entity.m_HatredTarget == null;
	}
}
