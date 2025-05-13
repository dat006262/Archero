




public class SkillAlone1028 : SkillAloneBase
{
	private float percent;

	protected override void OnInstall()
	{
		percent = float.Parse(base.m_SkillData.Args[0]);
		m_Entity.m_EntityData.Modify_HitCreate2(1, percent);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_HitCreate2(-1, percent);
	}
}
