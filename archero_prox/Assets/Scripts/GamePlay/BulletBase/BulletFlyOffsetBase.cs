




using Dxx.Util;
using UnityEngine;

public class BulletFlyOffsetBase : BulletBase
{
	[Header("横向偏移距离")]
	public float offsetposx;

	[Header("横向偏移速度")]
	public float speed;

	private float symbol;

	private float offsetcurrent;

	protected override void OnInit()
	{
		base.OnInit();
		offsetcurrent = 0f;
		init_offset();
	}

	private void init_offset()
	{
		symbol = ((offsetposx >= 0f) ? 1 : (-1));
		offsetposx = MathDxx.Abs(offsetposx);
		speed = MathDxx.Abs(speed);
	}

	public void SetOffset(float x, float speed)
	{
		offsetposx = x;
		this.speed = speed;
		init_offset();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		float num = speed * Updater.delta;
		if (offsetcurrent < offsetposx)
		{
			if (offsetcurrent + num > offsetposx)
			{
				num = offsetposx - offsetcurrent;
			}
			offsetcurrent += num;
		}
		else
		{
			num = 0f;
		}
		mTransform.position += new Vector3(0f - moveY, 0f, moveX) * num * symbol;
	}
}
