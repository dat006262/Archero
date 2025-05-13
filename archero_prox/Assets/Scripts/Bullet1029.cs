




public class Bullet1029 : BulletBase
{
	private float perdis = 4f;

	private float curdis;

	protected override void OnInit()
	{
		base.OnInit();
		curdis = perdis;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (base.CurrentDistance >= curdis)
		{
			curdis += perdis;
			Create3005();
		}
	}

	private void Create3005()
	{
		CreateOne3005(90f);
		CreateOne3005(-90f);
	}

	private void CreateOne3005(float angle)
	{
		GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 3005, mTransform.position, bulletAngle + angle);
	}
}
