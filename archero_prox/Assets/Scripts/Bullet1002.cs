




public class Bullet1002 : BulletBase
{
	protected override void AwakeInit()
	{
		Parabola_MaxHeight = 3f;
	}

	protected override void OnInit()
	{
		base.OnInit();
	}

	public override void SetTarget(EntityBase entity, int size = 1)
	{
		base.SetTarget(entity, size);
		if (PosFromStart2Target < 8f)
		{
			PosFromStart2Target = 8f;
		}
	}

	protected override void OnHitHero(EntityBase entity)
	{
	}
}
