




using UnityEngine;

public class AI3033 : AIBeeBase
{
	protected override void OnInit()
	{
		base.OnInit();
		AddAction(new AIMove1031(m_Entity));
	}

	protected override void OnDeadBefore()
	{
		base.OnDeadBefore();
		if (m_Entity.IsElite)
		{
			int num = 8;
			for (int i = 0; i < num; i++)
			{
				GameLogic.Release.Bullet.CreateBullet(m_Entity, 3008, m_Entity.position + new Vector3(0f, 1f, 0f), (float)i * 45f);
			}
		}
	}
}
