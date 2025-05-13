




using Dxx.Util;
using UnityEngine;

public class SkillAlone1044 : SkillAloneBase
{
	private GameObject good;

	private Transform child;

	private float time;

	private float delaytime;

	private float hitratio;

	private float range;

	private long clockindex;

	protected override void OnInstall()
	{
		delaytime = float.Parse(base.m_SkillData.Args[0]);
		hitratio = float.Parse(base.m_SkillData.Args[1]);
		range = float.Parse(base.m_SkillData.Args[2]);
		CreateSkillAlone();
		clockindex = TimeClock.Register("SkillAlone1044", delaytime, OnAttack);
	}

	protected override void OnUninstall()
	{
		UnityEngine.Object.Destroy(good);
		TimeClock.Unregister(clockindex);
	}

	private void CreateSkillAlone()
	{
		good = GameLogic.EffectGet(Utils.FormatString("Game/SkillPrefab/SkillAlone{0}", base.ClassID));
		good.transform.SetParent(m_Entity.m_Body.EffectMask.transform);
		good.transform.localPosition = Vector3.zero;
		good.transform.localRotation = Quaternion.identity;
		good.transform.localScale = Vector3.one;
		child = good.transform.Find("child");
	}

	private void OnAttack()
	{
		EntityBase nearEntity = GameLogic.Release.Entity.GetNearEntity(m_Entity, 100f, sameteam: false);
		if (nearEntity != null)
		{
			CreateBullet(child.position, nearEntity.position);
		}
	}

	private void CreateBullet(Vector3 startpos, Vector3 endpos)
	{
		BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(m_Entity, 3004);
		bulletBase.transform.position = startpos;
		bulletBase.transform.LookAt(endpos);
		BulletTransmit bulletAttribute = new BulletTransmit(m_Entity, 3004, clear: true);
		bulletBase.SetBulletAttribute(bulletAttribute);
	}
}
