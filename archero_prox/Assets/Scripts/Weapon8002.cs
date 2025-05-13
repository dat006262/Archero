




using Dxx.Util;

public class Weapon8002 : WeaponBase
{
	protected override void OnInit()
	{
		base.OnInit();
	}

	protected override void OnInstall()
	{
		base.OnInstall();
		m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} - 25", "AttackModify%"));
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
		m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + 25", "AttackModify%"));
	}
}
