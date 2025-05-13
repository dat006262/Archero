




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class AIMove1054 : AIMoveBase
{
	private EntityBase target;

	private Vector3 dir;

	private int flyframe = 60;

	private float g = 2f;

	private float endx;

	private float endz;

	private float perendx;

	private float perendz;

	private float delaytime;

	private float starttime;

	private float alltime = 0.6f;

	private float halftime;

	private Vector3 startpos;

	private const float skillspeed = -0.5f;

	private Sequence seq_play;

	private bool bAttack;

	public AIMove1054(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		if (!m_Entity.m_HatredTarget)
		{
			End();
			return;
		}
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", -0.5f);
		delaytime = Updater.AliveTime + 0.4f;
		starttime = Updater.AliveTime;
		halftime = alltime / 2f;
		startpos = m_Entity.position;
		bAttack = false;
		m_Entity.m_AniCtrl.SendEvent("Skill");
		Vector3 position = m_Entity.m_HatredTarget.position;
		endx = position.x;
		Vector3 position2 = m_Entity.m_HatredTarget.position;
		endz = position2.z;
		bool flag = MathDxx.RandomBool();
		float num = endx;
		Vector3 position3 = m_Entity.position;
		perendx = (num - position3.x) / (float)flyframe;
		float num2 = endz;
		Vector3 position4 = m_Entity.position;
		perendz = (num2 - position4.z) / (float)flyframe;
		m_Entity.SetObstacleCollider(value: false);
		dir = new Vector3(perendx, 0f, perendz);
		UpdateDirection();
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		if (Updater.AliveTime >= delaytime)
		{
			OnFly();
		}
	}

	private void OnFly()
	{
		float num = 1f - (delaytime + alltime - Updater.AliveTime) / alltime;
		float num2 = 0f;
		if (Updater.AliveTime < delaytime + halftime)
		{
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num * (float)flyframe, startpos.y + num2, startpos.z + perendz * num * (float)flyframe));
		}
		else if (Updater.AliveTime < delaytime + alltime)
		{
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num * (float)flyframe, startpos.y + num2, startpos.z + perendz * num * (float)flyframe));
		}
		else
		{
			if (!(Updater.AliveTime > delaytime + alltime))
			{
				return;
			}
			if (!bAttack)
			{
				for (int i = 0; i < 4; i++)
				{
					GameLogic.Release.Bullet.CreateBullet(m_Entity, 5077, m_Entity.position + new Vector3(0f, 1f, 0f), (float)(i * 90) + 45f);
				}
				bAttack = true;
			}
			if (Updater.AliveTime > delaytime + alltime + 1.1f)
			{
				End();
			}
		}
	}

	private void UpdateDirection()
	{
		m_MoveData.angle = Utils.getAngle(dir.x, dir.z);
		m_MoveData.direction = dir;
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	protected override void OnEnd()
	{
		m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 0.5f);
		m_Entity.SetObstacleCollider(value: true);
		if (seq_play != null)
		{
			seq_play.Kill();
			seq_play = null;
		}
	}
}
