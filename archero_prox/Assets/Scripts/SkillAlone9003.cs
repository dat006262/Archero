




public class SkillAlone9003 : SkillAloneBase
{
	protected override void OnInstall()
	{
		m_Entity.m_EntityData.AddElement(EElementType.eIce);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.RemoveElement(EElementType.eIce);
	}
}
