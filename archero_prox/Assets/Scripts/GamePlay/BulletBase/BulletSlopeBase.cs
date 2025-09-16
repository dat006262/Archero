




using UnityEngine;

public class BulletSlopeBase : BulletBase
{
	private Vector3 endpos;

	private Vector3 dir;

	protected override void OnInit()
	{
		base.OnInit();
	}

	public void SetEndPos(Vector3 endpos)
	{
		this.endpos = endpos;
	}

	protected override void OnUpdate()
	{
		dir = (mTransform.position - endpos).normalized;
		mTransform.position = Vector3.MoveTowards(mTransform.position, endpos, base.FrameDistance);
		if ((mTransform.position - endpos).magnitude < 0.1f)
		{
			overDistance();
		}
	}
}
