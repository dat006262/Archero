




using Dxx.Util;
using UnityEngine;

public class AIMoveBabyNormal : AIMoveBase
{
	private EntityBase mParent;

	protected float Move_NextTime;

	protected float Move_NextDurationTime;

	protected float Move_NextX;

	protected float Move_NextY;

	private float Move_NextDurationTimeMin;

	private float Move_NextDurationTimeMax;

	private int min;

	private int max;

	private float fardis;

	public AIMoveBabyNormal(EntityBase entity, int min, int max, float fardis)
		: base(entity)
	{
		if (entity is EntityCallBase)
		{
			EntityCallBase entityCallBase = entity as EntityCallBase;
			if ((bool)entityCallBase)
			{
				mParent = entityCallBase.GetParent();
			}
		}
		this.min = min;
		if (max < min)
		{
			max = min;
		}
		this.max = max;
		this.fardis = fardis;
	}

	protected override void OnInitBase()
	{
		Move_NextDurationTimeMin = (float)min / 1000f;
		Move_NextDurationTimeMax = (float)max / 1000f;
		RandomNextMove();
	}

	protected override void OnUpdate()
	{
		if (!m_Entity.m_AttackCtrl.GetAttacking())
		{
			MoveNormal();
		}
	}

	private void MoveNormal()
	{
		if (Updater.AliveTime < Move_NextTime + Move_NextDurationTime)
		{
			if (!m_Entity.m_MoveCtrl.GetMoving())
			{
				AIMoveStart();
			}
			else
			{
				AIMoving();
			}
		}
		else
		{
			AIMoveEnd();
		}
	}

	private void AIMoveStart()
	{
		m_MoveData.angle = Utils.getAngle(Move_NextX, Move_NextY);
		m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY) * 0.1f;
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

	protected virtual void RandomNextMove()
	{
		if ((bool)mParent && Vector3.Distance(m_Entity.position, mParent.position) > fardis)
		{
			Move_NextTime = Updater.AliveTime;
			Move_NextDurationTime = Move_NextDurationTimeMax;
			float angle = Utils.getAngle(mParent.position - m_Entity.position);
			angle += GameLogic.Random(-15f, 15f);
			Move_NextX = MathDxx.Sin(angle);
			Move_NextY = MathDxx.Cos(angle);
		}
		else
		{
			int num = 0;
			RandomNextMoveOnce();
			while (!IsRandomValid() && num < 100)
			{
				RandomNextMoveOnce();
				num++;
			}
		}
	}

	private void RandomNextMoveOnce()
	{
		Move_NextTime = Updater.AliveTime;
		Move_NextDurationTime = GameLogic.Random(Move_NextDurationTimeMin, Move_NextDurationTimeMax);
		Vector2 normalized = new Vector2(GameLogic.Random(-1f, 1f), GameLogic.Random(-1f, 1f)).normalized;
		Move_NextX = normalized.x;
		Move_NextY = normalized.y;
	}

	protected bool IsRandomValid()
	{
		return true;
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
