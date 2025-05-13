




using Dxx.Util;

public class SuperSkill1005 : SuperSkillBase
{
	private float percent;

	protected override void OnInit()
	{
		percent = base.m_Data.Args[0];
	}

	protected override void OnDeInit()
	{
	}

	protected override void OnUseSkill()
	{
		GameLogic.Self.m_EntityData.ExcuteAttributes(Utils.GetString("HPRecover%", " + ", percent));
	}
}
