




using Dxx.Util;
using UnityEngine;

public class AIMove1005 : AIMoveBase
{
	private float starttime;

	private float Move_BackTime = 1f;

	private bool bBack;

	protected float Move_NextX;

	protected float Move_NextY;

	protected float Move_NextDurationTime = 2f;

	protected virtual float moveRatio => 14f;

	public AIMove1005(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		bBack = false;
		starttime = Updater.AliveTime;
		if (CheckEnd())
		{
			End();
		}
		else
		{
			Move2Player();
		}
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

	private void MoveNormal()
	{
		if (Updater.AliveTime - starttime < Move_BackTime)
		{
			if (!m_Entity.m_MoveCtrl.GetMoving())
			{
				m_MoveData.angle = Utils.getAngle(Move_NextX, Move_NextY);
				m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY);
				m_MoveData._moveDirection = new Vector3(0f - Move_NextX, 0f, 0f - Move_NextY) * 0.3f;
				m_Entity.mAniCtrlBase.SetAnimationValue("Run", "Skill");
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Run", -0.5f);
				m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
				m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
			}
			else
			{
				AIMoving();
			}
		}
		else if (Updater.AliveTime - starttime < Move_NextDurationTime)
		{
			if (!bBack)
			{
				Move2Player();
				m_MoveData._moveDirection = Vector3.zero;
				m_Entity.mAniCtrlBase.SetAnimationValue("Run", "Run");
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Run", 0.5f);
				m_Entity.m_AniCtrl.SendEvent("Run", force: true);
				m_Entity.SetCollider(enable: true);
				bBack = true;
			}
			Move2Player();
			AIMoving();
		}
		else
		{
			End();
		}
	}

	private void AIMoveStart()
	{
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void AIMoving()
	{
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
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
		m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY) * 2f;
	}

	protected override void OnEnd()
	{
		m_Entity.SetCollider(enable: false);
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}

	private bool CheckEnd()
	{
		return m_Entity.m_HatredTarget == null;
	}
}
