




using UnityEngine;

public class Weapon5054 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		int num = 23;
		for (int i = 0; i < num; i++)
		{
			action.AddActionDelegate(delegate
			{
				Transform transform = CreateBullet(Vector3.zero, 0f);
				transform.position = GameLogic.Release.MapCreatorCtrl.RandomPosition();
				BulletBase component = transform.GetComponent<BulletBase>();
				component.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID));
				component.SetTarget(Target, ParabolaSize);
			});
			if (i < num - 1)
			{
				action.AddActionWait(0.05f);
			}
		}
	}
}
