




using DG.Tweening;
using UnityEngine;

public class Weapon1071 : WeaponBase
{
	private Sequence seq;

	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
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
		for (int i = 0; i < 3; i++)
		{
			int index = i;
			seq.AppendCallback(delegate
			{
				float num = 0f;
				num = ((index == 0) ? GameLogic.Random(-30f, 0f) : ((index != 1) ? GameLogic.Random(-15f, 15f) : GameLogic.Random(0f, 30f)));
				CreateBulletOverride(Vector3.zero, num);
			});
			seq.AppendInterval(0.33f);
		}
	}
}
