




using UnityEngine;

public class AI3056 : AIBeeBase
{
	protected override void OnInit()
	{
		base.OnInit();
		AddAction(new AIMove1031(m_Entity));
	}

	protected override void OnDeadBefore()
	{
		base.OnDeadBefore();
		for (int i = 0; i < 4; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, m_Entity.m_Data.WeaponID, m_Entity.position + new Vector3(0f, 1f, 0f), (float)i * 90f + 45f);
		}
	}
}
