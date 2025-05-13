




public class Bullet1065 : BulletBase
{
	protected override void OnSetBulletAttribute()
	{
		mBulletTransmit.attribute.ReboundWall.UpdateCount(1);
		mBulletTransmit.attribute.ReboundWall.UpdateMin(20);
		mBulletTransmit.attribute.ReboundWall.UpdateMax(20);
		mReboundWallCount = mBulletTransmit.attribute.ReboundWall.Value;
		mReboundWallMaxCount = mReboundWallCount;
	}
}
