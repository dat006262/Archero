




using UnityEngine;

public class Bullet5082 : BulletBase
{
	protected override void OnInit()
	{
		base.OnInit();
		mTransform.localRotation = Quaternion.identity;
		bFlyRotate = false;
		base.Distance = 2.14748365E+09f;
	}

	protected override void OnSetBulletAttribute()
	{
		mBulletTransmit.attribute.ReboundWall.UpdateCount(1);
		mBulletTransmit.attribute.ReboundWall.UpdateMin(int.MaxValue);
		mBulletTransmit.attribute.ReboundWall.UpdateMax(int.MaxValue);
		mReboundWallCount = mBulletTransmit.attribute.ReboundWall.Value;
		mReboundWallMaxCount = mReboundWallCount;
	}
}
