




public class SkillAlone9002 : SkillAloneBase
{
	protected override void OnInstall()
	{
		m_Entity.m_EntityData.AddElement(EElementType.eThunder);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.RemoveElement(EElementType.eThunder);
	}
}
