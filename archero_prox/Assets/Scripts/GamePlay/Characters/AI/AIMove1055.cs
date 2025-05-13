




using Dxx.Util;
using UnityEngine;

public class AIMove1055 : AIMoveBase
{
	private EntityBase target;

	private Vector3 dir;

	private float g = 5f;

	private float endx;

	private float endz;

	private float perendx;

	private float perendz;

	private float delaytime;

	private float starttime;

	private float alltime = 0.4f;

	private float halftime;

	private Vector3 startpos;

	public AIMove1055(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		delaytime = Updater.AliveTime + 0.2f;
		starttime = Updater.AliveTime;
		halftime = alltime / 2f;
		startpos = m_Entity.position;
		target = GameLogic.Self;
		Vector3 position = target.position;
		endx = position.x;
		Vector3 position2 = target.position;
		endz = position2.z;
		float num = endx;
		Vector3 position3 = m_Entity.position;
		perendx = num - position3.x;
		float num2 = endz;
		Vector3 position4 = m_Entity.position;
		perendz = num2 - position4.z;
		m_Entity.m_AniCtrl.SendEvent("Skill");
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
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num, startpos.y + num2, startpos.z + perendz * num));
			return;
		}
		if (Updater.AliveTime < delaytime + alltime)
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

	protected override void OnEnd()
	{
		GameLogic.Release.Bullet.CreateBullet(m_Entity, 5095, m_Entity.m_Body.FootMask.transform.position, 0f);
	}
}
