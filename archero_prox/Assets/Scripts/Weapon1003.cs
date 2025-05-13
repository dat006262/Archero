




public class Weapon1003 : WeaponBase
{
	protected override void OnInstall()
	{
		OnAttackStartEndAction = OnAttack1003;
		ParabolaSize = 2;
	}

	protected override void OnUnInstall()
	{
		OnAttackStartEndAction = null;
	}

	private void OnAttack1003()
	{
		m_Entity.WeaponHandShow(show: false);
		action.AddAction(new ActionBasic.ActionWait
		{
			waitTime = 1f
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
