




using Dxx.Util;
using UnityEngine;

public class Weapon5073 : Weapon1024
{
	private float angle = 45f;

	private int percount = 3;

	private BulletRedLineCtrls ctrl = new BulletRedLineCtrls();

	protected override void OnInstall()
	{
		AttackEffect = "WeaponHand1066Effect";
		ctrl.Init(m_Entity, percount, new float[3]
		{
			-45f,
			0f,
			45f
		});
		base.OnInstall();
	}

	protected override void OnUnInstall()
	{
		ctrl.Deinit();
		base.OnUnInstall();
	}

	protected override void OnAttack(object[] args)
	{
		ctrl.Deinit();
		for (int i = 0; i < percount; i++)
		{
			CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, percount, 90f));
		}
	}
}
