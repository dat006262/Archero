




using UnityEngine;

public class Weapon1021 : WeaponBase
{
	protected override void OnAttack(object[] args)
	{
		Transform transform = CreateBullet(Vector3.zero, 0f);
		transform.position = Target.position;
		BulletBase component = transform.GetComponent<BulletBase>();
		component.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID));
		component.SetTarget(Target, ParabolaSize);
	}
}
