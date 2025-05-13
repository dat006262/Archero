




using UnityEngine;

public class WeaponCallBase : WeaponBase
{
	protected override void OnAttack(object[] args)
	{
		BulletCallBase bulletCallBase = CreateBulletOverride(Vector3.zero, 0f) as BulletCallBase;
		bulletCallBase.SetTarget(null);
		bulletCallBase.SetEndPos(m_Entity.CallEndPos);
	}
}
