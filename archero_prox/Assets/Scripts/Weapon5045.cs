




using Dxx.Util;
using UnityEngine;

public class Weapon5045 : WeaponBase
{
	private int count = 7;

	private Vector3 dir;

	private float attackprevtime;

	private float starttime;

	private bool bAttackPrevEnd;

	protected override void OnInit()
	{
		bAttackPrevEnd = false;
		base.OnInit();
		starttime = Updater.AliveTime;
		attackprevtime = 0.5f;
		Updater.AddUpdate("Weapon5045", OnUpdate);
	}

	protected override void OnUnInstall()
	{
		Updater.RemoveUpdate("Weapon5045", OnUpdate);
		base.OnUnInstall();
	}

	protected override void OnAttack(object[] args)
	{
		bAttackPrevEnd = true;
		Vector3 eulerAngles = m_Entity.eulerAngles;
		dir = Utils.GetDirection(eulerAngles.y);
		for (int i = 0; i < 5; i++)
		{
			int index = i;
			action.AddActionDelegate(delegate
			{
				Transform transform = CreateBullet(Vector3.zero, 0f);
				transform.transform.position = m_Entity.position + dir * (index + 1) * 3.5f;
				BulletBase component = transform.GetComponent<BulletBase>();
				component.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID));
				component.SetTarget(Target);
			});
			if (i < count - 1)
			{
				action.AddActionWait(0.07f);
			}
		}
	}

	private void OnUpdate(float delta)
	{
		if (Updater.AliveTime - starttime >= attackprevtime)
		{
			Updater.RemoveUpdate("Weapon5045", OnUpdate);
		}
		else
		{
			m_Entity.m_AttackCtrl.RotateHero(m_Entity.m_HatredTarget);
		}
	}
}
