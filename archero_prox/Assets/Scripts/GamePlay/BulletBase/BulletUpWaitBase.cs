




using DG.Tweening;
using UnityEngine;

public class BulletUpWaitBase : BulletBase
{
	private bool bStart;

	[Header("升起时间")]
	public float upTime = 1f;

	[Header("升起高度")]
	public float upHeight = 1f;

	[Header("等待时间")]
	public float waitTime = 0.5f;

	protected override void OnInit()
	{
		base.OnInit();
		bStart = false;
		Sequence s = mSeqPool.Get();
		s.Append(mTransform.DOMoveY(upHeight, upTime)).AppendInterval(waitTime).AppendCallback(delegate
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
