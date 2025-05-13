




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class SkillAlone1029 : SkillAloneBase
{
	private Weapon_weapon weapondata;

	private float percent;

	protected override void OnInstall()
	{
		percent = float.Parse(base.m_SkillData.Args[0]);
		EntityBase entity = m_Entity;
		entity.OnKillAction = (Action<EntityBase, Vector3>)Delegate.Combine(entity.OnKillAction, new Action<EntityBase, Vector3>(OnKillAction));
		weapondata = LocalModelManager.Instance.Weapon_weapon.GetBeanById(3001);
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnKillAction = (Action<EntityBase, Vector3>)Delegate.Remove(entity.OnKillAction, new Action<EntityBase, Vector3>(OnKillAction));
	}

	private void OnKillAction(EntityBase entity, Vector3 HittedDirection)
	{
		float angle = Utils.getAngle(HittedDirection);
		for (int i = 0; i < 8; i++)
		{
			Transform transform = GameLogic.BulletGet(weapondata.WeaponID).transform;
			transform.SetParent(GameNode.m_PoolParent);
			Transform transform2 = transform;
			Vector3 position = entity.position;
			float x = position.x;
			Vector3 position2 = entity.position;
			transform2.position = new Vector3(x, 1f, position2.z);
			transform.localRotation = Quaternion.Euler(0f, angle + (float)i * 45f, 0f);
			transform.localScale = Vector3.one;
			transform.GetComponent<BulletBase>().Init(m_Entity, weapondata.WeaponID);
			BulletBase component = transform.GetComponent<BulletBase>();
			component.AddCantHit(entity);
			component.SetBulletAttribute(new BulletTransmit(m_Entity, weapondata.WeaponID, clear: true));
			component.mBulletTransmit.AddAttackRatio(percent);
		}
	}
}
