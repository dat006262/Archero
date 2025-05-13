




using Dxx.Util;
using UnityEngine;

public class AIMove1001 : AIMoveBase
{
	private float startx;

	private float endx = -1f;

	public AIMove1001(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		m_MoveData.angle = Utils.getAngle(GameLogic.Self.position - m_Entity.position);
		float x = MathDxx.Sin(m_MoveData.angle);
		float z = MathDxx.Cos(m_MoveData.angle);
		m_MoveData.direction = new Vector3(x, 0f, z);
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
		Vector3 localPosition = m_Entity.transform.localPosition;
		startx = localPosition.x;
	}

	protected override void OnUpdate()
	{
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
		Vector3 localPosition = m_Entity.transform.localPosition;
		float x = localPosition.x;
		if (x < endx)
		{
			x = endx;
		}
		float d = (x - endx) / (startx - endx);
		m_MoveData.direction = m_MoveData.direction.normalized * d;
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
