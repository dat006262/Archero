




using Dxx.Util;
using TableTool;
using UnityEngine;

public class Bullet5056 : BulletBase
{
	private AnimationCurve curve;

	private float mBoxTime;

	private float percent;

	private BoxCollider mBoxCollider;

	protected override void AwakeInit()
	{
		base.AwakeInit();
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(100019);
		mBoxCollider = boxList[0];
	}

	protected override void OnInit()
	{
		base.OnInit();
		mBoxTime = 0f;
		UpdateBox();
	}

	protected override void UpdateProcess()
	{
		base.UpdateProcess();
		mBoxTime += Updater.delta;
		mBoxTime = MathDxx.Clamp01(mBoxTime);
		UpdateBox();
	}

	private void UpdateBox()
	{
		BoxCollider boxCollider = mBoxCollider;
		Vector3 size = mBoxCollider.size;
		float x = size.x;
		Vector3 size2 = mBoxCollider.size;
		boxCollider.size = new Vector3(x, size2.y, curve.Evaluate(mBoxTime) * 20f);
		BoxCollider boxCollider2 = mBoxCollider;
		Vector3 center = mBoxCollider.center;
		float x2 = center.x;
		Vector3 center2 = mBoxCollider.center;
		float y = center2.y;
		Vector3 size3 = mBoxCollider.size;
		boxCollider2.center = new Vector3(x2, y, size3.z / 2f);
		mHitList.Clear();
	}
}
