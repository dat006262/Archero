




using DG.Tweening;
using Dxx.Util;
using TableTool;
using UnityEngine;

public class Bullet1069 : BulletBase
{
	private float time = 1f;

	private float updatetime;

	private float percent;

	private LineRenderer line;

	private Color color;

	private float colora;

	private AnimationCurve curve;

	protected override void OnInit()
	{
		base.OnInit();
		if (!line && (bool)mBulletModel)
		{
			line = mBulletModel.GetComponentInChildren<LineRenderer>();
			if ((bool)line)
			{
				color = line.material.GetColor("_TintColor");
				colora = color.a;
				curve = LocalModelManager.Instance.Curve_curve.GetCurve(100024);
			}
		}
		updatetime = Updater.AliveTime;
		BoxEnable(enable: false);
		Sequence s = mSeqPool.Get();
		s.AppendInterval(time * 0.2f).AppendCallback(delegate
		{
			BoxEnable(enable: true);
		});
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void OnUpdate()
	{
		if ((bool)line)
		{
			percent = (Updater.AliveTime - updatetime) / time;
			color = new Color(color.r, color.g, color.b, colora * curve.Evaluate(percent));
			line.material.SetColor("_TintColor", color);
		}
	}
}
