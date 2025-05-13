




using UnityEngine;

public class Weapon5105 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		GameLogic.Hold.Sound.PlayBulletCreate(2005001, m_Entity.position);
		int num = GameLogic.Random(5, 8);
		for (int i = 0; i < num; i++)
		{
			BulletBase bulletBase = CreateBulletOverride(Vector3.zero, GameLogic.Random(-75f, 75f));
			bulletBase.mBulletTransmit.attribute.ReboundWall = new EntityAttributeBase.ValueRange(1, 2, 2);
			bulletBase.UpdateBulletAttribute();
		}
	}
}
