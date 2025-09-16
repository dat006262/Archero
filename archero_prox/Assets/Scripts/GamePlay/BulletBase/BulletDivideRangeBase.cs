




using Dxx.Util;
using UnityEngine;

public class BulletDivideRangeBase : BulletBase
{
	[Header("分裂石头ID")]
	public int DivideID;

	[Header("分裂石头数量")]
	public int DivideCount;

	[Header("角度min")]
	public int angle_min;

	[Header("角度max")]
	public int angle_max;

	[Header("角度随机")]
	public int angle_random;

	[Header("清除子弹父亲属性")]
	public bool ClearAttribute = true;

	protected override void OnHitWall()
	{
		OnOverDistance();
	}

	protected override void OnOverDistance()
	{
		if (DivideID == 0)
		{
			SdkManager.Bugly_Report("BulletDivideRandomBase", Utils.FormatString("{0} OnOverDistance DivideID == 0", mTransform.name));
			return;
		}
		if (DivideCount == 0)
		{
			SdkManager.Bugly_Report("BulletDivideRandomBase", Utils.FormatString("{0} OnOverDistance DivideCount == 0", mTransform.name));
			return;
		}
		float num = GameLogic.Random((float)(-angle_random) / 2f, (float)angle_random / 2f);
		for (int i = 0; i < DivideCount; i++)
		{
			BulletManager bullet = GameLogic.Release.Bullet;
			EntityBase entity = base.m_Entity;
			int divideID = DivideID;
			Vector3 position = mTransform.position;
			float x = position.x;
			Vector3 position2 = mTransform.position;
			bullet.CreateBulletInternal(entity, divideID, new Vector3(x, 1f, position2.z), Utils.GetBulletAngle(i, DivideCount, angle_max - angle_min) + (float)(angle_max + angle_min) / 2f + num, ClearAttribute);
		}
	}
}
