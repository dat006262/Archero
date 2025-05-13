




using UnityEngine;

public class Weapon5047 : WeaponBase
{
	private int count = 4;

	protected override void OnInit()
	{
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.5f);
		base.OnInit();
	}

	protected override void OnUnInstall()
	{
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.5f);
		base.OnUnInstall();
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < count; i++)
		{
			action.AddActionDelegate(delegate
			{
				BulletBase bulletBase = CreateBulletOverride(Vector3.zero, 0f);
				bulletBase.SetArgs(100011f);
				BulletBase bulletBase2 = CreateBulletOverride(Vector3.zero, 0f);
				bulletBase2.SetArgs(100012f);
				BulletBase bulletBase3 = CreateBulletOverride(Vector3.zero, 60f);
				bulletBase3.SetArgs(100011f);
				BulletBase bulletBase4 = CreateBulletOverride(Vector3.zero, 60f);
				bulletBase4.SetArgs(100012f);
				BulletBase bulletBase5 = CreateBulletOverride(Vector3.zero, -60f);
				bulletBase5.SetArgs(100011f);
				BulletBase bulletBase6 = CreateBulletOverride(Vector3.zero, -60f);
				bulletBase6.SetArgs(100012f);
			});
			if (i < count - 1)
			{
				action.AddActionWait(0.15f);
			}
		}
	}
}
