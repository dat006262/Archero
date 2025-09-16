




public class SkillAloneShieldValueBase : SkillAloneBase
{
	private long shieldvalue;

	protected override void OnInstall()
	{
		shieldvalue = long.Parse(base.m_SkillData.Args[0]);
		m_Entity.m_EntityData.ExcuteAttributes("AddShieldValue", shieldvalue);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.ExcuteAttributes("AddShieldValue", -shieldvalue);
	}

	protected void ResetShieldHitValue()
	{
		m_Entity.m_EntityData.ResetShieldHitValue();
	}
}
