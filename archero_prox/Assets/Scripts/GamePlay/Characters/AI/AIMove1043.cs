




using Dxx.Util;
using UnityEngine;

public class AIMove1043 : AIMoveBase
{
	private float Move_NextTime;

	protected float Move_NextX;

	protected float Move_NextY;

	private bool isStart;

	protected float time;

	private float movetime;

	public AIMove1043(EntityBase entity, int time)
		: base(entity)
	{
		this.time = (float)time / 1000f;
	}

	protected override void OnInitBase()
	{
		movetime = 0f;
		isStart = false;
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
		Move_NextTime = Updater.AliveTime;
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		if (Updater.AliveTime >= Move_NextTime && Updater.AliveTime < Move_NextTime + time)
		{
			if (!isStart)
			{
				AIMoveStart();
				isStart = true;
			}
			movetime += Updater.delta;
			if (movetime >= 0.3f)
			{
				movetime -= 0.3f;
				GameLogic.Release.Bullet.CreateBullet(m_Entity, 5046, m_Entity.position + new Vector3(0f, 2f, 0f), GameLogic.Random(0f, 360f));
			}
			AIMoving();
		}
		else if (Updater.AliveTime >= Move_NextTime + time)
		{
			AIMoveEnd();
		}
	}

	private void AIMoveStart()
	{
		m_MoveData.angle = Utils.getAngle(Move_NextX, Move_NextY);
		m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY);
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
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
		m_Entity.m_AniCtrl.SendEvent("Idle", force: true);
	}
}
