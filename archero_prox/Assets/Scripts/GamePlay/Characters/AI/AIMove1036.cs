




using Dxx.Util;
using UnityEngine;

public class AIMove1036 : AIMove1008
{
	private float perattacktime = 0.3f;

	private float attackcurrenttime;

	public AIMove1036(EntityBase entity, float move2playerratio, int time)
		: base(entity, move2playerratio, time)
	{
	}

	protected override void OnInitBase()
	{
		base.OnInitBase();
		attackcurrenttime = 0f;
	}

	protected override void OnUpdate()
	{
		attackcurrenttime += Updater.delta;
		if (attackcurrenttime >= perattacktime)
		{
			attackcurrenttime -= perattacktime;
			CreateBullet();
		}
		base.OnUpdate();
	}

	private void CreateBullet()
	{
		GameLogic.Release.Bullet.CreateBullet(m_Entity, 5030, m_Entity.position + new Vector3(0f, 1f, 0f), GameLogic.Random(0, 360));
	}
}
