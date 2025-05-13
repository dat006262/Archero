




using Dxx.Util;
using UnityEngine;

public class BuffAlone1015 : BuffAloneBase
{
	private float range;

	protected override void OnStart()
	{
		range = buff_data.Args[0];
	}

	protected override void OnRemove()
	{
	}

	protected override void ExcuteBuff(BuffData data)
	{
		EntityBase nearEntity = GameLogic.Release.Entity.GetNearEntity(m_Entity, range, sameteam: false);
		if (nearEntity != null)
		{
			int bulletID = 3002;
			BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(m_Entity, bulletID, m_Entity.position + new Vector3(0f, 1f, 0f), 0f);
			if ((bool)bulletBase)
			{
				Vector3 dir = nearEntity.position - m_Entity.position;
				float angle = Utils.getAngle(dir);
				bulletBase.transform.rotation = Quaternion.Euler(0f, angle, 0f);
				bulletBase.SetTarget(nearEntity);
			}
		}
	}
}
