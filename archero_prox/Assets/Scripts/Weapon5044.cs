




using Dxx.Util;
using UnityEngine;

public class Weapon5044 : WeaponBase
{
	private int count = 7;

	private bool bRotate;

	protected override void OnInstall()
	{
		bRotate = true;
		Updater.AddUpdate("Weapon5044", OnUpdate);
	}

	protected override void OnUnInstall()
	{
		Updater.RemoveUpdate("Weapon5044", OnUpdate);
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < count; i++)
		{
			action.AddActionDelegate(delegate
			{
				CreateBulletOverride(Vector3.zero, GameLogic.Random(-45f, 45f));
			});
			if (i < count - 1)
			{
				action.AddActionWait(0.2f);
			}
		}
		action.AddActionDelegate(delegate
		{
			Updater.RemoveUpdate("Weapon5044", OnUpdate);
		});
	}

	private void OnUpdate(float delta)
	{
		m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(m_Entity.m_HatredTarget.position - m_Entity.position));
	}
}
