




public class SkillAlone1007 : SkillAloneBase
{
	protected override void OnInstall()
	{
		m_Entity.m_EntityData.Modify_FlyStone(1);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_FlyStone(-1);
	}
}
