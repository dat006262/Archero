




using UnityEngine;

public class AI3068 : AIBase
{
	protected override void OnInit()
	{
		AddAction(new AIMove1007(m_Entity));
	}

	protected override void OnAIDeInit()
	{
	}

	protected override void OnDeadBefore()
	{
		for (int i = 0; i < 6; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, m_Entity.m_Data.WeaponID, m_Entity.position + new Vector3(0f, 1f, 0f), (float)i * 60f);
		}
	}
}
