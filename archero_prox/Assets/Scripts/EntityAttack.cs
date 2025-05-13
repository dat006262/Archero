




using Dxx.Util;
using UnityEngine;

public class EntityAttack : EntityAttackBase
{
	private ActionBasic action = new ActionBasic();

	private bool bInstall;

	protected override void OnInit()
	{
		OnHatredTarget();
		if (m_Entity == null || m_Entity.m_HatredTarget == null)
		{
			UnInstall();
			AttackNotGo();
			return;
		}
		m_Entity.ChangeWeapon(AttackID);
		action.Init();
		AttackStart();
		bInstall = true;
		OnUnInstall = UnInstalls;
		AttackEnd();
	}

	protected override void DeInit()
	{
	}

	protected virtual void OnHatredTarget()
	{
	}

	public override void SetData(object[] args)
	{
	}

	private void AttackStart()
	{
        Debug.Log("@LOG EntityAttack.AttackStart");
		if (bRotate)
		{
			UpdateAttackAngle();
		}
		else
		{
			ref JoyData attackData = ref m_AttackData;
			Vector3 eulerAngles = m_Entity.eulerAngles;
			attackData.angle = eulerAngles.y;
			float x = MathDxx.Sin(m_AttackData.angle);
			float z = MathDxx.Cos(m_AttackData.angle);
			m_AttackData.direction = new Vector3(x, 0f, z);
		}
		if (m_Entity.m_AttackCtrl != null)
		{
			m_Entity.m_AttackCtrl.OnMoveStart(m_AttackData);
		}
		if (m_Entity.m_Weapon != null)
		{
			m_Entity.m_Weapon.SetTarget(m_Entity.m_HatredTarget);
		}
	}

	private void AttackEnd()
	{
		UnInstall();
	}

	public override void Install()
	{
	}

	private void UnInstalls()
	{
		action.DeInit();
		if (bInstall)
		{
			bInstall = false;
			if (m_Entity.m_AttackCtrl != null)
			{
				m_Entity.m_AttackCtrl.OnMoveEnd(m_AttackData);
			}
		}
	}
}
