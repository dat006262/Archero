




using DG.Tweening;
using System;
using UnityEngine;

public class AIMove1050 : AIMoveBase
{
	protected EntityBase target;

	protected Vector3 nextpos;

	protected Vector3 endpos;

	private Sequence seq;

	private Animation ani;

	private int skillendcount;

	private bool bDizzy;

	public AIMove1050(EntityBase entity)
		: base(entity)
	{
		name = "AIMove1050";
	}

	protected override void OnInitBase()
	{
		skillendcount = 0;
		target = GameLogic.Self;
		endpos = GameLogic.Release.MapCreatorCtrl.RandomPosition();
		SetAnimation();
	}

	private void SetAnimation()
	{
		m_Entity.m_AniCtrl.SetString("Skill", "Spawn");
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 1.5f);
		m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", revert: true);
		m_Entity.m_AniCtrl.SendEvent("Skill");
		m_Entity.m_AniCtrl.SetString("SkillEnd", string.Empty);
		m_Entity.ShowHP(show: false);
		m_Entity.SetCollider(enable: false);
		EntityBase entity = m_Entity;
		entity.OnSkillActionEnd = (Action)Delegate.Combine(entity.OnSkillActionEnd, new Action(OnSkillEnd));
		float animationTime = m_Entity.m_AniCtrl.GetAnimationTime("Skill");
		seq = DOTween.Sequence();
		seq.AppendInterval(animationTime + 0.5f);
		seq.AppendCallback(delegate
		{
			m_Entity.SetPosition(endpos);
			m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", revert: false);
			m_Entity.m_AniCtrl.SetString("SkillEnd", "SpinAttack");
			m_Entity.m_AniCtrl.SendEvent("Skill");
		});
		seq.AppendInterval(0.2f);
		seq.AppendCallback(delegate
		{
			m_Entity.SetCollider(enable: true);
		});
	}

	private void OnSkillEnd()
	{
		skillendcount++;
		if (skillendcount == 1)
		{
			EntityBase entity = m_Entity;
			Vector3 position = m_Entity.position;
			float x = position.x;
			Vector3 position2 = m_Entity.position;
			entity.SetPosition(new Vector3(x, -100f, position2.z));
		}
		else if (skillendcount == 2)
		{
			int num = 16;
			float num2 = 360f / (float)num;
			Vector3 pos = m_Entity.position + new Vector3(0f, 1f, 0f);
			for (int i = 0; i < num; i++)
			{
				GameLogic.Release.Bullet.CreateBullet(m_Entity, 5071, pos, (float)i * num2);
			}
			seq = DOTween.Sequence();
			seq.AppendInterval(0.5f).AppendCallback(base.End);
		}
	}

	private void DeInitSeq()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	protected override void OnEnd()
	{
		DeInitSeq();
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", -1.5f);
		EntityBase entity = m_Entity;
		entity.OnSkillActionEnd = (Action)Delegate.Remove(entity.OnSkillActionEnd, new Action(OnSkillEnd));
		m_Entity.m_AniCtrl.SetString("SkillEnd", string.Empty);
	}
}
