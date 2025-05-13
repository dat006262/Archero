




using Dxx.Util;
using UnityEngine;

public class AIMove1052 : AIMoveBase
{
	private int range;

	private Vector3 endpos;

	public AIMove1052(EntityBase entity, int range)
		: base(entity)
	{
		this.range = range;
	}

	protected override void OnInitBase()
	{
		GameLogic.Release.MapCreatorCtrl.RandomFly(m_Entity, range, out float endx, out float endz);
		endpos = new Vector3(endx, 0f, endz);
		UpdateDirection();
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
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
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	protected override void OnUpdate()
	{
		if ((endpos - m_Entity.position).magnitude > 0.2f)
		{
			UpdateDirection();
			m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
		}
		else
		{
			End();
		}
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
