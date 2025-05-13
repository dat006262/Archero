




using Dxx.Util;
using UnityEngine;

public class AIMove1009 : AIMoveBase
{
	private float starttime;

	protected float Move_NextDurationTime = 1.2f;

	protected float Move_BackTime = 0.5f;

	protected bool bBack;

	protected string runString = "Run";

	protected float runAniSpeed = 0.5f;

	protected float Move_NextX;

	protected float Move_NextY;

	protected virtual float moveRatio => 14f;

	public AIMove1009(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		bBack = false;
		starttime = Updater.AliveTime;
		SetHatred();
		if (CheckEnd())
		{
			End();
		}
		else
		{
			Move2Player();
		}
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

	private void MoveNormal()
	{
		if (Updater.AliveTime - starttime < Move_BackTime)
		{
			if (!m_Entity.m_MoveCtrl.GetMoving())
			{
				m_MoveData.angle = Utils.getAngle(Move_NextX, Move_NextY);
				m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY) * 2f;
				m_MoveData._moveDirection = new Vector3(0f - Move_NextX, 0f, 0f - Move_NextY);
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
				m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY) * moveRatio;
				m_MoveData._moveDirection = Vector3.zero;
				m_Entity.mAniCtrlBase.SetAnimationValue("Run", runString);
				m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Run", runAniSpeed);
				m_Entity.m_AniCtrl.SendEvent("Run", force: true);
				bBack = true;
				OnBackEvent();
			}
			AIMoving();
			OnSprintUpdate();
		}
		else
		{
			AIMoveEnd();
		}
	}

	private void AIMoveStart()
	{
		m_MoveData.angle = Utils.getAngle(Move_NextX, Move_NextY);
		m_MoveData.direction = new Vector3(Move_NextX, 0f, Move_NextY) * moveRatio;
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

	protected virtual void OnSprintUpdate()
	{
	}

	private void Move2Player()
	{
		Vector3 position = m_Entity.m_HatredTarget.position;
		float x = position.x;
		Vector3 position2 = m_Entity.position;
		float x2 = x - position2.x + GameLogic.Random(-2f, 2f);
		Vector3 position3 = m_Entity.m_HatredTarget.position;
		float z = position3.z;
		Vector3 position4 = m_Entity.position;
		Vector2 vector = new Vector2(x2, z - position4.z + GameLogic.Random(-2f, 2f));
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

	protected virtual void OnBackEvent()
	{
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
		m_Entity.mAniCtrlBase.SetAnimationValue("Run", "Run");
	}

	private bool CheckEnd()
	{
		return m_Entity.m_HatredTarget == null;
	}
}
