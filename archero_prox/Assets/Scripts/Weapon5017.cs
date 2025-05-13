




public class Weapon5017 : Weapon1020
{
	protected override void OnInstall()
	{
		base.OnInstall();
		m_Entity.AddSkill(1100003);
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
		m_Entity.RemoveSkill(1100003);
	}
}
