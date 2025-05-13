




using Dxx.Util;
using TableTool;
using UnityEngine;

public class AIMove1049 : AIMoveBase
{
	private EntityBase target;

	private float startTime;

	private float delaytime;

	private float jumptime;

	private float endtime;

	private bool bjumpend;

	private AnimationCurve curve;

	private Vector3 startpos;

	private Vector3 endpos;

	private float height = 5f;

	private float skillspeed = 0.2f;

	private GameObject obj;

	public AIMove1049(EntityBase entity)
		: base(entity)
	{
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(100020);
	}

	protected override void OnInitBase()
	{
		target = GameLogic.Self;
		startpos = m_Entity.position;
		endpos = target.position;
		startTime = Updater.AliveTime;
		delaytime = 0.1f / (1f - skillspeed);
		jumptime = 0.55f / (1f - skillspeed);
		endtime = 1.1f / (1f - skillspeed);
		bjumpend = false;
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		if (bjumpend && Updater.AliveTime - startTime > endtime)
		{
			End();
		}
		else if (!bjumpend && Updater.AliveTime - startTime > delaytime)
		{
			AIMoving();
		}
	}

	private void AIMoveStart()
	{
		float angle = Utils.getAngle(target.position - m_Entity.position);
		m_Entity.m_AttackCtrl.SetRotate(angle);
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 0f - skillspeed);
		m_Entity.m_AniCtrl.SendEvent("Skill");
		m_Entity.SetTrigger(value: true);
	}

	private void AIMoving()
	{
		float value = (Updater.AliveTime - startTime - delaytime) / jumptime;
		value = MathDxx.Clamp01(value);
		Vector3 a = (endpos - startpos) * value + startpos;
		m_Entity.SetPosition(a + new Vector3(0f, curve.Evaluate(value) * height, 0f));
		if (value == 1f)
		{
			obj = GameLogic.EffectGet("Effect/Boss/BossJumpHit5028");
			obj.transform.position = m_Entity.position;
			SkillAloneAttrGoodBase.Add(m_Entity, obj, true, 1f);
			bjumpend = true;
		}
	}

	protected override void OnEnd()
	{
		SkillAloneAttrGoodBase.Remove(obj);
		m_Entity.SetTrigger(value: false);
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", skillspeed);
	}
}
