




using DG.Tweening;
using UnityEngine;

public class BulletFlySwordBase : BulletBase
{
	private bool bStart;

	[Header("缩放时间")]
	public float scaleTime = 0.5f;

	[Header("等待时间")]
	public float waitTime = 0.6f;

	protected override void OnInit()
	{
		base.OnInit();
		bStart = false;
		mTransform.localScale = new Vector3(0f, 1f, 1f);
		Sequence s = mSeqPool.Get();
		s.Append(mTransform.DOScale(1f, scaleTime)).AppendInterval(waitTime).AppendCallback(delegate
		{
			TrailShow(show: true);
			bStart = true;
		});
	}

	protected override void OnSetBulletAttribute()
	{
		base.OnSetBulletAttribute();
		TrailShow(show: false);
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
