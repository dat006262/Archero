




public class SkillAlone1001 : SkillAloneBase
{
	private int skillid;

	protected override void OnInstall()
	{
		int.TryParse(base.m_SkillData.Args[0], out skillid);
		if (skillid > 0)
		{
			m_Entity.m_EntityData.AddBabyLearnSkillId(skillid);
		}
	}

	protected override void OnUninstall()
	{
	}
}
