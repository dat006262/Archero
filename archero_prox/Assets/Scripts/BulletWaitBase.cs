




using DG.Tweening;
using UnityEngine;

public class BulletWaitBase : BulletBase
{
	private bool bStart;

	[Header("等待时间")]
	public float waitTime = 0.5f;

	protected override void OnInit()
	{
		base.OnInit();
		bStart = false;
		Sequence s = mSeqPool.Get();
		s.AppendInterval(waitTime).AppendCallback(delegate
		{
			bStart = true;
		});
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void UpdateProcess()
	{
		if (bStart)
		{
			base.UpdateProcess();
		}
	}
}
