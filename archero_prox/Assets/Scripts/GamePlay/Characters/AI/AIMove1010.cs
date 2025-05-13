




using Dxx.Util;
using UnityEngine;

public class AIMove1010 : AIMoveBase
{
	private EntityBase parent;

	protected float Move_NextX;

	protected float Move_NextY;

	private bool isStart;

	private float speed;

	private int index;

	private int checkindex;

	private float fardis;

	public AIMove1010(EntityBase entity, EntityBase parent, float fardis)
		: base(entity)
	{
		this.parent = parent;
		this.fardis = fardis;
	}

	protected override void OnInitBase()
	{
		if ((bool)parent && !parent.m_MoveCtrl.GetMoving() && Vector3.Distance(parent.position, m_Entity.position) < fardis + 2f)
		{
			End();
			return;
		}
		float num = m_Entity.m_EntityData.GetSpeed();
		float num2 = parent.m_EntityData.GetSpeed();
		speed = num2 / num * 1.5f;
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		AIMoving();
		if (GetNear())
		{
			End();
		}
	}

	private bool GetNear()
	{
		if (!parent)
		{
			return false;
		}
		return Vector3.Distance(m_Entity.position, parent.position) < fardis / 2f;
	}

	private void AIMoveStart()
	{
		UpdateMoveDirection();
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void UpdateMoveDirection()
	{
		EntityBase entityBase = parent;
		if ((bool)entityBase)
		{
			Vector3 position = entityBase.position;
			float x = position.x;
			Vector3 position2 = m_Entity.position;
			float x2 = x - position2.x;
			Vector3 position3 = entityBase.position;
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
			m_MoveData.angle = Utils.getAngle(Move_NextX, Move_NextY);
			m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY) * speed;
			m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
		}
	}

	private void AIMoving()
	{
		UpdateMoveDirection();
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
	}

	private void AIMoveEnd()
	{
		End();
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
		m_Entity.m_AniCtrl.SendEvent("Idle", force: true);
	}
}
