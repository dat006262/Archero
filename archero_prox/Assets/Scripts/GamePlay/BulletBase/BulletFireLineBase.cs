




using Dxx.Util;
using UnityEngine;

public class BulletFireLineBase : BulletBase
{
	public float maxtime = 0.3f;

	public float endtime = 0.8f;

	public float MaxLength = 7f;

	private BoxCollider mBoxCollider;

	private float time;

	private float percent;

	protected override void OnInit()
	{
		base.OnInit();
		time = 0f;
		if (boxList.Length > 0)
		{
			mBoxCollider = boxList[0];
			UpdateBoxColloder(0f);
		}
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void OnUpdate()
	{
		if (time < endtime)
		{
			time += Updater.delta;
			percent = time / maxtime;
			percent = MathDxx.Clamp01(percent);
			UpdateBoxColloder(percent);
		}
		else
		{
			UpdateBoxColloder(0f);
		}
	}

	private void UpdateBoxColloder(float percent)
	{
		if ((bool)mBoxCollider)
		{
			BoxCollider boxCollider = mBoxCollider;
			Vector3 size = mBoxCollider.size;
			float x = size.x;
			Vector3 size2 = mBoxCollider.size;
			boxCollider.size = new Vector3(x, size2.y, percent * MaxLength);
			BoxCollider boxCollider2 = mBoxCollider;
			Vector3 center = mBoxCollider.center;
			float x2 = center.x;
			Vector3 center2 = mBoxCollider.center;
			boxCollider2.center = new Vector3(x2, center2.y, percent * MaxLength / 2f);
		}
	}
}
