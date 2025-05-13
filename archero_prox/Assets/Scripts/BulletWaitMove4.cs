




using DG.Tweening;
using UnityEngine;

public class BulletWaitMove4 : BulletBase
{
	private bool bStart;

	[Header("等待时间")]
	public float waitTime = 0.5f;

	protected override void OnInit()
	{
		base.OnInit();
		bStart = false;
		Sequence s = mSeqPool.Get();
		BoxEnable(enable: false);
		s.AppendInterval(waitTime).AppendCallback(delegate
		{
			BoxEnable(enable: true);
			bulletAngle = (float)GameLogic.Random(0, 4) * 90f;
			mTransform.rotation = Quaternion.Euler(0f, bulletAngle, 0f);
			UpdateMoveDirection();
			bStart = true;
		});
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void OnUpdate()
	{
		if (bStart)
		{
			base.OnUpdate();
		}
	}
}
