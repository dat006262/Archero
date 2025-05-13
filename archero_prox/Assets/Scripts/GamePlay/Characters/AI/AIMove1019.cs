




using Dxx.Util;
using UnityEngine;

public class AIMove1019 : AIMoveBase
{
	private EntityBase target;

	private Vector3 dir;

	public float g = 20f;

	public int attackid = 5010;

	private float endx;

	private float endz;

	private float perendx;

	private float perendz;

	private float delaytime;

	private float starttime;

	public float alltime = 1.3f;

	private float halftime;

	private Vector3 startpos;

	private bool bPlaySkill;

	private float speedratio;

	public AIMove1019(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		speedratio = alltime / 1.3f;
		delaytime = alltime * 0.23f;
		starttime = 0f;
		halftime = alltime / 2f;
		startpos = m_Entity.position;
		target = GameLogic.Self;
		RandomItem(out endx, out endz);
		float num = endx;
		Vector3 position = m_Entity.position;
		perendx = num - position.x;
		float num2 = endz;
		Vector3 position2 = m_Entity.position;
		perendz = num2 - position2.z;
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", speedratio - 1f);
		m_Entity.m_AniCtrl.SendEvent("Skill");
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		starttime += Updater.delta;
		if (starttime >= delaytime)
		{
			OnFly();
		}
	}

	private void OnFly()
	{
		if (!bPlaySkill)
		{
			bPlaySkill = true;
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Call", speedratio - 1f);
			m_Entity.m_AniCtrl.SendEvent("Call", force: true);
		}
		float num = 1f - (delaytime + alltime - starttime) / alltime;
		float num2 = -4f * g * (num - 0.5f) * (num - 0.5f) + g;
		if (starttime < delaytime + halftime)
		{
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num, startpos.y + num2, startpos.z + perendz * num));
			return;
		}
		if (starttime < delaytime + alltime)
		{
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num, startpos.y + num2, startpos.z + perendz * num));
			return;
		}
		EntityBase entity = m_Entity;
		Vector3 position = m_Entity.position;
		float x = position.x;
		Vector3 position2 = m_Entity.position;
		entity.SetPosition(new Vector3(x, 0f, position2.z));
		End();
	}

	private void RandomItem(out float endx, out float endz)
	{
		Vector3 position = target.position;
		endx = position.x;
		Vector3 position2 = target.position;
		endz = position2.z;
	}

	protected override void OnEnd()
	{
		if (bPlaySkill)
		{
			m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Call", 1f - speedratio);
		}
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 1f - speedratio);
		m_Entity.ChangeWeapon(attackid);
		m_Entity.m_Weapon.Attack();
		GameObject gameObject = GameLogic.EffectGet("Effect/Boss/BossJumpHit5005");
		gameObject.transform.position = m_Entity.position;
		gameObject.GetComponent<BossJumpHit5005>().Init(m_Entity, 1f);
	}
}
