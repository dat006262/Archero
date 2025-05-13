




using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1042 : AIMoveBase
{
	private Vector3 startpos;

	private Vector3 endpos;

	private int range;

	private float movetime;

	private float starttime;

	private float percent;

	private float percentby;

	private Vector3 dir;

	private float alldis;

	private float perdis = 1.5f;

	private float moveby;

	private float startattacktime;

	private bool bStartAttack;

	private int bulletindex;

	public AIMove1042(EntityBase entity, int range)
		: base(entity)
	{
		this.range = range;
	}

	protected override void OnInitBase()
	{
		bulletindex = GameLogic.Random(0, 2);
		bStartAttack = false;
		moveby = 0f;
		starttime = 0f;
		startpos = m_Entity.position;
		endpos = m_Entity.m_HatredTarget.position;
		dir = (endpos - startpos).normalized;
		alldis = 15f;
		movetime = alldis / 30f;
		endpos = startpos + dir * alldis;
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", -0.35f);
		m_Entity.m_AniCtrl.SetString("Skill", "ClawAttack");
		m_Entity.m_AniCtrl.SendEvent("Skill");
		m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(endpos - startpos));
		EntityBase entity = m_Entity;
		entity.OnSkillActionEnd = (Action)Delegate.Combine(entity.OnSkillActionEnd, new Action(base.End));
	}

	protected override void OnUpdate()
	{
		if (starttime == 0f)
		{
			if (m_Entity.m_AttackCtrl.RotateOver())
			{
				starttime = Updater.AliveTime + 0.5f;
				startattacktime = starttime + 0.2f;
			}
		}
		else if (starttime > 0f && Updater.AliveTime >= starttime)
		{
			if (!bStartAttack && Updater.AliveTime >= startattacktime)
			{
				bStartAttack = true;
				Attack();
			}
			moveby += Updater.delta / movetime * alldis;
			if (moveby >= perdis)
			{
				moveby -= perdis;
				AttackGround();
			}
			percentby = (Updater.AliveTime - starttime) / movetime - percent;
			percent = (Updater.AliveTime - starttime) / movetime;
			percent = MathDxx.Clamp01(percent);
			m_Entity.SetPositionBy((endpos - startpos) * percentby);
			if (percent == 1f)
			{
				starttime = -1f;
			}
		}
	}

	private void AttackGround()
	{
		bulletindex++;
		Vector3 eulerAngles = m_Entity.eulerAngles;
		float rota = eulerAngles.y + GameLogic.Random(-45f, 45f) + ((bulletindex % 2 != 0) ? (-90f) : 90f);
		GameLogic.Release.Bullet.CreateBullet(m_Entity, 5046, m_Entity.position + new Vector3(0f, 1f, 0f), rota);
	}

	private void Attack()
	{
		BulletBase bulletBase = GameLogic.Release.Bullet.CreateBullet(m_Entity, 5048);
		BulletTransmit bulletAttribute = new BulletTransmit(m_Entity, 5048, clear: true);
		bulletBase.SetBulletAttribute(bulletAttribute);
		bulletBase.transform.SetParent(m_Entity.m_Body.LeftWeapon.transform);
		bulletBase.transform.localPosition = Vector3.zero;
		bulletBase.transform.localRotation = Quaternion.identity;
		bulletBase.transform.localScale = Vector3.one;
	}

	protected override void OnEnd()
	{
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 0.35f);
		EntityBase entity = m_Entity;
		entity.OnSkillActionEnd = (Action)Delegate.Remove(entity.OnSkillActionEnd, new Action(base.End));
	}
}
