




public class Weapon1013 : WeaponBase
{
	protected override void OnInstall()
	{
		m_Entity.AddSkill(1100003);
	}

	protected override void OnUnInstall()
	{
		m_Entity.RemoveSkill(1100003);
	}
}
