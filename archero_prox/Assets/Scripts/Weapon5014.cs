




using UnityEngine;

public class Weapon5014 : WeaponBase
{
	protected override void OnInstall()
	{
		m_Entity.AddSkill(1100003);
	}

	protected override void OnUnInstall()
	{
		m_Entity.RemoveSkill(1100003);
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 3; i++)
		{
			action.AddActionDelegate(delegate
			{
				CreateBulletOverride(Vector3.zero, 0f);
			});
			action.AddActionWait(0.3f);
		}
	}
}
