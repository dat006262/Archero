




using TableTool;

public class Bullet5047 : Bullet5046
{
	protected override void OnSetArgs()
	{
		onetime = 2.4f;
		movedis = 2.5f;
		curveId = (int)mArgs[0];
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(curveId);
	}
}
