




using Dxx.Util;
using UnityEngine;

public class Bullet3004 : BulletBase
{
	protected override void OnUpdate()
	{
		base.CurrentDistance += base.Speed * Updater.delta;
		mTransform.Translate(Vector3.forward * base.Speed * Updater.delta);
		Vector3 position = mTransform.position;
		if (position.y < 0f || base.CurrentDistance > 100f)
		{
			overDistance();
		}
	}
}
