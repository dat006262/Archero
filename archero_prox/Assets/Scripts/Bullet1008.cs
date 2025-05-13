




using UnityEngine;

public class Bullet1008 : BulletBase
{
	public override void SetTarget(EntityBase entity, int size = 1)
	{
		base.SetTarget(entity, size);
		PosFromStart2Target += Random.Range(-3f, 3f);
		if (PosFromStart2Target < 8f)
		{
			PosFromStart2Target = 8f;
		}
	}
}
