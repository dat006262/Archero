




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class Weapon5113 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		int num = 7;
		List<Vector3> list = GameLogic.Release.MapCreatorCtrl.RandomOutPositions(num);
		for (int i = 0; i < num && i < list.Count; i++)
		{
			Vector3 vector = list[i];
			float angle = Utils.getAngle(m_Entity.position - vector);
			GameLogic.Release.Bullet.CreateBullet(m_Entity, BulletID, vector + new Vector3(0f, 1f, 0f), angle);
		}
	}
}
