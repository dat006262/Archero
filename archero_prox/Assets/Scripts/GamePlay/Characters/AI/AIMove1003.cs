




using Dxx.Util;
using UnityEngine;

public class AIMove1003 : AIMoveBase
{
	private EntityCallBase partbody;

	private EntityBase parentEntity;

	private Vector3 endpos;

	public AIMove1003(EntityCallBase entity)
		: base(entity)
	{
		ConditionUpdate = (() => m_Entity.m_HatredTarget != null);
		partbody = entity;
		parentEntity = partbody.GetParent();
	}

	protected override void OnInitBase()
	{
		UpdateDirection();
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		UpdateEndPostion();
		UpdateDirection();
		AIMoving();
	}

	private void AIMoveStart()
	{
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void UpdateEndPostion()
	{
		endpos = parentEntity.GetRotateFollowPosition(m_Entity);
	}

	private void UpdateDirection()
	{
		float x = endpos.x;
		Vector3 position = m_Entity.position;
		float x2 = x - position.x;
		float z = endpos.z;
		Vector3 position2 = m_Entity.position;
		float num = z - position2.z;
		m_MoveData.angle = Utils.getAngle(x2, num);
		m_MoveData.direction = new Vector3(x2, 0f, num).normalized;
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
}
