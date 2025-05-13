




using Dxx.Util;
using System;

public class SkillAlone1083 : SkillAloneBase
{
	private float ratio;

	private float hppercent;

	private EntityBase mParent;

	private bool bInit;

	protected override void OnInstall()
	{
		if (base.m_SkillData.Args.Length != 2)
		{
			SdkManager.Bugly_Report("SkillAlone1083", Utils.FormatString("SkillID:{0} args.length:{1} != 2", base.m_SkillData.SkillID, base.m_SkillData.Args.Length));
			return;
		}
		if (!float.TryParse(base.m_SkillData.Args[0], out ratio))
		{
			SdkManager.Bugly_Report("SkillAlone1083", Utils.FormatString("SkillID:{0} args[0] is not a float type.", base.m_SkillData.SkillID));
			return;
		}
		if (!float.TryParse(base.m_SkillData.Args[1], out hppercent))
		{
			SdkManager.Bugly_Report("SkillAlone1083", Utils.FormatString("SkillID:{0} args[1] is not a float type.", base.m_SkillData.SkillID));
			return;
		}
		EntityBabyBase entityBabyBase = m_Entity as EntityBabyBase;
		if (entityBabyBase == null || entityBabyBase.GetParent() == null)
		{
			SdkManager.Bugly_Report("SkillAlone1083", Utils.FormatString("entity : {0} is not a baby.", m_Entity.m_Data.CharID));
			return;
		}
		mParent = entityBabyBase.GetParent();
		EntityBase entity = m_Entity;
		entity.Event_OnAttack = (Action)Delegate.Combine(entity.Event_OnAttack, new Action(OnAttack));
		bInit = true;
	}

	private void OnAttack()
	{
		if (!(mParent == null) && !mParent.GetIsDead() && GameLogic.Random(0f, 100f) < ratio)
		{
			mParent.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + {1}", "HPRecoverFixed%", hppercent));
		}
	}

	protected override void OnUninstall()
	{
		if (bInit)
		{
			EntityBase entity = m_Entity;
			entity.Event_OnAttack = (Action)Delegate.Remove(entity.Event_OnAttack, new Action(OnAttack));
		}
	}
}
