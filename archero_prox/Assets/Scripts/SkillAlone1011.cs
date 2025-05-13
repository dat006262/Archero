




public class SkillAlone1011 : SkillAloneBase
{
	private long value;

	protected override void OnInstall()
	{
		long.TryParse(base.m_Data.Args[0], out value);
		m_Entity.m_EntityData.attribute.HitVampirePercent.UpdateValuePercent(value);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.attribute.HitVampirePercent.UpdateValuePercent(-value);
	}
}
