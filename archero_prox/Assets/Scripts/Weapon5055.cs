




using UnityEngine;

public class Weapon5055 : WeaponBase
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
		int num = 5;
		float num2 = 150f;
		float num3 = num2 / (float)(num - 1);
		float num4 = num3 * (float)(num - 1) / 2f;
		for (int i = 0; i < num; i++)
		{
			BulletBase bulletBase = CreateBulletOverride(Vector3.zero, (float)i * num3 - num4);
			bulletBase.mBulletTransmit.attribute.ReboundWall = new EntityAttributeBase.ValueRange(1, 6, 6);
			bulletBase.UpdateBulletAttribute();
		}
	}
}
