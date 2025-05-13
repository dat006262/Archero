




public class SkillAlone1010 : SkillAloneBase
{
	private int value;

	protected override void OnInstall()
	{
		int.TryParse(base.m_Data.Args[0], out value);
		m_Entity.m_EntityData.attribute.KillVampirePercent.UpdateValuePercent(value);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.attribute.KillVampirePercent.UpdateValuePercent(-value);
	}
}
