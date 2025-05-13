




using DG.Tweening;
using UnityEngine;

public class SkillCreate1003 : SkillCreateBase
{
	private Transform mCloud;

	private float hitratio;

	private float range;

	private Sequence seqcloud;

	private GameObject line;

	private LightingLineCtrl linectrl;

	protected override void OnAwake()
	{
		mCloud = base.transform.Find("child/cloud");
	}

	protected override void OnInit(string[] args)
	{
		time = float.Parse(args[0]);
		float interval = float.Parse(args[1]);
		hitratio = float.Parse(args[2]);
		range = float.Parse(args[3]);
		seqcloud = DOTween.Sequence().AppendInterval(interval).AppendCallback(delegate
		{
			OnStepUpdate();
		})
			.SetLoops(-1);
	}

	private void OnStepUpdate()
	{
		EntityBase nearEntity = GameLogic.Release.Entity.GetNearEntity(m_Entity, range, sameteam: false);
		if (nearEntity != null)
		{
			long num = (long)((float)m_Entity.m_EntityData.GetAttackBase() * hitratio);
			GameLogic.SendHit_Skill(nearEntity, -num);
			line = GameLogic.EffectGet("Game/SkillPrefab/SkillAlone1042_One");
			linectrl = line.GetComponent<LightingLineCtrl>();
			linectrl.Init(mCloud, nearEntity);
		}
	}

	protected override void OnDeinit()
	{
		if (seqcloud != null)
		{
			seqcloud.Kill();
		}
	}
}
