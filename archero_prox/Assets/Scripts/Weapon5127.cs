




using Dxx.Util;
using UnityEngine;

public class Weapon5127 : WeaponBase
{
	private float range = 45f;

	protected override void OnInstall()
	{
		Updater.AddUpdate("Weapon5127", OnUpdate);
	}

	protected override void OnUnInstall()
	{
		Updater.RemoveUpdate("Weapon5127", OnUpdate);
	}

	protected override void OnAttack(object[] args)
	{
		int num = 8;
		for (int i = 0; i < num; i++)
		{
			action.AddActionDelegate(delegate
			{
				CreateBulletOverride(Vector3.zero, GameLogic.Random(0f - range, range));
			});
			action.AddActionWait(GameLogic.Random(0f, 0.1f));
		}
		action.AddActionDelegate(delegate
		{
			Updater.RemoveUpdate("Weapon5127", OnUpdate);
		});
	}

	private void OnUpdate(float delta)
	{
		if ((bool)m_Entity && !m_Entity.GetIsDead() && (bool)m_Entity.m_HatredTarget)
		{
			Vector3 position = m_Entity.m_HatredTarget.position;
			float x = position.x;
			Vector3 position2 = m_Entity.position;
			float x2 = x - position2.x;
			Vector3 position3 = m_Entity.m_HatredTarget.position;
			float z = position3.z;
			Vector3 position4 = m_Entity.position;
			float y = z - position4.z;
			float angle = Utils.getAngle(x2, y);
			m_Entity.m_AttackCtrl.RotateHero(angle);
		}
	}
}
