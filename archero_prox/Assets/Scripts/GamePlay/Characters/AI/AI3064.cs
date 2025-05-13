




using UnityEngine;

public class AI3064 : AIBase
{
	private ActionBasic action = new ActionBasic();

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		action.Init();
	}

	protected override void OnInit()
	{
		AddAction(new AIMove1002(m_Entity, 1000, 1000));
		AddAttack();
	}

	private void AddAttack()
	{
		action.AddAction(GetActionWaitDelegate(2000, Attack));
	}

	private void Attack()
	{
		for (int i = 0; i < 15; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, m_Entity.m_Data.WeaponID, m_Entity.position + new Vector3(0f, 1f, 0f), GameLogic.Random(0f, 360f));
		}
		AddAttack();
	}

	protected override void OnAIDeInit()
	{
		action.DeInit();
	}
}
