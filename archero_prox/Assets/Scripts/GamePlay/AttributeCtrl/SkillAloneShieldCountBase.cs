




using UnityEngine;

public class SkillAloneShieldCountBase : SkillAloneBase
{
	private long shieldcount;

	private GameObject mShieldObj;

	protected override void OnInstall()
	{
		shieldcount = long.Parse(base.m_SkillData.Args[0]);
		m_Entity.m_EntityData.AddShieldCountAction(OnShieldCount);
		m_Entity.m_EntityData.AddShieldCount(shieldcount);
	}

	protected override void OnUninstall()
	{
		m_Entity.m_EntityData.AddShieldCount(-shieldcount);
		m_Entity.m_EntityData.RemoveShieldCountAction(OnShieldCount);
		OnShieldCount(0L);
	}

	private void OnShieldCount(long value)
	{
		if (value > 0 && mShieldObj == null)
		{
			mShieldObj = GameLogic.EffectGet("Game/SkillPrefab/SkillAlone1038");
			mShieldObj.transform.SetParent(m_Entity.m_Body.EffectMask.transform);
			mShieldObj.transform.localPosition = Vector3.zero;
			mShieldObj.transform.localRotation = Quaternion.identity;
			mShieldObj.transform.localScale = Vector3.one;
		}
		else if (value == 0 && mShieldObj != null)
		{
			GameLogic.EffectCache(mShieldObj);
			mShieldObj = null;
		}
	}

	protected void ResetShieldCount()
	{
		m_Entity.m_EntityData.ResetShieldCount();
	}
}
