




using Dxx.Util;
using System;
using System.Collections.Generic;

public class SkillAlone1069 : SkillAloneBase
{
	private List<SkillMasteryBase> mMasteries = new List<SkillMasteryBase>();

	protected override void OnInstall()
	{
		int i = 0;
		for (int num = base.m_SkillData.Args.Length; i < num; i++)
		{
			string text = base.m_SkillData.Args[i];
			if (text.Length <= 4 || !int.TryParse(text.Substring(0, 3), out int result))
			{
				SdkManager.Bugly_Report("SkillAlone1069", Utils.FormatString("SkillID:{0} args[{1}]:{2} is invalid.", base.m_SkillData.SkillID, i, text));
				continue;
			}
			Type type = Type.GetType(Utils.GetString("SkillMastery", result));
			SkillMasteryBase skillMasteryBase = type.Assembly.CreateInstance(Utils.GetString("SkillMastery", result)) as SkillMasteryBase;
			skillMasteryBase.Init(m_Entity, text.Substring(4, text.Length - 4));
		}
	}

	protected override void OnUninstall()
	{
	}
}
