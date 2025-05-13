




using Dxx.Util;
using UnityEngine;

public class Weapon1100 : WeaponBase
{
	protected override void OnAttack(object[] args)
	{
		int num = 4;
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)i * 90f;
			float x = MathDxx.Sin(num2);
			float z = MathDxx.Cos(num2);
			GameLogic.Release.Bullet.CreateBullet(m_Entity, 1100, m_Entity.position + new Vector3(x, 1f, z) * 0.5f, num2);
		}
	}
}
