




public class SkillAlone9004 : SkillAloneBase
{
	protected override void OnInstall()
	{
		m_Entity.m_EntityData.AddElement(EElementType.ePoison);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.RemoveElement(EElementType.ePoison);
	}
}
