




using UnityEngine;

public class AI3067 : AIBase
{
	private ActionBasic action = new ActionBasic();

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		action.Init();
		AddAttack();
	}

	protected override void OnInit()
	{
		AddAction(new AIMove1002(m_Entity, GameLogic.Random(400, 800)));
	}

	private void AddAttack()
	{
		action.AddAction(GetActionWaitDelegate(2000, Attack));
	}

	private void Attack()
	{
		for (int i = 0; i < 4; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, m_Entity.m_Data.WeaponID, m_Entity.position + new Vector3(0f, 1f, 0f), (float)i * 90f + 45f);
		}
		AddAttack();
	}

	protected override void OnAIDeInit()
	{
		action.DeInit();
	}
}
