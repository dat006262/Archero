




public class SkillAlone1061 : SkillAloneBase
{
	protected override void OnInstall()
	{
		m_Entity.m_EntityData.Modify_OnlyDemon(1);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_OnlyDemon(-1);
	}
}
