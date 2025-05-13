




public class SkillAlone9001 : SkillAloneBase
{
	protected override void OnInstall()
	{
		m_Entity.m_EntityData.AddElement(EElementType.eFire);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.RemoveElement(EElementType.eFire);
	}
}
