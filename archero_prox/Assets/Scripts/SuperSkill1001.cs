




using UnityEngine;

public class SuperSkill1001 : SuperSkillBase
{
	protected override void OnInit()
	{
	}

	protected override void OnDeInit()
	{
	}

	protected override void OnUseSkill()
	{
		int num = 16;
		float num2 = 360f / (float)num;
		for (int i = 0; i < num; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 8901, base.m_Entity.position + new Vector3(0f, 1f, 0f), (float)i * num2);
		}
	}
}
