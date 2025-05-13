




public class Weapon1009 : WeaponBase
{
	protected override void OnInstall()
	{
		OnAttackStartEndAction = OnAttack1009;
	}

	protected override void OnUnInstall()
	{
		OnAttackStartEndAction = null;
	}

	private void OnAttack1009()
	{
		m_Entity.WeaponHandShow(show: false);
		action.AddAction(new ActionBasic.ActionWait
		{
			waitTime = 0.5f
		});
		action.AddAction(new ActionBasic.ActionDelegate
		{
			action = delegate
			{
				m_Entity.WeaponHandShow(show: true);
			}
		});
	}
}
