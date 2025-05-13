




using UnityEngine;

public class Weapon5036 : Weapon1024
{
	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 3; i++)
		{
			CreateBulletOverride(Vector3.zero, (float)i * 45f);
		}
	}
}
