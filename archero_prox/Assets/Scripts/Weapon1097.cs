




using DG.Tweening;
using UnityEngine;

public class Weapon1097 : WeaponBase
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
		int num = 5;
		for (int i = 0; i < num; i++)
		{
			int index = i;
			seq.AppendCallback(delegate
			{
				float num2 = 0f;
				num2 = ((index == 0) ? GameLogic.Random(-30f, 0f) : ((index == 1) ? GameLogic.Random(0f, 30f) : ((index == 2) ? GameLogic.Random(-15f, 15f) : ((index != 3) ? GameLogic.Random(-30f, 30f) : GameLogic.Random(-20f, 20f)))));
				CreateBulletOverride(Vector3.zero, num2);
			});
			seq.AppendInterval(0.22f);
		}
	}
}
