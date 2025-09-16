




using Dxx.Util;
using UnityEngine;

public class BulletSlopeCreateBase : BulletSlopeBase
{
	[Header("分裂石头ID")]
	public int DivideID;

	[Header("分裂石头数量")]
	public int DivideCount;

	[Header("分裂石头初始角度偏移")]
	public int AngelOffset;

	[Header("分裂石头高度")]
	public float Height = 0.3f;

	[Header("分裂石头初始前进距离")]
	public float ForwardLength;

	private Vector3 shadowScaleInit = Vector3.one * -1f;

	private float height;

	protected override void OnInit()
	{
		base.OnInit();
		if (shadowScaleInit.x < 0f && (bool)shadow)
		{
			shadowScaleInit = shadow.localScale;
		}
		Vector3 position = base.transform.position;
		height = position.y;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		Vector3 position = base.transform.position;
		float num = position.y / height;
		if ((bool)shadow)
		{
			shadow.localScale = shadowScaleInit * (1f - num) * 0.7f + shadowScaleInit * 0.3f;
		}
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
			float num2 = (float)i * num + (float)AngelOffset;
			float num3 = MathDxx.Sin(num2) * ForwardLength;
			float num4 = MathDxx.Cos(num2) * ForwardLength;
			BulletManager bullet = GameLogic.Release.Bullet;
			EntityBase entity = base.m_Entity;
			int divideID = DivideID;
			Vector3 position = mTransform.position;
			float x = position.x + num3;
			float y = Height;
			Vector3 position2 = mTransform.position;
			bullet.CreateBullet(entity, divideID, new Vector3(x, y, position2.z + num4), num2);
		}
	}
}
