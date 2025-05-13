




using Dxx.Util;

public class SkillAloneBabyBase : SkillAloneBase
{
	protected EntityBabyBase baby;

	protected int mBabyID;

	protected override void OnInstall()
	{
		if (mBabyID == 0)
		{
			SdkManager.Bugly_Report("SkillAloneBabyBase.cs", Utils.FormatString("OnInstall SkillAlone {0} baby is null", base.ClassID));
		}
		baby = CreateBaby(mBabyID);
		if ((bool)baby)
		{
			baby.SetParent(m_Entity);
			baby.Init(mBabyID);
			m_Entity.m_EntityData.AddBaby(baby);
			m_Entity.AddBabySkillID(base.m_SkillData.SkillID);
		}
	}

	protected override void OnUninstall()
	{
		if ((bool)m_Entity && (bool)baby)
		{
			m_Entity.RemoveBabySkillID(base.m_SkillData.SkillID);
			m_Entity.m_EntityData.RemoveBaby(baby);
			GameLogic.Release.Entity.RemoveBaby(baby);
		}
	}
}
