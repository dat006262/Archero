




public class SkillAlone1040 : SkillAloneBase
{
	protected override void OnInstall()
	{
		m_Entity.m_EntityData.Modify_BulletLineCount(1);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_BulletLineCount(-1);
	}
}
