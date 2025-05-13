




public class SkillAlone2001 : SkillAloneBase
{
	protected override void OnInstall()
	{
		m_Entity.m_EntityData.Modify_MissHP(value: true);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_MissHP(value: false);
	}
}
