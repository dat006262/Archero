




using UnityEngine;

public class Weapon1026 : Weapon1024
{
	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < 4; i++)
		{
			CreateBulletOverride(Vector3.zero, 90f * (float)i);
		}
	}
}
