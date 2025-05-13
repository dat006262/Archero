




using UnityEngine;

public class Bullet1046 : BulletBase
{
	protected override void AwakeInit()
	{
	}

	protected override void OnInit()
	{
		base.OnInit();
		base.Speed *= GameLogic.Random(0.5f, 1.2f);
		Parabola_MaxHeight = GameLogic.Random(3f, 8f);
		PosFromStart2Target = GameLogic.Random(3f, 7f);
		mTransform.localScale = Vector3.one * GameLogic.Random(0.6f, 1f);
		TrailCtrl mTrailCtrl = base.mTrailCtrl;
		Vector3 localScale = mTransform.localScale;
		mTrailCtrl.UpdateTrailWidthScale(localScale.x);
	}
}
