




using Dxx.Util;
using UnityEngine;

public class BulletDivideBase : BulletBase
{
	[Header("分裂石头ID")]
	public int DivideID;

	[Header("分裂石头数量")]
	public int DivideCount;

	[Header("分裂石头初始角度偏移")]
	public int AngelOffset;

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
			SdkManager.Bugly_Report("BulletDivideBase", Utils.FormatString("{0} OnOverDistance DivideID == 0", mTransform.name));
			return;
		}
		if (DivideCount == 0)
		{
			SdkManager.Bugly_Report("BulletDivideBase", Utils.FormatString("{0} OnOverDistance DivideCount == 0", mTransform.name));
			return;
		}
		float num = 360f / (float)DivideCount;
		for (int i = 0; i < DivideCount; i++)
		{
			BulletManager bullet = GameLogic.Release.Bullet;
			EntityBase entity = base.m_Entity;
			int divideID = DivideID;
			Vector3 position = mTransform.position;
			float x = position.x;
			Vector3 position2 = mTransform.position;
			bullet.CreateBulletInternal(entity, divideID, new Vector3(x, 1f, position2.z), (float)i * num + (float)AngelOffset, ClearAttribute);
		}
	}
}
