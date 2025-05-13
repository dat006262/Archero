




using Dxx.Util;
using UnityEngine;

public class SkillAlone1036 : SkillAloneBase
{
	private float delaytime;

	private float time;

	private int index;

	protected override void OnInstall()
	{
		delaytime = float.Parse(base.m_SkillData.Args[0]);
		Updater.AddUpdate("SkillAlone1036", OnUpdate);
	}

	protected override void OnUninstall()
	{
		Updater.RemoveUpdate("SkillAlone1036", OnUpdate);
	}

	private void OnUpdate(float delta)
	{
		if (m_Entity.m_MoveCtrl.GetMoving())
		{
			time += delta;
			if (time >= delaytime)
			{
				time -= delaytime;
				index++;
				CreateBullet();
			}
		}
	}

	private void CreateBullet()
	{
		if ((bool)m_Entity && m_Entity.m_AttackCtrl != null && !(m_Entity.m_Body == null))
		{
			float currentAngle = m_Entity.m_AttackCtrl.GetCurrentAngle();
			BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(m_Entity, 3003, Vector3.zero, currentAngle);
			bulletBase.transform.SetParent(m_Entity.m_Body.transform);
			bulletBase.transform.localPosition = new Vector3(0f, 1f, 1f);
		}
	}
}
