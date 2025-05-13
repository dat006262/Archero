




using Dxx.Util;
using System.Collections.Generic;

public class SkillAlone1067 : SkillAloneBase
{
	private float range;

	private int debuffid;

	private TimeRepeat mTime;

	protected override void OnInstall()
	{
		if (base.m_SkillData.Args.Length < 2)
		{
			SdkManager.Bugly_Report("SkillAlone1067.cs", Utils.FormatString("SkillAlone1067 m_SkillData.Args.Length = {0}", base.m_SkillData.Args.Length));
		}
		else if (float.TryParse(base.m_SkillData.Args[0], out range) && int.TryParse(base.m_SkillData.Args[1], out debuffid) && (bool)m_Entity)
		{
			Updater.AddUpdate("SkillAlone1067", OnUpdate);
		}
	}

	protected override void OnUninstall()
	{
		Updater.RemoveUpdate("SkillAlone1067", OnUpdate);
	}

	private void OnUpdate(float delta)
	{
		List<EntityBase> roundEntities = GameLogic.Release.Entity.GetRoundEntities(m_Entity, range, haveself: false);
		int i = 0;
		for (int count = roundEntities.Count; i < count; i++)
		{
			GameLogic.SendBuff(roundEntities[i], m_Entity, debuffid);
		}
	}
}
