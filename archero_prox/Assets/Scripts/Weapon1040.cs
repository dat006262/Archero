




using UnityEngine;

public class Weapon1040 : WeaponBase
{
	protected override void OnInit()
	{
		base.OnInit();
	}

	protected override void OnAttack(object[] args)
	{
		int num = 4;
		float num2 = 360f / (float)num;
		for (int i = 0; i < num; i++)
		{
			Vector3 zero = Vector3.zero;
			float num3 = num2 * (float)i;
			Vector3 eulerAngles = m_Entity.eulerAngles;
			CreateBulletOverride(zero, num3 - eulerAngles.y + 45f);
		}
	}
}
