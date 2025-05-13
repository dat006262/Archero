




using Dxx.Util;
using UnityEngine;

public class Bullet8001 : BulletBase
{
	private TrailRenderer trail1;

	private float trail1time;

	private int state;

	protected override void AwakeInit()
	{
	}

	protected override void OnInit()
	{
		base.OnInit();
		HitWallAction = OnThroughWalls;
		state = 0;
	}

	private void OnThroughWalls(Collider o)
	{
		if (state == 0)
		{
			mHitList.Clear();
			state = 1;
			base.m_Entity.PlayEffect(1008001, mTransform.position);
			float value = base.m_Entity.m_EntityData.attribute.WeaponRoundBackAttackPercent.Value;
			if (value > 0f)
			{
				mBulletTransmit.AddAttackRatio(1f + value);
			}
		}
	}

	protected override void OnOverDistance()
	{
	}

	protected override void BoxEnable(bool enable)
	{
		base.BoxEnable(enable);
	}

	protected override void OnThroughTrailShow(bool show)
	{
	}

	protected override void OnUpdate()
	{
		if (state != 0 && state == 1)
		{
			Vector3 position = base.m_Entity.position;
			float x = position.x;
			Vector3 position2 = mTransform.position;
			float x2 = x - position2.x;
			Vector3 position3 = base.m_Entity.position;
			float z = position3.z;
			Vector3 position4 = mTransform.position;
			float y = z - position4.z;
			bulletAngle = Utils.getAngle(x2, y);
		}
		float frameDistance = base.FrameDistance;
		UpdateMoveDirection();
		mTransform.position += new Vector3(moveX, 0f, moveY) * frameDistance;
		OnRotate();
		if (state == 1 && Vector3.Distance(mTransform.position, base.m_Entity.position) < 2f)
		{
			overDistance();
		}
	}

	protected virtual void OnRotate()
	{
		if ((bool)base.childMesh)
		{
			Transform childMesh = base.childMesh;
			Vector3 eulerAngles = base.childMesh.eulerAngles;
			float x = eulerAngles.x;
			Vector3 eulerAngles2 = base.childMesh.eulerAngles;
			float y = eulerAngles2.y + m_Data.RotateSpeed;
			Vector3 eulerAngles3 = base.childMesh.eulerAngles;
			childMesh.rotation = Quaternion.Euler(x, y, eulerAngles3.z);
		}
	}
}
