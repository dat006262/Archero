




public class Bullet8002 : BulletBase
{
	protected override void AwakeInit()
	{
	}

	protected override void OnInit()
	{
		base.OnInit();
	}

	protected override void OnHitWall()
	{
	}

	protected override void OnHitHero(EntityBase entity)
	{
	}

	protected override void OnOverDistance()
	{
	}

	protected override void BoxEnable(bool enable)
	{
		base.BoxEnable(enable);
	}

	protected override void OnThroughTrailShow(bool show)
	{
		if (mTrailCtrl != null)
		{
			if (show)
			{
				mTrailCtrl.SetTrailTime(2f);
			}
			else
			{
				mTrailCtrl.SetTrailTime(1f);
			}
		}
	}
}
