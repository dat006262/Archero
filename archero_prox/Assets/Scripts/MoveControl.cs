




using Dxx.Util;
using System;
using UnityEngine;

//@TODO Show info
//[Serializable]
public class MoveControl
{
	public enum EMoveDirection
	{
		Down,
		Up,
		Left,
		Right
	}

	protected EntityBase m_Entity;

	[SerializeField]
	private bool bMoveing;

	private bool bTouchMove;

	protected JoyData m_JoyData;

	private bool bRegister;

	private float TouchStartTime;

	private Vector3 LastFramePosition;

	[SerializeField]
	protected Vector3 MoveDirection = Vector3.zero;

	private float Moving_angle;

	private Vector3 Moving_dir = default(Vector3);

	private Vector3 Update_Speed;

	public void Init(EntityBase entity)
	{
		m_Entity = entity;
		AddMoveSpeedUpdate();
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	private void AddMoveSpeedUpdate()
	{
		EntityAttributeBase attribute = m_Entity.m_EntityData.attribute;
		attribute.OnMoveSpeedUpdate = (Action)Delegate.Combine(attribute.OnMoveSpeedUpdate, new Action(OnMoveSpeedUpdate));
	}

	public void Start()
	{
		RegisterJoyEvent();
	}

	public void DeInit()
	{
		RemoveJoyEvent();
	}

	public void RegisterJoyEvent()
	{
		if (!bRegister && m_Entity.Type == EntityType.Hero)
		{
			ScrollCircle.On_JoyTouchStart += OnMoveStart;
			ScrollCircle.On_JoyTouching += OnMoving;
			ScrollCircle.On_JoyTouchEnd += OnMoveEnd;
			bRegister = true;
		}
	}

	public void RemoveJoyEvent()
	{
		if (bRegister)
		{
			ScrollCircle.On_JoyTouchStart -= OnMoveStart;
			ScrollCircle.On_JoyTouching -= OnMoving;
			ScrollCircle.On_JoyTouchEnd -= OnMoveEnd;
			bRegister = false;
		}
	}

	protected virtual void OnMoveSpeedUpdate()
	{
		if (bTouchMove && m_Entity.State == EntityState.Normal)
		{
			if (m_Entity.IsSelf)
			{
				MoveDirection = m_JoyData.MoveDirection * m_Entity.m_EntityData.GetSpeed();
			}
			else if (m_JoyData._moveDirection == Vector3.zero)
			{
				Moving_angle = m_Entity.m_AttackCtrl.GetCurrentAngle();
				Moving_dir.x = MathDxx.Sin(Moving_angle);
				Moving_dir.z = MathDxx.Cos(Moving_angle);
				MoveDirection = m_Entity.m_EntityData.GetSpeed() * Moving_dir * m_JoyData.MoveDirection.magnitude;
			}
			else
			{
				MoveDirection = m_Entity.m_EntityData.GetSpeed() * m_JoyData.MoveDirection;
			}
		}
	}

	public void AIMoveStart(JoyData data)
	{
		if (!bMoveing)
		{
			OnMoveStart(data);
		}
	}

	public void OnMoveStart(JoyData data)
	{
		if (!m_Entity.GetIsDead() && data.name == "MoveJoy")
		{
			MoveStart(data.action);
		}
	}

	public void AIMoving(JoyData data)
	{
		if (bMoveing)
		{
			OnMoving(data);
		}
	}

	public void OnMoving(JoyData data)
	{
		if (!m_Entity.GetIsDead() && data.name == "MoveJoy")
		{
			if (!bMoveing && !m_Entity.m_AttackCtrl.GetAttacking())
			{
				OnMoveStart(data);
			}
			if (bMoveing && m_Entity.m_AttackCtrl.GetAttacking())
			{
				MoveEnd();
			}
			if (bMoveing)
			{
				Moving(data);
			}
		}
	}

	private void MoveStart(string action = "Run")
	{
		bMoveing = true;
		bTouchMove = true;
		if (m_Entity.OnMoveEvent != null)
		{
			m_Entity.OnMoveEvent(obj: true);
		}
		m_Entity.m_AniCtrl.SendEvent(action);
		if (m_Entity.IsSelf)
		{
			GameLogic.Release.Game.SetRunning();
			GameLogic.Hold.Sound.PlayWalk();
		}
		m_Entity.m_AniCtrl.SetBool("TouchMoveJoy", value: true);
		MoveStartVirtual();
	}

	protected virtual void MoveStartVirtual()
	{
	}

	private void Moving(JoyData data)
	{
		m_JoyData = data;
		m_Entity.m_AttackCtrl.RotateHero(data.angle);
		OnMoveSpeedUpdate();
		MovingVirtual(data);
	}

	public void AIMoveEnd(JoyData data)
	{
		if (bMoveing)
		{
			OnMoveEnd(data);
		}
	}

	private void OnMoveEnd(JoyData data)
	{
        Debug.Log("@LOG OnMoveEnd data.name:" + data.name);
        if (!m_Entity.GetIsDead() && data.name == "MoveJoy")
		{
			bTouchMove = false;
			if (m_Entity.OnMoveEvent != null)
			{
				m_Entity.OnMoveEvent(obj: false);
			}
			MoveEnd();
			if (m_Entity.IsSelf)
			{
				GameLogic.Hold.Sound.StopWalk();
			}
			m_Entity.m_AniCtrl.SetBool("TouchMoveJoy", value: false);
			ResetRigidBody();
			MoveEndVirtual();
		}
	}

	protected virtual void MoveEndVirtual()
	{
	}

	protected virtual void MovingVirtual(JoyData data)
	{
	}

	public void OnMoveEnd()
	{
		OnMoveEnd(m_JoyData);
	}

	public void MoveEnd()
	{
		if (bMoveing)
		{
			bMoveing = false;
			m_Entity.m_AniCtrl.SendEvent("Idle");
			m_Entity.m_AttackCtrl.MoveEndCallBack();
			ResetRigidBody();
		}
	}

	public void SetMoving(bool moving)
	{
		if (bMoveing && !moving && bTouchMove)
		{
			MoveEnd();
		}
		else if (!bMoveing && moving && bTouchMove)
		{
			MoveStart();
		}
	}

	public void SetMovingInternal(bool value)
	{
		bMoveing = value;
	}

	public bool GetMoving()
	{
		return bMoveing;
	}

	public void UpdateProgress()
	{
		if (!m_Entity || m_Entity.GetIsDead())
		{
			return;
		}
		if (m_Entity.State == EntityState.Hitted)
		{
			MoveDirection = m_Entity.GetHittedDirection() * m_Entity.HittedV;
		}
		if (GameLogic.Release != null && GameLogic.Release.Game != null && !(MoveDirection == Vector3.zero))
		{
			Update_Speed = MoveDirection * Time.deltaTime;
			m_Entity.SelfMoveBy(Update_Speed);
			if ((bool)m_Entity.m_Body)
			{
				m_Entity.m_Body.SetOrder();
			}
		}
	}

	public void ResetRigidBody()
	{
		MoveDirection = Vector3.zero;
	}
}
