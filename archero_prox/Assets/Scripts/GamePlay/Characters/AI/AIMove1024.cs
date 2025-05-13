




using Dxx.Util;
using UnityEngine;

public class AIMove1024 : AIMoveBase
{
	private EntityBase target;

	private Vector3 dir;

	private int flyframe = 60;

	private float g = 0.5f;

	private float endx;

	private float endz;

	private float perendx;

	private float perendz;

	private float delaytime;

	private float starttime;

	private float alltime = 0.35f;

	private float halftime;

	private Vector3 startpos;

	private int range;

	protected virtual int AttackID => 1005;

	public AIMove1024(EntityBase entity, int range)
		: base(entity)
	{
		this.range = range;
	}

	protected override void OnInitBase()
	{
		delaytime = Updater.AliveTime + 0.15f;
		starttime = Updater.AliveTime;
		halftime = alltime / 2f;
		startpos = m_Entity.position;
		target = GameLogic.Self;
		if (!target)
		{
			End();
			return;
		}
		GameLogic.Release.MapCreatorCtrl.RandomItemSide(m_Entity, range, out endx, out endz);
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
		float num2 = -4f * g * (num - 0.5f) * (num - 0.5f) + g;
		if (Updater.AliveTime < delaytime + halftime)
		{
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num * (float)flyframe, startpos.y + num2, startpos.z + perendz * num * (float)flyframe));
		}
		else if (Updater.AliveTime < delaytime + alltime)
		{
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num * (float)flyframe, startpos.y + num2, startpos.z + perendz * num * (float)flyframe));
		}
		else if (Updater.AliveTime > delaytime + alltime + 0.8f)
		{
			End();
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
