




using Dxx.Util;
using UnityEngine;

public class AIMove1053 : AIMoveBase
{
	private Vector3 dir;

	private int flyframe = 60;

	private float g = 1f;

	private float endx;

	private float endz;

	private float perendx;

	private float perendz;

	private float delaytime;

	private float starttime;

	private float alltime = 0.3f;

	private float halftime;

	private Vector3 startpos;

	private bool bAttack;

	private int rangemin;

	private int rangemax;

	public AIMove1053(EntityBase entity, int rangemin, int rangemax)
		: base(entity)
	{
		this.rangemin = rangemin;
		this.rangemax = rangemax;
	}

	protected override void OnInitBase()
	{
		delaytime = Updater.AliveTime + 0.15f;
		starttime = Updater.AliveTime;
		halftime = alltime / 2f;
		startpos = m_Entity.position;
		bAttack = false;
		bool flag = MathDxx.RandomBool();
		if (!GameLogic.Release.MapCreatorCtrl.RandomItemLine(m_Entity, flag, rangemin, rangemax, out endx, out endz))
		{
			flag = !flag;
			if (!GameLogic.Release.MapCreatorCtrl.RandomItemLine(m_Entity, flag, rangemin, rangemax, out endx, out endz))
			{
				End();
				return;
			}
		}
		float num = endx;
		Vector3 position = m_Entity.position;
		perendx = (num - position.x) / (float)flyframe;
		float num2 = endz;
		Vector3 position2 = m_Entity.position;
		perendz = (num2 - position2.z) / (float)flyframe;
		m_Entity.SetObstacleCollider(value: false);
		m_Entity.m_AniCtrl.SendEvent("Skill");
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
			if (Updater.AliveTime > delaytime + alltime + 0.7f)
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
		m_Entity.SetObstacleCollider(value: true);
	}
}
