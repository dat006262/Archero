




using Dxx.Util;
using TableTool;
using UnityEngine;

public class Bullet5046 : BulletBase
{
	protected float onetime = 1f;

	protected float movedis = 0.6f;

	protected int curveId = 100011;

	protected AnimationCurve curve;

	private float percent;

	private float starttime;

	private float perspeed;

	private Vector3 straightpos;

	private Vector3 straightperpos;

	private Vector3 offset;

	protected override void AwakeInit()
	{
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(curveId);
	}

	protected override void OnInit()
	{
		base.OnInit();
		starttime = Updater.AliveTime;
		straightpos = mTransform.position;
	}

	protected override void OnSetArgs()
	{
	}

	protected override void OnUpdate()
	{
		if (Updater.AliveTime - starttime > onetime)
		{
			starttime += onetime;
		}
		percent = (Updater.AliveTime - starttime) / onetime;
		offset = new Vector3(0f - moveY, 0f, moveX) * movedis * curve.Evaluate(percent);
		float frameDistance = base.FrameDistance;
		straightperpos = new Vector3(moveX, 0f, moveY * 1.23f) * frameDistance;
		straightpos += straightperpos;
		mTransform.position = straightpos + offset;
		base.CurrentDistance += frameDistance;
		if (base.CurrentDistance >= base.Distance)
		{
			overDistance();
		}
	}
}
