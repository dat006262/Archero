




using UnityEngine;

public class SuperSkill1004 : SuperSkillBase
{
	private ActionBasic action = new ActionBasic();

	protected override void OnInit()
	{
		action.Init();
	}

	protected override void OnDeInit()
	{
		action.DeInit();
	}

	protected override void OnUseSkill()
	{
		int num = 10;
		for (int i = 0; i < num; i++)
		{
			action.AddActionDelegate(delegate
			{
				BulletManager bullet = GameLogic.Release.Bullet;
				EntityHero entity = base.m_Entity;
				Vector3 pos = base.m_Entity.position + new Vector3(0f, 1f, 0f);
				Vector3 eulerAngles = base.m_Entity.eulerAngles;
				bullet.CreateBullet(entity, 8901, pos, eulerAngles.y);
			});
			if (i < num - 1)
			{
				action.AddActionWait(0.07f);
			}
		}
	}
}
