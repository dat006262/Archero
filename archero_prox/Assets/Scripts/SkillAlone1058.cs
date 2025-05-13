




public class SkillAlone1058 : SkillAloneBase
{
	protected override void OnInstall()
	{
		m_Entity.m_EntityData.Modify_BulletScale(1);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_BulletScale(-1);
	}
}
