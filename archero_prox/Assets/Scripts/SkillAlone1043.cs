




using Dxx.Util;
using UnityEngine;

public class SkillAlone1043 : SkillAloneBase
{
	private GameObject line;

	private FireLineCtrl linectrl;

	private bool bLineShow = true;

	private float attack_time;

	private float attack_delaytime;

	private float find_time = 10f;

	private float find_delaytime = 0.5f;

	private EntityBase target;

	private float hitratio;

	private float range;

	private long clockindex;

	protected override void OnInstall()
	{
		attack_delaytime = float.Parse(base.m_SkillData.Args[0]);
		hitratio = float.Parse(base.m_SkillData.Args[1]);
		range = float.Parse(base.m_SkillData.Args[2]);
		range = 10f;
		CreateSkillAlone();
		Updater.AddUpdate("SkillAlone1043", OnUpdate);
	}

	protected override void OnUninstall()
	{
		GameLogic.EffectCache(line);
		Updater.RemoveUpdate("SkillAlone1043", OnUpdate);
	}

	private void CreateSkillAlone()
	{
		line = GameLogic.EffectGet("Game/SkillPrefab/SkillAlone1043_One");
		linectrl = line.GetComponent<FireLineCtrl>();
	}

	private void OnUpdate(float delta)
	{
		find_time += delta;
		if (find_time >= find_delaytime)
		{
			find_time -= find_delaytime;
			target = GameLogic.Release.Entity.GetNearEntity(m_Entity, range, sameteam: false);
		}
		attack_time += delta;
		if (attack_time >= attack_delaytime)
		{
			attack_time -= attack_delaytime;
			OnAttack();
		}
		LineShow(target != null && !target.GetIsDead());
		if (bLineShow)
		{
			linectrl.UpdateLine(m_Entity.m_Body.EffectMask.transform.position, target.m_Body.EffectMask.transform.position);
		}
	}

	private void LineShow(bool show)
	{
		if (bLineShow != show)
		{
			bLineShow = show;
			line.SetActive(show);
			if (show && (bool)target)
			{
				GameLogic.SendBuff(target, m_Entity, 1023);
			}
		}
	}

	private void OnAttack()
	{
		if (target != null)
		{
			int num = -(int)((float)m_Entity.m_EntityData.GetAttack(0) * hitratio);
			GameLogic.SendHit_Skill(target, num);
		}
	}
}
