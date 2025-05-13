




using UnityEngine;

public class Weapon1037 : WeaponBase
{
	private float range = 3f;

	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			action.AddActionDelegate(delegate
			{
				Transform transform = CreateBullet(Vector3.zero, 0f);
				transform.position = Target.position + new Vector3(GameLogic.Random(0f - range, range), 0f, GameLogic.Random(0f - range, range));
				BulletBase component = transform.GetComponent<BulletBase>();
				component.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID));
				component.SetTarget(Target, ParabolaSize);
			});
			if (i < num - 1)
			{
				action.AddActionWait(0.2f);
			}
		}
	}
}
