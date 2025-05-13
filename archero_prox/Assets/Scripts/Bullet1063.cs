




using TableTool;
using UnityEngine;

public class Bullet1063 : Bullet5046
{
	protected override void OnInit()
	{
		base.OnInit();
		bFlyRotate = false;
		mTransform.localRotation = Quaternion.identity;
		onetime = 2f;
		movedis = 2f;
		curveId = 100011;
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(curveId);
	}
}
