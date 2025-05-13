




using Dxx.Util;

public class SkillAlone1070 : SkillAloneBase
{
	private long clockindex;

	private int bulletid;

	private int createweight;

	private float hitratio;

	protected override void OnInstall()
	{
		bulletid = int.Parse(base.m_SkillData.Args[0]);
		createweight = int.Parse(base.m_SkillData.Args[1]);
		hitratio = float.Parse(base.m_SkillData.Args[2]);
		m_Entity.m_EntityData.AddAttackMeteorite(new AttackCallData(bulletid, hitratio, createweight));
	}

	protected override void OnUninstall()
	{
	}

	private void OnAttack()
	{
	}
}
