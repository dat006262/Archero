




using System.Collections.Generic;
using UnityEngine;

public class Weapon5126 : WeaponBase
{
	private List<Vector3> mPosList = new List<Vector3>();

	private float length = 3f;

	protected override void OnAttack(object[] args)
	{
		mPosList.Clear();
		mPosList.Add(Target.position);
		mPosList.Add(Target.position + new Vector3(0f - length, 0f, length));
		mPosList.Add(Target.position + new Vector3(0f - length, 0f, 0f - length));
		mPosList.Add(Target.position + new Vector3(length, 0f, length));
		mPosList.Add(Target.position + new Vector3(length, 0f, 0f - length));
		int i = 0;
		for (int count = mPosList.Count; i < count; i++)
		{
			Transform transform = CreateBullet(Vector3.zero, 0f);
			transform.position = mPosList[i];
			BulletBase component = transform.GetComponent<BulletBase>();
			component.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID));
			component.SetTarget(Target, ParabolaSize);
		}
	}
}
