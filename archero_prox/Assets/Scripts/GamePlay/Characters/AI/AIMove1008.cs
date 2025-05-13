




using Dxx.Util;
using UnityEngine;

public class AIMove1008 : AIMoveBase
{
	private float Move_NextTime;

	protected float Move_NextX;

	protected float Move_NextY;

	private bool isStart;

	protected float time;

	private float move2playertatio;

	private GameObject effect;

	protected virtual int MoveEffectID => 3100014;

	public AIMove1008(EntityBase entity, float move2playertatio, int time)
		: base(entity)
	{
		this.time = (float)time / 1000f;
		this.move2playertatio = move2playertatio;
	}

	protected override void OnInitBase()
	{
		m_Entity.SetSuperArmor(value: true);
		isStart = false;
		if (m_Entity.m_HatredTarget == null)
		{
			m_Entity.m_HatredTarget = GameLogic.Self;
		}
		Vector3 position = m_Entity.m_HatredTarget.position;
		float x = position.x;
		Vector3 position2 = m_Entity.position;
		float x2 = x - position2.x;
		Vector3 position3 = m_Entity.m_HatredTarget.position;
		float z = position3.z;
		Vector3 position4 = m_Entity.position;
		Vector2 vector = new Vector2(x2, z - position4.z);
		Vector2 normalized = vector.normalized;
		if (GameLogic.Random(0f, 1f) > move2playertatio)
		{
			float angle = GameLogic.Random(0f, 360f);
			normalized.x = MathDxx.Sin(angle);
			normalized.y = MathDxx.Cos(angle);
		}
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
			AIMoving();
		}
		else if (Updater.AliveTime >= Move_NextTime + time + 0.5f)
		{
			AIMoveEnd();
		}
	}

	private void AIMoveStart()
	{
		m_MoveData.angle = Utils.getAngle(Move_NextX, Move_NextY);
		m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY) * 2.8f;
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
		m_MoveData.action = "Continuous";
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
		CacheEffect();
		effect = m_Entity.GetEffect(MoveEffectID);
	}

	private void AIMoving()
	{
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
	}

	private void AIMoveEnd()
	{
		End();
	}

	private void CacheEffect()
	{
		if ((bool)effect)
		{
			GameLogic.EffectCache(effect);
			effect = null;
		}
	}

	protected override void OnEnd()
	{
		CacheEffect();
		m_Entity.SetSuperArmor(value: false);
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
		m_Entity.m_AniCtrl.SendEvent("Idle", force: true);
	}
}
