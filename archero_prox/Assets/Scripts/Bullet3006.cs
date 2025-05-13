




using Dxx.Util;
using UnityEngine;

public class Bullet3006 : BulletBase
{
	private float perangle;

	private float currentrotateangle;

	private float maxangle;

	private Vector3 endpos;

	private float tracktime = 1.5f;

	protected override void OnInit()
	{
		base.OnInit();
		currentrotateangle = 0f;
		perangle = -1f;
	}

	public void SetEndPos(Vector3 endpos, float offsetangle)
	{
		this.endpos = endpos;
		maxangle = MathDxx.Abs(offsetangle) + 60f;
	}

	protected override void OnUpdate()
	{
		float frameDistance = base.FrameDistance;
		float x = endpos.x;
		Vector3 position = mTransform.position;
		float x2 = x - position.x;
		float z = endpos.z;
		Vector3 position2 = mTransform.position;
		float y = z - position2.z;
		float angle = Utils.getAngle(x2, y);
		float bulletAngle = base.bulletAngle;
		perangle += 1f;
		if (currentrotateangle < maxangle)
		{
			float num = MathDxx.MoveTowardsAngle(bulletAngle, angle, perangle);
			if (num == angle)
			{
				currentrotateangle = 2.14748365E+09f;
			}
			float num2 = MathDxx.Abs(bulletAngle - num);
			float num3 = MathDxx.Abs(bulletAngle - num + 360f);
			float num4 = MathDxx.Abs(bulletAngle - num - 360f);
			if (num2 > num3)
			{
				num2 = num3;
			}
			if (num2 > num4)
			{
				num2 = num4;
			}
			currentrotateangle += num2;
			base.bulletAngle = num;
			UpdateMoveDirection();
		}
		mTransform.position += new Vector3(moveX, 0f, moveY * 1.23f) * frameDistance;
		base.CurrentDistance += frameDistance;
		if (base.CurrentDistance >= base.Distance)
		{
			overDistance();
		}
	}
}
