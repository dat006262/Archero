




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class Weapon5108 : WeaponBase
{
	private SequencePool mPool = new SequencePool();

	protected override void OnInstall()
	{
		mPool.Clear();
	}

	protected override void OnUnInstall()
	{
		mPool.Clear();
	}

	protected override void OnAttack(object[] args)
	{
		GameLogic.Hold.Sound.PlayBulletCreate(2005001, m_Entity.position);
		int count = 3;
		for (int i = 0; i < count; i++)
		{
			int index = i;
			Sequence s = mPool.Get();
			float offset = GameLogic.Random(-20f, 20f);
			for (int j = 0; j < 4; j++)
			{
				s.AppendCallback(delegate
				{
					float num = Utils.GetBulletAngle(index, count, 90f) + offset;
					Vector3 offsetpos = new Vector3(MathDxx.Sin(num), 0f, MathDxx.Cos(num)) * 1f;
					BulletBase bulletBase = CreateBulletOverride(offsetpos, num);
					bulletBase.mBulletTransmit.attribute.ReboundWall = new EntityAttributeBase.ValueRange(1, 2, 2);
					bulletBase.UpdateBulletAttribute();
				});
				s.AppendInterval(0.2f);
			}
		}
	}
}
