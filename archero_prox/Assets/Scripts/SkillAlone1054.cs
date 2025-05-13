




public class SkillAlone1054 : SkillAloneBase
{
	protected override void OnInstall()
	{
		m_Entity.m_EntityData.Modify_TurnTableCount(1);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_TurnTableCount(-1);
	}
}
