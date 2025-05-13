




using Dxx.Util;
using UnityEngine;

public class Bullet1010701 : BulletBase
{
	private float perangle = 2f;

	private float currentrotateangle;

	private float move1time;

	private float waittime;

	private float currenttime;

	private EntityBase mTarget;

	private bool bFind;

	protected override void OnInit()
	{
		base.OnInit();
		Refresh();
	}

	private void Refresh()
	{
		currentrotateangle = 0f;
		move1time = 0.6f;
		waittime = 0f;
		currenttime = Updater.AliveTime;
		bFind = false;
		mTarget = null;
	}

	protected override void OnArrowEject(EntityBase nexttarget)
	{
		mTarget = nexttarget;
	}

	protected override void OnUpdate()
	{
		float num = 0f;
		if (Updater.AliveTime - currenttime < move1time)
		{
			num = base.FrameDistance;
		}
		else if (!(Updater.AliveTime - currenttime < waittime))
		{
			num = base.FrameDistance;
			if (!bFind)
			{
				mTarget = GameLogic.Release.Entity.GetNearEntity(this, sameteam: false);
				bFind = true;
			}
			if ((bool)mTarget && !mTarget.GetIsDead())
			{
				Vector3 position = mTarget.position;
				float x = position.x;
				Vector3 position2 = mTransform.position;
				float x2 = x - position2.x;
				Vector3 position3 = mTarget.position;
				float z = position3.z;
				Vector3 position4 = mTransform.position;
				float y = z - position4.z;
				float angle = Utils.getAngle(x2, y);
				Vector3 eulerAngles = mTransform.eulerAngles;
				float y2 = eulerAngles.y;
				float num2 = MathDxx.MoveTowardsAngle(y2, angle, perangle);
				float num3 = MathDxx.Abs(y2 - num2);
				float num4 = MathDxx.Abs(y2 - num2 + 360f);
				float num5 = MathDxx.Abs(y2 - num2 - 360f);
				if (num3 > num4)
				{
					num3 = num4;
				}
				if (num3 > num5)
				{
					num3 = num5;
				}
				currentrotateangle += num3;
				bulletAngle = num2;
				UpdateMoveDirection();
			}
		}
		mTransform.position += new Vector3(moveX, 0f, moveY * 1.23f) * num;
		base.CurrentDistance += num;
		if (base.CurrentDistance >= base.Distance)
		{
			overDistance();
		}
	}
}
