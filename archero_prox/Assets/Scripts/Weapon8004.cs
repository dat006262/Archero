




using Dxx.Util;

public class Weapon8004 : Weapon1001
{
	protected override void OnInstall()
	{
		base.OnInstall();
		m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} - 15", "AttackModify%"));
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
		m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + 15", "AttackModify%"));
	}
}
