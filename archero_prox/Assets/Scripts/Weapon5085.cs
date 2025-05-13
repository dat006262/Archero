




using Dxx.Util;
using UnityEngine;

public class Weapon5085 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		if ((bool)m_Entity.m_HatredTarget)
		{
			int num = 7;
			float num2 = 0f;
			if ((bool)m_Entity.m_HatredTarget)
			{
				num2 = Utils.getAngle(m_Entity.m_HatredTarget.position - m_Entity.position);
			}
			for (int i = 0; i < num; i++)
			{
				float num3 = num2 + Utils.GetBulletAngle(i, num, 180f);
				float x = MathDxx.Sin(num3);
				float z = MathDxx.Cos(num3);
				GameLogic.Release.Bullet.CreateBullet(m_Entity, 5085, m_Entity.position + new Vector3(x, 0f, z) * 2f, num3);
			}
		}
	}
}
