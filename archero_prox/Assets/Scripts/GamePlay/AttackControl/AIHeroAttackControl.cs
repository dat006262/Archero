




public class AIHeroAttackControl : AttackControl
{
	private int attackState;

	protected override void OnStart()
	{
	}

	protected override void OnDestroys()
	{
	}

	public override void UpdateProgress()
	{
		base.UpdateProgress();
		AutoAttackUpdate();
	}

	private void AutoAttackUpdate()
	{
		if (GameLogic.Release.Game.RoomState != RoomState.Runing || m_EntityHero.GetIsDead() || m_EntityHero.m_MoveCtrl.GetMoving())
		{
			return;
		}
		if (attackState == 0)
		{
			if (m_EntityHero.m_HatredTarget != null)
			{
				OnMoveStart(m_JoyData);
				attackState = 1;
			}
		}
		else if (attackState == 1 && RotateOver())
		{
			OnMoveEnd(m_JoyData);
			attackState = 2;
		}
		RotateUpdate(m_EntityHero.m_HatredTarget);
	}
}
