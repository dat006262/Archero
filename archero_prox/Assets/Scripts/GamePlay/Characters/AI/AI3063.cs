




using Dxx.Util;
using UnityEngine;

public class AI3063 : AIBase
{
	private ActionBasic action = new ActionBasic();

	protected override void OnInitOnce()
	{
		base.OnInitOnce();
		action.Init();
	}

	protected override void OnInit()
	{
		AddAction(new AIMove1052(m_Entity, 3));
		AddAttack();
	}

	private void AddAttack()
	{
		action.AddAction(GetActionWaitDelegate(2000, Attack));
	}

	private void Attack()
	{
		EntityBase entityBase = GameLogic.FindTarget(m_Entity);
		if (entityBase != null)
		{
			float angle = Utils.getAngle(entityBase.position - m_Entity.position);
			for (int i = 0; i < 2; i++)
			{
				GameLogic.Release.Bullet.CreateBullet(m_Entity, m_Entity.m_Data.WeaponID, m_Entity.position + new Vector3(0f, 1f, 0f), angle + GameLogic.Random(-7f, 7f) + (float)i * 30f - 15f);
			}
		}
		AddAttack();
	}

	protected override void OnAIDeInit()
	{
		action.DeInit();
	}
}
