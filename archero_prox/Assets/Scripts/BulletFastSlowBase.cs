




using Dxx.Util;
using TableTool;
using UnityEngine;

public class BulletFastSlowBase : BulletBase
{
	private AnimationCurve curve;

	private float time;

	[Header("曲线变化总时间")]
	public float alltime = 0.7f;

	[Header("速度增加系数")]
	public float speedratio = 4f;

	protected override void OnInit()
	{
		base.OnInit();
		InitCurve();
		time = 0f;
	}

	private void InitCurve()
	{
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(100006);
	}

	protected override void OnUpdate()
	{
		time += Updater.delta;
		float num = base.FrameDistance + base.FrameDistance * speedratio * curve.Evaluate(time / alltime);
		UpdateMoveDirection();
		mTransform.position += new Vector3(moveX, 0f, moveY * 1.23f) * num;
		base.CurrentDistance += num;
		if (base.CurrentDistance >= base.Distance)
		{
			overDistance();
		}
	}
}
