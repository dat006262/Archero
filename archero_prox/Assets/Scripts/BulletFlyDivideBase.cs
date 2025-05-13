




using Dxx.Util;
using UnityEngine;

public class BulletFlyDivideBase : BulletBase
{
	[Header("分裂石头ID")]
	public int DivideID;

	[Header("分裂石头数量")]
	public int DivideCount;

	[Header("分裂石头初始角度偏移")]
	public int AngelOffset;

	[Header("分裂石头初始角度是否根据父子弹角度")]
	public bool DependBulletAngle;

	[Header("创建分裂石头时间间隔")]
	public float DivideTime;

	[Header("清除子弹父亲属性")]
	public bool ClearAttribute = true;

	private float updatetime;

	protected override void OnInit()
	{
		base.OnInit();
		updatetime = Updater.AliveTime;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (DivideTime > 0f && Updater.AliveTime - updatetime > DivideTime)
		{
			updatetime += DivideTime;
			CreateDivideBullet();
		}
	}

	private void CreateDivideBullet()
	{
		if (DivideID == 0)
		{
			SdkManager.Bugly_Report("BulletFlyDivideBase", Utils.FormatString("{0} CreateDivideBullet DivideID == 0", mTransform.name));
			return;
		}
		if (DivideCount == 0)
		{
			SdkManager.Bugly_Report("BulletFlyDivideBase", Utils.FormatString("{0} CreateDivideBullet DivideCount == 0", mTransform.name));
			return;
		}
		float num = 360f / (float)DivideCount;
		for (int i = 0; i < DivideCount; i++)
		{
			float num2 = 0f;
			if (DependBulletAngle)
			{
				num2 = bulletAngle;
			}
			BulletManager bullet = GameLogic.Release.Bullet;
			EntityBase entity = base.m_Entity;
			int divideID = DivideID;
			Vector3 position = mTransform.position;
			float x = position.x;
			Vector3 position2 = mTransform.position;
			bullet.CreateBulletInternal(entity, divideID, new Vector3(x, 1f, position2.z), (float)i * num + (float)AngelOffset + num2, ClearAttribute);
		}
	}
}
