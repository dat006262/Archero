




using UnityEngine;

public class AIMove1056 : AIMove1008
{
	private float createdis;

	private float currentdis;

	private float angle;

	public AIMove1056(EntityBase entity, int time, float move2playerratio, float createdis)
		: base(entity, move2playerratio, time)
	{
		this.createdis = createdis;
	}

	protected override void OnInitBase()
	{
		base.OnInitBase();
		currentdis = 0f;
		angle = GameLogic.Random(0f, 360f);
		m_Entity.Event_PositionBy += OnMoveBy;
	}

	private void OnMoveBy(Vector3 move)
	{
		currentdis += move.magnitude;
		if (currentdis >= createdis)
		{
			currentdis -= createdis;
			CreateBullet();
			angle += GameLogic.Random(70f, 110f);
		}
	}

	private void CreateBullet()
	{
		GameLogic.Release.Bullet.CreateBullet(m_Entity, 1058, m_Entity.m_Body.EffectMask.transform.position, angle);
	}

	protected override void OnEnd()
	{
		base.OnEnd();
		m_Entity.Event_PositionBy -= OnMoveBy;
	}
}
