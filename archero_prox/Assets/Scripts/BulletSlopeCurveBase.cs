




using TableTool;
using UnityEngine;

public class BulletSlopeCurveBase : BulletBase
{
	private Vector3 endpos;

	private Vector3 dir;

	protected float height = 2f;

	private AnimationCurve curve;

	private Vector3 temppos;

	private Vector3 curvepos = default(Vector3);

	private float percent;

	private Vector3 startpos;

	protected override void AwakeInit()
	{
		base.AwakeInit();
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(100002);
	}

	protected override void OnInit()
	{
		base.OnInit();
	}

	public void SetEndPos(Vector3 endpos)
	{
		startpos = mTransform.position;
		this.endpos = endpos;
	}

	protected override void OnUpdate()
	{
		dir = (mTransform.position - endpos).normalized;
		temppos = Vector3.MoveTowards(mTransform.position, endpos, base.FrameDistance);
		percent = (endpos - temppos).magnitude / (endpos - startpos).magnitude;
		curvepos.y = curve.Evaluate(percent);
		temppos += curvepos;
		if ((mTransform.position - endpos).magnitude < 0.1f)
		{
			overDistance();
		}
	}
}
