




using UnityEngine;

public class Bullet1006 : BulletBase
{
	protected virtual float RotateAngle => -1.1f;

	protected override void AwakeInit()
	{
	}

	protected override void OnInit()
	{
		base.OnInit();
	}

	protected override void OnUpdate()
	{
		float frameDistance = base.FrameDistance;
		mTransform.position += new Vector3(moveX, 0f, moveY * 1.23f) * frameDistance;
		childMesh.rotation = Quaternion.Euler(55f, 0f, 0f);
		bulletAngle += RotateAngle;
		UpdateMoveDirection();
		base.CurrentDistance += frameDistance;
		if (base.CurrentDistance >= base.Distance)
		{
			overDistance();
		}
	}
}
