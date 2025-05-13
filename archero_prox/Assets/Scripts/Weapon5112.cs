




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class Weapon5112 : WeaponBase
{
	protected override void OnInstall()
	{
	}

	protected override void OnUnInstall()
	{
	}

	protected override void OnAttack(object[] args)
	{
		Sequence s = mSeqPool.Get();
		bool flag = MathDxx.RandomBool();
		int count = 7;
		for (int i = 0; i < count; i++)
		{
			int index = i;
			if (flag)
			{
				index = count - i - 1;
			}
			s.AppendCallback(delegate
			{
				float bulletAngle = Utils.GetBulletAngle(index, count, 150f);
				float x = MathDxx.Sin(bulletAngle);
				float z = MathDxx.Cos(bulletAngle);
				CreateBulletOverride(new Vector3(x, 0f, z), bulletAngle);
			});
			s.AppendInterval(0.07f);
		}
	}
}
