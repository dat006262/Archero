




using Dxx.Util;

public class Weapon8001 : WeaponBase
{
	private int throughID = 1200001;

	private bool change1002;

	protected override void OnInstall()
	{
		base.OnInstall();
		m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} - 25", "AttackModify%"));
		m_Entity.AddSkillAttribute(throughID, 0.55f);
		OnAttackEndStartAction = OnAttackEndStartActions;
		OnAttackEndEndAction = OnAttackEndEndActions;
		OnAttackInterruptAction = OnAttackInterruptActions;
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
		m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + 25", "AttackModify%"));
		OnAttackEndStartAction = null;
		OnAttackEndEndAction = null;
		OnAttackInterruptAction = null;
		m_Entity.RemoveSkill(throughID);
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
