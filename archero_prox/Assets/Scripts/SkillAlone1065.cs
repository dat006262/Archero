




using Dxx.Util;
using UnityEngine;

public class SkillAlone1065 : SkillAloneBase
{
	private float delaytime;

	private float range;

	private int debuffid;

	private TimeRepeat mTime;

	protected override void OnInstall()
	{
		if (base.m_SkillData.Args.Length < 3)
		{
			SdkManager.Bugly_Report("SkillAlone1065.cs", Utils.FormatString("SkillAlone1065 m_SkillData.Args.Length = {0}", base.m_SkillData.Args.Length));
		}
		else if (float.TryParse(base.m_SkillData.Args[0], out delaytime) && float.TryParse(base.m_SkillData.Args[1], out range) && int.TryParse(base.m_SkillData.Args[2], out debuffid) && (bool)m_Entity)
		{
			mTime = new TimeRepeat("SkillAlone1065", delaytime, OnUpdate, firstdo: false, 0f);
		}
	}

	protected override void OnUninstall()
	{
		if (mTime != null)
		{
			mTime.UnRegister();
			mTime = null;
		}
	}

	private void OnUpdate()
	{
		EntityBase nearEntity = GameLogic.Release.Entity.GetNearEntity(m_Entity, range, sameteam: false);
		if ((bool)nearEntity)
		{
			GameObject gameObject = GameLogic.Release.MapEffect.Get("Game/SkillPrefab/SkillAlone1065_Effect");
			gameObject.transform.position = nearEntity.position;
			GameLogic.SendBuff(nearEntity, m_Entity, debuffid);
		}
	}
}
