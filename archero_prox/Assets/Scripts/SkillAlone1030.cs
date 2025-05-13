




public class SkillAlone1030 : SkillAloneBase
{
	protected override void OnInstall()
	{
		int i = 0;
		for (int num = base.m_SkillData.Args.Length; i < num; i++)
		{
			m_Entity.m_EntityData.AddBabyAttribute(base.m_SkillData.Args[i]);
		}
	}

	protected override void OnUninstall()
	{
		int i = 0;
		for (int num = base.m_SkillData.Args.Length; i < num; i++)
		{
			m_Entity.m_EntityData.RemoveBabyAttribute(base.m_SkillData.Args[i]);
		}
	}
}
