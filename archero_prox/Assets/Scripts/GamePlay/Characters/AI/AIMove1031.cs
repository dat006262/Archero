




using Dxx.Util;
using UnityEngine;

public class AIMove1031 : AIMoveBase
{
	private EntityBase target;

	public float peradd = 1f;

	private float maxadd = 50f;

	private float offsetangle;

	private int state;

	private float flytime;

	private float startflytime;

	public AIMove1031(EntityBase entity, float flytime = -1f)
		: base(entity)
	{
		this.flytime = flytime;
	}

	protected override void OnInitBase()
	{
		state = GameLogic.Random(0, 2);
		startflytime = Updater.AliveTime;
		target = GameLogic.Self;
		UpdateDirection();
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void UpdateDirection()
	{
		Vector3 position = target.position;
		float x = position.x;
		Vector3 position2 = m_Entity.position;
		float x2 = x - position2.x;
		Vector3 position3 = target.position;
		float z = position3.z;
		Vector3 position4 = m_Entity.position;
		float y = z - position4.z;
		float angle = Utils.getAngle(x2, y);
		if (state == 0)
		{
			offsetangle += peradd;
			if (MathDxx.Abs(offsetangle) >= maxadd)
			{
				state = 1;
			}
		}
		else
		{
			offsetangle -= peradd;
			if (MathDxx.Abs(offsetangle) >= maxadd)
			{
				state = 0;
			}
		}
		m_MoveData.angle = angle + offsetangle;
		m_MoveData.direction = new Vector3(MathDxx.Sin(m_MoveData.angle), 0f, MathDxx.Cos(m_MoveData.angle));
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	protected override void OnUpdate()
	{
		UpdateDirection();
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
		if (flytime > 0f && Updater.AliveTime - startflytime > flytime)
		{
			End();
		}
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
