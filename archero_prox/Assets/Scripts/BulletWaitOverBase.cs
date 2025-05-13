




using DG.Tweening;
using UnityEngine;

public class BulletWaitOverBase : BulletBase
{
	private bool bStart;

	[Header("等待时间")]
	public float waitTime = 0.5f;

	protected override void OnInit()
	{
		base.OnInit();
		bStart = false;
		Sequence s = mSeqPool.Get();
		SetBoxEnableOnce(0.3f);
		s.AppendInterval(waitTime).AppendCallback(delegate
		{
			overDistance();
		});
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void UpdateProcess()
	{
		base.UpdateProcess();
	}
}
