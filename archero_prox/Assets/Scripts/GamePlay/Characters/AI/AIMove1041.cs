




using Dxx.Util;
using UnityEngine;

public class AIMove1041 : AIMoveBase
{
	private string ContinuousName;

	private int count = 30;

	private ActionBattle action = new ActionBattle();

	public AIMove1041(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		action.Init(m_Entity);
		ContinuousName = m_Entity.m_AniCtrl.GetString("Continuous");
		m_Entity.m_AniCtrl.SetString("Continuous", "Recharging");
		m_Entity.m_AniCtrl.SendEvent("Continuous");
		for (int i = 0; i < count; i++)
		{
			action.AddActionDelegate(delegate
			{
				float angle = Utils.getAngle(m_Entity.m_HatredTarget.position - m_Entity.position);
				GameLogic.Release.Bullet.CreateBullet(m_Entity, 5046, m_Entity.position + new Vector3(0f, 2f, 0f), GameLogic.Random(angle - 90f, angle + 90f));
			});
			if (i < count - 1)
			{
				action.AddActionWait(0.1f);
			}
		}
		action.AddActionDelegate(base.End);
	}

	protected override void OnUpdate()
	{
		m_Entity.m_AttackCtrl.RotateHero(m_Entity.m_HatredTarget);
	}

	private void Attack()
	{
		for (int i = 0; i < 8; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, 5046, m_Entity.position + new Vector3(0f, 1f, 0f), (float)i * 45f);
		}
	}

	protected override void OnEnd()
	{
		action.DeInit();
		m_Entity.m_AniCtrl.SetString("Continuous", ContinuousName);
		m_Entity.m_AniCtrl.SendEvent("Idle", force: true);
	}
}
