




using UnityEngine;

public class Weapon5005 : WeaponBase
{
	protected float posoffset = 1f;

	protected int bulletcount = 20;

	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		float num = 90f;
		GameLogic.Hold.Sound.PlayBulletCreate(2003004, m_Entity.position);
		for (int i = 0; i < bulletcount; i++)
		{
			CreateBulletOverride(new Vector3(GameLogic.Random(0f - posoffset, posoffset), 0f, GameLogic.Random(0f - posoffset, posoffset)), GameLogic.Random(0f - num, num));
		}
	}
}
