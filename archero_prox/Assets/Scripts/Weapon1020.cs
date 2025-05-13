




using UnityEngine;

public class Weapon1020 : WeaponBase
{
	protected override void OnInstall()
	{
		SetDizzyCantRemove();
		OnAttackStartEndAction = OnAttack1003;
		OnBulletCache = OnBulletCaches;
		base.bAttackEndActionEnd = false;
	}

	protected override void OnUnInstall()
	{
		OnBulletCache = null;
		OnAttackStartEndAction = null;
	}

	protected override void OnAttack(object[] args)
	{
		CreateBullet1020(0f);
	}

	protected void CreateBullet1020(float angle)
	{
		BulletBase bulletBase = CreateBulletOverride(Vector3.zero, angle);
		bulletBase.mBulletTransmit.mThroughEnemy = 1;
		bulletBase.mBulletTransmit.mThroughRatio = 1f;
		bulletBase.SetTarget(Target, ParabolaSize);
		bulletBase.OnBulletCache = OnBulletCache;
	}

	private void OnBulletCaches()
	{
		m_Entity.WeaponHandShow(show: true);
		base.bAttackEndActionEnd = true;
	}

	private void OnAttack1003()
	{
		m_Entity.WeaponHandShow(show: false);
	}
}
