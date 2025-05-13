




using Dxx.Util;
using UnityEngine;

public class SkillAlone1037 : SkillAloneBase
{
	private float delaytime;

	private float time;

	private int index;

	protected override void OnInstall()
	{
		delaytime = float.Parse(base.m_SkillData.Args[0]);
		Updater.AddUpdate("SkillAlone1037", OnUpdate);
	}

	protected override void OnUninstall()
	{
		Updater.RemoveUpdate("SkillAlone1037", OnUpdate);
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
				CreateBullets();
			}
		}
	}

	private void CreateBullets()
	{
		float currentAngle = m_Entity.m_AttackCtrl.GetCurrentAngle();
		CreateBullet(currentAngle - 45f);
		CreateBullet(currentAngle);
		CreateBullet(currentAngle + 45f);
	}

	private void CreateBullet(float angle)
	{
		GameLogic.Release.Bullet.CreateBullet(m_Entity, 1001, m_Entity.position + new Vector3(0f, 1f, 0f), angle);
	}
}
