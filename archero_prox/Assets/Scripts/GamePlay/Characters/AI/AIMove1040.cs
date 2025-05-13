




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class AIMove1040 : AIMoveBase
{
	private const float Height = 6f;

	private EntityBase target;

	private Vector3 startpos;

	private Vector3 endpos;

	private int range;

	private float jumptime = 0.65f;

	private float alltime;

	private float starttime;

	private float percent;

	private AnimationCurve curve;

	private bool bJumpEnd;

	public AIMove1040(EntityBase entity, int range)
		: base(entity)
	{
		this.range = range;
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(100010);
	}

	protected override void OnInitBase()
	{
		bJumpEnd = false;
		starttime = 0f;
		GameLogic.Release.MapCreatorCtrl.RandomItemSide(GameLogic.Self, range, out float endx, out float endz);
		endpos = new Vector3(endx, 0f, endz);
		target = GameLogic.Self;
		starttime = Updater.AliveTime;
		startpos = m_Entity.position;
		m_Entity.m_AniCtrl.SendEvent("Skill");
		alltime = m_Entity.m_AniCtrl.GetAnimationTime("Skill");
		m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(endpos - startpos));
		EntityBase entity = m_Entity;
		entity.OnSkillActionEnd = (Action)Delegate.Combine(entity.OnSkillActionEnd, new Action(base.End));
	}

	protected override void OnUpdate()
	{
		if (starttime == 0f && m_Entity.m_AttackCtrl.RotateOver())
		{
			starttime = Updater.AliveTime;
		}
		if (starttime > 0f && !bJumpEnd)
		{
			percent = (Updater.AliveTime - starttime) / jumptime;
			percent = MathDxx.Clamp01(percent);
			m_Entity.SetPosition(startpos + (endpos - startpos) * percent + new Vector3(0f, curve.Evaluate(percent) * 6f, 0f));
			if (percent == 1f)
			{
				bJumpEnd = true;
				Attack();
			}
		}
	}

	private void Attack()
	{
		for (int i = 0; i < 8; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(m_Entity, 5043, m_Entity.position + new Vector3(0f, 1f, 0f), (float)i * 45f);
		}
	}

	protected override void OnEnd()
	{
		EntityBase entity = m_Entity;
		entity.OnSkillActionEnd = (Action)Delegate.Remove(entity.OnSkillActionEnd, new Action(base.End));
	}
}
