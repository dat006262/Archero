




public class SkillAlone1059 : SkillAloneBase
{
	private float speed;

	private float time;

	protected override void OnInstall()
	{
		speed = float.Parse(base.m_Data.Args[0]);
		time = float.Parse(base.m_Data.Args[1]);
		m_Entity.m_EntityData.Modify_BulletSpeedHitted(1, speed, time);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.Modify_BulletSpeedHitted(-1, 0f - speed, 0f - time);
	}
}
