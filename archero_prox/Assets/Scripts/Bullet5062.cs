




public class Bullet5062 : BulletBase
{
	private float angle;

	private float createdis;

	private float perdis = 3.5f;

	protected override void OnInit()
	{
		base.OnInit();
		createdis = perdis;
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void UpdateProcess()
	{
		base.UpdateProcess();
		if (base.CurrentDistance > createdis)
		{
			CreateBullets();
			createdis += perdis;
		}
	}

	private void CreateBullets()
	{
		angle = GameLogic.Random(0f, 360f);
		for (int i = 0; i < 4; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 5063, mTransform.position, angle + bulletAngle + (float)(i * 90));
		}
	}
}
