




using Dxx.Util;

public class Weapon8003 : WeaponBase
{
	protected override void OnInit()
	{
		base.OnInit();
	}

	protected override void OnInstall()
	{
		base.OnInstall();
		m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + 45", "AttackModify%"));
		OnAttackEndStartAction = OnAttackEndStartActions;
		OnAttackEndEndAction = OnAttackEndEndActions;
		OnAttackInterruptAction = OnAttackInterruptActions;
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
		m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} - 45", "AttackModify%"));
		OnAttackEndStartAction = null;
		OnAttackEndEndAction = null;
		OnAttackInterruptAction = null;
	}

	private void OnAttackEndStartActions()
	{
		m_Entity.WeaponHandShow(show: false);
	}

	private void OnAttackEndEndActions()
	{
		m_Entity.WeaponHandShow(show: true);
	}

	private void OnAttackInterruptActions()
	{
		m_Entity.WeaponHandShow(show: true);
	}
}
