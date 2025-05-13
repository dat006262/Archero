




public class SkillAlone1031 : SkillAloneBase
{
	private float speedratio;

	private float range;

	protected override void OnInstall()
	{
		speedratio = float.Parse(base.m_SkillData.Args[0]);
		range = float.Parse(base.m_SkillData.Args[1]);
		m_Entity.m_EntityData.Modify_BulletSpeedRatio(speedratio, range);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_BulletSpeedRatio(1f, 0f);
	}
}
