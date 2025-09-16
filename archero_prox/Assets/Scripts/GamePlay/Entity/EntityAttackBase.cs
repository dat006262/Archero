




using Dxx.Util;
using System;
using UnityEngine;

public abstract class EntityAttackBase
{
	protected int AttackID;

	protected bool bEnd = true;

	protected EntityBase m_Entity;

	protected JoyData m_AttackData = default(JoyData);

	protected JoyData m_MoveData = default(JoyData);

	private bool bInit;

	protected Action OnUnInstall;

	protected bool bRotate = true;

	private bool bAddActionEnd;

	public void Init(EntityBase entity, int AttackID)
	{
		m_Entity = entity;
		m_AttackData.name = "AttackJoy";
		m_MoveData.name = "MoveJoy";
		this.AttackID = AttackID;
		OnEnable();
		OnInit();
		if ((bool)m_Entity && !bAddActionEnd)
		{
			bAddActionEnd = true;
			Debugger.Log(m_Entity, "EntityAttackBase bAddActionEnd true  have weapon = " + (m_Entity.m_Weapon != null));
			if (m_Entity.m_Weapon != null)
			{
				WeaponBase weapon = m_Entity.m_Weapon;
				weapon.Event_EntityAttack_AttackEnd = (Action)Delegate.Combine(weapon.Event_EntityAttack_AttackEnd, new Action(OnAttackActionEnd));
			}
		}
	}

	private void OnAttackActionEnd()
	{
		UnregistAttackEnd();
	}

	protected void AttackNotGo()
	{
		OnAttackActionEnd();
	}

	public void SetIsEnd(bool isend)
	{
		bEnd = isend;
	}

	public bool GetIsEnd()
	{
		return bEnd;
	}

	protected abstract void OnInit();

	public void SetRotate(bool bRotate)
	{
		this.bRotate = bRotate;
	}

	private void OnEnable()
	{
		if (!bInit)
		{
			Updater.AddUpdate("EntityAttackBase", UpdateProcess);
			bInit = true;
		}
	}

	private void OnDisable()
	{
		if (bInit)
		{
			Updater.RemoveUpdate("EntityAttackBase", UpdateProcess);
			bInit = false;
		}
	}

	private void UnregistAttackEnd()
	{
		Debugger.Log(m_Entity, "EntityAttackBase UnregistAttackEnd bAddActionEnd  " + bAddActionEnd + "  have weapon = " + (m_Entity.m_Weapon != null));
		if (bAddActionEnd)
		{
			if ((bool)m_Entity && m_Entity.m_Weapon != null)
			{
				WeaponBase weapon = m_Entity.m_Weapon;
				weapon.Event_EntityAttack_AttackEnd = (Action)Delegate.Remove(weapon.Event_EntityAttack_AttackEnd, new Action(OnAttackActionEnd));
			}
			bAddActionEnd = false;
		}
	}

	protected virtual void DeInit()
	{
	}

	public virtual void SetData(params object[] args)
	{
	}

	protected void UpdateAttackAngle()
	{
		if (!(m_Entity == null) && !(m_Entity.m_HatredTarget == null))
		{
			ref Vector3 direction = ref m_AttackData.direction;
			Vector3 position = m_Entity.m_HatredTarget.position;
			float x = position.x;
			Vector3 position2 = m_Entity.position;
			direction.x = x - position2.x;
			ref Vector3 direction2 = ref m_AttackData.direction;
			Vector3 position3 = m_Entity.m_HatredTarget.position;
			float z = position3.z;
			Vector3 position4 = m_Entity.position;
			direction2.z = z - position4.z;
			m_AttackData.direction = m_AttackData.direction.normalized;
			m_AttackData.angle = Utils.getAngle(m_AttackData.direction);
		}
	}

	public abstract void Install();

	public void UnInstall()
	{
		UnregistAttackEnd();
		OnDisable();
		if (OnUnInstall != null)
		{
			OnUnInstall();
		}
	}

	protected virtual void UpdateProcess(float delta)
	{
	}
}
