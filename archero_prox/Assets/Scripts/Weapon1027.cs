




using UnityEngine;

public class Weapon1027 : Weapon1024
{
	private int bulletcount = 4;

	private float perangle;

	protected override void OnInstall()
	{
		perangle = 360f / (float)bulletcount;
		effectparent = m_Entity.m_Body.BulletList[0].transform;
		base.OnInstall();
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
	}

	protected override void OnAttack(object[] args)
	{
		for (int i = 0; i < bulletcount; i++)
		{
			CreateBulletOverride(Vector3.zero, perangle * (float)i);
		}
	}
}
