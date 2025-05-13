




public class SkillAlone1006 : SkillAloneBase
{
	private float ratio;

	protected override void OnInstall()
	{
		float.TryParse(base.m_Data.Args[0], out ratio);
		m_Entity.m_EntityData.Modify_HP2Attack(ratio);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_HP2Attack(0f - ratio);
	}
}
