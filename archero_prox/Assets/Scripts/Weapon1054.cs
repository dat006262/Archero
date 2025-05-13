




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class Weapon1054 : WeaponBase
{
	private Sequence seq;

	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
		KillSequence();
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	protected override void OnAttack(object[] args)
	{
		seq = DOTween.Sequence();
		seq.AppendCallback(delegate
		{
			CreateBulletByAngle(-30f);
		});
		seq.AppendInterval(0.05f);
		seq.AppendCallback(delegate
		{
			CreateBulletByAngle(-180f);
		});
		seq.AppendInterval(0.03f);
		seq.AppendCallback(delegate
		{
			CreateBulletByAngle(20f);
		});
	}

	private void CreateBulletByAngle(float angle)
	{
		float x = MathDxx.Sin(angle);
		float z = MathDxx.Cos(angle);
		CreateBulletOverride(new Vector3(x, 0f, z), angle);
	}
}
