




using Dxx.Util;
using System;

public class SkillAlone1066 : SkillAloneBase
{
	private int debuffid;

	protected override void OnInstall()
	{
		if (base.m_SkillData.Args.Length < 1)
		{
			SdkManager.Bugly_Report("SkillAlone1066.cs", Utils.FormatString("SkillAlone1066 m_SkillData.Args.Length = {0}", base.m_SkillData.Args.Length));
		}
		else if (int.TryParse(base.m_SkillData.Args[0], out debuffid))
		{
			EntityBase entity = m_Entity;
			entity.OnHitted = (Action<EntityBase, long>)Delegate.Combine(entity.OnHitted, new Action<EntityBase, long>(OnHitted));
		}
	}

	protected override void OnUninstall()
	{
		EntityBase entity = m_Entity;
		entity.OnHitted = (Action<EntityBase, long>)Delegate.Remove(entity.OnHitted, new Action<EntityBase, long>(OnHitted));
	}

	private void OnHitted(EntityBase source, long hit)
	{
		if ((bool)source && !source.GetIsDead())
		{
			GameLogic.SendBuff(source, debuffid);
		}
	}
}
