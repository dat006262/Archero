




using UnityEngine;

public class Bullet5005 : BulletBase
{
	public override void SetTarget(EntityBase entity, int size = 1)
	{
		base.SetTarget(entity, size);
		PosFromStart2Target += Random.Range(-5f, 5f);
	}
}
