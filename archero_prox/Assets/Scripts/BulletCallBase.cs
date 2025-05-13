




using TableTool;
using UnityEngine;

public class BulletCallBase : BulletBase
{
	private Vector3 endpos;

	protected float height = 2f;

	private AnimationCurve curve;

	private Vector3 temppos;

	private Vector3 curvepos = default(Vector3);

	private float percent;

	private Vector3 startpos;

	private Vector3 straightpos;

	public bool bShowCallEffect = true;

	protected override void AwakeInit()
	{
		base.AwakeInit();
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(100002);
	}

	public void SetEndPos(Vector3 endpos)
	{
		startpos = mTransform.position;
		straightpos = startpos;
		this.endpos = endpos;
	}

	protected override void OnUpdate()
	{
		straightpos = Vector3.MoveTowards(straightpos, endpos, base.FrameDistance);
		percent = (straightpos - startpos).magnitude / (endpos - startpos).magnitude;
		curvepos.y = curve.Evaluate(percent) * height;
		temppos = straightpos + curvepos;
		mTransform.position = temppos;
		if ((mTransform.position - endpos).magnitude < 0.1f)
		{
			overDistance();
		}
	}

	protected override void OnOverDistance()
	{
		if ((bool)base.m_Entity)
		{
			EntityMonsterBase entityMonsterBase = base.m_Entity as EntityMonsterBase;
			if ((bool)entityMonsterBase && entityMonsterBase.GetAI() != null)
			{
				entityMonsterBase.GetAI().CallOne(endpos, bShowCallEffect);
			}
		}
	}
}
